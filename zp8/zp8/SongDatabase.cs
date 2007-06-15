using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

using Finisar.SQLite;

namespace zp8
{
    public class SongDatabase
    {
        internal SQLiteConnection m_conn;
        internal SongDb m_dataset;
        internal SQLiteDataAdapter m_adapter;

        public SongDatabase(string filename)
        {
            if (File.Exists(filename))
            {
                m_conn = new SQLiteConnection(String.Format("Data Source={0};Version=3", filename));
                m_conn.Open();
            }
            else
            {
                m_conn = new SQLiteConnection(String.Format("Data Source={0};New=True;Version=3", filename));
                m_conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE song (ID INTEGER PRIMARY KEY, title VARCHAR, groupname VARCHAR, author VARCHAR, songtext TEXT, lang VARCHAR)", m_conn);
                cmd.ExecuteNonQuery();
            }
            m_adapter = new SQLiteDataAdapter("SELECT * FROM song", m_conn);
            m_dataset = new SongDb();
            m_adapter.Fill(m_dataset.song);
        }
        public void ImportZp6File(string filename)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(XmlReader.Create(new StringReader(reqtype.Template)));
            XmlDocument result = new XmlDocument();
            StringBuilder sb = new StringBuilder();
            xslt.Transform(request, XmlWriter.Create(sb));
            XmlDocument newreq = new XmlDocument();
            newreq.LoadXml(sb.ToString());
            DbRequest.ProcessRequest(newreq, out response, out data, logger, sql);
        }
    }
}
