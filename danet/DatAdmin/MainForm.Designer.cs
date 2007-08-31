namespace DatAdmin
{
    partial class MainForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.daTreeView1 = new DatAdmin.DATreeView();
            this.contentTabs = new System.Windows.Forms.TabControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(220, 414);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.daTreeView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(212, 388);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tree";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // daTreeView1
            // 
            this.daTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.daTreeView1.Location = new System.Drawing.Point(3, 3);
            this.daTreeView1.Name = "daTreeView1";
            this.daTreeView1.Root = null;
            this.daTreeView1.RootPath = null;
            this.daTreeView1.Size = new System.Drawing.Size(206, 382);
            this.daTreeView1.TabIndex = 0;
            // 
            // contentTabs
            // 
            this.contentTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentTabs.Location = new System.Drawing.Point(220, 0);
            this.contentTabs.Name = "contentTabs";
            this.contentTabs.SelectedIndex = 0;
            this.contentTabs.Size = new System.Drawing.Size(372, 414);
            this.contentTabs.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 414);
            this.Controls.Add(this.contentTabs);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "DatAdmin 2.0";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private DATreeView daTreeView1;
        private System.Windows.Forms.TabControl contentTabs;
    }
}

