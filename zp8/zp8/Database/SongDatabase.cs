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
    public abstract partial class AbstractSongDatabase
    {
        protected SongDb m_dataset;
        protected abstract void WantOpen();
        private bool m_disableTriggers;

        public event EventHandler SongChanged;

        public SongDb DataSet
        {
            get
            {
                WantOpen();
                return m_dataset;
            }
        }
        protected void UnInstallTriggers()
        {
            m_dataset.song.RowChanged -= song_RowChanged;
        }
        protected void InstallTriggers()
        {
            foreach (SongDb.songRow row in EnumSongs())
            {
                if (row.IssearchtextNull()) row.searchtext = MakeSearchText(row);
            }
            m_dataset.song.RowChanged += song_RowChanged;
            if (SongChanged != null) SongChanged(this, new EventArgs());
        }

        private static string MakeSearchText(SongDb.songRow row)
        {
            string stext = "";
            stext += Searching.MakeSearchText(row.title);
            stext += Searching.MakeSearchText(row.author);
            stext += Searching.MakeSearchText(row.groupname);
            stext += Searching.MakeSearchText(SongTool.RemoveChords(row.songtext));
            return stext;
        }

        void song_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (m_disableTriggers) return;
            if (SongChanged != null) SongChanged(sender, e);
            SongDb.songRow row = (SongDb.songRow)e.Row;
            if (row.RowState != DataRowState.Modified && row.RowState != DataRowState.Added) return;
            try
            {
                m_disableTriggers = true;
                row.searchtext = MakeSearchText(row);
            }
            finally
            {
                m_disableTriggers = false;
            }

        }
        public bool CanEditSong(SongDb.songRow song)
        {
            if (song.Isserver_idNull()) return true;
            int srvid = song.server_id;
            try
            {
                return !m_dataset.server.FindByID(srvid).isreadonly;
            }
            catch (Exception)
            {
                return true;
            }
        }
        public SongDb.songRow CreateSong()
        {
            SongDb.songRow song = DataSet.song.NewsongRow();
            song.title = "Nová píseò";
            song.groupname = "";
            song.author = "";
            song.songtext = "";
            DataSet.song.AddsongRow(song);
            return song;
        }
        public IEnumerable<SongDb.songRow> EnumSongs()
        {
            foreach (SongDb.songRow row in m_dataset.song.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached) yield return row;
            }
        }
    }

    public class SongDatabase : AbstractSongDatabase
    {
        SQLiteConnection m_conn;
        SQLiteDataAdapter m_song_adapter;
        SQLiteDataAdapter m_server_adapter;
        SQLiteDataAdapter m_deletedsong_adapter;
        string m_filename;
        bool m_opened = false;

        public SongDatabase(string filename)
        {
            m_filename = filename;
        }
        private void ExecuteSql(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, m_conn);
            cmd.ExecuteNonQuery();
        }
        protected override void WantOpen()
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
                ExecuteSql("CREATE TABLE song (ID INTEGER PRIMARY KEY, title VARCHAR, groupname VARCHAR, author VARCHAR, songtext TEXT, lang VARCHAR, server_id INT NULL, netID INT NULL, transp INT, searchtext VARCHAR, published DATETIME, localmodified INT)");
                ExecuteSql("CREATE TABLE server (ID INTEGER PRIMARY KEY, url VARCHAR, servertype VARCHAR, config TEXT, isreadonly INT)");
                ExecuteSql("CREATE TABLE deletedsong (ID INTEGER PRIMARY KEY, song_netID INT, server_id INT)");
            }
            m_song_adapter = new SQLiteDataAdapter("SELECT * FROM song", m_conn);
            m_server_adapter = new SQLiteDataAdapter("SELECT * FROM server", m_conn);
            m_deletedsong_adapter = new SQLiteDataAdapter("SELECT * FROM deletedsong", m_conn);
            m_dataset = new SongDb();
            m_song_adapter.Fill(m_dataset.song);
            m_server_adapter.Fill(m_dataset.server);
            m_deletedsong_adapter.Fill(m_dataset.deletedsong);

            SQLiteCommandBuilder song_cb = new SQLiteCommandBuilder(m_song_adapter);
            m_song_adapter.InsertCommand = (SQLiteCommand)song_cb.GetInsertCommand();

            SQLiteCommandBuilder server_cb = new SQLiteCommandBuilder(m_server_adapter);
            m_server_adapter.InsertCommand = (SQLiteCommand)server_cb.GetInsertCommand();

            SQLiteCommandBuilder deletedsong_cb = new SQLiteCommandBuilder(m_deletedsong_adapter);
            m_deletedsong_adapter.InsertCommand = (SQLiteCommand)deletedsong_cb.GetInsertCommand();

            m_opened = true;
            InstallTriggers();
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
            UnInstallTriggers();
            SQLiteTransaction t = m_conn.BeginTransaction();
            m_song_adapter.Update(m_dataset.song);
            m_server_adapter.Update(m_dataset.server);
            t.Commit();
            InstallTriggers();
        }
        public bool Modified
        {
            get
            {
                if (!m_opened) return false;
                return m_dataset.HasChanges();
            }
        }
    }
}
