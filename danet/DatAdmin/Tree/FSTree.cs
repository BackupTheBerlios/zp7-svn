using System;
using System.Collections.Generic;
using System.Text;
using DAIntf;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace DatAdmin
{
    public class FolderTreeNode : TreeNodeBase
    {
        string m_folder;

        public override string FileSystemPath { get { return System.IO.Path.Combine(Core.DataDirectory, m_folder); } }

        /// constructor called for non-root item
        public FolderTreeNode(FolderTreeNode parent, string folder)
            : base(parent, System.IO.Path.GetFileName(folder).ToLower(), "data:/" + folder)
        {
            m_folder = folder;
        }

        /// constructor called for root
        public FolderTreeNode()
            : base(null, "data:", "data:")
        {
            m_folder = "";
        }

        public override string Title
        {
            get { return System.IO.Path.GetFileName(m_folder); }
        }

        public override Bitmap Image
        {
            get { return StdIcons.img_folder; }
        }

        public override Bitmap ExpandedImage
        {
            get { return StdIcons.img_folder_expanded; }
        }

        public override ITreeNode[] GetChildren()
        {
            List<ITreeNode> result = new List<ITreeNode>();
            string[] dirs = Directory.GetDirectories(FileSystemPath);
            Array.Sort(dirs);
            foreach (string dir in dirs)
            {
                result.Add(new FolderTreeNode(this, System.IO.Path.Combine(m_folder, System.IO.Path.GetFileName(dir))));
            }
            string[] files = Directory.GetFiles(FileSystemPath);
            Array.Sort(files);
            foreach (string file in files)
            {
                ITreeNode node = NodeFactory.FromFile(this, System.IO.Path.Combine(FileSystemPath, System.IO.Path.GetFileName(file)));
                if (node != null)
                {
                    result.Add(node);
                }
            }
            return result.ToArray();
        }

        public override void InvokeGetChildren(SimpleCallback callback)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool PreparedChildren
        {
            get { return true; }
        }

        //public override void GetPopupMenu(IPopupMenuBuilder menu)
        //{
        //    menu.AddGroup("new", StdText.s_new);
        //    menu.AddItem("new/folder", StdText.s_folder, NewFolder);
        //    if (Parent != null) menu.AddItem("delete", StdText.s_delete, Remove);
        //}

        [PopupMenuEnabled("s_delete")]
        public bool CanRemove() { return Parent != null; }

        [PopupMenu("s_new/s_folder")]
        public void NewFolder()
        {
            string name = InputBox.Run(Texts.Get("s_name_of_new_folder"), "new_folder");
            if (name != null)
            {
                try
                {
                    Directory.CreateDirectory(System.IO.Path.Combine(FileSystemPath, name));
                    TreeNodeBase.CallRefresh(this);
                }
                catch (Exception e)
                {
                    StdDialog.ShowError(e);
                }
            }
        }

        [PopupMenu("s_new/s_connection")]
        public void NewConnection()
        {
            CreateDialog dlg = new CreateDialog(this);
            dlg.ShowDialog();
        }

        [PopupMenu("s_delete")]
        public void Remove()
        {
            if (DialogResult.Yes == MessageBox.Show(String.Format(Texts.Get("s_really_delete_folder"), m_folder), "DatAdmin", MessageBoxButtons.YesNo))
            {
                Directory.Delete(FileSystemPath, true);
                TreeNodeBase.CallRefresh(Parent);
            }
        }
    }

    public class RootTreeNode : FolderTreeNode
    {
        internal static ITreeNode CreateRoot() { return new RootTreeNode(); }
        public override string Title
        {
            get { return "Data"; }
        }
    }

}
