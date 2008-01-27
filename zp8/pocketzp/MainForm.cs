using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace pocketzp
{
    public partial class MainForm : Form
    {
        DbConnection m_conn;

        public MainForm()
        {
            InitializeComponent();
            m_conn = new DbConnection("songs");
            FillGroups();
        }

        private void FillGroups()
        {
            try
            {
                lbgroups.BeginUpdate();
                foreach (DbGroup grp in m_conn.Groups)
                {
                    lbgroups.Items.Add(grp);
                }
            }
            finally
            {
                lbgroups.EndUpdate();
            }
            if (lbgroups.Items.Count >= 0) lbgroups.SelectedIndex = 0;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            lbgroups.Width = ClientSize.Width / 2;
        }

        private void lbgroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            tmloadsongs.Enabled = false;
            tmloadsongs.Enabled = true;
        }

        private void LoadSongs()
        {
            if (lbgroups.SelectedIndex >= 0)
            {
                try
                {
                    lbsongs.BeginUpdate();
                    lbsongs.Items.Clear();
                    DbGroup grp = (DbGroup)lbgroups.SelectedItem;
                    foreach (DbSong song in grp.Songs)
                    {
                        lbsongs.Items.Add(song);
                    }
                }
                finally
                {
                    lbsongs.EndUpdate();
                }
            }
            if (lbsongs.Items.Count >= 0) lbsongs.SelectedIndex = 0;
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            ViewSong();
        }

        private void ViewSong()
        {
            if (lbsongs.SelectedIndex >= 0)
            {
                ViewForm frm = new ViewForm((DbSong)lbsongs.SelectedItem);
                frm.ShowDialog();
            }
        }

        private void lbgroups_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                lbsongs.Focus();
                e.Handled = true;
            }
        }

        private void lbsongs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                lbgroups.Focus();
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Enter)
            {
                ViewSong();
            }
        }

        private void tmloadsongs_Tick(object sender, EventArgs e)
        {
            tmloadsongs.Enabled = false;
            LoadSongs();
        }
    }
}