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
            this.iDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.authorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.songtextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.langDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.songDb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.songtextDataGridViewTextBoxColumn,
            this.langDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.songBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(603, 507);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
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
            // SongTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "SongTable";
            this.Size = new System.Drawing.Size(603, 507);
            this.ContextMenuStripChanged += new System.EventHandler(this.SongTable_ContextMenuStripChanged);
            ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.songDb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource songBindingSource;
        private SongDb songDb;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn authorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn songtextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn langDataGridViewTextBoxColumn;
    }
}
