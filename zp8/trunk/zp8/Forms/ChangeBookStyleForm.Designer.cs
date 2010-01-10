namespace zp8
{
    partial class ChangeBookStyleForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.lbstyle = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Styl zpìvníku";
            // 
            // lbstyle
            // 
            this.lbstyle.FormattingEnabled = true;
            this.lbstyle.Location = new System.Drawing.Point(12, 25);
            this.lbstyle.Name = "lbstyle";
            this.lbstyle.Size = new System.Drawing.Size(156, 173);
            this.lbstyle.TabIndex = 3;
            // 
            // ChangeBookStyleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(178, 239);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbstyle);
            this.Name = "ChangeBookStyleForm";
            this.Text = "Zmìnit styl zpìvníku";
            this.Load += new System.EventHandler(this.ChangeBookStyleForm_Load);
            this.Controls.SetChildIndex(this.lbstyle, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbstyle;
    }
}
