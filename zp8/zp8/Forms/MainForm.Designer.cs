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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbsongbook = new System.Windows.Forms.ComboBox();
            this.rbsongbook = new System.Windows.Forms.RadioButton();
            this.rbdatabase = new System.Windows.Forms.RadioButton();
            this.cbdatabase = new System.Windows.Forms.ComboBox();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.songView1 = new zp8.SongView();
            this.songDatabaseWrapper1 = new zp8.SongDatabaseWrapper(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.songTable1 = new zp8.SongTable();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pøidatDoZpìvníkuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.serversFrame1 = new zp8.ServersFrame();
            this.tbsongbook = new System.Windows.Forms.TabPage();
            this.songBookFrame1 = new zp8.SongBookFrame();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.dbname = new System.Windows.Forms.ToolStripStatusLabel();
            this.dbsize = new System.Windows.Forms.ToolStripStatusLabel();
            this.dbstatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tspages = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.souborToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.naèístToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.konecToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.databázeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewDb = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveDb = new System.Windows.Forms.ToolStripMenuItem();
            this.zpìvníkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.novýToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uložitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uložitNaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vlastnostiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.konecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zmìnitStylToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vytisknoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.písnìToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importPísníToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tisknoutAktuálníPíseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportPísnìDoPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nastavaníToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.obecnéToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stylyZpìvníkuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveZP = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.savePDF = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tbsongbook.SuspendLayout();
            this.StatusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbsongbook);
            this.panel1.Controls.Add(this.rbsongbook);
            this.panel1.Controls.Add(this.rbdatabase);
            this.panel1.Controls.Add(this.cbdatabase);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(671, 24);
            this.panel1.TabIndex = 1;
            // 
            // cbsongbook
            // 
            this.cbsongbook.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbsongbook.FormattingEnabled = true;
            this.cbsongbook.Location = new System.Drawing.Point(303, 4);
            this.cbsongbook.Name = "cbsongbook";
            this.cbsongbook.Size = new System.Drawing.Size(121, 21);
            this.cbsongbook.TabIndex = 3;
            this.cbsongbook.SelectedIndexChanged += new System.EventHandler(this.cbsongbook_SelectedIndexChanged);
            // 
            // rbsongbook
            // 
            this.rbsongbook.AutoSize = true;
            this.rbsongbook.Location = new System.Drawing.Point(231, 4);
            this.rbsongbook.Name = "rbsongbook";
            this.rbsongbook.Size = new System.Drawing.Size(66, 17);
            this.rbsongbook.TabIndex = 2;
            this.rbsongbook.TabStop = true;
            this.rbsongbook.Text = "Zpìvník";
            this.rbsongbook.UseVisualStyleBackColor = true;
            this.rbsongbook.CheckedChanged += new System.EventHandler(this.rbsongbook_CheckedChanged);
            // 
            // rbdatabase
            // 
            this.rbdatabase.AutoSize = true;
            this.rbdatabase.Location = new System.Drawing.Point(3, 3);
            this.rbdatabase.Name = "rbdatabase";
            this.rbdatabase.Size = new System.Drawing.Size(71, 17);
            this.rbdatabase.TabIndex = 1;
            this.rbdatabase.TabStop = true;
            this.rbdatabase.Text = "Databáze";
            this.rbdatabase.UseVisualStyleBackColor = true;
            // 
            // cbdatabase
            // 
            this.cbdatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbdatabase.FormattingEnabled = true;
            this.cbdatabase.Location = new System.Drawing.Point(80, 3);
            this.cbdatabase.Name = "cbdatabase";
            this.cbdatabase.Size = new System.Drawing.Size(145, 21);
            this.cbdatabase.TabIndex = 0;
            this.cbdatabase.SelectedIndexChanged += new System.EventHandler(this.dblist_SelectedIndexChanged);
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.tabPage1);
            this.TabControl1.Controls.Add(this.tabPage2);
            this.TabControl1.Controls.Add(this.tabPage3);
            this.TabControl1.Controls.Add(this.tbsongbook);
            this.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl1.Location = new System.Drawing.Point(0, 48);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(671, 438);
            this.TabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.songView1);
            this.tabPage1.Controls.Add(this.splitter1);
            this.tabPage1.Controls.Add(this.songTable1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(663, 412);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Písnì";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // songView1
            // 
            this.songView1.AutoScroll = true;
            this.songView1.BackColor = System.Drawing.SystemColors.Window;
            this.songView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songView1.Location = new System.Drawing.Point(449, 3);
            this.songView1.Name = "songView1";
            this.songView1.Size = new System.Drawing.Size(211, 406);
            this.songView1.SongDb = this.songDatabaseWrapper1;
            this.songView1.TabIndex = 7;
            // 
            // songDatabaseWrapper1
            // 
            this.songDatabaseWrapper1.Database = null;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(439, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 406);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // songTable1
            // 
            this.songTable1.ContextMenuStrip = this.contextMenuStrip1;
            this.songTable1.Dock = System.Windows.Forms.DockStyle.Left;
            this.songTable1.Location = new System.Drawing.Point(3, 3);
            this.songTable1.Name = "songTable1";
            this.songTable1.Size = new System.Drawing.Size(436, 406);
            this.songTable1.SongDb = this.songDatabaseWrapper1;
            this.songTable1.TabIndex = 5;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pøidatDoZpìvníkuToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(174, 26);
            // 
            // pøidatDoZpìvníkuToolStripMenuItem
            // 
            this.pøidatDoZpìvníkuToolStripMenuItem.Name = "pøidatDoZpìvníkuToolStripMenuItem";
            this.pøidatDoZpìvníkuToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.pøidatDoZpìvníkuToolStripMenuItem.Text = "Pøidat do zpìvníku";
            this.pøidatDoZpìvníkuToolStripMenuItem.Click += new System.EventHandler(this.pøidatDoZpìvníkuToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(663, 412);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Skupiny";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.serversFrame1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(663, 412);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Servery";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // serversFrame1
            // 
            this.serversFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serversFrame1.Location = new System.Drawing.Point(3, 3);
            this.serversFrame1.Name = "serversFrame1";
            this.serversFrame1.Size = new System.Drawing.Size(657, 406);
            this.serversFrame1.SongDb = this.songDatabaseWrapper1;
            this.serversFrame1.TabIndex = 0;
            // 
            // tbsongbook
            // 
            this.tbsongbook.Controls.Add(this.songBookFrame1);
            this.tbsongbook.Location = new System.Drawing.Point(4, 22);
            this.tbsongbook.Name = "tbsongbook";
            this.tbsongbook.Padding = new System.Windows.Forms.Padding(3);
            this.tbsongbook.Size = new System.Drawing.Size(663, 412);
            this.tbsongbook.TabIndex = 3;
            this.tbsongbook.Text = "Uspoøádání zpìvníku";
            this.tbsongbook.UseVisualStyleBackColor = true;
            // 
            // songBookFrame1
            // 
            this.songBookFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songBookFrame1.Location = new System.Drawing.Point(3, 3);
            this.songBookFrame1.Name = "songBookFrame1";
            this.songBookFrame1.Size = new System.Drawing.Size(657, 406);
            this.songBookFrame1.SongBook = null;
            this.songBookFrame1.TabIndex = 0;
            this.songBookFrame1.ChangedPageInfo += new System.EventHandler(this.songBookFrame1_ChangedPageInfo);
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.AutoSize = false;
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dbname,
            this.dbsize,
            this.dbstatus,
            this.tspages});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 486);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(671, 22);
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
            // tspages
            // 
            this.tspages.AutoSize = false;
            this.tspages.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tspages.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.tspages.Name = "tspages";
            this.tspages.Size = new System.Drawing.Size(109, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.souborToolStripMenuItem,
            this.databázeToolStripMenuItem,
            this.zpìvníkToolStripMenuItem,
            this.písnìToolStripMenuItem,
            this.nastavaníToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(671, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // souborToolStripMenuItem
            // 
            this.souborToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.naèístToolStripMenuItem1,
            this.toolStripMenuItem2,
            this.konecToolStripMenuItem1});
            this.souborToolStripMenuItem.Name = "souborToolStripMenuItem";
            this.souborToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.souborToolStripMenuItem.Text = "Soubor";
            // 
            // naèístToolStripMenuItem1
            // 
            this.naèístToolStripMenuItem1.Name = "naèístToolStripMenuItem1";
            this.naèístToolStripMenuItem1.ShortcutKeyDisplayString = "";
            this.naèístToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.naèístToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.naèístToolStripMenuItem1.Text = "Naèíst";
            this.naèístToolStripMenuItem1.Click += new System.EventHandler(this.naèístToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(151, 6);
            // 
            // konecToolStripMenuItem1
            // 
            this.konecToolStripMenuItem1.Name = "konecToolStripMenuItem1";
            this.konecToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.konecToolStripMenuItem1.Text = "Konec";
            this.konecToolStripMenuItem1.Click += new System.EventHandler(this.konecToolStripMenuItem1_Click);
            // 
            // databázeToolStripMenuItem
            // 
            this.databázeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewDb,
            this.mnuSaveDb});
            this.databázeToolStripMenuItem.Name = "databázeToolStripMenuItem";
            this.databázeToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.databázeToolStripMenuItem.Text = "Databáze";
            // 
            // mnuNewDb
            // 
            this.mnuNewDb.Name = "mnuNewDb";
            this.mnuNewDb.Size = new System.Drawing.Size(111, 22);
            this.mnuNewDb.Text = "Nová";
            this.mnuNewDb.Click += new System.EventHandler(this.mnuNewDb_Click);
            // 
            // mnuSaveDb
            // 
            this.mnuSaveDb.Name = "mnuSaveDb";
            this.mnuSaveDb.Size = new System.Drawing.Size(111, 22);
            this.mnuSaveDb.Text = "Uložit";
            this.mnuSaveDb.Click += new System.EventHandler(this.mnuSaveDb_Click);
            // 
            // zpìvníkToolStripMenuItem
            // 
            this.zpìvníkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.novýToolStripMenuItem,
            this.uložitToolStripMenuItem,
            this.uložitNaToolStripMenuItem,
            this.vlastnostiToolStripMenuItem,
            this.konecToolStripMenuItem,
            this.zmìnitStylToolStripMenuItem,
            this.vytisknoutToolStripMenuItem});
            this.zpìvníkToolStripMenuItem.Name = "zpìvníkToolStripMenuItem";
            this.zpìvníkToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.zpìvníkToolStripMenuItem.Text = "Zpìvník";
            // 
            // novýToolStripMenuItem
            // 
            this.novýToolStripMenuItem.Name = "novýToolStripMenuItem";
            this.novýToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.novýToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.novýToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.novýToolStripMenuItem.Text = "Nový";
            this.novýToolStripMenuItem.Click += new System.EventHandler(this.novýToolStripMenuItem_Click);
            // 
            // uložitToolStripMenuItem
            // 
            this.uložitToolStripMenuItem.Name = "uložitToolStripMenuItem";
            this.uložitToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.uložitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.uložitToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.uložitToolStripMenuItem.Text = "Uložit";
            this.uložitToolStripMenuItem.Click += new System.EventHandler(this.uložitToolStripMenuItem_Click);
            // 
            // uložitNaToolStripMenuItem
            // 
            this.uložitNaToolStripMenuItem.Name = "uložitNaToolStripMenuItem";
            this.uložitNaToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.uložitNaToolStripMenuItem.Text = "Uložit na";
            this.uložitNaToolStripMenuItem.Click += new System.EventHandler(this.uložitNaToolStripMenuItem_Click);
            // 
            // vlastnostiToolStripMenuItem
            // 
            this.vlastnostiToolStripMenuItem.Name = "vlastnostiToolStripMenuItem";
            this.vlastnostiToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.vlastnostiToolStripMenuItem.Text = "Vlastnosti";
            this.vlastnostiToolStripMenuItem.Click += new System.EventHandler(this.vlastnostiToolStripMenuItem_Click);
            // 
            // konecToolStripMenuItem
            // 
            this.konecToolStripMenuItem.Name = "konecToolStripMenuItem";
            this.konecToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.konecToolStripMenuItem.Text = "Export do PDF";
            this.konecToolStripMenuItem.Click += new System.EventHandler(this.pdfExportToolStripMenuItem_Click);
            // 
            // zmìnitStylToolStripMenuItem
            // 
            this.zmìnitStylToolStripMenuItem.Name = "zmìnitStylToolStripMenuItem";
            this.zmìnitStylToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.zmìnitStylToolStripMenuItem.Text = "Zmìnit styl";
            this.zmìnitStylToolStripMenuItem.Click += new System.EventHandler(this.zmìnitStylToolStripMenuItem_Click);
            // 
            // vytisknoutToolStripMenuItem
            // 
            this.vytisknoutToolStripMenuItem.Name = "vytisknoutToolStripMenuItem";
            this.vytisknoutToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.vytisknoutToolStripMenuItem.Text = "Vytisknout";
            this.vytisknoutToolStripMenuItem.Click += new System.EventHandler(this.vytisknoutToolStripMenuItem_Click);
            // 
            // písnìToolStripMenuItem
            // 
            this.písnìToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importPísníToolStripMenuItem,
            this.tisknoutAktuálníPíseToolStripMenuItem,
            this.exportPísnìDoPDFToolStripMenuItem});
            this.písnìToolStripMenuItem.Name = "písnìToolStripMenuItem";
            this.písnìToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.písnìToolStripMenuItem.Text = "Písnì";
            // 
            // importPísníToolStripMenuItem
            // 
            this.importPísníToolStripMenuItem.Name = "importPísníToolStripMenuItem";
            this.importPísníToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.importPísníToolStripMenuItem.Text = "Import písní";
            this.importPísníToolStripMenuItem.Click += new System.EventHandler(this.importPísníToolStripMenuItem_Click);
            // 
            // tisknoutAktuálníPíseToolStripMenuItem
            // 
            this.tisknoutAktuálníPíseToolStripMenuItem.Name = "tisknoutAktuálníPíseToolStripMenuItem";
            this.tisknoutAktuálníPíseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.tisknoutAktuálníPíseToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.tisknoutAktuálníPíseToolStripMenuItem.Text = "Tisk písnì";
            this.tisknoutAktuálníPíseToolStripMenuItem.Click += new System.EventHandler(this.tisknoutAktuálníPíseToolStripMenuItem_Click);
            // 
            // exportPísnìDoPDFToolStripMenuItem
            // 
            this.exportPísnìDoPDFToolStripMenuItem.Name = "exportPísnìDoPDFToolStripMenuItem";
            this.exportPísnìDoPDFToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.exportPísnìDoPDFToolStripMenuItem.Text = "Export písnì do PDF";
            this.exportPísnìDoPDFToolStripMenuItem.Click += new System.EventHandler(this.exportPísnìDoPDFToolStripMenuItem_Click);
            // 
            // nastavaníToolStripMenuItem
            // 
            this.nastavaníToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.obecnéToolStripMenuItem,
            this.stylyZpìvníkuToolStripMenuItem});
            this.nastavaníToolStripMenuItem.Name = "nastavaníToolStripMenuItem";
            this.nastavaníToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.nastavaníToolStripMenuItem.Text = "Nastavení";
            // 
            // obecnéToolStripMenuItem
            // 
            this.obecnéToolStripMenuItem.Name = "obecnéToolStripMenuItem";
            this.obecnéToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.obecnéToolStripMenuItem.Text = "Obecné";
            this.obecnéToolStripMenuItem.Click += new System.EventHandler(this.obecnéToolStripMenuItem_Click);
            // 
            // stylyZpìvníkuToolStripMenuItem
            // 
            this.stylyZpìvníkuToolStripMenuItem.Name = "stylyZpìvníkuToolStripMenuItem";
            this.stylyZpìvníkuToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.stylyZpìvníkuToolStripMenuItem.Text = "Styly zpìvníku";
            this.stylyZpìvníkuToolStripMenuItem.Click += new System.EventHandler(this.stylyZpìvníkuToolStripMenuItem_Click);
            // 
            // saveZP
            // 
            this.saveZP.Filter = "Zpìvníky (*.zp)|*.zp";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Zpìvníky (*.zp)|*.zp";
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // savePDF
            // 
            this.savePDF.Filter = "PDF soubory (*.pdf)|*.pdf";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 508);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.StatusStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Zpìvníkátor 8.0";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tbsongbook.ResumeLayout(false);
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
        private System.Windows.Forms.ComboBox cbdatabase;
        private System.Windows.Forms.TabControl TabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem zpìvníkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem novýToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem konecToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databázeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuNewDb;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveDb;
        private System.Windows.Forms.StatusStrip StatusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel dbname;
        private System.Windows.Forms.ToolStripStatusLabel dbsize;
        private System.Windows.Forms.ToolStripStatusLabel dbstatus;
        private SongView songView1;
        private System.Windows.Forms.Splitter splitter1;
        private SongTable songTable1;
        private System.Windows.Forms.TabPage tabPage3;
        private ServersFrame serversFrame1;
        private SongDatabaseWrapper songDatabaseWrapper1;
        private System.Windows.Forms.RadioButton rbdatabase;
        private System.Windows.Forms.RadioButton rbsongbook;
        private System.Windows.Forms.ComboBox cbsongbook;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem pøidatDoZpìvníkuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uložitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uložitNaToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveZP;
        private System.Windows.Forms.ToolStripMenuItem nastavaníToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem obecnéToolStripMenuItem;
        private System.Windows.Forms.TabPage tbsongbook;
        private SongBookFrame songBookFrame1;
        private System.Windows.Forms.ToolStripMenuItem vlastnostiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem souborToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem naèístToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem konecToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem písnìToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importPísníToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem stylyZpìvníkuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zmìnitStylToolStripMenuItem;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.ToolStripMenuItem tisknoutAktuálníPíseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportPísnìDoPDFToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog savePDF;
        private System.Windows.Forms.ToolStripMenuItem vytisknoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel tspages;
    }
}

