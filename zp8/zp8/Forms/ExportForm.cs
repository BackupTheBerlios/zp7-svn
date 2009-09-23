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
        //InetSongDb m_db;
        //SongDatabaseWrapper m_dbwrap;
        //IEnumerable<SongDb.songRow> m_selected;
        List<ISongFormatter> m_types = new List<ISongFormatter>();
        object m_dynamciProperties;

        public ExportForm(SongDatabase dbwrap, IEnumerable<SongData> selected)
        {
            InitializeComponent();
            //m_dbwrap = dbwrap;
            //m_selected = selected;
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

        private void AddSongs(IEnumerable rows)
        {
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
            //m_db = new InetSongDb();
            //if (rbcurrentsong.Checked) AddSongs(new SongDb.songRow[] { m_dbwrap.SelectedSong });
            //if (rbselectedsongs.Checked) AddSongs(m_selected);
            //if (rbwholedb.Checked) AddSongs(m_dbwrap.Database.DataSet.song);
            //if (rbcondition.Checked) AddSongs(m_dbwrap.Database.DataSet.song.Select(tbcondition.Text));
            //songBindingSource.DataSource = m_db.song;
        }

        public static void Run(SongDatabase dbwrap, IEnumerable<SongData> selected)
        {
            ExportForm frm = new ExportForm(dbwrap, selected);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                using (IWaitDialog wait = WaitForm.Show("Exportování písní", true))
                {
                    ISongFormatter exp = frm.m_types[frm.lbformat.SelectedIndex];
                    //exp.Format(frm.m_db, frm.m_dynamciProperties, wait);
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
