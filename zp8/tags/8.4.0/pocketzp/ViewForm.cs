using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace pocketzp
{
    public partial class ViewForm : Form
    {
        DbSong m_song;
        PaneGrp m_panegrp;
        int m_transp = 0;

        public ViewForm(DbSong song)
        {
            InitializeComponent();
            m_song = song;
            Text = m_song.Name;
        }

        private void Reformat(Graphics g)
        {
            panel1.Width = ClientSize.Width;
            string text = m_song.Text;
            if (m_transp != 0) text = Chords.Transpose(text, m_transp);
            SongFormatter fmt = new SongFormatter(text, new SongFormatOptions(panel1.Width - 8, g));
            fmt.Run();
            m_panegrp = fmt.Result;
            panel1.Height = (int)m_panegrp.FullHeight + 1;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (m_panegrp == null) Reformat(e.Graphics);
            if (m_panegrp != null) m_panegrp.Draw(e.Graphics, -AutoScrollPosition.Y, ClientSize.Height - 16);
        }

        private void ViewForm_Resize(object sender, EventArgs e)
        {
            ClearFormat();
        }

        private void ClearFormat()
        {
            m_panegrp = null;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            m_transp -= 1;
            if (m_transp < 0) m_transp += 12;
            ClearFormat();
            panel1.Refresh();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            m_transp += 1;
            if (m_transp >= 12) m_transp -= 12;
            ClearFormat();
            panel1.Refresh();
        }

        private void ViewForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                Point pos = AutoScrollPosition;
                pos.Y = -pos.Y + 100;
                AutoScrollPosition = pos;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                Point pos = AutoScrollPosition;
                pos.Y = -pos.Y - 100;
                AutoScrollPosition = pos;
                e.Handled = true;
            }
        }
    }
}