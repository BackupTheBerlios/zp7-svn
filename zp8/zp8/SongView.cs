using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace zp8
{
    public partial class SongView : UserControl
    {
        //ISongSource m_source;

        BindingSource m_bsrc;
        SongDatabaseWrapper m_dbwrap;
        SongDatabase m_db;
        PaneGrp m_panegrp;
        string m_origtext;
        string m_drawtext;
        int m_basetone;
        SongDb.songRow m_song;

        public SongView()
        {
            InitializeComponent();
        }

        public SongDatabaseWrapper SongDb
        {
            get { return m_dbwrap; }
            set
            {
                if (m_dbwrap != null) m_dbwrap.ChangedSongDatabase -= m_dbwrap_ChangedSongDatabase;
                if (m_bsrc != null) m_bsrc.PositionChanged -= src_PositionChanged;
                m_dbwrap = value;
                if (m_dbwrap != null)
                {
                    m_dbwrap.ChangedSongDatabase += m_dbwrap_ChangedSongDatabase;
                    m_bsrc = m_dbwrap.SongBindingSource;
                    m_bsrc.PositionChanged += src_PositionChanged;
                }
            }
        }

        public float ViewScale { get { return (float)Math.Pow(5, tbzoom.Value / 10.0); } }

        void m_dbwrap_ChangedSongDatabase(SongDatabase db)
        {
            m_db = db;
            m_panegrp = null;
            panel1.Invalidate();
            //textBox1.Text = "";
        }

        /*
        public ISongSource SongSource
        {
            get { return m_source; }
            set
            {
                m_source = value;
                m_bsrc = m_source.GetBindingSource();
                m_bsrc.PositionChanged += src_PositionChanged;
            }
        }
        */

        private void Redraw()
        {
            if (m_drawtext != null)
            {
                SongFormatter fmt = new SongFormatter(m_drawtext, new FormatOptions(panel1.Width / ViewScale));
                fmt.Run();
                m_panegrp = fmt.Result;
                panel1.Height = (int)(m_panegrp.FullHeight * ViewScale);
            }
            else
            {
                m_panegrp = null;
                panel1.Height = 0;
            }

            panel1.Invalidate();
        }

        private void SetSong(SongDb.songRow song)
        {
            m_song = song;
            m_origtext = song != null ? song.songtext : null;
            if (m_origtext != null) m_basetone = Chords.GetBaseTone(m_origtext);
            else m_basetone = -1;

            m_drawtext = m_origtext;

            if (m_basetone >= 0)
            {
                cbtransp.Enabled = true;
                int d = m_song.IstranspNull() ? 0 : m_song.transp;
                cbtransp.SelectedIndex = (m_basetone + d) % 12;
            }
            else
            {
                cbtransp.SelectedIndex = -1;
                cbtransp.Enabled = false;
            }
            Redraw();
        }

        private void src_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                SetSong(m_db.DataSet.song[m_bsrc.Position]);
            }
            catch (Exception)
            {
                SetSong(null);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (m_panegrp != null)
            {
                GraphicsState state = e.Graphics.Save();
                e.Graphics.ScaleTransform(ViewScale, ViewScale);
                m_panegrp.Draw(XGraphics.FromGraphics(e.Graphics, new XSize(panel1.Width, panel1.Height)));
                e.Graphics.Restore(state);
            }
        }

        private void SongView_Resize(object sender, EventArgs e)
        {
            panel1.Width = Width - 16;
        }

        private void tbzoom_Scroll(object sender, EventArgs e)
        {
            Redraw();
        }

        private void cbtransp_SelectedIndexChanged(object sender, EventArgs e)
        {
            int d = cbtransp.SelectedIndex - m_basetone;
            if (d < 0) d += 12;
            m_drawtext = Chords.Transpose(m_origtext, d);
            if (m_song != null) m_song.transp = d;
            Redraw();
        }
    }
}
