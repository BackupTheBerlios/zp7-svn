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
            this.logTabs = new System.Windows.Forms.TabControl();
            this.s_log = new System.Windows.Forms.TabPage();
            this.logListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.logTabs.SuspendLayout();
            this.s_log.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(220, 330);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.daTreeView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(212, 304);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "s_tree";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // daTreeView1
            // 
            this.daTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.daTreeView1.Location = new System.Drawing.Point(3, 3);
            this.daTreeView1.Name = "daTreeView1";
            this.daTreeView1.Root = null;
            this.daTreeView1.RootPath = null;
            this.daTreeView1.Size = new System.Drawing.Size(206, 298);
            this.daTreeView1.TabIndex = 0;
            // 
            // contentTabs
            // 
            this.contentTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentTabs.Location = new System.Drawing.Point(220, 0);
            this.contentTabs.Name = "contentTabs";
            this.contentTabs.SelectedIndex = 0;
            this.contentTabs.Size = new System.Drawing.Size(398, 330);
            this.contentTabs.TabIndex = 1;
            // 
            // logTabs
            // 
            this.logTabs.Controls.Add(this.s_log);
            this.logTabs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logTabs.Location = new System.Drawing.Point(0, 330);
            this.logTabs.Name = "logTabs";
            this.logTabs.SelectedIndex = 0;
            this.logTabs.Size = new System.Drawing.Size(618, 138);
            this.logTabs.TabIndex = 2;
            // 
            // s_log
            // 
            this.s_log.Controls.Add(this.logListView);
            this.s_log.Location = new System.Drawing.Point(4, 22);
            this.s_log.Name = "s_log";
            this.s_log.Padding = new System.Windows.Forms.Padding(3);
            this.s_log.Size = new System.Drawing.Size(610, 112);
            this.s_log.TabIndex = 0;
            this.s_log.Text = "s_log";
            this.s_log.UseVisualStyleBackColor = true;
            // 
            // logListView
            // 
            this.logListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.logListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logListView.Location = new System.Drawing.Point(3, 3);
            this.logListView.Name = "logListView";
            this.logListView.Size = new System.Drawing.Size(604, 106);
            this.logListView.TabIndex = 0;
            this.logListView.UseCompatibleStateImageBehavior = false;
            this.logListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "s_type";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "s_date";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "s_time";
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "s_message";
            this.columnHeader4.Width = 400;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 468);
            this.Controls.Add(this.contentTabs);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.logTabs);
            this.Name = "MainForm";
            this.Text = "DatAdmin 2.0";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.logTabs.ResumeLayout(false);
            this.s_log.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private DATreeView daTreeView1;
        private System.Windows.Forms.TabControl contentTabs;
        private System.Windows.Forms.TabControl logTabs;
        private System.Windows.Forms.TabPage s_log;
        private System.Windows.Forms.ListView logListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}

