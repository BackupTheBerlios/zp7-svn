namespace DatAdmin
{
    partial class DialogBase
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
            this.bt_ok = new System.Windows.Forms.Button();
            this.bt_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bt_ok
            // 
            this.bt_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bt_ok.Location = new System.Drawing.Point(189, 124);
            this.bt_ok.Name = "bt_ok";
            this.bt_ok.Size = new System.Drawing.Size(104, 25);
            this.bt_ok.TabIndex = 0;
            this.bt_ok.Text = "OK";
            this.bt_ok.UseVisualStyleBackColor = true;
            // 
            // bt_cancel
            // 
            this.bt_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_cancel.Location = new System.Drawing.Point(299, 124);
            this.bt_cancel.Name = "bt_cancel";
            this.bt_cancel.Size = new System.Drawing.Size(94, 25);
            this.bt_cancel.TabIndex = 1;
            this.bt_cancel.Text = "Cancel";
            this.bt_cancel.UseVisualStyleBackColor = true;
            // 
            // DialogBase
            // 
            this.AcceptButton = this.bt_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bt_cancel;
            this.ClientSize = new System.Drawing.Size(405, 161);
            this.Controls.Add(this.bt_cancel);
            this.Controls.Add(this.bt_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DialogBase";
            this.Text = "DatAdmin";
            this.Load += new System.EventHandler(this.DialogBase_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_ok;
        private System.Windows.Forms.Button bt_cancel;
    }
}