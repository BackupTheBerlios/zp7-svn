namespace Plugin.sqlite
{
    partial class ConnWizard
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbfilename = new System.Windows.Forms.TextBox();
            this.btbrowse = new System.Windows.Forms.Button();
            this.rbopenExisting = new System.Windows.Forms.RadioButton();
            this.rbcreateNew = new System.Windows.Forms.RadioButton();
            this.lbversion = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "s_filename";
            // 
            // tbfilename
            // 
            this.tbfilename.Location = new System.Drawing.Point(31, 50);
            this.tbfilename.Name = "tbfilename";
            this.tbfilename.Size = new System.Drawing.Size(265, 20);
            this.tbfilename.TabIndex = 1;
            // 
            // btbrowse
            // 
            this.btbrowse.Location = new System.Drawing.Point(302, 47);
            this.btbrowse.Name = "btbrowse";
            this.btbrowse.Size = new System.Drawing.Size(30, 23);
            this.btbrowse.TabIndex = 2;
            this.btbrowse.Text = "...";
            this.btbrowse.UseVisualStyleBackColor = true;
            this.btbrowse.Click += new System.EventHandler(this.btbrowse_Click);
            // 
            // rbopenExisting
            // 
            this.rbopenExisting.AutoSize = true;
            this.rbopenExisting.Location = new System.Drawing.Point(12, 11);
            this.rbopenExisting.Name = "rbopenExisting";
            this.rbopenExisting.Size = new System.Drawing.Size(101, 17);
            this.rbopenExisting.TabIndex = 3;
            this.rbopenExisting.Text = "s_open_existing";
            this.rbopenExisting.UseVisualStyleBackColor = true;
            this.rbopenExisting.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // rbcreateNew
            // 
            this.rbcreateNew.AutoSize = true;
            this.rbcreateNew.Location = new System.Drawing.Point(12, 76);
            this.rbcreateNew.Name = "rbcreateNew";
            this.rbcreateNew.Size = new System.Drawing.Size(92, 17);
            this.rbcreateNew.TabIndex = 4;
            this.rbcreateNew.Text = "s_create_new";
            this.rbcreateNew.UseVisualStyleBackColor = true;
            this.rbcreateNew.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // lbversion
            // 
            this.lbversion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lbversion.FormattingEnabled = true;
            this.lbversion.Items.AddRange(new object[] {
            "SQLite 2",
            "SQLite 3"});
            this.lbversion.Location = new System.Drawing.Point(31, 112);
            this.lbversion.Name = "lbversion";
            this.lbversion.Size = new System.Drawing.Size(265, 21);
            this.lbversion.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "s_version";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(179, 152);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "s_ok";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(260, 152);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "s_cancel";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "SQLite 3 database (*.db3)|*.db3|SQLite  database (*.db)|*.db";
            // 
            // ConnWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 187);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbversion);
            this.Controls.Add(this.rbcreateNew);
            this.Controls.Add(this.rbopenExisting);
            this.Controls.Add(this.btbrowse);
            this.Controls.Add(this.tbfilename);
            this.Controls.Add(this.label1);
            this.Name = "ConnWizard";
            this.Text = "SQLite";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbfilename;
        private System.Windows.Forms.Button btbrowse;
        private System.Windows.Forms.RadioButton rbopenExisting;
        private System.Windows.Forms.RadioButton rbcreateNew;
        private System.Windows.Forms.ComboBox lbversion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}