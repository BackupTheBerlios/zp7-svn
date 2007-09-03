using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DatAdmin
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CreateFactoryItemAttribute : Attribute
    {
    }

    public interface ICreateFactoryItem
    {
        string Title { get;}
        string Group { get;}
        Bitmap Bitmap { get;}
        bool Create(ITreeNode parent, string name);
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CreateFactoryAttribute : Attribute
    {
    }

    public interface ICreateFactory
    {
        ICreateFactoryItem[] GetItems(ITreeNode parent);
    }

}
