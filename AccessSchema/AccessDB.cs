using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Net;

namespace AccessSchema
{
    public class TableProcessStartedEventArgs : EventArgs
    {
        public string TableName { get; set; }

        public TableProcessStartedEventArgs(string tableName)
        {
            TableName = tableName;
        }

    }

    public class AccessDB
    {
        public event EventHandler TableProcessStarted;
        public string Filename { get; set; }
        public string Delimiter { get; set; }
        public string Quote { get; set; }
        public bool Headers { get; set; }
        public bool HtmlEncodeData { get; set; }
        public string NewlineReplacement { get; set; }
        public bool ValidateDates { get; set; }
        public string DateFormatOverride { get; set; }

        /// <summary>
        /// Dumps out data from tables. NB does so in ordinal order, which may not be the column order as read by C# here
        /// Does this by transferring values into an array indexed by col.Ordinal
        /// </summary>
        /// <param name="w"></param>
        /// <param name="t"></param>
        private void DumpTable(StreamWriter w, StreamWriter errors, DataTable t)
        {
            string[] values = new string[t.Columns.Count];
            string value = "";
            DateTime sqlMinDate = new DateTime(1753, 1, 1);

            if (Headers)
            {
                for (int i = 0; i < t.Columns.Count; i++)
                {
                    w.Write(t.Columns[i].ColumnName);
                    if (i < t.Columns.Count - 1)
                        w.Write(Delimiter);
                    else
                        w.WriteLine("");
                }
            }            
            foreach (DataRow row in t.Rows)
            {
                var rowErrors = new StringBuilder();

                foreach (DataColumn col in t.Columns)
                {
                    // Set the value in the output array based on column ordinal
                    values[col.Ordinal] = row[col.ColumnName].ToString();

                    // Overwrite out of range dates with minimum date from SQL Server
                    if (col.DataType == System.Type.GetType("System.DateTime") && values[col.Ordinal].Length > 0)
                    {
                        if (ValidateDates)
                        {
                            DateTime dateValue = DateTime.Parse(values[col.Ordinal]);
                            // January 1, 1753, through December 31, 9999                            
                            if (dateValue < sqlMinDate || dateValue > new DateTime(9999, 12, 31))
                            {
                                rowErrors.Append(values[col.Ordinal] + " changed to ");
                                values[col.Ordinal] = sqlMinDate.ToShortDateString();
                                rowErrors.Append(values[col.Ordinal]);
                                rowErrors.Append("\r\n");
                            }
                        }

                        if (DateFormatOverride.Length > 0)
                        {
                            DateTime dateValue = DateTime.Parse(values[col.Ordinal]);
                            values[col.Ordinal] = dateValue.ToString(DateFormatOverride);
                        }
                    }

                    // If it's a string field that has "Date" in the name, also validate it as a date
                    if (ValidateDates && col.DataType == System.Type.GetType("System.String") && 
                        col.ColumnName.IndexOf("date", StringComparison.OrdinalIgnoreCase) >= 0 && 
                        values[col.Ordinal].Trim().Length > 0)
                    {
                        // January 1, 1753, through December 31, 9999    
                        DateTime dateValue;
                        if (!DateTime.TryParse(values[col.Ordinal], out dateValue))
                        {
                            rowErrors.Append(values[col.Ordinal]);
                            rowErrors.Append("\r\n");
                            values[col.Ordinal] = values[col.Ordinal] + " #BAD#";                            
                        }
                        else if (dateValue < new DateTime(1753, 1, 1) || dateValue > new DateTime(9999, 12, 31))
                        {
                            rowErrors.Append(values[col.Ordinal]);
                            rowErrors.Append("\r\n");
                            values[col.Ordinal] = values[col.Ordinal] + " #BAD#";                            
                        }
                    }

                    if (col.DataType == System.Type.GetType("System.String") && row[col.ColumnName] != DBNull.Value)
                    {
                        // If the column is string, HTML code the value if needed
                        if (HtmlEncodeData)
                            value = WebUtility.HtmlEncode(values[col.Ordinal]);
                        else
                            value = values[col.Ordinal];

                        // Look for the quote in the data and replace with space as a defence
                        if (Quote.Length > 0 && value.Contains(Quote))
                            value = value.Replace(Quote, " ");

                        // Quote the string value if needed
                        values[col.Ordinal] = Quote + value + Quote;
                    }
                }

                // Build a CSV row for output from the values array
                StringBuilder b = new StringBuilder();
                for (int i = 0; i < t.Columns.Count; i++)
                {
                    b.Append(values[i]);
                    if (i < t.Columns.Count - 1)
                        b.Append(Delimiter);
                }

                string line = b.ToString();
                if (NewlineReplacement != null && NewlineReplacement.Length > 0)
                    line = line.Replace("\r\n", NewlineReplacement);

                w.WriteLine(line);
                if (rowErrors.Length > 0)
                {
                    errors.WriteLine(line);
                    errors.WriteLine(rowErrors.ToString());
                }
            }
        }

