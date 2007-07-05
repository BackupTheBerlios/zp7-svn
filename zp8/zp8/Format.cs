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
        public readonly XFont TextFont;
        public readonly XFont ChordFont;
        public readonly XFont LabelFont;
        public readonly XBrush TextColor;
        public readonly XBrush ChordColor;
        public readonly XBrush LabelColor;

        //public XFont TitleFont;
        public readonly float HTextSpace;
        public readonly float HChordSpace;
        public readonly float TextHeight;
        public readonly float ChordHeight;
        public readonly float LabelHeight;
        public readonly float PageWidth;

        public readonly XGraphics DummyGraphics;

        public FormatOptions(float pgwi, PersistentFont textFont, PersistentFont chordFont, PersistentFont labelFont)
        {
            ConvertFont(textFont, out TextFont, out TextColor);
            ConvertFont(chordFont, out ChordFont, out ChordColor);
            ConvertFont(labelFont, out LabelFont, out LabelColor);
            //TextFont = new XFont("Arial", 12, XFontStyle.Regular, options);
            //ChordFont = new XFont("Arial", 12, XFontStyle.Bold, options);
            //LabelFont = new XFont("Arial", 12, XFontStyle.Underline, options);
            //TitleFont = new XFont("Arial", 15, XFontStyle.Regular, options);

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.AddPage();
            DummyGraphics = XGraphics.FromPdfPage(page);
            HTextSpace = (float)DummyGraphics.MeasureString("i", TextFont).Width;
            HChordSpace = (float)DummyGraphics.MeasureString("i", ChordFont).Width;
            TextHeight = (float)DummyGraphics.MeasureString("M", TextFont).Height;
            ChordHeight = (float)DummyGraphics.MeasureString("M", ChordFont).Height;
            LabelHeight = (float)DummyGraphics.MeasureString("M", LabelFont).Height;
            PageWidth = pgwi;
        }

        private static void ConvertFont(PersistentFont pfont, out XFont xfont, out XBrush xcolor)
        {
            xfont = pfont.ToXFont();
            /*
            using (Font ft = pfont.ToFont())
            {
                ft.Unit = GraphicsUnit.World;
                xfont = new XFont(ft, FontOptions);
            }
            */
            using (Brush br = new SolidBrush(pfont.FontColor)) xcolor = (XBrush)br;
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

    public class LabelLinePane : Pane
    {
        protected readonly string m_label;
        public LabelLinePane(FormatOptions options, string label)
            : base(options)
        {
            m_label = label;
        }
        public override float Draw(XGraphics gfx, PointF pt)
        {
            gfx.DrawString(m_label, m_options.LabelFont, m_options.LabelColor, pt, XStringFormat.TopLeft);
            return m_options.LabelHeight;
        }
    }

    public abstract class LabelablePane : Pane
    {
        protected readonly float m_x0;
        protected readonly string m_label;

        protected LabelablePane(FormatOptions options, float x0, string label)
            : base(options)
        {
            m_x0 = x0;
            m_label = label;
        }

        protected void DrawLabel(XGraphics gfx, PointF pt, float baseline)
        {
            if (m_label != null)
            {
                gfx.DrawString(m_label, m_options.LabelFont, m_options.LabelColor, new PointF(pt.X, pt.Y + baseline - m_options.LabelHeight), XStringFormat.TopLeft);
            }
        }
    }

    public class TextLinePane : LabelablePane
    {
        string m_text;
        public TextLinePane(string text, FormatOptions options, float x0, string label)
            : base(options, x0, label)
        {
            m_text = text;
        }
        public override float Draw(XGraphics gfx, PointF pt)
        {
            float actx = m_x0;
            float acty = 0;
            SongLineParser par = new SongLineParser(m_text);
            bool wasword = false;
            while (par.Current != SongLineParser.Token.End)
            {
                if (par.Current == SongLineParser.Token.Word)
                {
                    if (wasword) actx += m_options.HTextSpace;
                    float wordwi = (float)gfx.MeasureString(par.Data, m_options.TextFont).Width;
                    if (actx + wordwi > m_options.PageWidth && actx > m_x0) // slovo se nevejde na radku
                    { // odradkujeme
                        actx = m_x0;
                        acty += m_options.TextHeight;
                    }
                    gfx.DrawString(par.Data, m_options.TextFont, m_options.TextColor, new PointF(pt.X + actx, pt.Y + acty), XStringFormat.TopLeft);
                    actx += wordwi;
                    wasword = true;
                }
                par.Read();
            }

            if (actx > 0) acty += m_options.TextHeight;
            DrawLabel(gfx, pt, m_options.TextHeight);
            return acty;
        }
    }

    public class ChordLinePane : LabelablePane
    {
        string m_text;
        public ChordLinePane(string text, FormatOptions options, float x0, string label)
            : base(options, x0, label)
        {
            m_text = text;
        }

        // resi odradkovani, muze se odradkovat jen pri Space
        private IEnumerable<string> GetLines(XGraphics gfx)
        {
            List<string> lines = new List<string>();
            float tpos = m_x0, apos = m_x0;
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
                        apos = tpos = m_x0;
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
                float tpos = m_x0, apos = m_x0;
                SongLineParser par = new SongLineParser(line);
                while (par.Current != SongLineParser.Token.End)
                {
                    if (par.Current == SongLineParser.Token.Word)
                    {
                        float wordwi = (float)gfx.MeasureString(par.Data, m_options.TextFont).Width;
                        gfx.DrawString(par.Data, m_options.TextFont, m_options.TextColor, new PointF(pt.X + tpos, acty + pt.Y + m_options.ChordHeight), XStringFormat.TopLeft);
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
                        gfx.DrawString(par.Data, m_options.ChordFont, m_options.ChordColor, new PointF(pt.X + apos, acty + pt.Y), XStringFormat.TopLeft);
                        apos += chordwi + m_options.HChordSpace;
                    }
                    par.Read();
                }
                acty += m_options.ChordHeight + m_options.TextHeight;
            }
            DrawLabel(gfx, pt, m_options.ChordHeight + m_options.TextHeight);

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
            string pending_label = null;
            float x0 = 0;
            foreach (string line in m_text.Split('\n'))
            {
                if (SongTool.IsLabelLine(line))
                {
                    if (pending_label != null) m_panegrp.Add(new LabelLinePane(m_options, pending_label));
                    pending_label = line.Substring(1);
                    x0 = (float)m_options.DummyGraphics.MeasureString(pending_label, m_options.LabelFont).Width;
                }
                else if (SongTool.IsChordLine(line))
                {
                    m_panegrp.Add(new ChordLinePane(line, m_options, x0, pending_label));
                    pending_label = null;
                }
                else
                {
                    m_panegrp.Add(new TextLinePane(line, m_options, x0, pending_label));
                    pending_label = null;
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
