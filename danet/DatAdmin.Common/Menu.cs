using System;
using System.Collections.Generic;
using System.Text;

namespace DatAdmin
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PopupMenuEnabledAttribute : System.Attribute
    {
        public PopupMenuEnabledAttribute(string path)
        {
            Path = path;
        }
        public readonly string Path;
    }


    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PopupMenuAttribute : System.Attribute
    {
        public PopupMenuAttribute(string path)
        {
            Path = path;
        }
        public readonly string Path;
    }

    public interface IPopupMenuBuilder
    {
        /// adds menu item, parent(path) must be added with AddGroup
        /// when onclick is null, item is disabled
        void AddItem(string path, SimpleCallback onclick);
        /// adds all public methods marked with attribute PopupMenu
        void AddObject(object obj);
    }
}
