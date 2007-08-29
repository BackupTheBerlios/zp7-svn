using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using DAIntf;

namespace Plugin.mssql
{
    public class Connection
    {
        public string ConnectionString;
        public bool OneDatabase;
    }

    [NodeFactory]
    public class ConnectionFactory : INodeFactory
    {

        #region INodeFactory Members

        public ITreeNode FromFile(string file)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(Connection));
                using (FileStream fr = new FileStream(file, FileMode.Open))
                {
                    Connection con = (Connection)ser.Deserialize(fr);
                    return null;
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
