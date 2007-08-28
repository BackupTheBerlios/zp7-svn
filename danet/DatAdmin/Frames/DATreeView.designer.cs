namespace DatAdmin
{
    partial class DATreeView
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
            this.components = new System.ComponentModel.Container();
            this.tree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // tree
            // 
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.imageList1;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            this.tree.SelectedImageIndex = 0;
            this.tree.Size = new System.Drawing.Size(295, 355);
            this.tree.TabIndex = 0;
            this.tree.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterCollapse);
            this.tree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_BeforeExpand);
            this.tree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterExpand);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // DATreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tree);
            this.Name = "DATreeView";
            this.Size = new System.Drawing.Size(295, 355);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tree;
        internal System.Windows.Forms.ImageList imageList1;
    }
}
