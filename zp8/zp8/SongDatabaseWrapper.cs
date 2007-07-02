using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace zp8
{
    public delegate void SongDatabaseChanged(SongDatabase db);

    public partial class SongDatabaseWrapper : Component
    {
        SongDatabase m_db;
        public SongDatabaseWrapper()
        {
            InitializeComponent();
        }

        public SongDatabaseWrapper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        public SongDatabase Database
        {
            get { return m_db; }
            set
            {
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
        public SongDb SongDb { get { return m_db.DataSet; } }
        public System.Windows.Forms.BindingSource SongBindingSource { get { return songbindingSource; } }
        public System.Windows.Forms.BindingSource ServerBindingSource { get { return serverbindingSource; } }
        public SongDb.songRow SelectedSong { get { return m_db.DataSet.song[songbindingSource.Position]; } }
        public SongDb.serverRow SelectedServer { get { return m_db.DataSet.server[serverbindingSource.Position]; } }
    }
}
