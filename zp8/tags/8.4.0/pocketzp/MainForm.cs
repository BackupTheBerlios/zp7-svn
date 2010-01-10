using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

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

            miselectdb.MenuItems.Clear();
            foreach (string fn in Directory.GetFiles(Tools.DbPath))
            {
                string name = Tools.PureName(fn);
                MenuItem item = new MenuItem();
                item.Text = name;
                item.Click += new EventHandler(item_Click);
                miselectdb.MenuItems.Add(item);
            }
        }

        private void FillGroups()
        {
            try
            {
                lbgroups.BeginUpdate();
                lbgroups.Items.Clear();
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

        private void miselectdb_Popup(object sender, EventArgs e)
        {
        }

        void item_Click(object sender, EventArgs e)
        {
            SelectDb(((MenuItem)sender).Text);
        }

        private void SelectDb(string name)
        {
            m_conn = new DbConnection(name);
            FillGroups();
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}