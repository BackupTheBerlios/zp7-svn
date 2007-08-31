using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace DAIntf
{
    public abstract class ConnectionTreeNode : TreeNodeBase
    {
        ICommonConnection m_conn;
        string m_filepath;
        bool m_connecting = false;

        public ConnectionTreeNode(ICommonConnection conn, ITreeNode parent, string filepath)
            : base(parent, System.IO.Path.GetFileName(filepath).ToLower())
        {
            m_conn = conn;
            m_filepath = filepath;
        }
        public override string Title
        {
            get {
                string suffix = "";
                if (m_connecting) suffix = String.Format("({0}...)", Texts.Get("s_connecting"));
                return System.IO.Path.GetFileName(m_filepath) + suffix; 
            }
        }

        [PopupMenu("s_connect")]
        public void Connect()
        {
            m_connecting = true;
            CallRefresh();
            Async.InvokeVoid(DoConnect, RealNode, CallRefresh);
        }
        [PopupMenu("s_disconnect")]
        public void Disconnect()
        {
            Async.InvokeVoid(DoDisconnect, RealNode, CallRefresh);
        }
        [PopupMenu("s_show_schema")]
        public void ShowSchema()
        {
            Toolkit.WindowToolkit.OpenSchemaWindow(m_conn.SystemConnection);
        }

        private void DoConnect()
        {
            try
            {
                m_conn.Open();
                OnConnect();
            }
            finally
            {
                m_connecting = false;
            }
        }
        private void DoDisconnect()
        {
            m_conn.Close();
            OnDisconnect();
        }

        protected virtual void OnConnect() { }
        protected virtual void OnDisconnect() { }
    }

    public class ServerSourceConnectionTreeNode : ConnectionTreeNode
    {
        ITreeNode[] m_children = null;
        IServerConnection m_conn;

        public ServerSourceConnectionTreeNode(IServerConnection conn, ITreeNode parent, string filepath)
            : base(conn, parent, filepath)
        {
            m_conn = conn;
        }

        public override ITreeNode[] GetChildren()
        {
            if (m_children == null) return new ITreeNode[] { };
            return m_children;
        }

        private void DoGetChildren()
        {
            List<ITreeNode> res = new List<ITreeNode>();
            foreach (string name in m_conn.Databases)
            {
                res.Add(new DatabaseSourceTreeNode(m_conn.GetDatabase(name), this, name));
            }
            m_children = res.ToArray();
        }
        public override bool PreparedChildren
        {
            get
            {
                if (m_conn.State == ConnectionStatus.Open)
                {
                    return m_children != null;
                }
                return true;
            }
        }
        public override void InvokeGetChildren(SimpleCallback callback)
        {
            Async.InvokeVoid(DoGetChildren, RealNode, callback);
        }
        public override System.Drawing.Bitmap Image
        {
            get
            {
                if (m_conn.State == ConnectionStatus.Open) return StdIcons.dbserver;
                else return StdIcons.dbserver_disconnected;
            }
        }
        protected override void OnDisconnect()
        {
            m_children = null;
        }
    }

    public class DatabaseSourceTreeNode : TreeNodeBase
    {
        IDatabaseConnection m_conn;
        string m_dbname;

        public DatabaseSourceTreeNode(IDatabaseConnection conn, ITreeNode parent, string dbname)
            : base(parent, dbname)
        {
            m_conn = conn;
            m_dbname = dbname;
        }

        public override ITreeNode[] GetChildren()
        {
            return new ITreeNode[] { };
        }
        public override void InvokeGetChildren(SimpleCallback callback)
        {
        }
        public override bool PreparedChildren
        {
            get { return true; }
        }
        public override string Title
        {
            get { return m_dbname; }
        }
        public override System.Drawing.Bitmap Image
        {
            get { return StdIcons.database; }
        }
    }
}
