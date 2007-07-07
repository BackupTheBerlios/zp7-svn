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
        SongDb.songRow m_song;

        public EditSongForm(SongDb.songRow song, SongDb songdb)
        {
            InitializeComponent();
            m_song = song;
            tbtitle.Text = song.title;
            tbauthor.Text = song.author;
            tbgroup.Text = song.groupname;
            tbtext.Text = song.songtext;
            songView1.SongText = song.songtext;

            if (songdb != null)
            {
                serverBindingSource.DataSource = songdb.server;
                if (cbserver.Items.Count == 0)
                {
                    cbuseserver.Enabled = false;
                    cbserver.Enabled = false;
                }
                else
                {
                    cbuseserver.Checked = !m_song.Isserver_idNull();
                    if (m_song.Isserver_idNull())
                    {
                        cbserver.Enabled = false;
                    }
                    else
                    {
                        cbserver.SelectedValue = m_song.server_id;
                    }
                    if (cbserver.SelectedIndex < 0) cbserver.SelectedIndex = 0;
                }
            }
            else
            {
                cbserver.Enabled = false;
            }
        }

        public static bool Run(SongDb.songRow song, SongDb songdb)
        {
            EditSongForm win = new EditSongForm(song, songdb);
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
            m_song.title = tbtitle.Text;
            m_song.author = tbauthor.Text;
            m_song.groupname = tbgroup.Text;
            m_song.songtext = tbtext.Text;
            if (cbuseserver.Checked)
            {
                m_song.server_id = (int)cbserver.SelectedValue;
            }
            else
            {
                m_song.Setserver_idNull();
            }
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
    }
}
