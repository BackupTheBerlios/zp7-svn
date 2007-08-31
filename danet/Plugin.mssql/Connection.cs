using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Data.SqlClient;
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
                if (con.OneDatabase)
                {
                    throw new Exception("not implemented");
                    //string dbname = "master";
                    //return new DatabaseSourceTreeNode(new GenericDatabaseConnection(sql, dbname), parent, dbname);
                }
                else
                {
                    return new ServerSourceConnectionTreeNode(new GenericServerConnection(sql), parent, file);
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
