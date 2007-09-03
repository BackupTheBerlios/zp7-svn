using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using DAIntf;

namespace DAIntf
{
    //public delegate void NodeCallback(ITreeNode node);

    public static class NodeFactory
    {
        private static List<INodeFactory> m_factories = new List<INodeFactory>();
        private static CreateRootNodeDelegate m_createroot;

        public static void RegisterNodeFactory(INodeFactory fact)
        {
            m_factories.Add(fact);
        }
        public static void RegisterRootCreator(CreateRootNodeDelegate createroot)
        {
            m_createroot = createroot;
        }
        public static ITreeNode FromFile(ITreeNode parent, string file)
        {
            foreach (INodeFactory fact in m_factories)
            {
                ITreeNode node = fact.FromFile(parent, file);
                if (node != null) return node;
            }
            return null;
        }
        public static ITreeNode CreateRoot()
        {
            return m_createroot();
        }
        /// returns node, can wait to connect databases
        public static ITreeNode GetNodeFromPath(string path)
        {
            if (path == "") return CreateRoot();
            string[] p = path.Split('/');
            if (p[0] != "data:") throw new TreeNodeNotFoundException(path);
            ITreeNode item = CreateRoot();
            for (int i = 1; i < p.Length; i++)
            {
                item = item.GetNamedChild(p[i]);
            }
            return item;
        }
        internal static void AddAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                foreach (NodeFactoryAttribute attr in type.GetCustomAttributes(typeof(NodeFactoryAttribute), true))
                {
                    RegisterNodeFactory((INodeFactory)type.GetConstructor(new Type[] { }).Invoke(new object[] { }));
                }
            }
        }
        //public static void InvokeNodeFromPath(string path, NodeCallback callback)
        //{
        //}
    }
}
