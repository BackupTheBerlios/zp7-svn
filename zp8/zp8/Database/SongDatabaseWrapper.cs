using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
                m_db = value;
                if (m_db != null)
                {
                    songbindingSource.DataSource = m_db.DataSet.song;
                    serverbindingSource.DataSource = m_db.DataSet.server;
                }
                else
                {
                    songbindingSource.DataSource = null;
                    serverbindingSource.DataSource = null;
                }

                if (ChangedSongDatabase != null)
                {
                    ChangedSongDatabase(m_db);
                }
            }
        }
        public event SongDatabaseChanged ChangedSongDatabase;
        [Browsable(false)]
        public SongDb SongDb { get { return m_db.DataSet; } }
        [Browsable(false)]
        public System.Windows.Forms.BindingSource SongBindingSource { get { return songbindingSource; } }
        [Browsable(false)]
        public System.Windows.Forms.BindingSource ServerBindingSource { get { return serverbindingSource; } }
        [Browsable(false)]
        public SongDb.songRow SelectedSong { get { return m_db.DataSet.song[songbindingSource.Position]; } }
        [Browsable(false)]
        public SongDb.serverRow SelectedServer { get { return m_db.DataSet.server[serverbindingSource.Position]; } }

        public bool CanEditSong(int index)
        {
            return m_db.CanEditSong(m_db.DataSet.song[index]);
        }
    }
}
