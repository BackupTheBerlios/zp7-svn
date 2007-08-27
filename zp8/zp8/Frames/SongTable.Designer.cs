namespace zp8
{
    partial class SongTable
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.songBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.songDb = new zp8.SongDb();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viditelnéSloupceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.authorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.songtextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.langDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.server_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.searchtext = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.published = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.localmodified = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.songDb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // songBindingSource
            // 
            this.songBindingSource.DataMember = "song";
            this.songBindingSource.DataSource = this.songDb;
            // 
            // songDb
            // 
            this.songDb.DataSetName = "SongDb";
            this.songDb.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn1,
            this.titleDataGridViewTextBoxColumn,
            this.groupnameDataGridViewTextBoxColumn,
            this.authorDataGridViewTextBoxColumn,
            this.remark,
            this.songtextDataGridViewTextBoxColumn,
            this.langDataGridViewTextBoxColumn,
            this.server_id,
            this.netID,
            this.transp,
            this.searchtext,
            this.published,
            this.localmodified});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.DataSource = this.songBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(603, 507);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viditelnéSloupceToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(165, 26);
            // 
            // viditelnéSloupceToolStripMenuItem
            // 
            this.viditelnéSloupceToolStripMenuItem.Name = "viditelnéSloupceToolStripMenuItem";
            this.viditelnéSloupceToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.viditelnéSloupceToolStripMenuItem.Text = "Viditelné sloupce";
            this.viditelnéSloupceToolStripMenuItem.Click += new System.EventHandler(this.viditelnéSloupceToolStripMenuItem_Click);
            // 
            // iDDataGridViewTextBoxColumn1
            // 
            this.iDDataGridViewTextBoxColumn1.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn1.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn1.Name = "iDDataGridViewTextBoxColumn1";
            // 
            // titleDataGridViewTextBoxColumn
            // 
            this.titleDataGridViewTextBoxColumn.DataPropertyName = "title";
            this.titleDataGridViewTextBoxColumn.HeaderText = "Název";
            this.titleDataGridViewTextBoxColumn.Name = "titleDataGridViewTextBoxColumn";
            // 
            // groupnameDataGridViewTextBoxColumn
            // 
            this.groupnameDataGridViewTextBoxColumn.DataPropertyName = "groupname";
            this.groupnameDataGridViewTextBoxColumn.HeaderText = "Skupina";
            this.groupnameDataGridViewTextBoxColumn.Name = "groupnameDataGridViewTextBoxColumn";
            // 
            // authorDataGridViewTextBoxColumn
            // 
            this.authorDataGridViewTextBoxColumn.DataPropertyName = "author";
            this.authorDataGridViewTextBoxColumn.HeaderText = "Autor";
            this.authorDataGridViewTextBoxColumn.Name = "authorDataGridViewTextBoxColumn";
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
            this.songtextDataGridViewTextBoxColumn.Visible = false;
            // 
            // langDataGridViewTextBoxColumn
            // 
            this.langDataGridViewTextBoxColumn.DataPropertyName = "lang";
            this.langDataGridViewTextBoxColumn.HeaderText = "Jazyk";
            this.langDataGridViewTextBoxColumn.Name = "langDataGridViewTextBoxColumn";
            // 
            // server_id
            // 
            this.server_id.DataPropertyName = "server_id";
            this.server_id.HeaderText = "Server ID";
            this.server_id.Name = "server_id";
            this.server_id.Visible = false;
            // 
            // netID
            // 
            this.netID.DataPropertyName = "netID";
            this.netID.HeaderText = "netID";
            this.netID.Name = "netID";
            this.netID.Visible = false;
            // 
            // transp
            // 
            this.transp.DataPropertyName = "transp";
            this.transp.HeaderText = "Transpozice";
            this.transp.Name = "transp";
            this.transp.Visible = false;
            // 
            // searchtext
            // 
            this.searchtext.DataPropertyName = "searchtext";
            this.searchtext.HeaderText = "Vyhledávací text";
            this.searchtext.Name = "searchtext";
            this.searchtext.ReadOnly = true;
            this.searchtext.Visible = false;
            // 
            // published
            // 
            this.published.DataPropertyName = "published";
            this.published.HeaderText = "Publikováno";
            this.published.Name = "published";
            this.published.Visible = false;
            // 
            // localmodified
            // 
            this.localmodified.DataPropertyName = "localmodified";
            this.localmodified.HeaderText = "Lokální zmìny";
            this.localmodified.Name = "localmodified";
            this.localmodified.Visible = false;
            // 
            // SongTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "SongTable";
            this.Size = new System.Drawing.Size(603, 507);
            ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.songDb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource songBindingSource;
        private SongDb songDb;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viditelnéSloupceToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn authorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn songtextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn langDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn server_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn netID;
        private System.Windows.Forms.DataGridViewTextBoxColumn transp;
        private System.Windows.Forms.DataGridViewTextBoxColumn searchtext;
        private System.Windows.Forms.DataGridViewTextBoxColumn published;
        private System.Windows.Forms.DataGridViewCheckBoxColumn localmodified;
    }
}
