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
        //ContextMenuStrip m_strip;
        //int? m_selectedRow;
        //int? m_returningRow;

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
        public IEnumerable<SongDb.songRow> GetSelectedSongs()
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (!row.IsNewRow) yield return m_dbwrap.SongDb.song[row.Index];
            }
            if (m_returningRow != null)
            {
                yield return m_dbwrap.SongDb.song[m_returningRow.Value];
            }
            
        }
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && dataGridView1.SelectedRows.Count == 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                m_selectedRow = e.RowIndex;
            }
        }

        void m_strip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (m_selectedRow != null)
            {
                dataGridView1.Rows[(int)m_selectedRow].Selected = false;
                m_returningRow = m_selectedRow;
                m_selectedRow = null;
            }
        }

        private void SongTable_ContextMenuStripChanged(object sender, EventArgs e)
        {
            if (m_strip != null) m_strip.Closed -= m_strip_Closed;
            m_strip = ContextMenuStrip;
            if (m_strip != null) m_strip.Closed += m_strip_Closed;
        }
        */

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!m_dbwrap.CanEditSong(e.RowIndex)) e.Cancel = true;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            m_dbwrap.SongByIndex(e.RowIndex).localmodified = true;
        }

        private void viditelnéSloupceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VisibleColumnsForm.Run(dataGridView1);
        }
        public IEnumerable<SongDb.songRow> GetSelectedSongs()
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (!row.IsNewRow) yield return m_dbwrap.SongByIndex(row.Index);
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
