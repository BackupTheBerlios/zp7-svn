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
        AbstractSongDatabase m_db;
        PaneGrp m_panegrp;
        string m_origtext;
        string m_drawtext;
        int m_basetone;
        SongDb.songRow m_song;
        int m_colwidth = 0, m_colheight = 0; //no resized sizes
        List<List<Pane>> m_cols = new List<List<Pane>>();
        int m_colhspace = 20;

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

        public float ViewScale { get { return zczoom.Zoom; } }

        void m_dbwrap_ChangedSongDatabase(AbstractSongDatabase db)
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
                if (edcolumns.Value == 1)
                {
                    SongFormatter fmt = new SongFormatter(m_drawtext, CfgTools.CreateSongViewFormatOptions(panel1.Width / ViewScale));
                    fmt.Run();
                    m_panegrp = fmt.Result;
                    panel1.Height = (int)(m_panegrp.FullHeight * ViewScale);
                    m_colheight = m_colwidth = 0;
                }
                else
                {
                    SongFormatter fmt = new SongFormatter(m_drawtext, CfgTools.CreateSongViewFormatOptions(panel1.Width / ViewScale / (int)edcolumns.Value));
                    fmt.Run();
                    m_panegrp = fmt.Result;
                    int colcnt = (int)edcolumns.Value;
                    m_colwidth = (ClientSize.Width - 8 - m_colhspace * (colcnt - 1)) / colcnt;
                    m_colheight = ClientSize.Height - 8 - panel2.Height;
                    m_cols.Clear();
                    m_cols.Add(new List<Pane>());
                    float colhi = 0;
                    foreach (Pane pane in m_panegrp.Panes)
                    {
                        if (colhi + pane.Height * ViewScale > m_colheight)
                        {
                            m_cols.Add(new List<Pane>());
                            colhi = 0;
                        }

                        List<Pane> lastpanes = m_cols[m_cols.Count - 1];
                        if (lastpanes.Count == 0 && pane.IsDelimiter)
                        {
                            // preskocime delimitery na zacatku
                        }
                        else
                        {
                            colhi += pane.Height * ViewScale;
                            lastpanes.Add(pane);
                        }
                    }
                    panel1.Height = m_colheight;
                    panel1.Width = m_cols.Count * m_colwidth + (m_cols.Count - 1) * m_colhspace;
                }
            }
            else
            {
                m_panegrp = null;
                panel1.Height = 0;
            }

            panel1.Invalidate();
        }

        private void SetText(string text)
        {
            m_origtext = text;
            if (m_origtext != null) m_basetone = Chords.GetBaseTone(m_origtext);
            else m_basetone = -1;

            m_drawtext = m_origtext;

            if (m_basetone >= 0)
            {
                btreset.Enabled = cbtransp.Enabled = true;
                int d = 0;
                if (m_song != null && !m_song.IstranspNull()) d = m_song.transp;
                cbtransp.SelectedIndex = (m_basetone + d) % 12;
            }
            else
            {
                cbtransp.SelectedIndex = -1;
                btreset.Enabled = cbtransp.Enabled = false;
            }
            Redraw();
        }

        private void SetSong(SongDb.songRow song)
        {
            m_song = song;
            SetText(song != null ? song.SongText : null);
            button1.Enabled = song != null ? song.Link_1 != "" : false;
            button2.Enabled = song != null ? song.Link_2 != "" : false;
        }

        private void src_PositionChanged(object sender, EventArgs e)
        {
            LoadSong();
        }

        public void LoadSong()
        {
            try
            {
                SetSong(m_dbwrap.SelectedSong);
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
                if (m_colwidth > 0)
                {
                    XGraphics gfx = XGraphics.FromGraphics(e.Graphics, new XSize(panel1.Width * ViewScale, panel1.Height * ViewScale));
                    float x = 0;
                    foreach (List<Pane> panes in m_cols)
                    {
                        float y = 0;
                        foreach (Pane pane in panes)
                        {
                            pane.Draw(gfx, new PointF(x, y), true);
                            y += pane.Height;
                        }
                        x += m_colwidth / ViewScale + m_colhspace / ViewScale;
                        gfx.DrawLine(XPens.Black, x - m_colhspace / ViewScale / 2, 0, x - m_colhspace / ViewScale / 2, m_colheight / ViewScale);
                    }
                }
                else
                {
                    m_panegrp.Draw(XGraphics.FromGraphics(e.Graphics, new XSize(panel1.Width * ViewScale, panel1.Height * ViewScale)));
                }
                e.Graphics.Restore(state);
            }
        }

        private void SongView_Resize(object sender, EventArgs e)
        {
            panel1.Width = Width - 16;
            Redraw();
        }

        private void cbtransp_SelectedIndexChanged(object sender, EventArgs e)
        {
            int d = cbtransp.SelectedIndex - m_basetone;
            if (d < 0) d += 12;
            m_drawtext = Chords.Transpose(m_origtext, d);
            if (m_song != null && m_dbwrap.Database.CanEditSong(m_song))
            {
                if (d == 0)
                {
                    if (!m_song.IstranspNull() && m_song.transp != 0)
                        m_song.transp = d;
                }
                else
                {
                    if (m_song.IstranspNull() || m_song.transp != d)
                        m_song.transp = d;
                }
            }
            Redraw();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cbtransp.SelectedIndex = m_basetone;
            m_drawtext = m_origtext;
            if (m_song != null) m_song.transp = 0;
            Redraw();
        }

        private void zczoom_ChangedZoom(object sender, EventArgs e)
        {
            Redraw();
        }

        public SongDb.songRow Song { get { return m_song; } }
        public string SongText
        {
            get { return m_origtext; }
            set { SetText(value); }
        }

        private void edcolumns_ValueChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (m_song != null && m_song.Link_1 != "")
            {
                System.Diagnostics.Process.Start(m_song.Link_1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (m_song != null && m_song.Link_2 != "")
            {
                System.Diagnostics.Process.Start(m_song.Link_2);
            }
        }
    }
}
