using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Finisar.SQLite;

namespace zp8
{
    public class SongDbConnection
    {
        internal SQLiteConnection m_conn;
        public SongDbConnection(string filename)
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
        }
    }
}
