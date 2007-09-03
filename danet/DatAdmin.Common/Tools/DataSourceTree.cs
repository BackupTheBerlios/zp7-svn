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
        IServerConnection m_conn;

        public ServerSourceConnectionTreeNode(IServerConnection conn, ITreeNode parent, string filepath)
            : base(conn, parent, filepath)
        {
            m_conn = conn;
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
            if (m_conn.State == ConnectionStatus.Open)
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
        IServerConnection m_conn;

        public DatabasesTreeNode(IServerConnection conn, ITreeNode parent)
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
        IDatabaseConnection m_conn;

        public DatabaseSourceConnectionTreeNode(IDatabaseConnection conn, ITreeNode parent, string filepath)
            : base(conn, parent, filepath)
        {
            m_conn = conn;
        }

        public override System.Drawing.Bitmap Image
        {
            get
            {
                if (m_conn.State == ConnectionStatus.Open) return StdIcons.database;
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
            if (m_conn.State == ConnectionStatus.Open)
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
        IDatabaseConnection m_conn;

        public TablesTreeNode(IDatabaseConnection conn, ITreeNode parent)
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
            Async.InvokeVoid(DoGetChildren, RealNode, callback);
        }
        public override string Title
        {
            get { return Texts.Get("s_tables"); }
        }
    }

    public class TableSourceTreeNode : TreeNodeBase
    {
        ITableConnection m_conn;
        string m_tblname;

        public TableSourceTreeNode(ITableConnection conn, ITreeNode parent, string tblname)
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
