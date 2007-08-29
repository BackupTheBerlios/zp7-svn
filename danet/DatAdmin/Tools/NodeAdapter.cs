using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using DAIntf;

namespace DatAdmin
{
    public class NodeAdapter : TreeNode, IRealTreeNode
    {
        internal ITreeNode m_node;
        DATreeView m_tree;
        bool m_filledChildren = false;
        //ContextMenu m_menu;
        ContextMenuStrip m_menu;

        public NodeAdapter(ITreeNode node, DATreeView tree)
        {
            m_tree = tree;
            m_node = node;
            Text = node.Title;
            Nodes.Add("__dummy__");
            UpdateImageIndex();
            m_menu = new ContextMenuStrip();
            this.ContextMenuStrip = m_menu;
            m_menu.Opening += new System.ComponentModel.CancelEventHandler(m_menu_Opening);
                //+= new EventHandler(m_menu_Popup);
        }

        void m_menu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_menu.Items.Clear();
            m_node.GetPopupMenu(new PopupMenuBuilder(m_menu, m_tree));
        }

        //void m_menu_Popup(object sender, EventArgs e)
        //{
        //    //m_menu.MenuItems.Clear();
        //    m_menu.Items.Clear();
        //    m_node.GetPopupMenu(new PopupMenuBuilder(m_menu, m_tree));
        //}

        public bool AllowExpend()
        {
            if (m_filledChildren) return true;
            if (!m_node.PreparedChildren)
            {
                m_node.InvokeGetChildren(OnPreparedExpand);
                return false;
            }
            FillChildren();
            return true;
        }

        private void OnPreparedExpand()
        {
            FillChildren();
            Expand();
        }
        private void FillChildren()
        {
            if (m_filledChildren) return;
            m_filledChildren = true;
            Nodes.Clear();
            foreach (ITreeNode child in m_node.GetChildren())
            {
                Nodes.Add(new NodeAdapter(child, m_tree));
            }
        }

        private void UpdateImageIndex()
        {
            Bitmap img;
            if (IsExpanded && m_node.ExpandedImage != null) img = m_node.ExpandedImage;
            else img = m_node.Image;
            int index = m_tree.GetImageIndex(img);
            ImageIndex = index;
            SelectedImageIndex = index;
        }

        internal void AfterExpand()
        {
            UpdateImageIndex();
        }

        internal void AfterCollapse()
        {
            UpdateImageIndex();
        }

        #region IRealTreeNode Members

        public void RefreshChilds()
        {
        }

        public void RefreshSelf()
        {
            Text = m_node.Title;
            UpdateImageIndex();
        }

        #endregion
    }
}
