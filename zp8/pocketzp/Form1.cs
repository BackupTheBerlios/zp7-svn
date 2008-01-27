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
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            lbgroups.Width = ClientSize.Width / 2;
        }

        private void lbgroups_SelectedIndexChanged(object sender, EventArgs e)
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
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (lbsongs.SelectedIndex >= 0)
            {
                ViewForm frm = new ViewForm((DbSong)lbsongs.SelectedItem);
                frm.ShowDialog();
            }
        }
    }
}