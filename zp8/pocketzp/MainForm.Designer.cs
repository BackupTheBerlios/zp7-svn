namespace pocketzp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.lbgroups = new System.Windows.Forms.ListBox();
            this.lbsongs = new System.Windows.Forms.ListBox();
            this.tmloadsongs = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Prohlížet";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "Konec";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // lbgroups
            // 
            this.lbgroups.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbgroups.Location = new System.Drawing.Point(0, 0);
            this.lbgroups.Name = "lbgroups";
            this.lbgroups.Size = new System.Drawing.Size(122, 268);
            this.lbgroups.TabIndex = 0;
            this.lbgroups.SelectedIndexChanged += new System.EventHandler(this.lbgroups_SelectedIndexChanged);
            this.lbgroups.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbgroups_KeyDown);
            // 
            // lbsongs
            // 
            this.lbsongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbsongs.Location = new System.Drawing.Point(122, 0);
            this.lbsongs.Name = "lbsongs";
            this.lbsongs.Size = new System.Drawing.Size(118, 268);
            this.lbsongs.TabIndex = 1;
            this.lbsongs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbsongs_KeyDown);
            // 
            // tmloadsongs
            // 
            this.tmloadsongs.Interval = 300;
            this.tmloadsongs.Tick += new System.EventHandler(this.tmloadsongs_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.lbsongs);
            this.Controls.Add(this.lbgroups);
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.Text = "Zpìvníkátor";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbgroups;
        private System.Windows.Forms.ListBox lbsongs;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.Timer tmloadsongs;
    }
}

