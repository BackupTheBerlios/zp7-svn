using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Resources;

using Finisar.SQLite;

namespace zp8
{
    public class SongDatabase
    {
        internal SQLiteConnection m_conn;
        internal SongDb m_dataset;
        internal SQLiteDataAdapter m_adapter;
        string m_filename;
        bool m_opened = false;

        public SongDatabase(string filename)
        {
            m_filename = filename;
        }
        private void WantOpen()
        {
            if (m_opened) return;
            if (File.Exists(m_filename))
            {
                m_conn = new SQLiteConnection(String.Format("Data Source={0};Version=3", m_filename));
                m_conn.Open();
            }
            else
            {
                m_conn = new SQLiteConnection(String.Format("Data Source={0};New=True;Version=3", m_filename));
                m_conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE song (ID INTEGER PRIMARY KEY, title VARCHAR, groupname VARCHAR, author VARCHAR, songtext TEXT, lang VARCHAR)", m_conn);
                cmd.ExecuteNonQuery();
            }
            m_adapter = new SQLiteDataAdapter("SELECT * FROM song", m_conn);
            m_dataset = new SongDb();
            m_adapter.Fill(m_dataset.song);
            m_opened = true;
        }
        public void ImportZp6File(string filename)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            ResourceManager mgr = new ResourceManager(typeof(SongDatabase));
            string zp6_to_zp8 = mgr.GetString("zp6_tp_zp8");
            xslt.Load(XmlReader.Create(new StringReader(zp6_to_zp8)));
            XmlDocument result = new XmlDocument();
            StringBuilder sb = new StringBuilder();
            XmlDocument zp6doc = new XmlDocument();
            zp6doc.Load(filename);
            xslt.Transform(zp6doc, XmlWriter.Create(sb));
            using (StringReader sr = new StringReader(sb.ToString()))
            {
                ImportZp8Xml(sr);
            }
        }

        public void ImportZp8Xml(StringReader fr)
        {
            SQLiteTransaction t = m_conn.BeginTransaction();
            m_dataset.song.ReadXml(fr);
            m_adapter.Update(m_dataset.song);
            t.Commit();
        }
        public static string ExtractDbName(string filename) {return Path.GetFileName(filename).ToLower(); }
        public string Name { get { return ExtractDbName(m_filename); } }
    }
}
