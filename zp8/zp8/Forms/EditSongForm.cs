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

        public EditSongForm(SongDatabase db, SongData song)
        {
            InitializeComponent();
            m_db = db;
            m_song = song;
            tbtitle.Text = m_song.Title;
            tbauthor.Text = m_song.Author;
            tbgroup.Text = m_song.GroupName;
            tbtext.Text = (m_song.SongText ?? "").Replace("\r", "").Replace("\n", "\r\n");
            tbremark.Text = m_song.Remark;
            //tblink_1.Text = m_song.Link_1;
            //tblink_2.Text = m_song.Link_2;
            songView1.SongText = m_song.OrigText;
            lbxLinks.Items.Clear();
            foreach (var data in m_song.GetData(SongDataType.Link))
            {
                lbxLinks.Items.Add(data.TextData);
            }
            if (m_db != null)
            {
                cbserver.Items.Clear();
                using (var reader = m_db.ExecuteReader("select id, url from server"))
                {
                    while (reader.Read())
                    {
                        cbserver.Items.Add(reader[0].ToString());
                    }
                }
                if (cbserver.Items.Count == 0)
                {
                    cbuseserver.Enabled = false;
                    cbserver.Enabled = false;
                }
                else
                {
                    int? serverid = (int?)m_db.ExecuteScalar("select server_id from song where id=@id", "id", m_song.LocalID);
                    cbuseserver.Checked = serverid != null;
                    if (serverid == null)
                    {
                        cbserver.Enabled = false;
                    }
                    else
                    {
                        cbserver.SelectedValue = serverid;
                    }
                    if (cbserver.SelectedIndex < 0) cbserver.SelectedIndex = 0;
                }
            }
            else
            {
                cbserver.Enabled = false;
            }
        }

        public static bool Run(SongDatabase db, SongData song)
        {
            EditSongForm win = new EditSongForm(db, song);
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
            m_song.Title = tbtitle.Text;
            m_song.Author = tbauthor.Text;
            m_song.GroupName = tbgroup.Text;
            m_song.OrigText = tbtext.Text.Replace("\r", "");
            m_song.Remark = tbremark.Text;
            m_song.DeleteData(SongDataType.Link);
            foreach (object link in lbxLinks.Items) m_song.AddLink(link.ToString());
            m_db.SaveSong(m_song, cbuseserver.Checked ? (int)cbserver.SelectedValue : (int?)null);
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (openLinkDialog.ShowDialog() == DialogResult.OK)
            {
                lbxLinks.Items.Add(openLinkDialog.FileName);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (lbxLinks.SelectedIndex >= 0) lbxLinks.Items.RemoveAt(lbxLinks.SelectedIndex);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lbxLinks.SelectedIndex >= 0)
            {
                openLinkDialog.FileName = lbxLinks.Items[lbxLinks.SelectedIndex].ToString();
                if (openLinkDialog.ShowDialog() == DialogResult.OK)
                {
                    lbxLinks.Items[lbxLinks.SelectedIndex] = openLinkDialog.FileName;
                }
            }
        }
    }
}
