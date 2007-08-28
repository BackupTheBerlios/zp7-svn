using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.IO;
using DAIntf;

namespace DatAdmin
{
    public partial class DATreeView : UserControl
    {
        NodeAdapter m_root = null;
        Dictionary<string, int> m_images = new Dictionary<string, int>();

        public ITreeNode Root { 
            get
            {
                if (m_root != null) return m_root.m_node;
                return null;
            }
            set
            {
                SetRoot(value);
            }
        }

        public string RootPath
        {
            get
            {
                if (m_root != null) return Root.Path;
                return null;
            }
            set
            {

                Root = NodeFactory.GetNodeFromPath(value);
            }
        }

        public DATreeView()
        {
            InitializeComponent();
            //m_manager = new ResourceManager("StdIcons", typeof(DATreeView).Assembly);
        }

        private void tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (!((NodeAdapter)e.Node).AllowExpend())
            {
                e.Cancel = true;
            }
        }

        private void SetRoot(ITreeNode node)
        {
            m_root = new NodeAdapter(node, this);
            tree.Nodes.Clear();
            tree.Nodes.Add(m_root);
        }

        public int GetImageIndex(string image_name)
        {
            if (m_images.ContainsKey(image_name)) return m_images[image_name];
            Bitmap bmp = (Bitmap)StdIcons.ResourceManager.GetObject(image_name);
            int res = imageList1.Images.Count;
            imageList1.Images.Add(bmp);
            m_images[image_name] = res;
            return res;
        }

        private void tree_AfterExpand(object sender, TreeViewEventArgs e)
        {
            ((NodeAdapter)e.Node).AfterExpand();
        }

        private void tree_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            ((NodeAdapter)e.Node).AfterCollapse();
        }

        public ITreeNode Selected
        {
            get
            {
                if (tree.SelectedNode == null) return null;
                return ((NodeAdapter)tree.SelectedNode).m_node;
            }
        }
    }
}
