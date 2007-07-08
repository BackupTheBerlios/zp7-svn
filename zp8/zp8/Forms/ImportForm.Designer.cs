namespace zp8
{
    partial class ImportForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportForm));
            this.wizard1 = new Gui.Wizard.Wizard();
            this.wizardPage1 = new Gui.Wizard.WizardPage();
            this.lbserver = new System.Windows.Forms.ComboBox();
            this.serverBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.songDb = new zp8.SongDb();
            this.label3 = new System.Windows.Forms.Label();
            this.cbserver = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imptype = new System.Windows.Forms.ListBox();
            this.header1 = new Gui.Wizard.Header();
            this.wizardPage3 = new Gui.Wizard.WizardPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.filelist = new System.Windows.Forms.ListBox();
            this.header3 = new Gui.Wizard.Header();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.wizard1.SuspendLayout();
            this.wizardPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.songDb)).BeginInit();
            this.wizardPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizard1
            // 
            this.wizard1.Controls.Add(this.wizardPage1);
            this.wizard1.Controls.Add(this.wizardPage3);
            this.wizard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizard1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizard1.Location = new System.Drawing.Point(0, 0);
            this.wizard1.Name = "wizard1";
            this.wizard1.Pages.AddRange(new Gui.Wizard.WizardPage[] {
            this.wizardPage1,
            this.wizardPage3});
            this.wizard1.Size = new System.Drawing.Size(483, 355);
            this.wizard1.TabIndex = 0;
            // 
            // wizardPage1
            // 
            this.wizardPage1.Controls.Add(this.lbserver);
            this.wizardPage1.Controls.Add(this.label3);
            this.wizardPage1.Controls.Add(this.cbserver);
            this.wizardPage1.Controls.Add(this.label2);
            this.wizardPage1.Controls.Add(this.description);
            this.wizardPage1.Controls.Add(this.label1);
            this.wizardPage1.Controls.Add(this.imptype);
            this.wizardPage1.Controls.Add(this.header1);
            this.wizardPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPage1.IsFinishPage = false;
            this.wizardPage1.Location = new System.Drawing.Point(0, 0);
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(483, 307);
            this.wizardPage1.TabIndex = 1;
            // 
            // lbserver
            // 
            this.lbserver.DataSource = this.serverBindingSource;
            this.lbserver.DisplayMember = "url";
            this.lbserver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lbserver.Enabled = false;
            this.lbserver.FormattingEnabled = true;
            this.lbserver.Location = new System.Drawing.Point(275, 261);
            this.lbserver.Name = "lbserver";
            this.lbserver.Size = new System.Drawing.Size(121, 21);
            this.lbserver.TabIndex = 7;
            this.lbserver.ValueMember = "ID";
            // 
            // serverBindingSource
            // 
            this.serverBindingSource.DataMember = "server";
            this.serverBindingSource.DataSource = this.songDb;
            // 
            // songDb
            // 
            this.songDb.DataSetName = "SongDb";
            this.songDb.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(272, 245);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Server";
            // 
            // cbserver
            // 
            this.cbserver.AutoSize = true;
            this.cbserver.Location = new System.Drawing.Point(59, 245);
            this.cbserver.Name = "cbserver";
            this.cbserver.Size = new System.Drawing.Size(88, 17);
            this.cbserver.TabIndex = 5;
            this.cbserver.Text = "Zadat server";
            this.cbserver.UseVisualStyleBackColor = true;
            this.cbserver.CheckedChanged += new System.EventHandler(this.cbserver_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(272, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Popis";
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(275, 118);
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.ReadOnly = true;
            this.description.Size = new System.Drawing.Size(145, 108);
            this.description.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Typ importovaných dat";
            // 
            // imptype
            // 
            this.imptype.FormattingEnabled = true;
            this.imptype.Location = new System.Drawing.Point(59, 118);
            this.imptype.Name = "imptype";
            this.imptype.Size = new System.Drawing.Size(156, 108);
            this.imptype.TabIndex = 1;
            this.imptype.SelectedIndexChanged += new System.EventHandler(this.imptype_SelectedIndexChanged);
            // 
            // header1
            // 
            this.header1.BackColor = System.Drawing.SystemColors.Control;
            this.header1.CausesValidation = false;
            this.header1.Description = "Základní údaje";
            this.header1.Dock = System.Windows.Forms.DockStyle.Top;
            this.header1.Image = ((System.Drawing.Image)(resources.GetObject("header1.Image")));
            this.header1.Location = new System.Drawing.Point(0, 0);
            this.header1.Name = "header1";
            this.header1.Size = new System.Drawing.Size(483, 64);
            this.header1.TabIndex = 0;
            this.header1.Title = "Import písní";
            // 
            // wizardPage3
            // 
            this.wizardPage3.Controls.Add(this.button2);
            this.wizardPage3.Controls.Add(this.button1);
            this.wizardPage3.Controls.Add(this.label4);
            this.wizardPage3.Controls.Add(this.filelist);
            this.wizardPage3.Controls.Add(this.header3);
            this.wizardPage3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPage3.IsFinishPage = false;
            this.wizardPage3.Location = new System.Drawing.Point(0, 0);
            this.wizardPage3.Name = "wizardPage3";
            this.wizardPage3.Size = new System.Drawing.Size(483, 307);
            this.wizardPage3.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(120, 272);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Odstranit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(39, 272);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Pøidat";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Soubory";
            // 
            // filelist
            // 
            this.filelist.FormattingEnabled = true;
            this.filelist.Location = new System.Drawing.Point(39, 106);
            this.filelist.Name = "filelist";
            this.filelist.Size = new System.Drawing.Size(390, 160);
            this.filelist.TabIndex = 1;
            // 
            // header3
            // 
            this.header3.BackColor = System.Drawing.SystemColors.Control;
            this.header3.CausesValidation = false;
            this.header3.Description = "Importované soubory";
            this.header3.Dock = System.Windows.Forms.DockStyle.Top;
            this.header3.Image = ((System.Drawing.Image)(resources.GetObject("header3.Image")));
            this.header3.Location = new System.Drawing.Point(0, 0);
            this.header3.Name = "header3";
            this.header3.Size = new System.Drawing.Size(483, 64);
            this.header3.TabIndex = 0;
            this.header3.Title = "Import písní";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.ShowReadOnly = true;
            // 
            // ImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 355);
            this.Controls.Add(this.wizard1);
            this.Name = "ImportForm";
            this.Text = "Import písní";
            this.wizard1.ResumeLayout(false);
            this.wizardPage1.ResumeLayout(false);
            this.wizardPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.songDb)).EndInit();
            this.wizardPage3.ResumeLayout(false);
            this.wizardPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Gui.Wizard.Wizard wizard1;
        private Gui.Wizard.WizardPage wizardPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox imptype;
        private Gui.Wizard.Header header1;
        private Gui.Wizard.WizardPage wizardPage3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.BindingSource serverBindingSource;
        private SongDb songDb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox filelist;
        private Gui.Wizard.Header header3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox cbserver;
        private System.Windows.Forms.ComboBox lbserver;
        private System.Windows.Forms.Label label3;
    }
}