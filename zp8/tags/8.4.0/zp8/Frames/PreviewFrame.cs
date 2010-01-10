using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using PdfSharp.Drawing;

namespace zp8
{
    public partial class PreviewFrame : UserControl
    {
        IPreviewSource m_source;
        float m_mmky;
        float m_zoomFactor = 1;

        public PreviewFrame()
        {
            InitializeComponent();
        }
        public IPreviewSource Source
        {
            get { return m_source; }
            set
            {
                m_source = value;
                if (m_source != null && m_source.PageCount == 0) m_source = null;
                if (m_source == null)
                {
                    tbpage.Enabled = false;
                    lbpage.Text = "Strana ???";
                }
                else
                {
                    tbpage.Minimum = 0;
                    tbpage.Maximum = m_source.PageCount - 1;
                    tbpage.Value = 0;
                    tbpage.Enabled = true;
                    tbpage_Scroll(null, null);
                }
                Redraw();
            }
        }

        private void Redraw()
        {
            if (m_source != null)
            {
                float zoom = zoomControl1.Zoom * m_zoomFactor;
                plpage.Width = (int)(m_source.PageWidth * zoom);
                plpage.Height = (int)(m_source.PageHeight * zoom);
                plpage.Invalidate();
            }
            else
            {
                plpage.Width = 0;
                plpage.Height = 0;
            }
        }

        private void zoomControl1_ChangedZoom(object sender, EventArgs e)
        {
            Redraw();
        }

        private void tbpage_Scroll(object sender, EventArgs e)
        {
            ChangedPage();
        }

        private void ChangedPage()
        {
            lbpage.Text = m_source.PageTitle(tbpage.Value);
            Redraw();
        }

        private void plpage_Paint(object sender, PaintEventArgs e)
        {
            if (m_source != null)
            {
                float zoom = zoomControl1.Zoom * m_zoomFactor;
                GraphicsState state = e.Graphics.Save();
                e.Graphics.ScaleTransform(zoom, zoom);
                XSize size = new XSize(plpage.Width * zoom, plpage.Height * zoom);
                m_source.DrawPage(XGraphics.FromGraphics(e.Graphics, size), tbpage.Value);
                e.Graphics.Restore(state);
            }
        }

        public void ScrollPage(int dpage)
        {
            int newval = tbpage.Value + dpage;
            if (newval >= tbpage.Minimum && newval <= tbpage.Maximum)
            {
                tbpage.Value = newval;
                ChangedPage();
            }
        }

        public float mmky
        {
            get { return m_mmky; }
            set
            {
                m_mmky = value;
                m_zoomFactor = 2.834f / m_mmky;
            }
        }
    }
}
