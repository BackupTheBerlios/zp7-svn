using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;

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

    public static class CreateFactory
    {
        static List<ICreateFactory> m_facts = new List<ICreateFactory>();
        static List<ICreateFactoryItem> m_items = new List<ICreateFactoryItem>();

        private static void RegisterFactory(ICreateFactory factory)
        {
            m_facts.Add(factory);
        }
        private static void RegisterFactory(ICreateFactoryItem item)
        {
            m_items.Add(item);
        }
        public static IEnumerable<ICreateFactoryItem> GetItems(ITreeNode parent)
        {
            foreach (ICreateFactory fact in m_facts)
            {
                foreach (ICreateFactoryItem f in fact.GetItems(parent))
                {
                    yield return f;
                }
            }
            foreach (ICreateFactoryItem f in m_items)
            {
                yield return f;
            }
        }
        internal static void AddAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (Array.IndexOf(type.GetInterfaces(), typeof(ICreateFactoryItem)) >= 0)
                    RegisterFactory((ICreateFactoryItem) type.GetConstructor(new Type[] { }).Invoke(new object[] { }));
                if (Array.IndexOf(type.GetInterfaces(), typeof(ICreateFactory)) >= 0)
                    RegisterFactory((ICreateFactory)type.GetConstructor(new Type[] { }).Invoke(new object[] { }));
            }
        }
    }
}
