using System;
using System.Collections.Generic;
using System.Text;
using DAIntf;

namespace DatAdmin
{
    public delegate void NodeCallback(ITreeNode node);
    public static class NodeFactory
    {
        private static List<INodeFactory> m_factories = new List<INodeFactory>();
        public static void RegisterNodeFactory(INodeFactory fact)
        {
            m_factories.Add(fact);
        }
        public static ITreeNode FromFile(string file)
        {
            foreach (INodeFactory fact in m_factories)
            {
                ITreeNode node = fact.FromFile(file);
                if (node != null) return node;
            }
            return null;
        }
        public static ITreeNode CreateRoot()
        {
            return new RootTreeNode();
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
        public static void InvokeNodeFromPath(string path, NodeCallback callback)
        {
        }
    }
}
