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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tbfilter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbsongbook = new System.Windows.Forms.ComboBox();
            this.cbdatabase = new System.Windows.Forms.ComboBox();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tbsongbook = new System.Windows.Forms.TabPage();
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
            this.znovuNaèístDatabáziToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importPísníToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportPísníToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otevøítVDatAdminuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zpìvníkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.novýToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vlastnostiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zmìnitStylToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.konecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vytisknoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.pøidatVybranouPíseòDoZpìvníkuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nastavaníToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.obecnéToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stylyZpìvníkuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filtryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nápovìdaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.obsahToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wWWStránkaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oProgramuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveZP = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.savePDF = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.písnìToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nováPíseòToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prohlížetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upravitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smazatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tiskPísnìToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportPísnìDoPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.songView1 = new zp8.SongView();
            this.songDatabaseWrapper1 = new zp8.SongDatabaseWrapper(this.components);
            this.songTable1 = new zp8.SongTable();
            this.songsByGroupFrame1 = new zp8.SongsByGroupFrame();
            this.serversFrame1 = new zp8.ServersFrame();
            this.songBookFrame1 = new zp8.SongBookFrame();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.nováDatabázeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tbsongbook.SuspendLayout();
            this.StatusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.tbfilter);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbsongbook);
            this.panel1.Controls.Add(this.cbdatabase);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(671, 24);
            this.panel1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(214, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Zpìvník";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Databáze";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(581, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(30, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(530, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Zruš";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbfilter
            // 
            this.tbfilter.Location = new System.Drawing.Point(424, 2);
            this.tbfilter.Name = "tbfilter";
            this.tbfilter.Size = new System.Drawing.Size(100, 20);
            this.tbfilter.TabIndex = 5;
            this.tbfilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbfilter_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(397, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Filtr";
            // 
            // cbsongbook
            // 
            this.cbsongbook.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbsongbook.FormattingEnabled = true;
            this.cbsongbook.Location = new System.Drawing.Point(268, 3);
            this.cbsongbook.Name = "cbsongbook";
            this.cbsongbook.Size = new System.Drawing.Size(121, 21);
            this.cbsongbook.TabIndex = 3;
            this.cbsongbook.SelectedIndexChanged += new System.EventHandler(this.cbsongbook_SelectedIndexChanged);
            // 
            // cbdatabase
            // 
            this.cbdatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbdatabase.FormattingEnabled = true;
            this.cbdatabase.Location = new System.Drawing.Point(63, 3);
            this.cbdatabase.Name = "cbdatabase";
            this.cbdatabase.Size = new System.Drawing.Size(145, 21);
            this.cbdatabase.TabIndex = 0;
            this.cbdatabase.SelectedIndexChanged += new System.EventHandler(this.dblist_SelectedIndexChanged);
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.tabPage1);
            this.TabControl1.Controls.Add(this.tabPage3);
            this.TabControl1.Controls.Add(this.tbsongbook);
            this.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl1.ImageList = this.imageList1;
            this.TabControl1.Location = new System.Drawing.Point(0, 73);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(671, 391);
            this.TabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.songView1);
            this.tabPage1.Controls.Add(this.splitter1);
            this.tabPage1.Controls.Add(this.tabControl2);
            this.tabPage1.ImageIndex = 2;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(663, 364);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Písnì";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(420, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 358);
            this.splitter1.TabIndex = 10;
            this.splitter1.TabStop = false;
            // 
            // tabControl2
            // 
            this.tabControl2.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.DataBindings.Add(new System.Windows.Forms.Binding("Size", global::zp8.Properties.Settings.Default, "LeftPanelSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl2.ImageList = this.imageList1;
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Multiline = true;
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = global::zp8.Properties.Settings.Default.LeftPanelSize;
            this.tabControl2.TabIndex = 8;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.songTable1);
            this.tabPage2.ImageIndex = 4;
            this.tabPage2.Location = new System.Drawing.Point(23, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(390, 350);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Tabulka";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.songsByGroupFrame1);
            this.tabPage4.ImageIndex = 0;
            this.tabPage4.Location = new System.Drawing.Point(23, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(390, 350);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Skupiny";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "group.png");
            this.imageList1.Images.SetKeyName(1, "server.png");
            this.imageList1.Images.SetKeyName(2, "song.png");
            this.imageList1.Images.SetKeyName(3, "songbook.png");
            this.imageList1.Images.SetKeyName(4, "table.png");
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.serversFrame1);
            this.tabPage3.ImageIndex = 1;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(663, 364);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Servery";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tbsongbook
            // 
            this.tbsongbook.Controls.Add(this.songBookFrame1);
            this.tbsongbook.ImageIndex = 3;
            this.tbsongbook.Location = new System.Drawing.Point(4, 23);
            this.tbsongbook.Name = "tbsongbook";
            this.tbsongbook.Padding = new System.Windows.Forms.Padding(3);
            this.tbsongbook.Size = new System.Drawing.Size(663, 364);
            this.tbsongbook.TabIndex = 3;
            this.tbsongbook.Text = "Uspoøádání zpìvníku";
            this.tbsongbook.UseVisualStyleBackColor = true;
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.AutoSize = false;
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dbname,
            this.dbsize,
            this.dbstatus,
            this.tspages});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 464);
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
            this.písnìToolStripMenuItem,
            this.zpìvníkToolStripMenuItem,
            this.nastavaníToolStripMenuItem,
            this.nápovìdaToolStripMenuItem});
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
            this.souborToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.souborToolStripMenuItem.Text = "Soubor";
            // 
            // naèístToolStripMenuItem1
            // 
            this.naèístToolStripMenuItem1.Name = "naèístToolStripMenuItem1";
            this.naèístToolStripMenuItem1.ShortcutKeyDisplayString = "";
            this.naèístToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.naèístToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.naèístToolStripMenuItem1.Text = "Naèíst";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
            // 
            // konecToolStripMenuItem1
            // 
            this.konecToolStripMenuItem1.Name = "konecToolStripMenuItem1";
            this.konecToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.konecToolStripMenuItem1.Text = "Konec";
            this.konecToolStripMenuItem1.Click += new System.EventHandler(this.konecToolStripMenuItem1_Click);
            // 
            // databázeToolStripMenuItem
            // 
            this.databázeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.znovuNaèístDatabáziToolStripMenuItem,
            this.nováDatabázeToolStripMenuItem,
            this.toolStripMenuItem3,
            this.importPísníToolStripMenuItem,
            this.exportPísníToolStripMenuItem,
            this.toolStripMenuItem4,
            this.xToolStripMenuItem,
            this.otevøítVDatAdminuToolStripMenuItem});
            this.databázeToolStripMenuItem.Name = "databázeToolStripMenuItem";
            this.databázeToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.databázeToolStripMenuItem.Text = "Databáze";
            // 
            // znovuNaèístDatabáziToolStripMenuItem
            // 
            this.znovuNaèístDatabáziToolStripMenuItem.Name = "znovuNaèístDatabáziToolStripMenuItem";
            this.znovuNaèístDatabáziToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.znovuNaèístDatabáziToolStripMenuItem.Text = "Znovu naèíst databázi";
            this.znovuNaèístDatabáziToolStripMenuItem.Click += new System.EventHandler(this.znovuNaèístDatabáziToolStripMenuItem_Click);
            // 
            // importPísníToolStripMenuItem
            // 
            this.importPísníToolStripMenuItem.Name = "importPísníToolStripMenuItem";
            this.importPísníToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.importPísníToolStripMenuItem.Text = "Import písní";
            this.importPísníToolStripMenuItem.Click += new System.EventHandler(this.importPísníToolStripMenuItem_Click);
            // 
            // exportPísníToolStripMenuItem
            // 
            this.exportPísníToolStripMenuItem.Name = "exportPísníToolStripMenuItem";
            this.exportPísníToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.exportPísníToolStripMenuItem.Text = "Export písní";
            this.exportPísníToolStripMenuItem.Click += new System.EventHandler(this.exportPísníToolStripMenuItem_Click);
            // 
            // xToolStripMenuItem
            // 
            this.xToolStripMenuItem.Name = "xToolStripMenuItem";
            this.xToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.xToolStripMenuItem.Text = "Stáhnout novinky";
            // 
            // otevøítVDatAdminuToolStripMenuItem
            // 
            this.otevøítVDatAdminuToolStripMenuItem.Name = "otevøítVDatAdminuToolStripMenuItem";
            this.otevøítVDatAdminuToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.otevøítVDatAdminuToolStripMenuItem.Text = "Otevøít v DatAdminu";
            // 
            // zpìvníkToolStripMenuItem
            // 
            this.zpìvníkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.novýToolStripMenuItem,
            this.vlastnostiToolStripMenuItem,
            this.zmìnitStylToolStripMenuItem,
            this.toolStripMenuItem5,
            this.konecToolStripMenuItem,
            this.vytisknoutToolStripMenuItem,
            this.toolStripMenuItem6,
            this.pøidatVybranouPíseòDoZpìvníkuToolStripMenuItem});
            this.zpìvníkToolStripMenuItem.Name = "zpìvníkToolStripMenuItem";
            this.zpìvníkToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.zpìvníkToolStripMenuItem.Text = "Zpìvník";
            // 
            // novýToolStripMenuItem
            // 
            this.novýToolStripMenuItem.Name = "novýToolStripMenuItem";
            this.novýToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.novýToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.novýToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.novýToolStripMenuItem.Text = "Nový";
            this.novýToolStripMenuItem.Click += new System.EventHandler(this.newSongList_Click);
            // 
            // vlastnostiToolStripMenuItem
            // 
            this.vlastnostiToolStripMenuItem.Name = "vlastnostiToolStripMenuItem";
            this.vlastnostiToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.vlastnostiToolStripMenuItem.Text = "Vlastnosti";
            this.vlastnostiToolStripMenuItem.Click += new System.EventHandler(this.vlastnostiToolStripMenuItem_Click);
            // 
            // zmìnitStylToolStripMenuItem
            // 
            this.zmìnitStylToolStripMenuItem.Name = "zmìnitStylToolStripMenuItem";
            this.zmìnitStylToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.zmìnitStylToolStripMenuItem.Text = "Zmìnit styl";
            this.zmìnitStylToolStripMenuItem.Click += new System.EventHandler(this.zmìnitStylToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(272, 6);
            // 
            // konecToolStripMenuItem
            // 
            this.konecToolStripMenuItem.Name = "konecToolStripMenuItem";
            this.konecToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.konecToolStripMenuItem.Text = "Export do PDF";
            this.konecToolStripMenuItem.Click += new System.EventHandler(this.pdfExportToolStripMenuItem_Click);
            // 
            // vytisknoutToolStripMenuItem
            // 
            this.vytisknoutToolStripMenuItem.Name = "vytisknoutToolStripMenuItem";
            this.vytisknoutToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.vytisknoutToolStripMenuItem.Text = "Vytisknout";
            this.vytisknoutToolStripMenuItem.Click += new System.EventHandler(this.vytisknoutToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(272, 6);
            // 
            // pøidatVybranouPíseòDoZpìvníkuToolStripMenuItem
            // 
            this.pøidatVybranouPíseòDoZpìvníkuToolStripMenuItem.Name = "pøidatVybranouPíseòDoZpìvníkuToolStripMenuItem";
            this.pøidatVybranouPíseòDoZpìvníkuToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.pøidatVybranouPíseòDoZpìvníkuToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.pøidatVybranouPíseòDoZpìvníkuToolStripMenuItem.Text = "Pøidat vybranou píseò do zpìvníku";
            this.pøidatVybranouPíseòDoZpìvníkuToolStripMenuItem.Click += new System.EventHandler(this.pøidatVybranouPíseòDoZpìvníkuToolStripMenuItem_Click);
            // 
            // nastavaníToolStripMenuItem
            // 
            this.nastavaníToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.obecnéToolStripMenuItem,
            this.stylyZpìvníkuToolStripMenuItem,
            this.filtryToolStripMenuItem});
            this.nastavaníToolStripMenuItem.Name = "nastavaníToolStripMenuItem";
            this.nastavaníToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.nastavaníToolStripMenuItem.Text = "Nastavení";
            // 
            // obecnéToolStripMenuItem
            // 
            this.obecnéToolStripMenuItem.Name = "obecnéToolStripMenuItem";
            this.obecnéToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.obecnéToolStripMenuItem.Text = "Obecné";
            this.obecnéToolStripMenuItem.Click += new System.EventHandler(this.obecnéToolStripMenuItem_Click);
            // 
            // stylyZpìvníkuToolStripMenuItem
            // 
            this.stylyZpìvníkuToolStripMenuItem.Name = "stylyZpìvníkuToolStripMenuItem";
            this.stylyZpìvníkuToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.stylyZpìvníkuToolStripMenuItem.Text = "Styly zpìvníku";
            this.stylyZpìvníkuToolStripMenuItem.Click += new System.EventHandler(this.stylyZpìvníkuToolStripMenuItem_Click);
            // 
            // filtryToolStripMenuItem
            // 
            this.filtryToolStripMenuItem.Name = "filtryToolStripMenuItem";
            this.filtryToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.filtryToolStripMenuItem.Text = "Filtry";
            this.filtryToolStripMenuItem.Click += new System.EventHandler(this.filtryToolStripMenuItem_Click);
            // 
            // nápovìdaToolStripMenuItem
            // 
            this.nápovìdaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.obsahToolStripMenuItem,
            this.wWWStránkaToolStripMenuItem,
            this.oProgramuToolStripMenuItem});
            this.nápovìdaToolStripMenuItem.Name = "nápovìdaToolStripMenuItem";
            this.nápovìdaToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.nápovìdaToolStripMenuItem.Text = "Nápovìda";
            // 
            // obsahToolStripMenuItem
            // 
            this.obsahToolStripMenuItem.Name = "obsahToolStripMenuItem";
            this.obsahToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.obsahToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.obsahToolStripMenuItem.Text = "Obsah";
            this.obsahToolStripMenuItem.Click += new System.EventHandler(this.obsahToolStripMenuItem_Click);
            // 
            // wWWStránkaToolStripMenuItem
            // 
            this.wWWStránkaToolStripMenuItem.Name = "wWWStránkaToolStripMenuItem";
            this.wWWStránkaToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.wWWStránkaToolStripMenuItem.Text = "WWW stránka";
            this.wWWStránkaToolStripMenuItem.Click += new System.EventHandler(this.wWWStránkaToolStripMenuItem_Click);
            // 
            // oProgramuToolStripMenuItem
            // 
            this.oProgramuToolStripMenuItem.Name = "oProgramuToolStripMenuItem";
            this.oProgramuToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.oProgramuToolStripMenuItem.Text = "O programu";
            this.oProgramuToolStripMenuItem.Click += new System.EventHandler(this.oProgramuToolStripMenuItem_Click);
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
            // statusStrip2
            // 
            this.statusStrip2.Location = new System.Drawing.Point(0, 486);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(671, 22);
            this.statusStrip2.TabIndex = 4;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton3,
            this.toolStripButton9,
            this.toolStripButton2,
            this.toolStripButton8});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(671, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::zp8.StdResources._new;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(98, 22);
            this.toolStripButton1.Text = "Nový zpìvník";
            this.toolStripButton1.Click += new System.EventHandler(this.newSongList_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = global::zp8.StdResources.open;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(63, 22);
            this.toolStripButton3.Text = "Otevøít";
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.Image = global::zp8.StdResources.save;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(57, 22);
            this.toolStripButton9.Text = "Uložit";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::zp8.StdResources.print;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(79, 22);
            this.toolStripButton2.Text = "Tisk písnì";
            this.toolStripButton2.Click += new System.EventHandler(this.printSong_Click);
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.Image = global::zp8.StdResources.song;
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(86, 22);
            this.toolStripButton8.Text = "Nová píseò";
            this.toolStripButton8.Click += new System.EventHandler(this.newSong_Click);
            // 
            // písnìToolStripMenuItem
            // 
            this.písnìToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nováPíseòToolStripMenuItem,
            this.prohlížetToolStripMenuItem,
            this.upravitToolStripMenuItem,
            this.smazatToolStripMenuItem,
            this.toolStripMenuItem1,
            this.tiskPísnìToolStripMenuItem,
            this.exportPísnìDoPDFToolStripMenuItem});
            this.písnìToolStripMenuItem.Name = "písnìToolStripMenuItem";
            this.písnìToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.písnìToolStripMenuItem.Text = "Písnì";
            // 
            // nováPíseòToolStripMenuItem
            // 
            this.nováPíseòToolStripMenuItem.Name = "nováPíseòToolStripMenuItem";
            this.nováPíseòToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.nováPíseòToolStripMenuItem.Text = "Nová píseò";
            this.nováPíseòToolStripMenuItem.Click += new System.EventHandler(this.newSong_Click);
            // 
            // prohlížetToolStripMenuItem
            // 
            this.prohlížetToolStripMenuItem.Name = "prohlížetToolStripMenuItem";
            this.prohlížetToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.prohlížetToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.prohlížetToolStripMenuItem.Text = "Prohlížet";
            this.prohlížetToolStripMenuItem.Click += new System.EventHandler(this.kinoToolStripMenuItem_Click);
            // 
            // upravitToolStripMenuItem
            // 
            this.upravitToolStripMenuItem.Name = "upravitToolStripMenuItem";
            this.upravitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.upravitToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.upravitToolStripMenuItem.Text = "Upravit";
            this.upravitToolStripMenuItem.Click += new System.EventHandler(this.upravitPíseòToolStripMenuItem_Click);
            // 
            // smazatToolStripMenuItem
            // 
            this.smazatToolStripMenuItem.Name = "smazatToolStripMenuItem";
            this.smazatToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.smazatToolStripMenuItem.Text = "Smazat";
            this.smazatToolStripMenuItem.Click += new System.EventHandler(this.smazatToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(176, 6);
            // 
            // tiskPísnìToolStripMenuItem
            // 
            this.tiskPísnìToolStripMenuItem.Name = "tiskPísnìToolStripMenuItem";
            this.tiskPísnìToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.tiskPísnìToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.tiskPísnìToolStripMenuItem.Text = "Tisk písnì";
            this.tiskPísnìToolStripMenuItem.Click += new System.EventHandler(this.printSong_Click);
            // 
            // exportPísnìDoPDFToolStripMenuItem
            // 
            this.exportPísnìDoPDFToolStripMenuItem.Name = "exportPísnìDoPDFToolStripMenuItem";
            this.exportPísnìDoPDFToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.exportPísnìDoPDFToolStripMenuItem.Text = "Export písnì do PDF";
            this.exportPísnìDoPDFToolStripMenuItem.Click += new System.EventHandler(this.exportPísnìDoPDFToolStripMenuItem_Click);
            // 
            // songView1
            // 
            this.songView1.AutoScroll = true;
            this.songView1.BackColor = System.Drawing.SystemColors.Window;
            this.songView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songView1.Location = new System.Drawing.Point(430, 3);
            this.songView1.Name = "songView1";
            this.songView1.Size = new System.Drawing.Size(230, 358);
            this.songView1.SongDb = this.songDatabaseWrapper1;
            this.songView1.SongText = null;
            this.songView1.TabIndex = 11;
            // 
            // songDatabaseWrapper1
            // 
            this.songDatabaseWrapper1.Database = null;
            this.songDatabaseWrapper1.SearchText = null;
            this.songDatabaseWrapper1.SongID = 0;
            // 
            // songTable1
            // 
            this.songTable1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songTable1.Location = new System.Drawing.Point(3, 3);
            this.songTable1.Name = "songTable1";
            this.songTable1.Size = new System.Drawing.Size(384, 344);
            this.songTable1.SongDb = this.songDatabaseWrapper1;
            this.songTable1.TabIndex = 6;
            // 
            // songsByGroupFrame1
            // 
            this.songsByGroupFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songsByGroupFrame1.Location = new System.Drawing.Point(3, 3);
            this.songsByGroupFrame1.Name = "songsByGroupFrame1";
            this.songsByGroupFrame1.Size = new System.Drawing.Size(384, 344);
            this.songsByGroupFrame1.SongDb = this.songDatabaseWrapper1;
            this.songsByGroupFrame1.TabIndex = 0;
            // 
            // serversFrame1
            // 
            this.serversFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serversFrame1.Location = new System.Drawing.Point(3, 3);
            this.serversFrame1.Name = "serversFrame1";
            this.serversFrame1.Size = new System.Drawing.Size(657, 358);
            this.serversFrame1.SongDb = this.songDatabaseWrapper1;
            this.serversFrame1.TabIndex = 0;
            // 
            // songBookFrame1
            // 
            this.songBookFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songBookFrame1.Location = new System.Drawing.Point(3, 3);
            this.songBookFrame1.Name = "songBookFrame1";
            this.songBookFrame1.Size = new System.Drawing.Size(657, 358);
            this.songBookFrame1.SongBook = null;
            this.songBookFrame1.TabIndex = 0;
            this.songBookFrame1.ChangedPageInfo += new System.EventHandler(this.songBookFrame1_ChangedPageInfo);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(186, 6);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(186, 6);
            // 
            // nováDatabázeToolStripMenuItem
            // 
            this.nováDatabázeToolStripMenuItem.Name = "nováDatabázeToolStripMenuItem";
            this.nováDatabázeToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.nováDatabázeToolStripMenuItem.Text = "Nová databáze";
            this.nováDatabázeToolStripMenuItem.Click += new System.EventHandler(this.mnuNewDatabase_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 508);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.StatusStrip1);
            this.Controls.Add(this.statusStrip2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Zpìvníkátor 8.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tbsongbook.ResumeLayout(false);
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn textDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupDataGridViewTextBoxColumn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbdatabase;
        private System.Windows.Forms.TabControl TabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem zpìvníkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem novýToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem konecToolStripMenuItem;
        private System.Windows.Forms.StatusStrip StatusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel dbname;
        private System.Windows.Forms.ToolStripStatusLabel dbsize;
        private System.Windows.Forms.ToolStripStatusLabel dbstatus;
        private System.Windows.Forms.TabPage tabPage3;
        private ServersFrame serversFrame1;
        private System.Windows.Forms.ComboBox cbsongbook;
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
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem stylyZpìvníkuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zmìnitStylToolStripMenuItem;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.SaveFileDialog savePDF;
        private System.Windows.Forms.ToolStripMenuItem vytisknoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel tspages;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage4;
        private SongView songView1;
        private System.Windows.Forms.Splitter splitter1;
        private SongTable songTable1;
        private zp8.SongsByGroupFrame songsByGroupFrame1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbfilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem pøidatVybranouPíseòDoZpìvníkuToolStripMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem filtryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem nápovìdaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wWWStránkaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oProgramuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem obsahToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private SongDatabaseWrapper songDatabaseWrapper1;
        private System.Windows.Forms.ToolStripMenuItem databázeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem znovuNaèístDatabáziToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importPísníToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportPísníToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otevøítVDatAdminuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem písnìToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nováPíseòToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prohlížetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem upravitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smazatToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tiskPísnìToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportPísnìDoPDFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nováDatabázeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
    }
}

