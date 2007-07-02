using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Resources;
using System.Windows.Forms;
using System.Data;

using System.Data.SQLite;

namespace zp8
{
    /*
    public interface ISongSource
    {
        BindingSource GetBindingSource();
        SongDatabase GetDataSet();
    }
    */
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

        /*
        public void ImportZp8Xml(StringReader fr)
        {
            m_dataset.song.ReadXml(fr);
        }
        */

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

        public void ImportSongs(Stream fr, int? serverid)
        {
            InetSongDb xmldb = new InetSongDb();
            xmldb.ReadXml(fr);
            ImportSongs(serverid, xmldb);
        }

        public void ImportSongs(TextReader fr, int? serverid)
        {
            InetSongDb xmldb = new InetSongDb();
            xmldb.ReadXml(fr);
            ImportSongs(serverid, xmldb);
        }

        private void ImportSongs(int? serverid, InetSongDb xmldb)
        {
            foreach (InetSongDb.songRow row in xmldb.song.Rows)
            {
                SongDb.songRow newrow = DataSet.song.NewsongRow();
                newrow.title = row.title;
                newrow.groupname = row.groupname;
                newrow.author = row.author;
                newrow.songtext = row.songtext;
                if (serverid.HasValue) newrow.server_id = serverid.Value;
                newrow.lang = row.lang;
                newrow.netID = row.ID;
                DataSet.song.AddsongRow(newrow);
            }
        }

        public void GetSongsAsInetXml(int serverid, Stream fw)
        {
            InetSongDb xmldb = new InetSongDb();
            foreach (SongDb.songRow row in m_dataset.song.Rows)
            {
                if (row.server_id == serverid)
                {
                    InetSongDb.songRow newrow = xmldb.song.NewsongRow();
                    newrow.title = row.title;
                    newrow.groupname = row.groupname;
                    newrow.author = row.author;
                    newrow.songtext = row.songtext;
                    newrow.lang = row.lang;
                    newrow.ID = row.netID;
                    xmldb.song.AddsongRow(newrow);
                }
            }
            xmldb.WriteXml(fw);
        }
    }
}
