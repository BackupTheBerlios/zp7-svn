namespace Plugin.mssql
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnWizard));
            this.wizard1 = new Gui.Wizard.Wizard();
            this.pgcreadentials = new Gui.Wizard.WizardPage();
            this.pgselectdb = new Gui.Wizard.WizardPage();
            this.pgname = new Gui.Wizard.WizardPage();
            this.header1 = new Gui.Wizard.Header();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.datasource = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.login = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.connectionstring = new System.Windows.Forms.TextBox();
            this.wizard1.SuspendLayout();
            this.pgcreadentials.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizard1
            // 
            this.wizard1.Controls.Add(this.pgcreadentials);
            this.wizard1.Controls.Add(this.pgselectdb);
            this.wizard1.Controls.Add(this.pgname);
            this.wizard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizard1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizard1.Location = new System.Drawing.Point(0, 0);
            this.wizard1.Name = "wizard1";
            this.wizard1.Pages.AddRange(new Gui.Wizard.WizardPage[] {
            this.pgcreadentials,
            this.pgselectdb,
            this.pgname});
            this.wizard1.Size = new System.Drawing.Size(452, 280);
            this.wizard1.TabIndex = 0;
            this.wizard1.Load += new System.EventHandler(this.wizard1_Load);
            // 
            // pgcreadentials
            // 
            this.pgcreadentials.Controls.Add(this.connectionstring);
            this.pgcreadentials.Controls.Add(this.label5);
            this.pgcreadentials.Controls.Add(this.comboBox1);
            this.pgcreadentials.Controls.Add(this.password);
            this.pgcreadentials.Controls.Add(this.login);
            this.pgcreadentials.Controls.Add(this.label4);
            this.pgcreadentials.Controls.Add(this.label3);
            this.pgcreadentials.Controls.Add(this.datasource);
            this.pgcreadentials.Controls.Add(this.label2);
            this.pgcreadentials.Controls.Add(this.label1);
            this.pgcreadentials.Controls.Add(this.header1);
            this.pgcreadentials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgcreadentials.IsFinishPage = false;
            this.pgcreadentials.Location = new System.Drawing.Point(0, 0);
            this.pgcreadentials.Name = "pgcreadentials";
            this.pgcreadentials.Size = new System.Drawing.Size(452, 232);
            this.pgcreadentials.TabIndex = 2;
            this.pgcreadentials.CloseFromNext += new Gui.Wizard.PageEventHandler(this.pgcreadentials_CloseFromNext);
            // 
            // pgselectdb
            // 
            this.pgselectdb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgselectdb.IsFinishPage = false;
            this.pgselectdb.Location = new System.Drawing.Point(0, 0);
            this.pgselectdb.Name = "pgselectdb";
            this.pgselectdb.Size = new System.Drawing.Size(452, 232);
            this.pgselectdb.TabIndex = 3;
            // 
            // pgname
            // 
            this.pgname.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgname.IsFinishPage = false;
            this.pgname.Location = new System.Drawing.Point(0, 0);
            this.pgname.Name = "pgname";
            this.pgname.Size = new System.Drawing.Size(553, 344);
            this.pgname.TabIndex = 4;
            // 
            // header1
            // 
            this.header1.BackColor = System.Drawing.SystemColors.Control;
            this.header1.CausesValidation = false;
            this.header1.Description = "Description";
            this.header1.Dock = System.Windows.Forms.DockStyle.Top;
            this.header1.Image = ((System.Drawing.Image)(resources.GetObject("header1.Image")));
            this.header1.Location = new System.Drawing.Point(0, 0);
            this.header1.Name = "header1";
            this.header1.Size = new System.Drawing.Size(452, 64);
            this.header1.TabIndex = 0;
            this.header1.Title = "Title";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "s_host";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "s_authentization";
            // 
            // datasource
            // 
            this.datasource.Location = new System.Drawing.Point(190, 96);
            this.datasource.Name = "datasource";
            this.datasource.Size = new System.Drawing.Size(193, 21);
            this.datasource.TabIndex = 3;
            this.datasource.TextChanged += new System.EventHandler(this.datasource_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "s_login";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "s_password";
            // 
            // login
            // 
            this.login.Location = new System.Drawing.Point(205, 150);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(178, 21);
            this.login.TabIndex = 7;
            this.login.TextChanged += new System.EventHandler(this.datasource_TextChanged);
            // 
            // password
            // 
            this.password.AcceptsReturn = true;
            this.password.Location = new System.Drawing.Point(205, 177);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(178, 21);
            this.password.TabIndex = 8;
            this.password.TextChanged += new System.EventHandler(this.datasource_TextChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Windows (SSPI)",
            "SQL Server"});
            this.comboBox1.Location = new System.Drawing.Point(190, 123);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(193, 21);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(43, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "s_connection_string";
            // 
            // connectionstring
            // 
            this.connectionstring.Location = new System.Drawing.Point(190, 204);
            this.connectionstring.Name = "connectionstring";
            this.connectionstring.ReadOnly = true;
            this.connectionstring.Size = new System.Drawing.Size(193, 21);
            this.connectionstring.TabIndex = 11;
            // 
            // ConnWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 280);
            this.Controls.Add(this.wizard1);
            this.Name = "ConnWizard";
            this.Text = "ConnWizard";
            this.wizard1.ResumeLayout(false);
            this.pgcreadentials.ResumeLayout(false);
            this.pgcreadentials.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Gui.Wizard.Wizard wizard1;
        private Gui.Wizard.WizardPage pgcreadentials;
        private Gui.Wizard.WizardPage pgselectdb;
        private Gui.Wizard.WizardPage pgname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Gui.Wizard.Header header1;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox login;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox datasource;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox connectionstring;
        private System.Windows.Forms.Label label5;
    }
}