using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using DAIntf;

namespace DatAdmin
{
    public class MethodInvoker
    {
        object m_obj;
        MethodInfo m_mtd;
        public MethodInvoker(object obj, MethodInfo mtd)
        {
            m_obj = obj;
            m_mtd = mtd;
        }
        public void InvokeVoid()
        {
            m_mtd.Invoke(m_obj, null);
        }
        public bool InvokeBool()
        {
            return (bool)m_mtd.Invoke(m_obj, null);
        }
    }

    public class PopupMenuItem
    {
        //public List<PopupMenuItem> m_items = new List<PopupMenuItem>();
        public Dictionary<string, PopupMenuItem> m_items = new Dictionary<string, PopupMenuItem>();
        public string m_name;
        public ToolStripMenuItem m_menu;
        //public MenuItem m_menu;
        //public Menu.MenuItemCollection m_parent;
        public ToolStripItemCollection m_parent;

        public ToolStripMenuItem FindOrCreate(string[] path)
        {
            if (path.Length == 0) return m_menu;
            if (m_items.ContainsKey(path[0]))
            {
                return m_items[path[0]].FindOrCreate(PyList.SliceFrom(path, 1));
            }
            ToolStripMenuItem newitem = new ToolStripMenuItem();
            m_parent.Add(newitem);
            newitem.Text = path[0];

            PopupMenuItem popup = new PopupMenuItem();
            popup.m_parent = newitem.DropDownItems;
            popup.m_menu = newitem;
            return popup.FindOrCreate(PyList.SliceFrom(path, 1));
            // create new item
            //MenuItem newitem = m_parent.Add(Texts.Get(path[0]));
            //PopupMenuItem popup = new PopupMenuItem();
            //popup.m_parent = newitem.MenuItems;
            //popup.m_menu = newitem;
            //m_items[path[0]] = popup;
            //return popup.FindOrCreate(PyList.SliceFrom(path, 1));
        }
    }

    public class PopupMenuBuilder : IPopupMenuBuilder
    {
        ContextMenuStrip m_menu;
        PopupMenuItem m_root;
        DATreeView m_tree;
        //Dictionary<string, MenuItem> m_groups = new Dictionary<string, MenuItem>();

        public PopupMenuBuilder(ContextMenuStrip menu, DATreeView tree)
        {
            m_menu = menu;
            m_tree = tree;
            m_root = new PopupMenuItem();
            m_root.m_parent = menu.Items;
            //m_root.m_parent = menu.MenuItems;
        }

        //Menu.MenuItemCollection GetParent(string path)
        //{
        //    string[] items = path.Split('/');
        //    if (items.Length > 1)
        //    {
        //        return m_groups[String.Join("/", items, 0, items.Length - 1)].MenuItems;
        //    }
        //    else
        //    {
        //        return m_menu.MenuItems;
        //    }
        //}

        #region IPopupMenuBuilder Members

        //public void AddGroup(string path, string title)
        //{
        //    MenuItem mi = GetParent(path).Add(title);
        //    m_groups[path] = mi;
        //}

        public void AddItem(string path, SimpleCallback onclick)
        {
            ToolStripMenuItem mi = m_root.FindOrCreate(path.Split('/'));
            if(onclick != null) mi.Click += delegate(object sender, EventArgs e) { onclick(); };
            else mi.Enabled = false;
        }

        public void AddObject(object obj)
        {
            Dictionary<string, bool> enabled = new Dictionary<string, bool>();
            foreach (MethodAttribute<PopupMenuEnabledAttribute> rec in ReflTools.GetMethods<PopupMenuEnabledAttribute>(obj))
            {
                enabled[rec.Attribute.Path] = (bool)rec.Method.Invoke(obj, null);
            }

            foreach (MethodAttribute<PopupMenuAttribute> rec in ReflTools.GetMethods<PopupMenuAttribute>(obj))
            {
                bool e;
                try { e = enabled[rec.Attribute.Path]; }
                catch (Exception) { e = true; }
                if (e) AddItem(rec.Attribute.Path, new MethodInvoker(obj, rec.Method).InvokeVoid);
                else AddItem(rec.Attribute.Path, null);
            }

            //foreach (MethodInfo mtd in obj.GetType().GetMethods())
            //{
            //    PopupMenuAttribute[] attr = (PopupMenuAttribute[])mtd.GetCustomAttributes(typeof(PopupMenuAttribute), true);
            //    if (attr.Length > 0)
            //    {
            //        AddItem(attr[0].Path, new MethodInvoker(obj, mtd).InvokeVoid);
            //        //AddItem(attr[0].Path, delegate() { mtd.Invoke(obj, null); });
            //    }
            //}
        }

        #endregion
    }
}
