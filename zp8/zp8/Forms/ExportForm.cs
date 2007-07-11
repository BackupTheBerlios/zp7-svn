using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace zp8
{
    public partial class ExportForm : Form
    {
        InetSongDb m_db;
        SongDatabaseWrapper m_dbwrap;
        IEnumerable<SongDb.songRow> m_selected;
        List<IDbExportType> m_types = new List<IDbExportType>();

        public ExportForm(SongDatabaseWrapper dbwrap, IEnumerable<SongDb.songRow> selected)
        {
            InitializeComponent();
            m_dbwrap = dbwrap;
            m_selected = selected;
            foreach (IDbExportType exp in DbExport.Types)
            {
                m_types.Add(exp);
                lbformat.Items.Add(exp.Title);
            }
            lbformat.SelectedIndex = 0;
        }

        private void rbcondition_CheckedChanged(object sender, EventArgs e)
        {
            tbcondition.Enabled = rbcondition.Checked;
        }

        private void AddSongs(IEnumerable rows)
        {
            foreach (SongDb.songRow src in rows)
            {
                InetSongDb.songRow dst = m_db.song.NewsongRow();
                DbTools.LocalSongRowToInetSongRow(src, dst);
                m_db.song.AddsongRow(dst);
            }
        }

        private void wizardPage2_ShowFromNext(object sender, EventArgs e)
        {
            m_db = new InetSongDb();
            if (rbcurrentsong.Checked) AddSongs(new SongDb.songRow[] { m_dbwrap.SelectedSong });
            if (rbselectedsongs.Checked) AddSongs(m_selected);
            if (rbwholedb.Checked) AddSongs(m_dbwrap.Database.DataSet.song);
            if (rbcondition.Checked) AddSongs(m_dbwrap.Database.DataSet.song.Select(tbcondition.Text));
            songBindingSource.DataSource = m_db.song;
        }

        public static void Run(SongDatabaseWrapper dbwrap, IEnumerable<SongDb.songRow> selected)
        {
            ExportForm frm = new ExportForm(dbwrap, selected);
            while (frm.tbfilename.Text == "")
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    IDbExportType exp = frm.m_types[frm.lbformat.SelectedIndex];
                    exp.Run(frm.m_db, frm.tbfilename.Text);
                }
                else
                {
                    break;
                }

            }
        }

        private void lbformat_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = m_types[lbformat.SelectedIndex].FileDialogFilter;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbfilename.Text = saveFileDialog1.FileName;
            }
        }
    }
}