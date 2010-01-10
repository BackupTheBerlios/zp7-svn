namespace zp8
{
    public partial class VisibleColumnsForm : DialogBase
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
            this.lbcolumns = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // lbcolumns
            // 
            this.lbcolumns.FormattingEnabled = true;
            this.lbcolumns.Location = new System.Drawing.Point(12, 12);
            this.lbcolumns.Name = "lbcolumns";
            this.lbcolumns.Size = new System.Drawing.Size(219, 199);
            this.lbcolumns.TabIndex = 0;
            // 
            // VisibleColumnsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 265);
            this.Controls.Add(this.lbcolumns);
            this.Name = "VisibleColumnsForm";
            this.Text = "Viditelné sloupce";
            this.Controls.SetChildIndex(this.lbcolumns, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox lbcolumns;
    }
}