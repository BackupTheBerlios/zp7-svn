using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class SongTable : UserControl
    {
        //SongDatabase m_dataset;
        SongDatabaseWrapper m_dbwrap;

        public SongTable()
        {
            InitializeComponent();
        }
        /*
        public void Bind(SongDatabase db)
        {
            songBindingSource.DataSource = db.DataSet.song;
            m_dataset = db;
            //songDbBindingSource.DataSource = db.DataSet;
            //songDbBindingSource.DataMember = "song";
        }
        */
        public SongDatabaseWrapper SongDb
        {
            get { return m_dbwrap; }
            set
            {
                //if (m_dbwrap != null) m_dbwrap.ChangedSongDatabase -= m_dbwrap_ChangedSongDatabase;
                m_dbwrap = value;
                if (m_dbwrap != null)
                {
                    dataGridView1.DataSource = m_dbwrap.SongBindingSource;
                }
                //m_dbwrap.ChangedSongDatabase += m_dbwrap_ChangedSongDatabase;
            }
        }

        /*
        void m_dbwrap_ChangedSongDatabase(SongDatabase db)
        {
            dataGridView1.DataSource = m_dbwrap.SongBindingSource;
        }
        */

        //public BindingSource GetBindingSource() { return songBindingSource; }
        //public SongDatabase GetDataSet() { return m_dataset; }
    }
}
