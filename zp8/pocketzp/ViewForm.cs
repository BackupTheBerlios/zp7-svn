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

        public ViewForm(DbSong song)
        {
            InitializeComponent();
            m_song = song;
            Text = m_song.Name;
        }

        private void Reformat(Graphics g)
        {
            panel1.Width = ClientSize.Width;
            SongFormatter fmt = new SongFormatter(m_song.Text, new SongFormatOptions(panel1.Width - 8, g));
            fmt.Run();
            m_panegrp = fmt.Result;
            panel1.Height = (int)m_panegrp.FullHeight + 1;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (m_panegrp == null) Reformat(e.Graphics);
            if (m_panegrp != null) m_panegrp.Draw(e.Graphics);
        }

        private void ViewForm_Resize(object sender, EventArgs e)
        {
            ClearFormat();
        }

        private void ClearFormat()
        {
            m_panegrp = null;
        }
    }
}