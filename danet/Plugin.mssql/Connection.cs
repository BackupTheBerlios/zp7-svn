using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Data.SqlClient;
using System.Data.Common;
using DatAdmin;

namespace Plugin.mssql
{
    public class MsSqlStoredConnection : StoredConnection<MsSqlStoredConnection>
    {
        public string ConnectionString;
        public bool OneDatabase;
    }

    //public class ServerConnectionHooks : HooksBase
    //{
    //    IServerSource m_conn;
    //    public ServerConnectionHooks(IServerSource conn)
    //    {
    //        m_conn = conn;
    //    }
    //    public override void AfterOpen()
    //    {
    //        m_conn.SystemConnection.ChangeDatabase("master");
    //    }
    //}

    [NodeFactory]
    public class ConnectionFactory : INodeFactory
    {

        #region INodeFactory Members

        public ITreeNode FromFile(ITreeNode parent, string file)
        {
            if (file.ToLower().EndsWith(".con"))
            {
                try
                {
                    MsSqlStoredConnection con = MsSqlStoredConnection.Load(file);
                    SqlConnection sql = new SqlConnection(con.ConnectionString);
                    DbProviderFactory fact = SqlClientFactory.Instance;
                    GenericDbConnection phys = new GenericDbConnection(sql, fact);
                    if (con.OneDatabase)
                    {
                        throw new Exception("not implemented");
                        //string dbname = "master";
                        //return new DatabaseSourceTreeNode(new GenericDatabaseConnection(sql, dbname), parent, dbname);
                    }
                    else
                    {
                        IServerSource conn = new GenericServerSource(phys);
                        //ServerConnectionHooks hooks = new ServerConnectionHooks(conn);
                        //conn.Hooks = hooks;
                        return new ServerSourceConnectionTreeNode(conn, parent, file);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        #endregion
    }
}
