using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace DatAdmin
{
    public abstract class ConnectionTreeNode : TreeNodeBase
    {
        IPhysicalConnection m_conn;
        string m_filepath;
        bool m_connecting = false;

        public ConnectionTreeNode(IPhysicalConnection conn, ITreeNode parent, string filepath)
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
            m_conn.Open().OnFinish(delegate()
            {
                m_connecting = false; 
                CallRefresh();
                OnConnect();
            }, RealNode.Invoker);
            //Async.InvokeVoid(DoConnect, RealNode, CallRefresh);
        }
        [PopupMenu("s_disconnect")]
        public void Disconnect()
        {
            m_conn.Close().OnFinish(delegate()
            {
                OnDisconnect();
                CallRefresh();
            }, RealNode.Invoker);
        }
        [PopupMenu("s_show_schema")]
        public void ShowSchema()
        {
            Toolkit.WindowToolkit.OpenSchemaWindow(m_conn);
        }

        //private void DoConnect()
        //{
        //    try
        //    {
        //        m_conn.Open();
        //        OnConnect();
        //    }
        //    finally
        //    {
        //        m_connecting = false;
        //    }
        //}
        //private void DoDisconnect()
        //{
        //    m_conn.Close();
        //    OnDisconnect();
        //}

        protected virtual void OnConnect() { }
        protected virtual void OnDisconnect() { }
    }

    public class ServerSourceConnectionTreeNode : ConnectionTreeNode
    {
        IServerSource m_conn;

        public ServerSourceConnectionTreeNode(IServerSource conn, ITreeNode parent, string filepath)
            : base(conn.Connection, parent, filepath)
        {
            m_conn = conn;
        }

        public override System.Drawing.Bitmap Image
        {
            get
            {
                if (m_conn.Connection.IsOpened) return StdIcons.dbserver;
                else return StdIcons.dbserver_disconnected;
            }
        }
        protected override void OnDisconnect()
        {
        }
        protected override void OnConnect()
        {
            RealNode.Expand();
            RealNode.ChildByName("databases").Expand();
        }
        public override void InvokeGetChildren(SimpleCallback callback)
        {
        }
        public override bool PreparedChildren
        {
            get { return true; }
        }
        public override ITreeNode[] GetChildren()
        {
            if (m_conn.Connection.IsOpened)
            {
                return new ITreeNode[] { new DatabasesTreeNode(m_conn, this) };
            }
            else
            {
                return new ITreeNode[] { };
            }
        }
    }

    public class DatabasesTreeNode : TreeNodeBase
    {
        ITreeNode[] m_children = null;
        IServerSource m_conn;

        public DatabasesTreeNode(IServerSource conn, ITreeNode parent)
            : base(parent, "databases")
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
                if (m_conn.Connection.IsOpened)
                {
                    return m_children != null;
                }
                return true;
            }
        }
        public override void InvokeGetChildren(SimpleCallback callback)
        {
            ConnTools.InvokeVoid(m_conn, DoGetChildren, RealNode.Invoker, callback);
        }
        public override string Title
        {
            get { return Texts.Get("s_databases"); }
        }
        public override System.Drawing.Bitmap Image
        {
            get { return StdIcons.database; }
        }
    }

    public class DatabaseSourceTreeNode : TreeNodeBase
    {
        IDatabaseSource m_conn;
        string m_dbname;

        public DatabaseSourceTreeNode(IDatabaseSource conn, ITreeNode parent, string dbname)
            : base(parent, dbname)
        {
            m_conn = conn;
            m_dbname = dbname;
        }

        public override ITreeNode[] GetChildren()
        {
            return new ITreeNode[] { new TablesTreeNode(m_conn, this) };
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

    public class DatabaseSourceConnectionTreeNode : ConnectionTreeNode
    {
        IDatabaseSource m_conn;

        public DatabaseSourceConnectionTreeNode(IDatabaseSource conn, ITreeNode parent, string filepath)
            : base(conn.Connection, parent, filepath)
        {
            m_conn = conn;
        }

        public override System.Drawing.Bitmap Image
        {
            get
            {
                if (m_conn.Connection.IsOpened) return StdIcons.database;
                else return StdIcons.database_disconnected;
            }
        }
        protected override void OnDisconnect()
        {
        }
        public override void InvokeGetChildren(SimpleCallback callback)
        {
        }
        public override bool PreparedChildren
        {
            get { return true; }
        }
        public override ITreeNode[] GetChildren()
        {
            if (m_conn.Connection.IsOpened)
            {
                return new ITreeNode[] { new TablesTreeNode(m_conn, this) };
            }
            else
            {
                return new ITreeNode[] { };
            }
        }
    }

    public class TablesTreeNode : TreeNodeBase
    {
        ITreeNode[] m_children = null;
        IDatabaseSource m_conn;

        public TablesTreeNode(IDatabaseSource conn, ITreeNode parent)
            : base(parent, "tables")
        {
            m_conn = conn;
        }

        public override System.Drawing.Bitmap Image
        {
            get { return StdIcons.table; }
        }

        public override ITreeNode[] GetChildren()
        {
            if (m_children == null) return new ITreeNode[] { };
            return m_children;
        }

        private void DoGetChildren()
        {
            List<ITreeNode> res = new List<ITreeNode>();
            foreach (string name in m_conn.Tables)
            {
                res.Add(new TableSourceTreeNode(m_conn.GetTable(name), this, name));
            }
            m_children = res.ToArray();
        }
        public override bool PreparedChildren
        {
            get
            {
                return m_children != null;
            }
        }
        public override void InvokeGetChildren(SimpleCallback callback)
        {
            ConnTools.InvokeVoid(m_conn, DoGetChildren, RealNode.Invoker, callback);
        }
        public override string Title
        {
            get { return Texts.Get("s_tables"); }
        }
    }

    public class TableSourceTreeNode : TreeNodeBase
    {
        ITableSource m_conn;
        string m_tblname;

        public TableSourceTreeNode(ITableSource conn, ITreeNode parent, string tblname)
            : base(parent, tblname)
        {
            m_conn = conn;
            m_tblname = tblname;
        }

        public override ITreeNode[] GetChildren()
        {
            return new ITreeNode[] {  };
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
            get { return m_tblname; }
        }
        public override System.Drawing.Bitmap Image
        {
            get { return StdIcons.table; }
        }

        [PopupMenu("s_open_table")]
        public void OpenTable()
        {
            Toolkit.WindowToolkit.OpenTable(m_conn);
        }

    }

}
