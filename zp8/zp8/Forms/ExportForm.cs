using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace zp8
{
    public partial class ExportForm : Form
    {
        InetSongDb m_db;
        SongDatabaseWrapper m_dbwrap;
        List<int> m_selected;
        //IEnumerable<SongDb.songRow> m_selected;
        List<ISongFormatter> m_types = new List<ISongFormatter>();
        object m_dynamciProperties;

        public ExportForm(SongDatabaseWrapper dbwrap, List<int> selected)
        {
            InitializeComponent();
            m_dbwrap = dbwrap;
            m_selected = selected;
            foreach (ISongFormatter exp in SongFilters.EnumFilters<ISongFormatter>())
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

        private void AddSongs(IEnumerable<SongData> songs)
        {
            foreach (var song in songs)
            {
                m_db.Songs.Add(song);
            }
        }

        private void AddSongs(List<int> rows)
        {
            AddSongs(m_dbwrap.Database.LoadSongs(rows));
            //foreach (SongDb.songRow src in rows)
            //{
            //    if (src.RowState == DataRowState.Deleted || src.RowState == DataRowState.Detached) continue;
            //    InetSongDb.songRow dst = m_db.song.NewsongRow();
            //    DbTools.CopySong(src, dst);
            //    dst.published = DateTime.UtcNow;
            //    m_db.song.AddsongRow(dst);
            //}
        }

        private void wizardPage2_ShowFromNext(object sender, EventArgs e)
        {
            m_db = new InetSongDb();
            if (rbcurrentsong.Checked) AddSongs(new List<int> { m_dbwrap.SongID });
            if (rbselectedsongs.Checked) AddSongs(m_selected);
            if (rbwholedb.Checked) AddSongs(m_dbwrap.Database.LoadSongs(null, "", "1=1"));
            if (rbcondition.Checked) AddSongs(m_dbwrap.Database.LoadSongs(null, "", tbcondition.Text));
            dataGridView1.DataSource = m_db.GetAsTable();
        }

        public static void Run(SongDatabaseWrapper dbwrap, List<int> selected)
        {
            ExportForm frm = new ExportForm(dbwrap, selected);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                using (IWaitDialog wait = WaitForm.Show("Exportování písní", true))
                {
                    ISongFormatter exp = frm.m_types[frm.lbformat.SelectedIndex];
                    exp.Format(frm.m_db, frm.m_dynamciProperties, wait);
                }
            }
        }

        private void lbformat_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_dynamciProperties = m_types[lbformat.SelectedIndex].CreateDynamicProperties();
            propertyGrid1.SelectedObject = m_dynamciProperties;
        }
    }
}
