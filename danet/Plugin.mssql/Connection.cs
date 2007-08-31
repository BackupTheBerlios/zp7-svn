using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Data.SqlClient;
using System.Data.Common;
using DAIntf;

namespace Plugin.mssql
{
    public class Connection
    {
        public string ConnectionString;
        public bool OneDatabase;

        public void Save(string file)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Connection));
            using (FileStream fw = new FileStream(file, FileMode.Create))
            {
                ser.Serialize(fw, this);
            }
        }

        public static Connection Load(string file)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Connection));
            using (FileStream fr = new FileStream(file, FileMode.Open))
            {
                Connection con = (Connection)ser.Deserialize(fr);
                return con;
            }
        }
    }

    public class ServerConnectionHooks : HooksBase
    {
        IServerConnection m_conn;
        public ServerConnectionHooks(IServerConnection conn)
        {
            m_conn = conn;
        }
        public override void AfterOpen()
        {
            m_conn.SystemConnection.ChangeDatabase("master");
        }
    }

    [NodeFactory]
    public class ConnectionFactory : INodeFactory
    {

        #region INodeFactory Members

        public ITreeNode FromFile(ITreeNode parent, string file)
        {
            try
            {
                Connection con = Connection.Load(file);
                SqlConnection sql = new SqlConnection(con.ConnectionString);
                DbProviderFactory fact = SqlClientFactory.Instance;
                if (con.OneDatabase)
                {
                    throw new Exception("not implemented");
                    //string dbname = "master";
                    //return new DatabaseSourceTreeNode(new GenericDatabaseConnection(sql, dbname), parent, dbname);
                }
                else
                {
                    IServerConnection conn = new GenericServerConnection(sql, fact);
                    ServerConnectionHooks hooks = new ServerConnectionHooks(conn);
                    conn.Hooks = hooks;
                    return new ServerSourceConnectionTreeNode(conn, parent, file);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
