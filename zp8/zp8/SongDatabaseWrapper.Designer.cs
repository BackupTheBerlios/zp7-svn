namespace zp8
{
    partial class SongDatabaseWrapper
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
            this.songDb = new zp8.SongDb();
            this.songbindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.serverbindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.songDb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.songbindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.serverbindingSource)).BeginInit();
            // 
            // songDb
            // 
            this.songDb.DataSetName = "SongDb";
            this.songDb.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // songbindingSource
            // 
            this.songbindingSource.DataMember = "song";
            this.songbindingSource.DataSource = this.songDb;
            // 
            // serverbindingSource
            // 
            this.serverbindingSource.DataMember = "server";
            this.serverbindingSource.DataSource = this.songDb;
            ((System.ComponentModel.ISupportInitialize)(this.songDb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.songbindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.serverbindingSource)).EndInit();

        }

        #endregion

        private SongDb songDb;
        private System.Windows.Forms.BindingSource songbindingSource;
        private System.Windows.Forms.BindingSource serverbindingSource;
    }
}
