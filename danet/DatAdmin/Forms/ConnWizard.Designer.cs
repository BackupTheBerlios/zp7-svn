namespace DatAdmin
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
            this.wpprovider = new Gui.Wizard.WizardPage();
            this.provider = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.header1 = new Gui.Wizard.Header();
            this.wpfinish = new Gui.Wizard.WizardPage();
            this.infoContainer1 = new Gui.Wizard.InfoContainer();
            this.wpselectdb = new Gui.Wizard.WizardPage();
            this.header3 = new Gui.Wizard.Header();
            this.wpconnprops = new Gui.Wizard.WizardPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.header2 = new Gui.Wizard.Header();
            this.wizard1.SuspendLayout();
            this.wpprovider.SuspendLayout();
            this.wpfinish.SuspendLayout();
            this.wpselectdb.SuspendLayout();
            this.wpconnprops.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizard1
            // 
            this.wizard1.Controls.Add(this.wpprovider);
            this.wizard1.Controls.Add(this.wpconnprops);
            this.wizard1.Controls.Add(this.wpfinish);
            this.wizard1.Controls.Add(this.wpselectdb);
            this.wizard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizard1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizard1.Location = new System.Drawing.Point(0, 0);
            this.wizard1.Name = "wizard1";
            this.wizard1.Pages.AddRange(new Gui.Wizard.WizardPage[] {
            this.wpprovider,
            this.wpconnprops,
            this.wpselectdb,
            this.wpfinish});
            this.wizard1.Size = new System.Drawing.Size(587, 396);
            this.wizard1.TabIndex = 0;
            this.wizard1.Load += new System.EventHandler(this.wizard1_Load);
            // 
            // wpprovider
            // 
            this.wpprovider.Controls.Add(this.provider);
            this.wpprovider.Controls.Add(this.label1);
            this.wpprovider.Controls.Add(this.header1);
            this.wpprovider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpprovider.IsFinishPage = false;
            this.wpprovider.Location = new System.Drawing.Point(0, 0);
            this.wpprovider.Name = "wpprovider";
            this.wpprovider.Size = new System.Drawing.Size(587, 348);
            this.wpprovider.TabIndex = 1;
            this.wpprovider.CloseFromNext += new Gui.Wizard.PageEventHandler(this.wpprovider_CloseFromNext);
            // 
            // provider
            // 
            this.provider.FormattingEnabled = true;
            this.provider.Location = new System.Drawing.Point(176, 106);
            this.provider.Name = "provider";
            this.provider.Size = new System.Drawing.Size(322, 21);
            this.provider.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "s_select_provider";
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
            this.header1.Size = new System.Drawing.Size(587, 64);
            this.header1.TabIndex = 0;
            this.header1.Title = "Title";
            // 
            // wpfinish
            // 
            this.wpfinish.Controls.Add(this.infoContainer1);
            this.wpfinish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpfinish.IsFinishPage = true;
            this.wpfinish.Location = new System.Drawing.Point(0, 0);
            this.wpfinish.Name = "wpfinish";
            this.wpfinish.Size = new System.Drawing.Size(587, 348);
            this.wpfinish.TabIndex = 4;
            this.wpfinish.CloseFromNext += new Gui.Wizard.PageEventHandler(this.wpfinish_CloseFromNext);
            // 
            // infoContainer1
            // 
            this.infoContainer1.BackColor = System.Drawing.Color.White;
            this.infoContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoContainer1.Image = ((System.Drawing.Image)(resources.GetObject("infoContainer1.Image")));
            this.infoContainer1.Location = new System.Drawing.Point(0, 0);
            this.infoContainer1.Name = "infoContainer1";
            this.infoContainer1.PageTitle = "s_finished";
            this.infoContainer1.Size = new System.Drawing.Size(587, 348);
            this.infoContainer1.TabIndex = 0;
            // 
            // wpselectdb
            // 
            this.wpselectdb.Controls.Add(this.header3);
            this.wpselectdb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpselectdb.IsFinishPage = false;
            this.wpselectdb.Location = new System.Drawing.Point(0, 0);
            this.wpselectdb.Name = "wpselectdb";
            this.wpselectdb.Size = new System.Drawing.Size(587, 348);
            this.wpselectdb.TabIndex = 3;
            // 
            // header3
            // 
            this.header3.BackColor = System.Drawing.SystemColors.Control;
            this.header3.CausesValidation = false;
            this.header3.Description = "Description";
            this.header3.Dock = System.Windows.Forms.DockStyle.Top;
            this.header3.Image = ((System.Drawing.Image)(resources.GetObject("header3.Image")));
            this.header3.Location = new System.Drawing.Point(0, 0);
            this.header3.Name = "header3";
            this.header3.Size = new System.Drawing.Size(587, 64);
            this.header3.TabIndex = 0;
            this.header3.Title = "Title";
            // 
            // wpconnprops
            // 
            this.wpconnprops.Controls.Add(this.propertyGrid1);
            this.wpconnprops.Controls.Add(this.header2);
            this.wpconnprops.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpconnprops.IsFinishPage = false;
            this.wpconnprops.Location = new System.Drawing.Point(0, 0);
            this.wpconnprops.Name = "wpconnprops";
            this.wpconnprops.Size = new System.Drawing.Size(587, 348);
            this.wpconnprops.TabIndex = 2;
            this.wpconnprops.CloseFromNext += new Gui.Wizard.PageEventHandler(this.wpconnprops_CloseFromNext);
            this.wpconnprops.ShowFromNext += new System.EventHandler(this.wpconnprops_ShowFromNext);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(3, 70);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(581, 275);
            this.propertyGrid1.TabIndex = 1;
            // 
            // header2
            // 
            this.header2.BackColor = System.Drawing.SystemColors.Control;
            this.header2.CausesValidation = false;
            this.header2.Description = "Description";
            this.header2.Dock = System.Windows.Forms.DockStyle.Top;
            this.header2.Image = ((System.Drawing.Image)(resources.GetObject("header2.Image")));
            this.header2.Location = new System.Drawing.Point(0, 0);
            this.header2.Name = "header2";
            this.header2.Size = new System.Drawing.Size(587, 64);
            this.header2.TabIndex = 0;
            this.header2.Title = "Title";
            // 
            // ConnWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 396);
            this.Controls.Add(this.wizard1);
            this.Name = "ConnWizard";
            this.Text = "ConnWizard";
            this.wizard1.ResumeLayout(false);
            this.wpprovider.ResumeLayout(false);
            this.wpprovider.PerformLayout();
            this.wpfinish.ResumeLayout(false);
            this.wpselectdb.ResumeLayout(false);
            this.wpconnprops.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Gui.Wizard.Wizard wizard1;
        private Gui.Wizard.WizardPage wpfinish;
        private Gui.Wizard.WizardPage wpselectdb;
        private Gui.Wizard.WizardPage wpconnprops;
        private Gui.Wizard.WizardPage wpprovider;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private Gui.Wizard.Header header2;
        private System.Windows.Forms.ComboBox provider;
        private System.Windows.Forms.Label label1;
        private Gui.Wizard.Header header1;
        private Gui.Wizard.Header header3;
        private Gui.Wizard.InfoContainer infoContainer1;
    }
}