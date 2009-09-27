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
        int m_preserveRow;
        bool m_sorting;
        //SongDb.songRow m_sortPreserveRow;
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
                if (m_dbwrap != null)
                {
                    m_dbwrap.ChangedSongDatabase -= m_dbwrap_ChangedSongDatabase;
                    m_dbwrap.SongSetChanged -= m_dbwrap_SongSetChanged;
                }
                m_dbwrap = value;
                if (m_dbwrap != null)
                {
                    m_dbwrap.ChangedSongDatabase += m_dbwrap_ChangedSongDatabase;
                    m_dbwrap.SongSetChanged += m_dbwrap_SongSetChanged;
                }
            }
        }

        int m_songSetChangedRowId;
        void m_dbwrap_SongSetChanged(object sender, EventArgs e)
        {
            m_songSetChangedRowId = m_dbwrap.SongID;
            Reload();
            Application.Idle += Application_Idle_SongSetChanged;
        }

        void Application_Idle_SongSetChanged(object sender, EventArgs e)
        {
            SetCurrentSong(m_songSetChangedRowId);
            Application.Idle -= Application_Idle_SongSetChanged;
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
            //if (!m_dbwrap.CanEditSong(e.RowIndex)) e.Cancel = true;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //m_dbwrap.SongByIndex(e.RowIndex).localmodified = true;
        }

        private void viditelnéSloupceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VisibleColumnsForm.Run(dataGridView1);
        }
        public IEnumerable<SongData> GetSelectedSongs()
        {
            yield break;
            //foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            //{
            //    if (!row.IsNewRow) yield return m_dbwrap.SongByIndex(row.Index);
            //}
        }

        public IEnumerable<SongData> GetSelectedSongsOrFocused()
        {
            yield break;
            //if (dataGridView1.SelectedRows.Count > 0) return GetSelectedSongs();
            //else return new SongDb.songRow[] { m_dbwrap.SelectedSong };
        }

        private void SetCurrentSong(int songid)
        {
            using (IWaitDialog dlg = WaitForm.Show("Hledám aktuální píseò", false))
            {
                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    if (GetSongID(row) == songid)
                    {
                        int col = 0;
                        if (dataGridView1.CurrentCell != null) col = dataGridView1.CurrentCell.ColumnIndex;
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[col];
                        m_dbwrap.SongID = songid;
                        return;
                    }
                }
            }
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            try
            {
                if (m_preserveRow > 0)
                {
                    SetCurrentSong(m_preserveRow);
                }
            }
            finally
            {
                m_sorting = false;
            }
        }

        private int GetSongID(int rowindex)
        {
            if (rowindex < 0 || rowindex >= dataGridView1.Rows.Count) return 0;
            DataGridViewRow gridrow = dataGridView1.Rows[rowindex];
            DataRow row = ((DataRowView)gridrow.DataBoundItem).Row;
            return row.SafeInt(0);
        }

        private void SelectedRow(int rowindex)
        {
            if (rowindex < 0 || m_sorting) return;
            m_preserveRow = GetSongID(rowindex);
            m_dbwrap.SongID = m_preserveRow;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) m_sorting = true;
            else SelectedRow(e.RowIndex);
        }

        void m_dbwrap_ChangedSongDatabase(object sender, EventArgs e)
        {
            Reload();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                SelectedRow(dataGridView1.CurrentCell.RowIndex);
            }
        }

        //public BindingSource GetBindingSource() { return songBindingSource; }
        //public SongDatabase GetDataSet() { return m_dataset; }

        public void LoadCurrentSong()
        {
            SetCurrentSong(m_dbwrap.SongID);
        }

        public void Reload()
        {
            dataGridView1.DataSource = m_dbwrap.GetSongTable();
        }
    }
}
