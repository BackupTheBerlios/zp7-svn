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
    public interface ISongRow
    {
        int ID { get;set;}
        string Title { get;set;}
        string Author { get;set;}
        string GroupName { get;set;}
        string SongText { get;set;}
        string Lang { get;set;}
        string Remark { get;set;}
    }

    public class SongData : ISongRow
    {
        int m_ID = 0;
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        string m_title = "";
        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        string m_author = "";
        public string Author
        {
            get { return m_author; }
            set { m_author = value; }
        }

        string m_groupname = "";
        public string GroupName
        {
            get { return m_groupname; }
            set { m_groupname = value; }
        }

        string m_songtext = "";
        public string SongText
        {
            get { return m_songtext; }
            set { m_songtext = value; }
        }

        string m_lang = "";
        public string Lang
        {
            get { return m_lang; }
            set { m_lang = value; }
        }

        string m_remark = "";
        public string Remark
        {
            get { return m_remark; }
            set { m_remark = value; }
        }
    }


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
            stext += Searching.MakeSearchText(row.Title);
            stext += Searching.MakeSearchText(row.Author);
            stext += Searching.MakeSearchText(row.GroupName);
            stext += Searching.MakeSearchText(SongTool.RemoveChords(row.SongText));
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
            song.Title = "Nov� p�se�";
            song.GroupName = "";
            song.Author = "";
            song.SongText = "";
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
        private static void AddParameter(SQLiteCommand cmd, object value)
        {
            SQLiteParameter p = new SQLiteParameter();
            p.Value = value;
            cmd.Parameters.Add(p);
        }
        public void SetInfo(string name, string value)
        {
            SQLiteCommand cmd_del = new SQLiteCommand("DELETE FROM info WHERE name=?", m_conn);
            AddParameter(cmd_del, name);
            cmd_del.ExecuteNonQuery();

            SQLiteCommand cmd_ins = new SQLiteCommand("INSERT INTO info (name, value) VALUES (?, ?)", m_conn);
            AddParameter(cmd_ins, name);
            AddParameter(cmd_ins, value);
            cmd_ins.ExecuteNonQuery();
        }
        public string GetInfo(string name)
        {
            SQLiteCommand cmd = new SQLiteCommand("SELECT value FROM info WHERE name=?", m_conn);
            AddParameter(cmd, name);
            object res = cmd.ExecuteScalar();
            if (res is DBNull) return null;
            if (res == null) return null;
            return res.ToString();
        }
        public int GetDbVersion()
        {
            try
            {
                return Int32.Parse(GetInfo("dbversion"));
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private void Upgrade(int version)
        {
            if (version < 1)
            {
                ExecuteSql("ALTER TABLE song ADD remark VARCHAR");
                ExecuteSql("CREATE TABLE info (name VARCHAR PRIMARY KEY, value TEXT)");
                SetInfo("dbversion", "1");
            }
            if (version < 2)
            {
                ExecuteSql("ALTER TABLE song ADD link_1 VARCHAR");
                ExecuteSql("ALTER TABLE song ADD link_2 VARCHAR");
                SetInfo("dbversion", "2");
            }
        }
        protected override void WantOpen()
        {
            if (m_opened) return;
            using (IWaitDialog wait = WaitForm.Show("Na��t�m datab�zi " + Path.GetFileName(m_filename), false))
            {
                if (File.Exists(m_filename))
                {
                    m_conn = new SQLiteConnection(String.Format("Data Source={0};Version=3", m_filename));
                    m_conn.Open();
                    int version = GetDbVersion();
                    Upgrade(version);
                }
                else
                {
                    m_conn = new SQLiteConnection(String.Format("Data Source={0};New=True;Version=3", m_filename));
                    m_conn.Open();
                    ExecuteSql("CREATE TABLE song (ID INTEGER PRIMARY KEY, title VARCHAR, groupname VARCHAR, author VARCHAR, songtext TEXT, lang VARCHAR, server_id INT NULL, netID INT NULL, transp INT, searchtext VARCHAR, published DATETIME, localmodified INT, remark VARCHAR, link_1 VARCHAR, link_2 VARCHAR)");
                    ExecuteSql("CREATE TABLE server (ID INTEGER PRIMARY KEY, url VARCHAR, servertype VARCHAR, config TEXT, isreadonly INT)");
                    ExecuteSql("CREATE TABLE deletedsong (ID INTEGER PRIMARY KEY, song_netID INT, server_id INT)");
                    ExecuteSql("CREATE TABLE info (name VARCHAR PRIMARY KEY, value TEXT)");
                    SetInfo("dbversion", "1");
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
            using (IWaitDialog wait = WaitForm.Show("Ukl�d�m datab�zi " + Path.GetFileName(m_filename), false))
            {
                WantOpen();
                UnInstallTriggers();
                SQLiteTransaction t = m_conn.BeginTransaction();
                m_song_adapter.Update(m_dataset.song);
                m_server_adapter.Update(m_dataset.server);
                t.Commit();
                InstallTriggers();
            }
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
