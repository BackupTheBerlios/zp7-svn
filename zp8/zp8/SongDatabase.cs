using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Resources;
using System.Windows.Forms;
using System.Data;

using System.Data.SQLite;

namespace zp8
{
    public interface ISongSource
    {
        BindingSource GetBindingSource();
        SongDatabase GetDataSet();
    }
    public class SongDatabase
    {
        SQLiteConnection m_conn;
        SongDb m_dataset;
        SQLiteDataAdapter m_song_adapter;
        SQLiteDataAdapter m_server_adapter;
        string m_filename;
        bool m_opened = false;

        public SongDatabase(string filename)
        {
            m_filename = filename;
        }
        public SongDb DataSet
        {
            get
            {
                WantOpen();
                return m_dataset;
            }
        }
        private void ExecuteSql(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, m_conn);
            cmd.ExecuteNonQuery();
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
                ExecuteSql("CREATE TABLE song (ID INTEGER PRIMARY KEY, title VARCHAR, groupname VARCHAR, author VARCHAR, songtext TEXT, lang VARCHAR, server_id INT NULL)");
                ExecuteSql("CREATE TABLE server (ID INTEGER PRIMARY KEY, url VARCHAR, servertype VARCHAR, config TEXT)");
            }
            m_song_adapter = new SQLiteDataAdapter("SELECT * FROM song", m_conn);
            m_server_adapter = new SQLiteDataAdapter("SELECT * FROM server", m_conn);
            m_dataset = new SongDb();
            m_song_adapter.Fill(m_dataset.song);
            m_server_adapter.Fill(m_dataset.server);

            SQLiteCommandBuilder song_cb = new SQLiteCommandBuilder(m_song_adapter);
            m_song_adapter.InsertCommand = (SQLiteCommand)song_cb.GetInsertCommand();

            SQLiteCommandBuilder server_cb = new SQLiteCommandBuilder(m_server_adapter);
            m_server_adapter.InsertCommand = (SQLiteCommand)server_cb.GetInsertCommand();

            m_opened = true;
        }
        public void ImportZp6File(string filename)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(XmlReader.Create(new StringReader(xsls.zp6_to_zp8)));
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
            m_dataset.song.ReadXml(fr);
        }
        public static string ExtractDbName(string filename) {return Path.GetFileName(filename).ToLower(); }
        public string Name { get { return ExtractDbName(m_filename); } }
        public void Commit()
        {
            WantOpen();
            SQLiteTransaction t = m_conn.BeginTransaction();
            m_song_adapter.Update(m_dataset.song);
            m_server_adapter.Update(m_dataset.server);
            t.Commit();
        }
        public bool Modified
        {
            get
            {
                if (!m_opened) return false;
                return m_dataset.HasChanges();
            }
        }
        public void DeleteSongsFromServer(int server)
        {
            foreach (SongDb.songRow row in m_dataset.song.Rows)
            {
                if (row.server_id == server) row.Delete();
            }
        }
    }
}
