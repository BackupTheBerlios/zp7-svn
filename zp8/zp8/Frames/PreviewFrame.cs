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
                plpage.Width = (int)(m_source.PageWidth * zoomControl1.Zoom);
                plpage.Height = (int)(m_source.PageHeight * zoomControl1.Zoom);
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
                GraphicsState state = e.Graphics.Save();
                e.Graphics.ScaleTransform(zoomControl1.Zoom, zoomControl1.Zoom);
                XSize size = new XSize(plpage.Width * zoomControl1.Zoom, plpage.Height * zoomControl1.Zoom);
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
    }
}
