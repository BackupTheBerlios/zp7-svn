using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;

namespace zp8
{
    public delegate void SongDatabaseChanged(AbstractSongDatabase db);

    public partial class SongDatabaseWrapper : Component
    {
        AbstractSongDatabase m_db;
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
        public AbstractSongDatabase Database
        {
            get { return m_db; }
            set
            {
                if (m_db == value) return;

                if (m_db != null)
                {
                    m_db.SongChanged -= m_db_SongChanged;
                }

                m_db = value;
                if (m_db != null)
                {
                    m_db.SongChanged += m_db_SongChanged;
                    songbindingSource.DataSource = m_db.DataSet.song;
                    serverbindingSource.DataSource = m_db.DataSet.server;
                }
                else
                {
                    songbindingSource.DataSource = null;
                    serverbindingSource.DataSource = null;
                }

                if (ChangedSongDatabase != null) ChangedSongDatabase(m_db);
                if (SongChanged != null) SongChanged(this, new EventArgs());
            }
        }

        void m_db_SongChanged(object sender, EventArgs e)
        {
            if (SongChanged != null) SongChanged(sender, e);
        }
        public event SongDatabaseChanged ChangedSongDatabase;
        public event EventHandler SongChanged;

        [Browsable(false)]
        public SongDb SongDb { get { return m_db.DataSet; } }
        [Browsable(false)]
        public System.Windows.Forms.BindingSource SongBindingSource { get { return songbindingSource; } }
        [Browsable(false)]
        public System.Windows.Forms.BindingSource ServerBindingSource { get { return serverbindingSource; } }
        [Browsable(false)]
        public SongDb.songRow SelectedSong { get { return SongByIndex(songbindingSource.Position); } }
        [Browsable(false)]
        public SongDb.serverRow SelectedServer { get { return (zp8.SongDb.serverRow)ServerByIndex(serverbindingSource.Position); } }

        public bool CanEditSong(int index)
        {
            return m_db.CanEditSong(SongByIndex(index));
        }

        public SongDb.songRow SongByIndex(int index)
        {
            DataRowView view = (DataRowView)SongBindingSource.List[index];
            return (zp8.SongDb.songRow)view.Row;
        }

        public SongDb.serverRow ServerByIndex(int index)
        {
            DataRowView view = (DataRowView)ServerBindingSource.List[index];
            return (zp8.SongDb.serverRow)view.Row;
        }

    }
}
