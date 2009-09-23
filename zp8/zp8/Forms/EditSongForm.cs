using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class EditSongForm : Form
    {
        SongDatabase m_db;
        SongData m_song;

        public EditSongForm(SongDatabase db, int id)
        {
            InitializeComponent();
            m_db = db;
            m_song = m_db.LoadSong(id);
            tbtitle.Text = m_song.Title;
            tbauthor.Text = m_song.Author;
            tbgroup.Text = m_song.GroupName;
            tbtext.Text = m_song.SongText.Replace("\r", "").Replace("\n", "\r\n");
            tbremark.Text = m_song.Remark;
            //tblink_1.Text = m_song.Link_1;
            //tblink_2.Text = m_song.Link_2;
            songView1.SongText = m_song.SongText;

            //if (songdb != null)
            //{
            //    serverBindingSource.DataSource = songdb.server;
            //    if (cbserver.Items.Count == 0)
            //    {
            //        cbuseserver.Enabled = false;
            //        cbserver.Enabled = false;
            //    }
            //    else
            //    {
            //        cbuseserver.Checked = !m_song.Isserver_idNull();
            //        if (m_song.Isserver_idNull())
            //        {
            //            cbserver.Enabled = false;
            //        }
            //        else
            //        {
            //            cbserver.SelectedValue = m_song.server_id;
            //        }
            //        if (cbserver.SelectedIndex < 0) cbserver.SelectedIndex = 0;
            //    }
            //}
            //else
            //{
            //    cbserver.Enabled = false;
            //}
        }

        public static bool Run(SongDatabase db, int id)
        {
            EditSongForm win = new EditSongForm(db, id);
            return win.ShowDialog() == DialogResult.OK;
        }

        private void EditSongForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                zavøítToolStripMenuItem_Click(sender, e);
            }
        }

        private void zavøítToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void uložitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //m_song.Title = tbtitle.Text;
            //m_song.Author = tbauthor.Text;
            //m_song.GroupName = tbgroup.Text;
            //m_song.SongText = tbtext.Text.Replace("\r", "");
            //m_song.Remark = tbremark.Text;
            //m_song.link_1 = tblink_1.Text;
            //m_song.link_2 = tblink_2.Text;
            //if (cbuseserver.Checked)
            //{
            //    m_song.server_id = (int)cbserver.SelectedValue;
            //}
            //else
            //{
            //    m_song.Setserver_idNull();
            //}
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            songView1.SongText = tbtext.Text;
        }

        private void Transp(int d)
        {
            tbtext.Text = Chords.Transpose(tbtext.Text, (d + 12) % 12);
            songView1.SongText = tbtext.Text;
        }

        private void nahoruToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transp(-1);
        }

        private void nahoruToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Transp(1);
        }

        private void dolùOKvintuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transp(-7);
        }

        private void nahoruOKvintuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transp(7);
        }

        private void cbuseserver_CheckedChanged(object sender, EventArgs e)
        {
            cbserver.Enabled = cbuseserver.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openLinkDialog.FileName = tblink_1.Text;
            if (openLinkDialog.ShowDialog() == DialogResult.OK)
            {
                tblink_1.Text = openLinkDialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openLinkDialog.FileName = tblink_2.Text;
            if (openLinkDialog.ShowDialog() == DialogResult.OK)
            {
                tblink_2.Text = openLinkDialog.FileName;
            }
        }
    }
}