        public void DumpData()
        {

            string datafolder = Filename + ".Export";
            if (!Directory.Exists(datafolder))
            {
                Directory.CreateDirectory(datafolder);
            }
            string conenctionString = String.Format(Properties.Settings.Default.ConnectionStringTemplate, Filename);

            using (var con = new OleDbConnection(conenctionString))
            {
                con.Open();
                List<string> tableNames = new List<string>();

                var tables = con.GetSchema("TABLES", new string[] { null, null, null, "TABLE" });
                foreach (System.Data.DataRow row in tables.Rows)
                {
                    var tableName = row["TABLE_NAME"].ToString();
                    if (TableProcessStarted != null)
                        TableProcessStarted(this, new TableProcessStartedEventArgs(tableName));

                    var cmd = new OleDbCommand("SELECT * FROM [" + tableName + "]", con);
                    var adapter = new OleDbDataAdapter(cmd);
                    var table = new DataTable();
                    adapter.Fill(table);
                    var outputFile = datafolder + "\\" + tableName + ".txt";
                    var errorFile = datafolder + "\\" + tableName + ".log";
                    if (table.Rows.Count > 0)
                    {
                        if (File.Exists(outputFile))
                            File.Delete(outputFile);

                        using (var file = new StreamWriter(outputFile))
                        {
                            bool emptyFile = false;
                            using (var errors = new StreamWriter(errorFile))
                            {
                                DumpTable(file, errors, table);
                                emptyFile = (errors.BaseStream.Position == 0);
                            }
                            if (emptyFile)
                                File.Delete(errorFile);
                        }
                    }

                }

            }
        }

        public void DumpSchema()
        {
            //DumpLinks();

            var mdbName = Path.GetFileName(Filename);
            string conenctionString = String.Format(Properties.Settings.Default.ConnectionStringTemplate, Filename);

            using (var con = new OleDbConnection(conenctionString))
            {
                con.Open();

                List<string> tableNames = new List<string>();

                var tables = con.GetSchema("TABLES", new string[] { null, null, null, "TABLE" });
                foreach (System.Data.DataRow row in tables.Rows)
                    tableNames.Add(row["TABLE_NAME"].ToString());

                var outputFile = Filename + ".csv";
                if (File.Exists(outputFile))
                    File.Delete(outputFile);
                using (var file = new StreamWriter(outputFile))
                {
                    file.WriteLine("Database name, Table Name,Column name, Type, Length, Column Position");
                    foreach (var tableName in tableNames)
                    {
                        if (TableProcessStarted != null)
                            TableProcessStarted(this, new TableProcessStartedEventArgs(tableName));

                        var cols = con.GetSchema("COLUMNS", new string[] { null, null, tableName, null });
                        string[] schemaRow = new string[cols.Rows.Count];
                        foreach (DataRow col in cols.Rows)
                        {
                            int ordinal = int.Parse(col["ORDINAL_POSITION"].ToString());
                            string typeName = oleDbToNetTypeConverter((int)col["DATA_TYPE"]).ToString();
                            string maxLength = col["CHARACTER_MAXIMUM_LENGTH"].ToString();
                            if ("OleDbType.BSTR OleDbType.Char OleDbType.Guid OleDbType.LongVarChar OleDbType.LongVarWChar OleDbType.PropVariant OleDbType.VarWChar OleDbType.WChar".IndexOf(typeName, StringComparison.OrdinalIgnoreCase) == -1)
                                maxLength = "";
                            schemaRow[ordinal - 1] = "\"" + mdbName + "\",\"" + col["TABLE_NAME"].ToString() + "\",\"" + col["COLUMN_NAME"].ToString() + "\",\"" + typeName + "\"," + maxLength + "," + ordinal.ToString();
                        }

                        // Deal with columns being listed in schema out of ordninal position
                        for (int i = 0; i < schemaRow.Length; i++)
                            file.WriteLine(schemaRow[i]);
                    }
                }
            }
        }

