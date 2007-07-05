namespace zp8
{
    partial class SongView
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btreset = new System.Windows.Forms.Button();
            this.cbtransp = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.zczoom = new zp8.ZoomControl();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Location = new System.Drawing.Point(0, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(370, 372);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.zczoom);
            this.panel2.Controls.Add(this.btreset);
            this.panel2.Controls.Add(this.cbtransp);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(461, 42);
            this.panel2.TabIndex = 1;
            // 
            // btreset
            // 
            this.btreset.Location = new System.Drawing.Point(323, 12);
            this.btreset.Name = "btreset";
            this.btreset.Size = new System.Drawing.Size(56, 21);
            this.btreset.TabIndex = 4;
            this.btreset.Text = "Reset";
            this.btreset.UseVisualStyleBackColor = true;
            this.btreset.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbtransp
            // 
            this.cbtransp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbtransp.FormattingEnabled = true;
            this.cbtransp.Items.AddRange(new object[] {
            "C",
            "C#",
            "D",
            "D#",
            "E",
            "F",
            "F#",
            "G",
            "G#",
            "A",
            "Bb",
            "H"});
            this.cbtransp.Location = new System.Drawing.Point(247, 12);
            this.cbtransp.MaxDropDownItems = 12;
            this.cbtransp.Name = "cbtransp";
            this.cbtransp.Size = new System.Drawing.Size(70, 21);
            this.cbtransp.TabIndex = 3;
            this.cbtransp.SelectedIndexChanged += new System.EventHandler(this.cbtransp_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Transpozice";
            // 
            // zczoom
            // 
            this.zczoom.Location = new System.Drawing.Point(3, 2);
            this.zczoom.Name = "zczoom";
            this.zczoom.Size = new System.Drawing.Size(175, 40);
            this.zczoom.TabIndex = 5;
            this.zczoom.ChangedZoom += new System.EventHandler(this.zczoom_ChangedZoom);
            // 
            // SongView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SongView";
            this.Size = new System.Drawing.Size(461, 488);
            this.Resize += new System.EventHandler(this.SongView_Resize);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbtransp;
        private System.Windows.Forms.Button btreset;
        private zp8.ZoomControl zczoom;

    }
}
