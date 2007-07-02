using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace zp8
{
    /*
    public interface ISongData
    {
        string Title {get;}
        //string Author { get;}
        string Text { get;}
    }
    */

    public class FormatOptions
    {
        public XFont TextFont;
        public XFont TitleFont;

        public readonly XGraphics DummyGraphics;

        public FormatOptions()
        {
            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            TextFont = new XFont("Arial", 10, XFontStyle.Regular, options);
            TitleFont = new XFont("Arial", 12, XFontStyle.Regular, options);

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.AddPage();
            DummyGraphics = XGraphics.FromPdfPage(page);
        }
    }

    public abstract class Pane
    {
        protected FormatOptions m_options;
        double? m_height;

        protected Pane(FormatOptions options)
        {
            m_options = options;
        }
        public abstract void Draw(XGraphics gfx, PointF pt);

        protected abstract double CountHeight(XGraphics gfx);
        public double Height
        {
            get
            {
                if (!m_height.HasValue) m_height = CountHeight(m_options.DummyGraphics);
                return m_height.Value;
            }
        }
        

        //MemoryStream m_data;
        //Metafile m_image;
        //int m_height;
        //Graphics m_graphics;

        //public Pane()
        //{
        //    m_data = new MemoryStream();
        //    //Graphics.FromHwnd(
        //    m_image = new Metafile(m_data, MainForm.HDC);
        //    m_height = 0;
        //    m_graphics = Graphics.FromImage(m_image);
        //}

        //public Metafile Image { get { return m_image; } }
        //public int Height { get { return m_height; } }
        //public Graphics Graphics { get { return m_graphics; } }

        //internal void WantHi(int hi)
        //{
        //    if (m_height < hi) m_height = hi;
        //}
    }

    public class ChordLinePane : Pane
    {
        string m_text;
        public ChordLinePane(string text, FormatOptions options)
            : base(options)
        {
            m_text = text;
        }
        public override void Draw(XGraphics gfx, PointF pt)
        {
            gfx.DrawString(m_text, m_options.TextFont, XBrushes.Black, pt);
        }
        protected override double CountHeight(XGraphics gfx)
        {
            return gfx.MeasureString(m_text, m_options.TextFont).Height;
        }
    }

    public class PaneGrp
    {
        int m_width;
        List<Pane> m_panes = new List<Pane>();
        public void Draw(Graphics g)
        {
            int y = 0;
            foreach (Pane pane in m_panes)
            {
                g.DrawImage(pane.Image, 0, y);
                y += pane.Height;
            }
        }
        public Pane AddPane()
        {
            Pane res = new Pane();
            m_panes.Add(res);
            return res;
        }
    }
    public class SongFormatter
    {
        Pane m_actpane;
        PaneGrp m_panegrp;
        string m_text;
        Font m_font = new Font(FontFamily.GenericSansSerif, 10);

        public SongFormatter(string text)
        {
            m_panegrp = new PaneGrp();
            m_text = text;
        }

        private Pane WantPane(int hi)
        {
            if (m_actpane == null) m_actpane = m_panegrp.AddPane();
            m_actpane.WantHi(hi);
            return m_actpane;
        }

        private void LineFeed()
        {
            m_actpane = null;
        }

        public void Run()
        {
            foreach (string line in m_text.Split('\n'))
            {
                Pane pane = WantPane(20);
                pane.Graphics.DrawString(line, m_font, Brushes.Black, 0, 0);
            }
        }

        public PaneGrp Result { get { return m_panegrp; } }
    }
}
