namespace zp8
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.dblist = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.songView1 = new zp8.SongView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.songTable1 = new zp8.SongTable();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.dbname = new System.Windows.Forms.ToolStripStatusLabel();
            this.dbsize = new System.Windows.Forms.ToolStripStatusLabel();
            this.dbstatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.zpìvníkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.novýToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.naèístToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.konecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databázeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewDb = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zeStaréhoZpìvníkátoruToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveDb = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.StatusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dblist);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(672, 24);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Databáze";
            // 
            // dblist
            // 
            this.dblist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dblist.FormattingEnabled = true;
            this.dblist.Location = new System.Drawing.Point(71, 3);
            this.dblist.Name = "dblist";
            this.dblist.Size = new System.Drawing.Size(145, 21);
            this.dblist.TabIndex = 0;
            this.dblist.SelectedIndexChanged += new System.EventHandler(this.dblist_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 48);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(672, 431);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.songView1);
            this.tabPage1.Controls.Add(this.splitter1);
            this.tabPage1.Controls.Add(this.songTable1);
            this.tabPage1.Controls.Add(this.StatusStrip1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(664, 405);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Písnì";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // songView1
            // 
            this.songView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songView1.Location = new System.Drawing.Point(449, 3);
            this.songView1.Name = "songView1";
            this.songView1.Size = new System.Drawing.Size(212, 377);
            this.songView1.SongSource = this.songTable1;
            this.songView1.TabIndex = 7;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(439, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 377);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // songTable1
            // 
            this.songTable1.Dock = System.Windows.Forms.DockStyle.Left;
            this.songTable1.Location = new System.Drawing.Point(3, 3);
            this.songTable1.Name = "songTable1";
            this.songTable1.Size = new System.Drawing.Size(436, 377);
            this.songTable1.TabIndex = 5;
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.AutoSize = false;
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dbname,
            this.dbsize,
            this.dbstatus});
            this.StatusStrip1.Location = new System.Drawing.Point(3, 380);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(658, 22);
            this.StatusStrip1.TabIndex = 1;
            this.StatusStrip1.Text = "statusStrip1";
            // 
            // dbname
            // 
            this.dbname.AutoSize = false;
            this.dbname.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.dbname.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.dbname.Name = "dbname";
            this.dbname.Size = new System.Drawing.Size(150, 17);
            // 
            // dbsize
            // 
            this.dbsize.AutoSize = false;
            this.dbsize.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.dbsize.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.dbsize.Name = "dbsize";
            this.dbsize.Size = new System.Drawing.Size(100, 17);
            // 
            // dbstatus
            // 
            this.dbstatus.AutoSize = false;
            this.dbstatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.dbstatus.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.dbstatus.Name = "dbstatus";
            this.dbstatus.Size = new System.Drawing.Size(100, 17);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(664, 405);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Skupiny";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zpìvníkToolStripMenuItem,
            this.databázeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(672, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // zpìvníkToolStripMenuItem
            // 
            this.zpìvníkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.novýToolStripMenuItem,
            this.naèístToolStripMenuItem,
            this.toolStripMenuItem1,
            this.konecToolStripMenuItem});
            this.zpìvníkToolStripMenuItem.Name = "zpìvníkToolStripMenuItem";
            this.zpìvníkToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.zpìvníkToolStripMenuItem.Text = "Zpìvník";
            // 
            // novýToolStripMenuItem
            // 
            this.novýToolStripMenuItem.Name = "novýToolStripMenuItem";
            this.novýToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.novýToolStripMenuItem.Text = "Nový";
            // 
            // naèístToolStripMenuItem
            // 
            this.naèístToolStripMenuItem.Name = "naèístToolStripMenuItem";
            this.naèístToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.naèístToolStripMenuItem.Text = "Naèíst";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(111, 6);
            // 
            // konecToolStripMenuItem
            // 
            this.konecToolStripMenuItem.Name = "konecToolStripMenuItem";
            this.konecToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.konecToolStripMenuItem.Text = "Konec";
            // 
            // databázeToolStripMenuItem
            // 
            this.databázeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewDb,
            this.importToolStripMenuItem,
            this.mnuSaveDb});
            this.databázeToolStripMenuItem.Name = "databázeToolStripMenuItem";
            this.databázeToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.databázeToolStripMenuItem.Text = "Databáze";
            // 
            // mnuNewDb
            // 
            this.mnuNewDb.Name = "mnuNewDb";
            this.mnuNewDb.Size = new System.Drawing.Size(141, 22);
            this.mnuNewDb.Text = "Nová";
            this.mnuNewDb.Click += new System.EventHandler(this.mnuNewDb_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zeStaréhoZpìvníkátoruToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.importToolStripMenuItem.Text = "Import písní";
            // 
            // zeStaréhoZpìvníkátoruToolStripMenuItem
            // 
            this.zeStaréhoZpìvníkátoruToolStripMenuItem.Name = "zeStaréhoZpìvníkátoruToolStripMenuItem";
            this.zeStaréhoZpìvníkátoruToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.zeStaréhoZpìvníkátoruToolStripMenuItem.Text = "Ze starého Zpìvníkátoru";
            this.zeStaréhoZpìvníkátoruToolStripMenuItem.Click += new System.EventHandler(this.zeStaréhoZpìvníkátoruToolStripMenuItem_Click);
            // 
            // mnuSaveDb
            // 
            this.mnuSaveDb.Name = "mnuSaveDb";
            this.mnuSaveDb.Size = new System.Drawing.Size(141, 22);
            this.mnuSaveDb.Text = "Uložit";
            this.mnuSaveDb.Click += new System.EventHandler(this.mnuSaveDb_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "XML soubory|*.xml";
            this.openFileDialog1.Multiselect = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 479);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Zpìvníkátor 8.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn textDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupDataGridViewTextBoxColumn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox dblist;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem zpìvníkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem novýToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem naèístToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem konecToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databázeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuNewDb;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zeStaréhoZpìvníkátoruToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveDb;
        private System.Windows.Forms.StatusStrip StatusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel dbname;
        private System.Windows.Forms.ToolStripStatusLabel dbsize;
        private System.Windows.Forms.ToolStripStatusLabel dbstatus;
        private SongView songView1;
        private System.Windows.Forms.Splitter splitter1;
        private SongTable songTable1;
    }
}

