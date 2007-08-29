using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DAIntf
{
    public class GenericServerConnection : IServerConnection
    {
        DbConnection m_conn;
        public GenericServerConnection(DbConnection conn)
        {
            m_conn = conn;
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
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    public class GenericDatabaseConnection : IDatabaseConnection
    {
        DbConnection m_conn;
        string m_dbname;
        public GenericDatabaseConnection(DbConnection conn, string dbname)
        {
            m_conn = conn;
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
