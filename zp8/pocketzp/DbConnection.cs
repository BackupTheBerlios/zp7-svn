using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;
using System.Data;

namespace pocketzp
{
    public class DbSong
    {
        DbConnection m_conn;
        string m_name;

        public string Name
        {
            get { return m_name; }
        }

        int m_id;
        public int Id
        {
            get { return m_id; }
        }

        string m_text;
        public string Text
        {
            get
            {
                if (m_text == null)
                {
                    m_text = m_conn.GetSongText(m_id);
                }
                return m_text;
            }
        }

        public DbSong(DbConnection conn, int id, string name)
        {
            m_conn = conn;
            m_id = id;
            m_name = name;
        }

        public override string ToString()
        {
            return m_name;
        }
    }

    public class DbGroup
    {
        List<DbSong> m_songs;
        DbConnection m_conn;

        string m_name;
        public DbGroup(DbConnection conn, string name)
        {
            m_name = name;
            m_conn = conn;
        }

        public IList<DbSong> Songs
        {
            get
            {
                if (m_songs == null)
                {
                    m_songs = new List<DbSong>();
                    using (DbDataReader reader = m_conn.GetSongs(m_name))
                    {
                        while (reader.Read())
                        {
                            m_songs.Add(new DbSong(m_conn, Int32.Parse(reader[0].ToString()), reader[1].ToString()));
                        }
                    }
                }
                return m_songs;
            }
        }

        public override string ToString()
        {
            return m_name;
        }
    }

    public class DbConnection
    {
        SQLiteConnection m_conn;
        string m_filename;
        List<DbGroup> m_groups;
        SQLiteCommand m_selectSongs;
        SQLiteCommand m_selectSongText;

        public DbConnection(string dbname)
        {
            m_filename = dbname + ".db3";
            m_conn = new SQLiteConnection(String.Format("Data Source={0};Version=3", m_filename));
            m_conn.Open();
            m_selectSongs = new SQLiteCommand("SELECT id, songname FROM songnames WHERE groupname=@group ORDER BY songname", m_conn);
            m_selectSongs.Parameters.Add("@group", DbType.String);
            m_selectSongText = new SQLiteCommand("SELECT songtext FROM songdetails WHERE id=@id", m_conn);
            m_selectSongText.Parameters.Add("@id", DbType.Int32);
        }

        internal DbDataReader GetSongs(string group)
        {
            m_selectSongs.Parameters[0].Value = group;
            return m_selectSongs.ExecuteReader();
        }

        internal string GetSongText(int songid)
        {
            m_selectSongText.Parameters[0].Value = songid;
            return m_selectSongText.ExecuteScalar().ToString();
        }

        public IList<DbGroup> Groups
        {
            get
            {
                if (m_groups == null)
                {
                    m_groups = new List<DbGroup>();
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM groupnames ORDER BY groupname", m_conn))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                m_groups.Add(new DbGroup(this, reader[0].ToString()));
                            }
                        }
                    }
                }
                return m_groups;
            }
        }
    }
}
