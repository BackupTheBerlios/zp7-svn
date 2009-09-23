using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class SongsByGroupFrame : UserControl
    {
        SongDatabase m_db;
        SongDatabaseWrapper m_dbwrap;
        //List<zp8.SongDb.songRow> m_loadedSongs = new List<zp8.SongDb.songRow>();

        public SongsByGroupFrame()
        {
            InitializeComponent();
        }

        private void SongsByGroupFrame_Resize(object sender, EventArgs e)
        {
            lbgroups.Width = ClientSize.Width / 2;
        }

        public SongDatabaseWrapper SongDb
        {
            get { return m_dbwrap; }
            set
            {
                if (m_dbwrap != null)
                {
                    m_dbwrap.ChangedSongDatabase -= m_dbwrap_ChangedSongDatabase;
                    m_dbwrap.SongChanged -= m_dbwrap_SongChanged;
                }
                m_db = null;
                m_dbwrap = value;
                if (m_dbwrap != null)
                {
                    m_dbwrap.ChangedSongDatabase += m_dbwrap_ChangedSongDatabase;
                    m_dbwrap.SongChanged += m_dbwrap_SongChanged;
                    m_db = m_dbwrap.Database;
                }
                Reload();
            }
        }

        void m_dbwrap_SongChanged(object sender, EventArgs e)
        {
            //SelectCurrentSong();
        }

        public void Reload()
        {
            lbgroups.Items.Clear();
            if (m_db == null) return;
            using (var reader = m_db.ExecuteReader("select distinct groupname from song order by groupname"))
            {
                while (reader.Read())
                {
                    lbgroups.Items.Add(reader.SafeString(0));
                }
            }

            //Dictionary<string, bool> groups = new Dictionary<string, bool>();
            //lbgroups.Items.Clear();
            //lbsongs.Items.Clear();
            //if (m_dbwrap != null && m_dbwrap.Database != null)
            //{
            //    foreach (zp8.SongDb.songRow song in m_dbwrap.EnumVisibleSongs())
            //    {
            //        if (!groups.ContainsKey(song.GroupName))
            //        {
            //            groups[song.GroupName] = true;
            //        }
            //    }
            //    List<string> grps = new List<string>();
            //    grps.AddRange(groups.Keys);
            //    grps.Sort();
            //    foreach (string grp in grps) lbgroups.Items.Add(grp);
            //    SelectCurrentSong();
            //}
        }

        //private void SelectCurrentSong()
        //{
        //    //int index = m_dbwrap.SongBindingSource.Position;
        //    //if (index >= 0)
        //    //{
        //    //    zp8.SongDb.songRow song = m_dbwrap.SongByIndex(index);
        //    //    lbgroups.SelectedIndex = lbgroups.Items.IndexOf(song.GroupName);
        //    //    lbsongs.SelectedIndex = lbsongs.Items.IndexOf(song.Title);
        //    //}
        //}


        void m_dbwrap_ChangedSongDatabase(object sender, EventArgs e)
        {
            m_db = m_dbwrap.Database;
            Reload();
            //if (m_db != null) m_db.SongChanged -= m_db_SongChanged;
            //Reload();
            //m_db = db;
            //if (m_db != null) m_db.SongChanged += m_db_SongChanged;
        }

        void m_db_SongChanged(object sender, EventArgs e)
        {
            Reload();
        }

        private void lbgroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            int gindex = lbgroups.SelectedIndex;
            //m_loadedSongs.Clear();
            lbsongs.Items.Clear();
            if (m_db == null) return;

            if (gindex >= 0)
            {
                string grp = (string)lbgroups.Items[gindex];
                using (var reader = m_db.ExecuteReader("select ID, title from song where groupname=@grp", "grp", grp))
                {
                    while (reader.Read())
                    {
                        lbsongs.Items.Add(new SongRecord { ID = reader.SafeInt(0), Title = reader.SafeString(1) });
                    }
                }
                //m_dbwrap.SongID = 
                //foreach ( zp8.SongDb.songRow song in m_dbwrap.EnumVisibleSongs())
                //    if (song.GroupName == grp)
                //        m_loadedSongs.Add(song);
                //Sorting.Sort(m_loadedSongs, SongOrder.TitleGroup);
                //foreach (zp8.SongDb.songRow song in m_loadedSongs)
                //    lbsongs.Items.Add(song.Title);
                //if (m_dbwrap.SongBindingSource.Position >= 0)
                //{
                //    int relindex = m_loadedSongs.IndexOf(m_dbwrap.SongByIndex(m_dbwrap.SongBindingSource.Position));
                //    if (relindex >= 0) lbsongs.SelectedIndex = relindex;
                //}
            }
        }

        private void lbsongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbsongs.SelectedIndex == -1) return;
            var rec = lbsongs.SelectedItem as SongRecord;
            if (rec == null) return;
            m_dbwrap.SongID = rec.ID;
            //if (m_dbwrap.SongByIndex(m_dbwrap.SongBindingSource.Position) != m_loadedSongs[lbsongs.SelectedIndex])
            //{
            //    m_dbwrap.SongBindingSource.Position = m_dbwrap.SongIndex(m_loadedSongs[lbsongs.SelectedIndex]);
            //}
        }

        public void LoadCurrentSong()
        {
            if (m_db == null) return;
            string grp = m_db.ExecuteScalar("select groupname from song where ID = @id", "id", m_dbwrap.SongID).ToString();
            lbgroups.SelectedIndex = lbgroups.Items.IndexOf(grp);
            if (lbgroups.SelectedIndex >= 0)
            {
                int index = 0;
                foreach (SongRecord song in lbsongs.Items)
                {
                    if (song.ID == m_dbwrap.SongID)
                    {
                        lbsongs.SelectedIndex = index;
                        break;
                    }
                    index++;
                }
            }
        }

        class SongRecord
        {
            internal string Title;
            internal int ID;
            public override string ToString() { return Title; }
        }
    }
}
