namespace zp8.Frames
{
    partial class SongsByGroupFrame
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
            this.lbgroups = new System.Windows.Forms.ListBox();
            this.lbsongs = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbgroups
            // 
            this.lbgroups.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbgroups.FormattingEnabled = true;
            this.lbgroups.Location = new System.Drawing.Point(0, 0);
            this.lbgroups.Name = "lbgroups";
            this.lbgroups.Size = new System.Drawing.Size(202, 407);
            this.lbgroups.TabIndex = 1;
            this.lbgroups.SelectedIndexChanged += new System.EventHandler(this.lbgroups_SelectedIndexChanged);
            // 
            // lbsongs
            // 
            this.lbsongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbsongs.FormattingEnabled = true;
            this.lbsongs.Location = new System.Drawing.Point(202, 0);
            this.lbsongs.Name = "lbsongs";
            this.lbsongs.Size = new System.Drawing.Size(221, 407);
            this.lbsongs.TabIndex = 2;
            this.lbsongs.SelectedIndexChanged += new System.EventHandler(this.lbsongs_SelectedIndexChanged);
            // 
            // SongsByGroupFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbsongs);
            this.Controls.Add(this.lbgroups);
            this.Name = "SongsByGroupFrame";
            this.Size = new System.Drawing.Size(423, 410);
            this.Resize += new System.EventHandler(this.SongsByGroupFrame_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbgroups;
        private System.Windows.Forms.ListBox lbsongs;
    }
}
