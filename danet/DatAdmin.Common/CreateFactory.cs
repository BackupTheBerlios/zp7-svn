using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;

namespace DAIntf
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
                foreach (CreateFactoryItemAttribute attr in type.GetCustomAttributes(typeof(CreateFactoryItemAttribute), false))
                {
                    RegisterFactory((ICreateFactoryItem)type.GetConstructor(new Type[] { }).Invoke(new object[] { }));
                }

                foreach (CreateFactoryAttribute attr in type.GetCustomAttributes(typeof(CreateFactoryAttribute), false))
                {
                    RegisterFactory((ICreateFactory)type.GetConstructor(new Type[] { }).Invoke(new object[] { }));
                }
            }
        }
    }
}
