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
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ITableConnection GetTable(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
