namespace AccessSchema
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.databaseLabel = new System.Windows.Forms.Label();
            this.tableLabel = new System.Windows.Forms.Label();
            this.DataPicture = new System.Windows.Forms.PictureBox();
            this.SchemaPicture = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.QuoteTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.DelimiterTextBox = new System.Windows.Forms.TextBox();
            this.HeadersCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.DataPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SchemaPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AllowDrop = true;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Drop access databases below to export";
            // 
            // databaseLabel
            // 
            this.databaseLabel.AllowDrop = true;
            this.databaseLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.databaseLabel.BackColor = System.Drawing.SystemColors.Control;
            this.databaseLabel.Location = new System.Drawing.Point(12, 157);
            this.databaseLabel.MinimumSize = new System.Drawing.Size(200, 0);
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.Size = new System.Drawing.Size(322, 15);
            this.databaseLabel.TabIndex = 7;
            // 
            // tableLabel
            // 
            this.tableLabel.AllowDrop = true;
            this.tableLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLabel.BackColor = System.Drawing.SystemColors.Control;
            this.tableLabel.Location = new System.Drawing.Point(12, 184);
            this.tableLabel.MinimumSize = new System.Drawing.Size(200, 0);
            this.tableLabel.Name = "tableLabel";
            this.tableLabel.Size = new System.Drawing.Size(322, 15);
            this.tableLabel.TabIndex = 8;
            // 
            // DataPicture
            // 
            this.DataPicture.AllowDrop = true;
            this.DataPicture.Image = ((System.Drawing.Image)(resources.GetObject("DataPicture.Image")));
            this.DataPicture.Location = new System.Drawing.Point(133, 52);
            this.DataPicture.Name = "DataPicture";
            this.DataPicture.Size = new System.Drawing.Size(85, 72);
            this.DataPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.DataPicture.TabIndex = 3;
            this.DataPicture.TabStop = false;
            this.DataPicture.DragDrop += new System.Windows.Forms.DragEventHandler(this.DataPicture_DragDrop);
            this.DataPicture.DragEnter += new System.Windows.Forms.DragEventHandler(this.DataPicture_DragEnter);
            // 
            // SchemaPicture
            // 
            this.SchemaPicture.AllowDrop = true;
            this.SchemaPicture.Image = ((System.Drawing.Image)(resources.GetObject("SchemaPicture.Image")));
            this.SchemaPicture.Location = new System.Drawing.Point(15, 52);
            this.SchemaPicture.Name = "SchemaPicture";
            this.SchemaPicture.Size = new System.Drawing.Size(100, 72);
            this.SchemaPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SchemaPicture.TabIndex = 4;
            this.SchemaPicture.TabStop = false;
            this.SchemaPicture.DragDrop += new System.Windows.Forms.DragEventHandler(this.SchemaPicture_DragDrop);
            this.SchemaPicture.DragEnter += new System.Windows.Forms.DragEventHandler(this.SchemaPicture_DragEnter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(130, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Export Data";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Export Schema";
            // 
            // QuoteTextBox
            // 
            this.QuoteTextBox.Location = new System.Drawing.Point(295, 80);
            this.QuoteTextBox.Name = "QuoteTextBox";
            this.QuoteTextBox.Size = new System.Drawing.Size(34, 20);
            this.QuoteTextBox.TabIndex = 6;
            this.QuoteTextBox.Text = "\"";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(236, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Quote";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(236, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Delimiter ";
            // 
            // DelimiterTextBox
            // 
            this.DelimiterTextBox.Location = new System.Drawing.Point(295, 52);
            this.DelimiterTextBox.Name = "DelimiterTextBox";
            this.DelimiterTextBox.Size = new System.Drawing.Size(34, 20);
            this.DelimiterTextBox.TabIndex = 4;
            this.DelimiterTextBox.Text = "[[";
            // 
            // HeadersCheckBox
            // 
            this.HeadersCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.HeadersCheckBox.Checked = true;
            this.HeadersCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HeadersCheckBox.Location = new System.Drawing.Point(239, 106);
            this.HeadersCheckBox.Name = "HeadersCheckBox";
            this.HeadersCheckBox.Size = new System.Drawing.Size(90, 18);
            this.HeadersCheckBox.TabIndex = 9;
            this.HeadersCheckBox.Text = "Headers";
            this.HeadersCheckBox.UseVisualStyleBackColor = true;
            this.HeadersCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 209);
            this.Controls.Add(this.HeadersCheckBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DelimiterTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.QuoteTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SchemaPicture);
            this.Controls.Add(this.DataPicture);
            this.Controls.Add(this.tableLabel);
            this.Controls.Add(this.databaseLabel);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Data Masking Export";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SchemaPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label databaseLabel;
        private System.Windows.Forms.Label tableLabel;
        private System.Windows.Forms.PictureBox DataPicture;
        private System.Windows.Forms.PictureBox SchemaPicture;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox QuoteTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox DelimiterTextBox;
        private System.Windows.Forms.CheckBox HeadersCheckBox;
    }
}

