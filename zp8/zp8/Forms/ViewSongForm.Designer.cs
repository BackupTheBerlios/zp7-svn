namespace zp8
{
    partial class ViewSongForm
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
            this.songView1 = new zp8.SongView();
            this.SuspendLayout();
            // 
            // songView1
            // 
            this.songView1.AutoScroll = true;
            this.songView1.BackColor = System.Drawing.SystemColors.Window;
            this.songView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songView1.Location = new System.Drawing.Point(0, 0);
            this.songView1.Name = "songView1";
            this.songView1.Size = new System.Drawing.Size(499, 420);
            this.songView1.SongDb = null;
            this.songView1.SongText = null;
            this.songView1.TabIndex = 0;
            // 
            // ViewSongForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 420);
            this.Controls.Add(this.songView1);
            this.Name = "ViewSongForm";
            this.Text = "Kino";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewSongForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private SongView songView1;

    }
}