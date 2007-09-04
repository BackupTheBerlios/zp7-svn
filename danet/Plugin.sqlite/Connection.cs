using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;
using DatAdmin;

namespace Plugin.sqlite
{
    public class SQLiteStoredConnection : StoredConnection<SQLiteStoredConnection>
    {
        public string ConnectionString;
    }

    [NodeFactory]
    public class ConnectionFactory : INodeFactory
    {
        #region INodeFactory Members

        public ITreeNode FromFile(ITreeNode parent, string file)
        {
            DbProviderFactory fact = SQLiteFactory.Instance;
            if (file.ToLower().EndsWith(".con"))
            {
                try
                {
                    SQLiteStoredConnection con = SQLiteStoredConnection.Load(file);
                    SQLiteConnection sql = new SQLiteConnection(con.ConnectionString);
                    GenericDbConnection physconn = new GenericDbConnection(sql, fact);

                    IDatabaseSource conn = new GenericDatabaseSource(physconn);
                    //ServerConnectionHooks hooks = new ServerConnectionHooks(conn);
                    //conn.Hooks = hooks;
                    return new DatabaseSourceConnectionTreeNode(conn, parent, file);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            if (file.ToLower().EndsWith(".db3"))
            {
                SQLiteConnection sql = new SQLiteConnection(String.Format("Data Source={0}", file));
                GenericDbConnection physconn = new GenericDbConnection(sql, fact);
                IDatabaseSource conn = new GenericDatabaseSource(physconn);
                return new DatabaseSourceConnectionTreeNode(conn, parent, file);
            }
            return null;
        }

        #endregion
    }
}
