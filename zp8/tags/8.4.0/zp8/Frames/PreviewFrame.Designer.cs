namespace zp8
{
    partial class PreviewFrame
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
            this.tbpage = new System.Windows.Forms.TrackBar();
            this.zoomControl1 = new zp8.ZoomControl();
            this.lbpage = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.plpage = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbpage)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tbpage);
            this.panel1.Controls.Add(this.zoomControl1);
            this.panel1.Controls.Add(this.lbpage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(447, 42);
            this.panel1.TabIndex = 0;
            // 
            // tbpage
            // 
            this.tbpage.Location = new System.Drawing.Point(280, -1);
            this.tbpage.Name = "tbpage";
            this.tbpage.Size = new System.Drawing.Size(123, 42);
            this.tbpage.TabIndex = 3;
            this.tbpage.Scroll += new System.EventHandler(this.tbpage_Scroll);
            // 
            // zoomControl1
            // 
            this.zoomControl1.Location = new System.Drawing.Point(-1, -1);
            this.zoomControl1.Name = "zoomControl1";
            this.zoomControl1.Size = new System.Drawing.Size(175, 40);
            this.zoomControl1.TabIndex = 2;
            this.zoomControl1.ChangedZoom += new System.EventHandler(this.zoomControl1_ChangedZoom);
            // 
            // lbpage
            // 
            this.lbpage.AutoSize = true;
            this.lbpage.Location = new System.Drawing.Point(180, 14);
            this.lbpage.Name = "lbpage";
            this.lbpage.Size = new System.Drawing.Size(59, 13);
            this.lbpage.TabIndex = 1;
            this.lbpage.Text = "Strana ???";
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.plpage);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 42);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(447, 448);
            this.panel2.TabIndex = 1;
            // 
            // plpage
            // 
            this.plpage.BackColor = System.Drawing.SystemColors.Window;
            this.plpage.Location = new System.Drawing.Point(0, 0);
            this.plpage.Name = "plpage";
            this.plpage.Size = new System.Drawing.Size(307, 271);
            this.plpage.TabIndex = 0;
            this.plpage.Paint += new System.Windows.Forms.PaintEventHandler(this.plpage_Paint);
            // 
            // PreviewFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "PreviewFrame";
            this.Size = new System.Drawing.Size(447, 490);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbpage)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbpage;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel plpage;
        private ZoomControl zoomControl1;
        private System.Windows.Forms.TrackBar tbpage;
    }
}
