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
        AbstractSongDatabase m_db;
        SongDatabaseWrapper m_dbwrap;
        List<zp8.SongDb.songRow> m_loadedSongs = new List<zp8.SongDb.songRow>();

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
                if (m_dbwrap != null) m_dbwrap.ChangedSongDatabase -= m_dbwrap_ChangedSongDatabase;
                m_dbwrap = value;
                Reload();
                m_dbwrap.ChangedSongDatabase += m_dbwrap_ChangedSongDatabase;
            }
        }
        public void Reload()
        {
            Dictionary<string, bool> groups = new Dictionary<string, bool>();
            lbgroups.Items.Clear();
            lbsongs.Items.Clear();
            if (m_dbwrap != null && m_dbwrap.Database != null)
            {
                foreach (zp8.SongDb.songRow song in m_dbwrap.EnumVisibleSongs())
                {
                    if (!groups.ContainsKey(song.groupname))
                    {
                        groups[song.groupname] = true;
                    }
                }
                List<string> grps = new List<string>();
                grps.AddRange(groups.Keys);
                grps.Sort();
                foreach (string grp in grps) lbgroups.Items.Add(grp);
                int index = m_dbwrap.SongBindingSource.Position;
                if (index >= 0)
                {
                    string grp = m_dbwrap.SongByIndex(index).groupname;
                    lbgroups.SelectedIndex = grps.IndexOf(grp);
                }
            }
        }
        

        void m_dbwrap_ChangedSongDatabase(AbstractSongDatabase db)
        {
            if (m_db != null) m_db.SongChanged -= m_db_SongChanged;
            Reload();
            m_db = db;
            if (m_db != null) m_db.SongChanged += m_db_SongChanged;
        }

        void m_db_SongChanged(object sender, EventArgs e)
        {
            Reload();
        }

        private void lbgroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            int gindex = lbgroups.SelectedIndex;
            m_loadedSongs.Clear();
            lbsongs.Items.Clear();

            if (gindex >= 0)
            {
                string grp = (string)lbgroups.Items[gindex];
                foreach (zp8.SongDb.songRow song in m_dbwrap.EnumVisibleSongs())
                    if (song.groupname == grp)
                        m_loadedSongs.Add(song);
                Sorting.Sort(m_loadedSongs, SongOrder.TitleGroup);
                foreach (zp8.SongDb.songRow song in m_loadedSongs)
                    lbsongs.Items.Add(song.title);
                if (m_dbwrap.SongBindingSource.Position >= 0)
                {
                    int relindex = m_loadedSongs.IndexOf(m_dbwrap.SongByIndex(m_dbwrap.SongBindingSource.Position));
                    if (relindex >= 0) lbsongs.SelectedIndex = relindex;
                }
            }
        }

        private void lbsongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_dbwrap.SongByIndex(m_dbwrap.SongBindingSource.Position) != m_loadedSongs[lbsongs.SelectedIndex])
            {
                m_dbwrap.SongBindingSource.Position = m_dbwrap.SongIndex(m_loadedSongs[lbsongs.SelectedIndex]);
            }
        }
    }
}
