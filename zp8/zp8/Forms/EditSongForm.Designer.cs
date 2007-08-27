namespace zp8
{
    partial class EditSongForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.p�se�ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ulo�itToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.konecToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.zav��tToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transpoziceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nahoruToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nahoruToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dol�OKvintuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nahoruOKvintuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbuseserver = new System.Windows.Forms.CheckBox();
            this.cbserver = new System.Windows.Forms.ComboBox();
            this.serverBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.songDb = new zp8.SongDb();
            this.label4 = new System.Windows.Forms.Label();
            this.tbgroup = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbauthor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbtitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbtext = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.songView1 = new zp8.SongView();
            this.label5 = new System.Windows.Forms.Label();
            this.tbremark = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.songDb)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.p�se�ToolStripMenuItem,
            this.transpoziceToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(628, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // p�se�ToolStripMenuItem
            // 
            this.p�se�ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ulo�itToolStripMenuItem,
            this.konecToolStripMenuItem,
            this.zav��tToolStripMenuItem});
            this.p�se�ToolStripMenuItem.Name = "p�se�ToolStripMenuItem";
            this.p�se�ToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.p�se�ToolStripMenuItem.Text = "P�se�";
            // 
            // ulo�itToolStripMenuItem
            // 
            this.ulo�itToolStripMenuItem.Name = "ulo�itToolStripMenuItem";
            this.ulo�itToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.ulo�itToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.ulo�itToolStripMenuItem.Text = "Ulo�it";
            this.ulo�itToolStripMenuItem.Click += new System.EventHandler(this.ulo�itToolStripMenuItem_Click);
            // 
            // konecToolStripMenuItem
            // 
            this.konecToolStripMenuItem.Name = "konecToolStripMenuItem";
            this.konecToolStripMenuItem.Size = new System.Drawing.Size(146, 6);
            // 
            // zav��tToolStripMenuItem
            // 
            this.zav��tToolStripMenuItem.Name = "zav��tToolStripMenuItem";
            this.zav��tToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.zav��tToolStripMenuItem.Text = "Zav��t";
            this.zav��tToolStripMenuItem.Click += new System.EventHandler(this.zav��tToolStripMenuItem_Click);
            // 
            // transpoziceToolStripMenuItem
            // 
            this.transpoziceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nahoruToolStripMenuItem,
            this.nahoruToolStripMenuItem1,
            this.dol�OKvintuToolStripMenuItem,
            this.nahoruOKvintuToolStripMenuItem});
            this.transpoziceToolStripMenuItem.Name = "transpoziceToolStripMenuItem";
            this.transpoziceToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.transpoziceToolStripMenuItem.Text = "Transpozice";
            // 
            // nahoruToolStripMenuItem
            // 
            this.nahoruToolStripMenuItem.Name = "nahoruToolStripMenuItem";
            this.nahoruToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.nahoruToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.nahoruToolStripMenuItem.Text = "Dol�";
            this.nahoruToolStripMenuItem.Click += new System.EventHandler(this.nahoruToolStripMenuItem_Click);
            // 
            // nahoruToolStripMenuItem1
            // 
            this.nahoruToolStripMenuItem1.Name = "nahoruToolStripMenuItem1";
            this.nahoruToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.nahoruToolStripMenuItem1.Size = new System.Drawing.Size(161, 22);
            this.nahoruToolStripMenuItem1.Text = "Nahoru";
            this.nahoruToolStripMenuItem1.Click += new System.EventHandler(this.nahoruToolStripMenuItem1_Click);
            // 
            // dol�OKvintuToolStripMenuItem
            // 
            this.dol�OKvintuToolStripMenuItem.Name = "dol�OKvintuToolStripMenuItem";
            this.dol�OKvintuToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.dol�OKvintuToolStripMenuItem.Text = "Dol� o kvintu";
            this.dol�OKvintuToolStripMenuItem.Click += new System.EventHandler(this.dol�OKvintuToolStripMenuItem_Click);
            // 
            // nahoruOKvintuToolStripMenuItem
            // 
            this.nahoruOKvintuToolStripMenuItem.Name = "nahoruOKvintuToolStripMenuItem";
            this.nahoruOKvintuToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.nahoruOKvintuToolStripMenuItem.Text = "Nahoru o kvintu";
            this.nahoruOKvintuToolStripMenuItem.Click += new System.EventHandler(this.nahoruOKvintuToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbremark);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cbuseserver);
            this.panel1.Controls.Add(this.cbserver);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.tbgroup);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.tbauthor);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbtitle);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(209, 492);
            this.panel1.TabIndex = 3;
            // 
            // cbuseserver
            // 
            this.cbuseserver.AutoSize = true;
            this.cbuseserver.Location = new System.Drawing.Point(137, 178);
            this.cbuseserver.Name = "cbuseserver";
            this.cbuseserver.Size = new System.Drawing.Size(57, 17);
            this.cbuseserver.TabIndex = 8;
            this.cbuseserver.Text = "Pou��t";
            this.cbuseserver.UseVisualStyleBackColor = true;
            this.cbuseserver.CheckedChanged += new System.EventHandler(this.cbuseserver_CheckedChanged);
            // 
            // cbserver
            // 
            this.cbserver.DataSource = this.serverBindingSource;
            this.cbserver.DisplayMember = "url";
            this.cbserver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbserver.FormattingEnabled = true;
            this.cbserver.Location = new System.Drawing.Point(12, 194);
            this.cbserver.Name = "cbserver";
            this.cbserver.Size = new System.Drawing.Size(182, 21);
            this.cbserver.TabIndex = 7;
            this.cbserver.ValueMember = "ID";
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Server";
            // 
            // tbgroup
            // 
            this.tbgroup.Location = new System.Drawing.Point(12, 116);
            this.tbgroup.Name = "tbgroup";
            this.tbgroup.Size = new System.Drawing.Size(182, 20);
            this.tbgroup.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Skupina";
            // 
            // tbauthor
            // 
            this.tbauthor.Location = new System.Drawing.Point(12, 77);
            this.tbauthor.Name = "tbauthor";
            this.tbauthor.Size = new System.Drawing.Size(182, 20);
            this.tbauthor.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Autor";
            // 
            // tbtitle
            // 
            this.tbtitle.Location = new System.Drawing.Point(12, 38);
            this.tbtitle.Name = "tbtitle";
            this.tbtitle.Size = new System.Drawing.Size(182, 20);
            this.tbtitle.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "N�zev";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(209, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(419, 492);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbtext);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(411, 466);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Editor";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbtext
            // 
            this.tbtext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbtext.Location = new System.Drawing.Point(3, 3);
            this.tbtext.Multiline = true;
            this.tbtext.Name = "tbtext";
            this.tbtext.Size = new System.Drawing.Size(405, 460);
            this.tbtext.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.songView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(411, 466);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "N�hled";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // songView1
            // 
            this.songView1.AutoScroll = true;
            this.songView1.BackColor = System.Drawing.SystemColors.Window;
            this.songView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songView1.Location = new System.Drawing.Point(3, 3);
            this.songView1.Name = "songView1";
            this.songView1.Size = new System.Drawing.Size(405, 460);
            this.songView1.SongDb = null;
            this.songView1.SongText = null;
            this.songView1.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Pozn�mka";
            // 
            // tbremark
            // 
            this.tbremark.Location = new System.Drawing.Point(12, 155);
            this.tbremark.Name = "tbremark";
            this.tbremark.Size = new System.Drawing.Size(182, 20);
            this.tbremark.TabIndex = 10;
            // 
            // EditSongForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 516);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EditSongForm";
            this.Text = "�prava p�sn�";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditSongForm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.songDb)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem p�se�ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ulo�itToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator konecToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zav��tToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbgroup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbauthor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbtitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox tbtext;
        private System.Windows.Forms.TabPage tabPage2;
        private SongView songView1;
        private System.Windows.Forms.ToolStripMenuItem transpoziceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nahoruToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nahoruToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dol�OKvintuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nahoruOKvintuToolStripMenuItem;
        private System.Windows.Forms.ComboBox cbserver;
        private System.Windows.Forms.BindingSource serverBindingSource;
        private SongDb songDb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbuseserver;
        private System.Windows.Forms.TextBox tbremark;
        private System.Windows.Forms.Label label5;
    }
}