        public OleDbType oleDbToNetTypeConverter(int oleDbTypeNumber)
        {
            switch (oleDbTypeNumber)
            {
                case (int)OleDbType.Binary: return OleDbType.Binary;
                case (int)OleDbType.Boolean: return OleDbType.Boolean;
                case (int)OleDbType.BSTR: return OleDbType.BSTR;
                case (int)OleDbType.Char: return OleDbType.Char;
                case (int)OleDbType.Currency: return OleDbType.Currency;
                case (int)OleDbType.Date: return OleDbType.Date;
                case (int)OleDbType.DBDate: return OleDbType.DBDate;
                case (int)OleDbType.DBTime: return OleDbType.DBTime;
                case (int)OleDbType.DBTimeStamp: return OleDbType.DBTimeStamp;
                case (int)OleDbType.Decimal: return OleDbType.Decimal;
                case (int)OleDbType.Double: return OleDbType.Double;
                case (int)OleDbType.Empty: return OleDbType.Empty;
                case (int)OleDbType.Error: return OleDbType.Error;
                case (int)OleDbType.Filetime: return OleDbType.Filetime;
                case (int)OleDbType.Guid: return OleDbType.Guid;
                case (int)OleDbType.IDispatch: return OleDbType.IDispatch;
                case (int)OleDbType.Integer: return OleDbType.Integer;
                case (int)OleDbType.IUnknown: return OleDbType.IUnknown;
                case (int)OleDbType.LongVarBinary: return OleDbType.LongVarBinary;
                case (int)OleDbType.LongVarChar: return OleDbType.LongVarChar;
                case (int)OleDbType.LongVarWChar: return OleDbType.LongVarWChar;
                case (int)OleDbType.Numeric: return OleDbType.Numeric;
                case (int)OleDbType.PropVariant: return OleDbType.PropVariant;
                case (int)OleDbType.Single: return OleDbType.Single;
                case (int)OleDbType.SmallInt: return OleDbType.SmallInt;
                case (int)OleDbType.TinyInt: return OleDbType.TinyInt;
                case (int)OleDbType.UnsignedBigInt: return OleDbType.UnsignedBigInt;
                case (int)OleDbType.UnsignedInt: return OleDbType.UnsignedInt;
                case (int)OleDbType.UnsignedSmallInt: return OleDbType.UnsignedSmallInt;
                case (int)OleDbType.UnsignedTinyInt: return OleDbType.UnsignedTinyInt;
                case (int)OleDbType.VarBinary: return OleDbType.VarBinary;
                case (int)OleDbType.VarChar: return OleDbType.VarChar;
                case (int)OleDbType.Variant: return OleDbType.Variant;
                case (int)OleDbType.VarNumeric: return OleDbType.VarNumeric;
                case (int)OleDbType.VarWChar: return OleDbType.VarWChar;
                case (int)OleDbType.WChar: return OleDbType.WChar;
            }
            throw (new Exception("DataType Not Supported"));
        }

        public void DumpLinks()
        {
            var mdbName = Path.GetFileName(Filename);
            string conenctionString = String.Format(Properties.Settings.Default.ConnectionStringTemplate, Filename);

            using (var con = new OleDbConnection(conenctionString))
            {
                con.Open();

                List<string> tableNames = new List<string>();

                var outputFile = Filename + ".links.txt";
                if (File.Exists(outputFile))
                    File.Delete(outputFile);

                using (var file = new StreamWriter(outputFile))
                {
                    file.WriteLine("Database name, Table Name,Column name, Type, Length, Column Position");

                    var tables = con.GetSchema("TABLES", new string[] { null, null, null, "PASS-THROUGH" });
                    foreach (System.Data.DataRow row in tables.Rows)
                    {
                        tableNames.Add(row["TABLE_NAME"].ToString());
                    }


                }
            }
        }
    }
}
