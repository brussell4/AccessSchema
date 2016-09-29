using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Data.OleDb;

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

        private void DumpTable(StreamWriter w, DataTable t)
        {
            string[] values = new string[t.Columns.Count];

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
                foreach (DataColumn col in t.Columns)
                {
                    values[col.Ordinal] = row[col.ColumnName].ToString();
                    if (row[col.ColumnName] == DBNull.Value)
                        Debug.WriteLine("null");

                    if ("System.String".IndexOf(col.DataType.ToString(), StringComparison.OrdinalIgnoreCase) >= 0 && row[col.ColumnName] != DBNull.Value)
                    {
                        values[col.Ordinal] = Quote + values[col.Ordinal] + Quote;
                    }                    
                }
                StringBuilder b = new StringBuilder();
                for (int i = 0; i < t.Columns.Count; i++)
                {
                    b.Append(values[i]);
                    if (i < t.Columns.Count - 1)
                        b.Append(Delimiter);
                }
                w.WriteLine(b.ToString());
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
                    if (table.Rows.Count > 0)
                    {
                        if (File.Exists(outputFile))
                            File.Delete(outputFile);
                        using (var file = new StreamWriter(outputFile))
                        {
                            DumpTable(file, table);
                        }
                    }

                }

            }
        }

        public void DumpSchema()
        {
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
    }
}
