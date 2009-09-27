using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Linq;
using System.Data.SQLite;

namespace zp8
{
    // wrapper nese informaci o aktualni databazi, pisni a filtrovani
    public partial class SongDatabaseWrapper : Component
    {
        SongDatabase m_db;
        int m_selectedSong;
        string m_searchText;

        public SongDatabaseWrapper()
        {
            InitializeComponent();
        }

        public SongDatabaseWrapper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        [Browsable(false)]
        public SongDatabase Database
        {
            get { return m_db; }
            set
            {
                if (m_db == value) return;
                m_db = value;
                m_selectedSong = 0;
                if (ChangedSongDatabase != null) ChangedSongDatabase(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        public int SongID
        {
            get { return m_selectedSong; }
            set
            {
                m_selectedSong = value;
                if (SongChanged != null) SongChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler ChangedSongDatabase;
        public event EventHandler SongChanged;
        public event EventHandler SongSetChanged;

        public DataTable GetSongTable()
        {
            string cond = "";
            if (m_searchText != null)
            {
                var idx = m_db.SearchIndex;
                var ids = idx.SearchText(m_searchText);
                cond = " where ID in (0," + String.Join(",", (from i in ids select i.ToString()).ToArray()) + ")";
            }

            using (IWaitDialog dlg = WaitForm.Show("Naèítám data", false))
            {
                SQLiteDataAdapter adapter = new SQLiteDataAdapter("select " + SongAccessor.GetSongFields(true) + " from song " + cond, Database.Connection);
                DataTable res = new DataTable();
                adapter.Fill(res);
                return res;
            }
        }

        public SongData CurrentSong
        {
            get
            {
                if (SongID == 0) return null;
                return Database.LoadSong(SongID);
            }
        }

        public string SearchText
        {
            get { return m_searchText; }
            set
            {
                m_searchText = value;
                if (SongSetChanged != null) SongSetChanged(this, EventArgs.Empty);
            }
        }
    }

    //public class ChangedSongDatabaseEventArgs : EventArgs
    //{
    //    public SongDatabase Database;
    //}
    //public delegate void ChangedSongDatabaseEvent(object sender, ChangedSongDatabaseEventArgs e);
}
