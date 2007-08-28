using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DAIntf
{
    public interface ICreateFactoryItem
    {
        string Title { get;}
        string Group { get;}
        Bitmap Bitmap { get;}
        bool Create(ITreeNode parent, string name);
    }

    public interface ICreateFactory
    {
        ICreateFactoryItem[] GetItems(ITreeNode parent);
    }
}
