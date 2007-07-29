namespace zp8
{
    partial class WaitForm
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
            this.txaction = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txlog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txaction
            // 
            this.txaction.AutoSize = true;
            this.txaction.Location = new System.Drawing.Point(12, 9);
            this.txaction.Name = "txaction";
            this.txaction.Size = new System.Drawing.Size(35, 13);
            this.txaction.TabIndex = 0;
            this.txaction.Text = "label1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 172);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Storno";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txlog
            // 
            this.txlog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txlog.Location = new System.Drawing.Point(12, 25);
            this.txlog.Multiline = true;
            this.txlog.Name = "txlog";
            this.txlog.ReadOnly = true;
            this.txlog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txlog.Size = new System.Drawing.Size(491, 141);
            this.txlog.TabIndex = 2;
            this.txlog.WordWrap = false;
            // 
            // WaitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 207);
            this.Controls.Add(this.txlog);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txaction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "WaitForm";
            this.Text = "Prosím èekejte ...";
            this.TopMost = true;
            this.UseWaitCursor = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label txaction;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txlog;
    }
}