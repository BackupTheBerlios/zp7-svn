using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DAIntf;

namespace DAIntf
{
    public abstract class TreeNodeBase : ITreeNode
    {
        protected IRealTreeNode m_realnode;
        protected readonly ITreeNode m_parent;
        protected readonly string m_name;
        protected readonly string m_fullpath;

        protected TreeNodeBase(ITreeNode parent, string name, string fullpath)
        {
            m_name = name;
            m_parent = parent;
            m_fullpath = fullpath;
        }

        public static void CallRefreshChilds(ITreeNode node)
        {
            if (node.RealNode != null) node.RealNode.RefreshChilds();
        }

        public static void CallRefreshSelf(ITreeNode node)
        {
            if (node.RealNode != null) node.RealNode.RefreshSelf();
        }

        public static void CallRefresh(ITreeNode node)
        {
            CallRefreshSelf(node);
            CallRefreshChilds(node);
        }

        //public static string NodePath(ITreeNode node)
        //{
        //    string res = node.Name;
        //    node = node.Parent;
        //    while (node != null)
        //    {
        //        res = node.Name + "/" + res;
        //        node = node.Parent;
        //    }
        //}

        #region ITreeNode Members

        public IRealTreeNode RealNode
        {
            get { return m_realnode; }
            set { m_realnode = value; }
        }

        public abstract string Title { get;}
        public string Name
        {
            get { return m_name; }
        }

        public virtual Bitmap Image { get { return null; } }
        public virtual Bitmap ExpandedImage { get { return null; } }

        public abstract ITreeNode[] GetChildren();
        public abstract void InvokeGetChildren(SimpleCallback callback);
        public abstract bool PreparedChildren {get;}
        public virtual void GetPopupMenu(IPopupMenuBuilder menu)
        {
            menu.AddObject(this);
        }
        
        public ITreeNode Parent
        {
            get { return m_parent; }
        }

        public virtual void Refresh()
        {
        }

        public virtual string Path
        {
            get { return m_fullpath; }
            //get
            //{
            //    if (Parent != null) return Parent.Path + "/" + Name;
            //    return Name;
            //}
        }

        public ITreeNode GetNamedChild(string child_name)
        {
            foreach (ITreeNode node in GetChildren())
            {
                if (node.Name == child_name) return node;
            }
            throw new TreeNodeNotFoundException(child_name);
        }

        public virtual string FileSystemPath
        {
            get { throw new NodeInvalidOperationException(); }
        }

        #endregion
    }
}
