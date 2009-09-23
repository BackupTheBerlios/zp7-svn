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
            this.serverBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.wizardPage1 = new Gui.Wizard.WizardPage();
            this.lbserver = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbserver = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imptype = new System.Windows.Forms.ListBox();
            this.header1 = new Gui.Wizard.Header();
            this.wizardPage3 = new Gui.Wizard.WizardPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.header3 = new Gui.Wizard.Header();
            this.wizard1 = new Gui.Wizard.Wizard();
            this.wizardPage2 = new Gui.Wizard.WizardPage();
            this.header2 = new Gui.Wizard.Header();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.songBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.authorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.songtextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.langDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publishedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.serverBindingSource)).BeginInit();
            this.wizardPage1.SuspendLayout();
            this.wizardPage3.SuspendLayout();
            this.wizard1.SuspendLayout();
            this.wizardPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // serverBindingSource
            // 
            this.serverBindingSource.DataMember = "server";
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
            this.wizardPage1.Size = new System.Drawing.Size(567, 369);
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
            this.header1.Size = new System.Drawing.Size(567, 64);
            this.header1.TabIndex = 0;
            this.header1.Title = "Import písní";
            // 
            // wizardPage3
            // 
            this.wizardPage3.Controls.Add(this.propertyGrid1);
            this.wizardPage3.Controls.Add(this.header3);
            this.wizardPage3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPage3.IsFinishPage = false;
            this.wizardPage3.Location = new System.Drawing.Point(0, 0);
            this.wizardPage3.Name = "wizardPage3";
            this.wizardPage3.Size = new System.Drawing.Size(567, 369);
            this.wizardPage3.TabIndex = 3;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(12, 70);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(543, 285);
            this.propertyGrid1.TabIndex = 1;
            // 
            // header3
            // 
            this.header3.BackColor = System.Drawing.SystemColors.Control;
            this.header3.CausesValidation = false;
            this.header3.Description = "Importované soubory / zdroj dat pro import";
            this.header3.Dock = System.Windows.Forms.DockStyle.Top;
            this.header3.Image = ((System.Drawing.Image)(resources.GetObject("header3.Image")));
            this.header3.Location = new System.Drawing.Point(0, 0);
            this.header3.Name = "header3";
            this.header3.Size = new System.Drawing.Size(567, 64);
            this.header3.TabIndex = 0;
            this.header3.Title = "Import písní";
            // 
            // wizard1
            // 
            this.wizard1.Controls.Add(this.wizardPage2);
            this.wizard1.Controls.Add(this.wizardPage3);
            this.wizard1.Controls.Add(this.wizardPage1);
            this.wizard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizard1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizard1.Location = new System.Drawing.Point(0, 0);
            this.wizard1.Name = "wizard1";
            this.wizard1.Pages.AddRange(new Gui.Wizard.WizardPage[] {
            this.wizardPage1,
            this.wizardPage3,
            this.wizardPage2});
            this.wizard1.Size = new System.Drawing.Size(567, 417);
            this.wizard1.TabIndex = 0;
            // 
            // wizardPage2
            // 
            this.wizardPage2.Controls.Add(this.header2);
            this.wizardPage2.Controls.Add(this.dataGridView1);
            this.wizardPage2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPage2.IsFinishPage = false;
            this.wizardPage2.Location = new System.Drawing.Point(0, 0);
            this.wizardPage2.Name = "wizardPage2";
            this.wizardPage2.Size = new System.Drawing.Size(567, 369);
            this.wizardPage2.TabIndex = 4;
            this.wizardPage2.ShowFromNext += new System.EventHandler(this.wizardPage2_ShowFromNext);
            // 
            // header2
            // 
            this.header2.BackColor = System.Drawing.SystemColors.Control;
            this.header2.CausesValidation = false;
            this.header2.Description = "Prohlížení importovaných písní";
            this.header2.Dock = System.Windows.Forms.DockStyle.Top;
            this.header2.Image = ((System.Drawing.Image)(resources.GetObject("header2.Image")));
            this.header2.Location = new System.Drawing.Point(0, 0);
            this.header2.Name = "header2";
            this.header2.Size = new System.Drawing.Size(567, 64);
            this.header2.TabIndex = 1;
            this.header2.Title = "Import písní";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.titleDataGridViewTextBoxColumn,
            this.authorDataGridViewTextBoxColumn,
            this.groupnameDataGridViewTextBoxColumn,
            this.remark,
            this.songtextDataGridViewTextBoxColumn,
            this.langDataGridViewTextBoxColumn,
            this.publishedDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.songBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(3, 74);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(557, 283);
            this.dataGridView1.TabIndex = 0;
            // 
            // songBindingSource
            // 
            this.songBindingSource.DataMember = "song";
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            // 
            // titleDataGridViewTextBoxColumn
            // 
            this.titleDataGridViewTextBoxColumn.DataPropertyName = "title";
            this.titleDataGridViewTextBoxColumn.HeaderText = "Název";
            this.titleDataGridViewTextBoxColumn.Name = "titleDataGridViewTextBoxColumn";
            // 
            // authorDataGridViewTextBoxColumn
            // 
            this.authorDataGridViewTextBoxColumn.DataPropertyName = "author";
            this.authorDataGridViewTextBoxColumn.HeaderText = "Autor";
            this.authorDataGridViewTextBoxColumn.Name = "authorDataGridViewTextBoxColumn";
            // 
            // groupnameDataGridViewTextBoxColumn
            // 
            this.groupnameDataGridViewTextBoxColumn.DataPropertyName = "groupname";
            this.groupnameDataGridViewTextBoxColumn.HeaderText = "Skupina";
            this.groupnameDataGridViewTextBoxColumn.Name = "groupnameDataGridViewTextBoxColumn";
            // 
            // remark
            // 
            this.remark.DataPropertyName = "remark";
            this.remark.HeaderText = "Poznámka";
            this.remark.Name = "remark";
            // 
            // songtextDataGridViewTextBoxColumn
            // 
            this.songtextDataGridViewTextBoxColumn.DataPropertyName = "songtext";
            this.songtextDataGridViewTextBoxColumn.HeaderText = "Text";
            this.songtextDataGridViewTextBoxColumn.Name = "songtextDataGridViewTextBoxColumn";
            // 
            // langDataGridViewTextBoxColumn
            // 
            this.langDataGridViewTextBoxColumn.DataPropertyName = "lang";
            this.langDataGridViewTextBoxColumn.HeaderText = "lang";
            this.langDataGridViewTextBoxColumn.Name = "langDataGridViewTextBoxColumn";
            this.langDataGridViewTextBoxColumn.Visible = false;
            // 
            // publishedDataGridViewTextBoxColumn
            // 
            this.publishedDataGridViewTextBoxColumn.DataPropertyName = "published";
            this.publishedDataGridViewTextBoxColumn.HeaderText = "published";
            this.publishedDataGridViewTextBoxColumn.Name = "publishedDataGridViewTextBoxColumn";
            this.publishedDataGridViewTextBoxColumn.Visible = false;
            // 
            // ImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 417);
            this.Controls.Add(this.wizard1);
            this.Name = "ImportForm";
            this.Text = "Import písní";
            ((System.ComponentModel.ISupportInitialize)(this.serverBindingSource)).EndInit();
            this.wizardPage1.ResumeLayout(false);
            this.wizardPage1.PerformLayout();
            this.wizardPage3.ResumeLayout(false);
            this.wizard1.ResumeLayout(false);
            this.wizardPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inetSongDb)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource serverBindingSource;
        private Gui.Wizard.WizardPage wizardPage1;
        private System.Windows.Forms.ComboBox lbserver;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbserver;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox imptype;
        private Gui.Wizard.Header header1;
        private Gui.Wizard.WizardPage wizardPage3;
        private Gui.Wizard.Header header3;
        private Gui.Wizard.Wizard wizard1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private Gui.Wizard.WizardPage wizardPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource songBindingSource;
        private InetSongDb inetSongDb;
        private Gui.Wizard.Header header2;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn authorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn songtextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn langDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn publishedDataGridViewTextBoxColumn;
    }
}