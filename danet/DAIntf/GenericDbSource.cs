using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DAIntf
{
    public class GenericCommonConnection : ICommonConnection
    {
        protected DbConnection m_conn;

        #region IServerConnection Members

        public GenericCommonConnection(DbConnection conn)
        {
            m_conn = conn;
        }

        public void Open()
        {
            Logging.Info("Opening connection {0}", m_conn.ConnectionString);
            m_conn.Open();
        }

        public void Close()
        {
            m_conn.Close();
        }

        public ConnectionStatus State
        {
            get
            {
                ConnectionState state = m_conn.State;
                if (state == ConnectionState.Open) return ConnectionStatus.Open;
                else return ConnectionStatus.Closed;
            }
        }

        public DbConnection SystemConnection
        {
            get { return m_conn; }
        }

        #endregion
    }

    public class GenericServerConnection : GenericCommonConnection, IServerConnection
    {
        public GenericServerConnection(DbConnection conn)
            : base(conn)
        {
        }

        #region IServerConnection Members

        public IEnumerable<string> Databases
        {
            get
            {
                DataTable dbs = m_conn.GetSchema("Databases");
                foreach (DataRow row in dbs.Rows) yield return row[0].ToString();
            }
        }

        public IDatabaseConnection GetDatabase(string name)
        {
            return new GenericDatabaseConnection(m_conn, name);
        }

        #endregion
    }

    public class GenericDatabaseConnection : GenericCommonConnection, IDatabaseConnection
    {
        string m_dbname;

        public GenericDatabaseConnection(DbConnection conn, string dbname)
            : base(conn)
        {
            m_dbname = dbname;
        }

        #region IDatabaseConnection Members

        public IEnumerable<string> Tables
        {
            get
            {
                string[] restr = new string[] { m_dbname };
                DataTable dbs = m_conn.GetSchema("Tables", restr);
                foreach (DataRow row in dbs.Rows) yield return row[0].ToString();
            }
        }

        public ITableConnection GetTable(string name)
        {
            return new GenericTableConnection(m_conn, m_dbname, name);
        }

        #endregion
    }

    public class GenericTableConnection : GenericCommonConnection, ITableConnection
    {
        string m_dbname;
        string m_tblname;

        public GenericTableConnection(DbConnection conn, string dbname, string tblname)
            : base(conn)
        {
            m_dbname = dbname;
            m_tblname = tblname;
        }
    }
}
