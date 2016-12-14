using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccessSchema
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void AccDB_TableProcessStarted(object sender, EventArgs e)
        {
            TableProcessStartedEventArgs args = (TableProcessStartedEventArgs)e;
            this.tableLabel.Text = args.TableName;
            this.tableLabel.Refresh();
        }


        private void DataPicture_DragDrop(object sender, DragEventArgs e)
        {
            string[] filenames = e.Data.GetData(DataFormats.FileDrop, false) as string[];

            foreach (string filename in filenames)
            {
                var accDB = new AccessDB();
                accDB.Filename = filename;
                accDB.TableProcessStarted += AccDB_TableProcessStarted;
                databaseLabel.Text = filename;
                databaseLabel.Refresh();
                accDB.Delimiter = DelimiterTextBox.Text;
                accDB.Quote = QuoteTextBox.Text;
                accDB.HtmlEncodeData = HtmlEncodeCheckBox.Checked;
                accDB.Headers = HeadersCheckBox.Checked;
                accDB.NewlineReplacement = txtNewline.Text;
                accDB.ValidateDates = DateValidationCheckBox.Checked;
                accDB.DateFormatOverride = DateFormatComboBox.Text.Trim();
                accDB.DumpData();                
                tableLabel.Text = "Done";
            }
        }

        private void DataPicture_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void SchemaPicture_DragDrop(object sender, DragEventArgs e)
        {
            string[] filenames = e.Data.GetData(DataFormats.FileDrop, false) as string[];

            foreach (string filename in filenames)
            {
                var accDB = new AccessDB();
                accDB.Filename = filename;
                accDB.TableProcessStarted += AccDB_TableProcessStarted;
                databaseLabel.Text = filename;
                databaseLabel.Refresh();
                accDB.DumpSchema();
                tableLabel.Text = "Done";
            }
        }

        private void SchemaPicture_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = Application.ProductName + " " + Application.ProductVersion;
        }

        private void DataPicture_Click(object sender, EventArgs e)
        {

        }
    }
}
