using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using System.Data.SQLite;
using System.Data;

namespace zp8
{
    [StaticSongFilter]
    public class PocketPCFormatter : SingleFileExporter
    {
        public override string Title
        {
            get { return "Pocket-PC databáze"; }
        }

        public override string Description
        {
            get { return "DB3 soubor vhodný pro Zpìvníkátor do Kapsy"; }
        }

        public override string FileDialogFilter
        {
            get { return "DB3 soubory (*.db3)|*.db3"; }
        }

        public override void Format(InetSongDb xmldb, Stream fw, IWaitDialog wait)
        {
            using (PocketPCExporter exp = new PocketPCExporter(xmldb, fw, wait))
            {
                exp.Run();
            }
        }
    }

    public class PocketPCExporter : IDisposable
    {
        InetSongDb m_xmldb;
        Stream m_fw;
        IWaitDialog m_wait;
        SQLiteConnection m_conn;
        string m_filename;
        SQLiteCommand m_insertGroup;
        SQLiteCommand m_insertSongName;
        SQLiteCommand m_insertSongDetail;

        public PocketPCExporter(InetSongDb xmldb, Stream fw, IWaitDialog wait)
        {
            m_filename = Path.Combine(DbManager.DbPath, "tmp" + DateTime.Now.Ticks + ".db3");
            m_xmldb = xmldb;
            m_fw = fw;
            m_wait = wait;
            m_conn = new SQLiteConnection(String.Format("Data Source={0};New=True;Version=3", m_filename));
            m_insertGroup = new SQLiteCommand("INSERT INTO groupnames (groupname) VALUES (@group)", m_conn);
            m_insertGroup.Parameters.Add("@group", DbType.String);

            m_insertSongName = new SQLiteCommand("INSERT INTO songnames (id, songname, groupname) VALUES (@id, @song, @group)", m_conn);
            m_insertSongName.Parameters.Add("@id", DbType.Int32);
            m_insertSongName.Parameters.Add("@song", DbType.String);
            m_insertSongName.Parameters.Add("@group", DbType.String);

            m_insertSongDetail = new SQLiteCommand("INSERT INTO songdetails (id, songtext, author, remark) VALUES (@id, @songtext, @author, @remark)", m_conn);
            m_insertSongDetail.Parameters.Add("@id", DbType.Int32);
            m_insertSongDetail.Parameters.Add("@songtext", DbType.String);
            m_insertSongDetail.Parameters.Add("@author", DbType.String);
            m_insertSongDetail.Parameters.Add("@remark", DbType.String);
        }

        private void ExecuteSql(string sql)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sql, m_conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void Run()
        {
            m_conn.Open();

            SQLiteTransaction t = m_conn.BeginTransaction();
            ExecuteSql("CREATE TABLE groupnames (groupname VARCHAR)");
            ExecuteSql("CREATE TABLE songnames (id INTEGER PRIMARY KEY, songname VARCHAR, groupname VARCHAR)");
            ExecuteSql("CREATE TABLE songdetails (id INTEGER PRIMARY KEY, songtext TEXT, author VARCHAR, remark VARCHAR)");
            ExecuteSql("CREATE INDEX songnames_group ON songnames (groupname, id)");
            DirectorySongHolder dsh = new DirectorySongHolder(m_xmldb.song.Rows);
            foreach (GroupOfSongs grp in dsh.SortedGroups)
            {
                m_insertGroup.Parameters[0].Value = grp.Name;
                m_insertGroup.ExecuteNonQuery();

                foreach (ISongRow song in grp.Songs)
                {
                    m_insertSongName.Parameters[0].Value = song.ID;
                    m_insertSongName.Parameters[1].Value = song.Title;
                    m_insertSongName.Parameters[2].Value = song.GroupName;
                    m_insertSongName.ExecuteNonQuery();

                    m_insertSongDetail.Parameters[0].Value = song.ID;
                    m_insertSongDetail.Parameters[1].Value = song.SongText;
                    m_insertSongDetail.Parameters[2].Value = song.Author;
                    m_insertSongDetail.Parameters[3].Value = song.Remark;
                    m_insertSongDetail.ExecuteNonQuery();
                }
            }
            t.Commit();

            m_conn.Close();
            using (FileStream fr = new FileStream(m_filename, FileMode.Open))
            {
                IOTools.CopyStream(fr, m_fw);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            File.Delete(m_filename);
        }

        #endregion
    }
}
