namespace zp8
{
    partial class ZoomControl
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
            this.tbzoom = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.lbzoom = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbzoom)).BeginInit();
            this.SuspendLayout();
            // 
            // tbzoom
            // 
            this.tbzoom.Location = new System.Drawing.Point(59, 0);
            this.tbzoom.Minimum = -10;
            this.tbzoom.Name = "tbzoom";
            this.tbzoom.Size = new System.Drawing.Size(116, 42);
            this.tbzoom.TabIndex = 0;
            this.tbzoom.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Zvìtšení";
            // 
            // lbscale
            // 
            this.lbzoom.AutoSize = true;
            this.lbzoom.Location = new System.Drawing.Point(3, 13);
            this.lbzoom.Name = "lbscale";
            this.lbzoom.Size = new System.Drawing.Size(33, 13);
            this.lbzoom.TabIndex = 2;
            this.lbzoom.Text = "100%";
            // 
            // ZoomControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbzoom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbzoom);
            this.Name = "ZoomControl";
            this.Size = new System.Drawing.Size(175, 40);
            ((System.ComponentModel.ISupportInitialize)(this.tbzoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbzoom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbzoom;
    }
}
