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
        public XFont ChordFont;
        public XFont TitleFont;
        public float HTextSpace;
        public float HChordSpace;
        public float TextHeight;
        public float ChordHeight;
        public float PageWidth;

        public readonly XGraphics DummyGraphics;

        public FormatOptions(float pgwi)
        {
            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            TextFont = new XFont("Arial", 10, XFontStyle.Regular, options);
            ChordFont = new XFont("Arial", 10, XFontStyle.Bold, options);
            TitleFont = new XFont("Arial", 12, XFontStyle.Regular, options);

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.AddPage();
            DummyGraphics = XGraphics.FromPdfPage(page);
            HTextSpace = (float)DummyGraphics.MeasureString("i", TextFont).Width;
            HChordSpace = (float)DummyGraphics.MeasureString("i", ChordFont).Width;
            TextHeight = (float)DummyGraphics.MeasureString("M", TextFont).Height;
            ChordHeight = (float)DummyGraphics.MeasureString("M", ChordFont).Height;
            PageWidth = pgwi;
        }
    }

    public abstract class Pane
    {
        protected FormatOptions m_options;
        protected float? m_height;

        protected Pane(FormatOptions options)
        {
            m_options = options;
        }
        public abstract float Draw(XGraphics gfx, PointF pt);

        public float Height
        {
            get
            {
                if (!m_height.HasValue) m_height = Draw(m_options.DummyGraphics, new PointF(0, 0));
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

    public class TextLinePane : Pane
    {
        string m_text;
        public TextLinePane(string text, FormatOptions options)
            : base(options)
        {
            m_text = text;
        }
        public override float Draw(XGraphics gfx, PointF pt)
        {
            float actx = 0;
            float acty = 0;
            SongLineParser par = new SongLineParser(m_text);
            bool wasword = false;
            while (par.Current != SongLineParser.Token.End)
            {
                if (par.Current == SongLineParser.Token.Word)
                {
                    if (wasword) actx += m_options.HTextSpace;
                    float wordwi = (float)gfx.MeasureString(par.Data, m_options.TextFont).Width;
                    if (actx + wordwi > m_options.PageWidth) // slovo se nevejde na radku
                    { // odradkujeme
                        actx = 0;
                        acty += m_options.TextHeight;
                    }
                    gfx.DrawString(par.Data, m_options.TextFont, XBrushes.Black, new PointF(pt.X + actx, pt.Y + acty), XStringFormat.TopLeft);
                    actx += wordwi;
                    wasword = true;
                }
                par.Read();
            }

            if (actx > 0) acty += m_options.TextHeight;
            return acty;
        }
    }

    public class ChordLinePane : Pane
    {
        string m_text;
        public ChordLinePane(string text, FormatOptions options)
            : base(options)
        {
            m_text = text;
        }

        // resi odradkovani, muze se odradkovat jen pri Space
        private IEnumerable<string> GetLines(XGraphics gfx)
        {
            List<string> lines = new List<string>();
            float tpos = 0, apos = 0;
            SongLineParser par = new SongLineParser(m_text);

            SongLineParser.ParserState lastflushed = par.InitState;
            SongLineParser.ParserState lastspace = par.InitState;

            while (true)
            {
                if (par.Current == SongLineParser.Token.Word)
                {
                    float wordwi = (float)gfx.MeasureString(par.Data, m_options.TextFont).Width;
                    tpos += wordwi;
                    par.Read();
                }
                else if (par.Current == SongLineParser.Token.Space || par.Current == SongLineParser.Token.End)
                {
                    if ((apos > m_options.PageWidth || tpos > m_options.PageWidth) && (lastspace.Position > lastflushed.Position))
                    {
                        yield return par.Original.Substring(lastflushed.Position, lastspace.Position - lastflushed.Position);
                        lastflushed = lastspace;
                        par.State = lastspace;
                        apos = tpos = 0;
                    }
                    else
                    {
                        if (par.Current == SongLineParser.Token.End) break;
                        tpos += m_options.HTextSpace;
                        lastspace = par.State;
                        par.Read();
                    }
                    if (par.Current == SongLineParser.Token.End) break;
                }
                else if (par.Current == SongLineParser.Token.Chord)
                {
                    if (tpos < apos) tpos = apos; // aby nebyly 2 akordy pres sebe
                    apos = tpos;
                    float chordwi = (float)gfx.MeasureString(par.Data, m_options.ChordFont).Width;
                    apos += chordwi + m_options.HChordSpace;
                    par.Read();
                }
            }
            yield return par.Original.Substring(lastflushed.Position);
        }

        public override float Draw(XGraphics gfx, PointF pt)
        {
            float acty = 0;

            foreach (string line in GetLines(gfx))
            {
                float tpos = 0, apos = 0;
                SongLineParser par = new SongLineParser(line);
                while (par.Current != SongLineParser.Token.End)
                {
                    if (par.Current == SongLineParser.Token.Word)
                    {
                        float wordwi = (float)gfx.MeasureString(par.Data, m_options.TextFont).Width;
                        gfx.DrawString(par.Data, m_options.TextFont, XBrushes.Black, new PointF(pt.X + tpos, acty + pt.Y + m_options.ChordHeight), XStringFormat.TopLeft);
                        tpos += wordwi;
                    }
                    if (par.Current == SongLineParser.Token.Space)
                    {
                        tpos += m_options.HTextSpace;
                    }
                    if (par.Current == SongLineParser.Token.Chord)
                    {
                        if (tpos < apos) tpos = apos; // aby nebyly 2 akordy pres sebe
                        apos = tpos;
                        float chordwi = (float)gfx.MeasureString(par.Data, m_options.ChordFont).Width;
                        gfx.DrawString(par.Data, m_options.ChordFont, XBrushes.Black, new PointF(pt.X + apos, acty + pt.Y), XStringFormat.TopLeft);
                        apos += chordwi + m_options.HChordSpace;
                    }
                    par.Read();
                }
                acty += m_options.ChordHeight + m_options.TextHeight;
            }

            return acty;
        }
    }

    public class PaneGrp
    {
        List<Pane> m_panes = new List<Pane>();
        public void Draw(XGraphics gfx)
        {
            float y = 0;
            foreach (Pane pane in m_panes)
            {
                pane.Draw(gfx, new PointF(0, y));
                y += pane.Height;
            }
        }
        public float FullHeight
        {
            get
            {
                float res = 0;
                foreach (Pane pane in m_panes) res += pane.Height;
                return res;
            }
        }
        //public Pane AddPane()
        //{
        //    Pane res = new Pane();
        //    m_panes.Add(res);
        //    return res;
        //}

        public void Add(Pane pane)
        {
            m_panes.Add(pane);
        }
    }
    public class SongFormatter
    {
        PaneGrp m_panegrp;
        string m_text;
        FormatOptions m_options;

        public SongFormatter(string text, FormatOptions options)
        {
            m_panegrp = new PaneGrp();
            m_text = text;
            m_options = options;
        }

        public void Run()
        {
            foreach (string line in m_text.Split('\n'))
            {
                if (SongTool.IsLabelLine(line))
                {
                }
                else if (SongTool.IsChordLine(line))
                {
                    m_panegrp.Add(new ChordLinePane(line, m_options));
                }
                else
                {
                    m_panegrp.Add(new TextLinePane(line, m_options));
                }
            }
        }

        public PaneGrp Result { get { return m_panegrp; } }

        //Pane m_actpane;
        //PaneGrp m_panegrp;
        //string m_text;
        //Font m_font = new Font(FontFamily.GenericSansSerif, 10);

        //public SongFormatter(string text)
        //{
        //    m_panegrp = new PaneGrp();
        //    m_text = text;
        //}

        //private Pane WantPane(int hi)
        //{
        //    if (m_actpane == null) m_actpane = m_panegrp.AddPane();
        //    m_actpane.WantHi(hi);
        //    return m_actpane;
        //}

        //private void LineFeed()
        //{
        //    m_actpane = null;
        //}

        //public void Run()
        //{
        //    foreach (string line in m_text.Split('\n'))
        //    {
        //        Pane pane = WantPane(20);
        //        pane.Graphics.DrawString(line, m_font, Brushes.Black, 0, 0);
        //    }
        //}

        //public PaneGrp Result { get { return m_panegrp; } }
    }
}
