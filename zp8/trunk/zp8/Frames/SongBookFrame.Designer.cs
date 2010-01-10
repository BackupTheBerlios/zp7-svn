namespace zp8
{
    partial class SongBookFrame
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbsequence = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.savepdf = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbprinter = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.previewFrame1 = new zp8.PreviewFrame();
            this.previewFrame2 = new zp8.PreviewFrame();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.cbprinter);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lbsequence);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(218, 520);
            this.panel2.TabIndex = 1;
            // 
            // lbsequence
            // 
            this.lbsequence.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbsequence.FormattingEnabled = true;
            this.lbsequence.Location = new System.Drawing.Point(2, 92);
            this.lbsequence.Name = "lbsequence";
            this.lbsequence.Size = new System.Drawing.Size(208, 420);
            this.lbsequence.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(218, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(416, 520);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.previewFrame1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(408, 494);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Náhled";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.previewFrame2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(408, 494);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Logické stránky";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // savepdf
            // 
            this.savepdf.Filter = "PDF soubory (*.pdf)|*.pdf";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Písnì ve zpìvníku";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Formátovat pro tiskárnu";
            // 
            // cbprinter
            // 
            this.cbprinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbprinter.FormattingEnabled = true;
            this.cbprinter.Location = new System.Drawing.Point(2, 25);
            this.cbprinter.Name = "cbprinter";
            this.cbprinter.Size = new System.Drawing.Size(127, 21);
            this.cbprinter.TabIndex = 3;
            this.cbprinter.SelectedIndexChanged += new System.EventHandler(this.cbprinter_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(135, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Nastavení";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // previewFrame1
            // 
            this.previewFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewFrame1.Location = new System.Drawing.Point(3, 3);
            this.previewFrame1.Name = "previewFrame1";
            this.previewFrame1.Size = new System.Drawing.Size(402, 488);
            this.previewFrame1.Source = null;
            this.previewFrame1.TabIndex = 0;
            // 
            // previewFrame2
            // 
            this.previewFrame2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewFrame2.Location = new System.Drawing.Point(3, 3);
            this.previewFrame2.Name = "previewFrame2";
            this.previewFrame2.Size = new System.Drawing.Size(402, 488);
            this.previewFrame2.Source = null;
            this.previewFrame2.TabIndex = 0;
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // SongBookFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.Name = "SongBookFrame";
            this.Size = new System.Drawing.Size(634, 520);
            this.Load += new System.EventHandler(this.SongBookFrame_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox lbsequence;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private PreviewFrame previewFrame1;
        private System.Windows.Forms.SaveFileDialog savepdf;
        private PreviewFrame previewFrame2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbprinter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PrintDialog printDialog1;
    }
}
