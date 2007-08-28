using System;
using System.Collections.Generic;
using System.Text;

namespace DAIntf
{
    public interface IRealTreeNode
    {
        void RefreshChilds();
        void RefreshSelf();
    }


    public class TreeNodeNotFoundException : Exception
    {
        public string Node;
        public TreeNodeNotFoundException(string node) { Node = node; }
    }

    public class NodeInvalidOperationException : Exception
    {
    }

    public interface ITreeNode
    {
        IRealTreeNode RealNode { get; set;}

        /// text, which will displayed
        string Title { get;}

        /// language independend node name, can be used for node pathing
        string Name { get;}

        /// image name, loaded from resources
        string ImageName { get;}

        /// image name, loaded from resources
        string ExpandedImageName { get;}

        ITreeNode[] GetChildren();

        /// prepares children, when finished, calls callback
        /// if this functions is called from GUI thread, callback is called from GUI thread
        void InvokeGetChildren(SimpleCallback callback);
        bool PreparedChildren { get;}

        /// creates popup menu (this popup menu can be extended from plugins)
        void GetPopupMenu(IPopupMenuBuilder menu);

        ITreeNode Parent { get;}

        string Path { get;}

        void Refresh();

        /// finds child node with name child_name, throws TreeNodeNotFoundException, if not found
        ITreeNode GetNamedChild(string child_name);

        /// gets file system path, throws, if node is not represented by file system object
        string FileSystemPath { get;}
    }

    public interface INodeFactory
    {
        /// returns node or null, when node cannot be created; 
        /// file is relative to datadirectory
        ITreeNode FromFile(string file);
    }
}
