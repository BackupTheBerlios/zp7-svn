using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace pocketzp
{
    public struct PointF
    {
        public float X;
        public float Y;
        public PointF(float x, float y) { X = x; Y = y; }
    }

    public abstract class Pane
    {
        protected SongFormatOptions m_options;
        protected float? m_height;

        protected Pane(SongFormatOptions options)
        {
            m_options = options;
        }
        public abstract float Draw(Graphics gfx, PointF pt, bool dorender);
        public abstract bool IsDelimiter { get;}

        public float Height
        {
            get
            {
                if (!m_height.HasValue) m_height = Draw(m_options.InfoContext, new PointF(0, 0), false);
                return m_height.Value;
            }
        }
    }

    public class PaneGrp
    {
        List<Pane> m_panes = new List<Pane>();
        public void Draw(Graphics gfx)
        {
            float y = 0;
            foreach (Pane pane in m_panes)
            {
                pane.Draw(gfx, new PointF(0, y), true);
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

        public void Add(Pane pane)
        {
            m_panes.Add(pane);
        }
        public void Insert(Pane pane)
        {
            m_panes.Insert(0, pane);
        }

        public IEnumerable<Pane> Panes { get { return m_panes; } }

        public int CountExtraSheets(float heightWithDelims, float heightWithoutDelims, float maxPageHeight)
        {
            int res = 0;
            foreach (Pane pane in m_panes)
            {
                if (heightWithDelims + pane.Height > maxPageHeight)
                {
                    res += 1;
                    heightWithDelims = 0;
                    heightWithoutDelims = 0;
                }
                heightWithDelims += pane.Height;
                if (!pane.IsDelimiter) heightWithoutDelims = heightWithDelims;
            }
            return res;
        }
        public int SheetCount(float maxPageHeight)
        {
            return CountExtraSheets(maxPageHeight, maxPageHeight, maxPageHeight);
        }
        public Pane FirstPane { get { return m_panes[0]; } }

    }


    public class SongFormatOptions
    {
        public readonly Graphics InfoContext;
        public readonly float PageWidth;

        public readonly Font TextFont = new Font("Courier mew", 10, FontStyle.Regular);
        public readonly Font LabelFont = new Font("Courier mew", 10, FontStyle.Regular);
        public readonly Font ChordFont = new Font("Courier mew", 10, FontStyle.Regular);
        public readonly Brush TextColor = new SolidBrush(Color.Black);
        public readonly Brush ChordColor = new SolidBrush(Color.Blue);
        public readonly Brush LabelColor = new SolidBrush(Color.Black);

        public readonly float HTextSpace;
        public readonly float HChordSpace;
        public readonly float TextHeight;
        public readonly float ChordHeight;
        public readonly float LabelHeight;

        public SongFormatOptions(float pgwi, Graphics infoContext)
        {
            InfoContext = infoContext;
            PageWidth = pgwi;
            HTextSpace = (float)InfoContext.MeasureString("i", TextFont).Width;
            HChordSpace = (float)InfoContext.MeasureString("i", ChordFont).Width;
            TextHeight = (float)InfoContext.MeasureString("M", TextFont).Height;
            ChordHeight = (float)InfoContext.MeasureString("M", ChordFont).Height;
            LabelHeight = (float)InfoContext.MeasureString("M", LabelFont).Height;
        }
    }

    public abstract class SongFormatPane : Pane
    {
        protected SongFormatOptions Options { get { return (SongFormatOptions)m_options; } }

        public SongFormatPane(SongFormatOptions options)
            : base(options)
        {
        }
    }

    public class ParagraphSeparatorPane : SongFormatPane
    {
        public ParagraphSeparatorPane(SongFormatOptions options)
            : base(options)
        {
        }
        public override float Draw(Graphics gfx, PointF pt, bool dorender)
        {
            return Options.TextHeight / 2;
        }
        public override bool IsDelimiter { get { return true; } }
    }

    public class LabelLinePane : SongFormatPane
    {
        protected readonly string m_label;
        public LabelLinePane(SongFormatOptions options, string label)
            : base(options)
        {
            m_label = label;
        }
        public override float Draw(Graphics gfx, PointF pt, bool dorender)
        {
            if (dorender) gfx.DrawString(m_label, Options.LabelFont, Options.LabelColor, pt.X, pt.Y);
            return Options.LabelHeight;
        }
        public override bool IsDelimiter { get { return false; } }
    }

    public abstract class LabelablePane : SongFormatPane
    {
        protected readonly float m_x0;
        protected readonly string m_label;

        protected LabelablePane(SongFormatOptions options, float x0, string label)
            : base(options)
        {
            m_x0 = x0;
            m_label = label;
        }

        protected void DrawLabel(Graphics gfx, PointF pt, float baseline)
        {
            if (m_label != null)
            {
                gfx.DrawString(m_label, Options.LabelFont, Options.LabelColor, pt.X, pt.Y + baseline - Options.LabelHeight);
            }
        }
        public override bool IsDelimiter { get { return false; } }
    }

    public class TextLinePane : LabelablePane
    {
        string m_text;
        public TextLinePane(string text, SongFormatOptions options, float x0, string label)
            : base(options, x0, label)
        {
            m_text = text;
        }
        public override float Draw(Graphics gfx, PointF pt, bool dorender)
        {
            float actx = m_x0;
            float acty = 0;
            SongLineParser par = new SongLineParser(m_text);
            bool wasword = false;
            while (par.Current != SongLineParser.Token.End)
            {
                if (par.Current == SongLineParser.Token.Word)
                {
                    if (wasword) actx += Options.HTextSpace;
                    float wordwi = (float)gfx.MeasureString(par.Data, Options.TextFont).Width;
                    if (actx + wordwi > Options.PageWidth && actx > m_x0) // slovo se nevejde na radku
                    { // odradkujeme
                        actx = m_x0;
                        acty += Options.TextHeight;
                    }
                    if (dorender) gfx.DrawString(par.Data, Options.TextFont, Options.TextColor, pt.X + actx, pt.Y + acty);
                    actx += wordwi;
                    wasword = true;
                }
                par.Read();
            }

            if (actx > 0) acty += Options.TextHeight;
            if (dorender) DrawLabel(gfx, pt, Options.TextHeight);
            return acty;
        }
    }

    public class ChordLinePane : LabelablePane
    {
        string m_text;
        public ChordLinePane(string text, SongFormatOptions options, float x0, string label)
            : base(options, x0, label)
        {
            m_text = text;
        }

        // resi odradkovani, muze se odradkovat jen pri Space
        private IEnumerable<string> GetLines(Graphics gfx)
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
                    float wordwi = (float)gfx.MeasureString(par.Data, Options.TextFont).Width;
                    tpos += wordwi;
                    par.Read();
                }
                else if (par.Current == SongLineParser.Token.Space || par.Current == SongLineParser.Token.End)
                {
                    if ((apos > Options.PageWidth || tpos > Options.PageWidth) && (lastspace.Position > lastflushed.Position))
                    {
                        yield return par.Original.Substring(lastflushed.Position, lastspace.Position - lastflushed.Position);
                        lastflushed = lastspace;
                        par.State = lastspace;
                        apos = tpos = m_x0;
                    }
                    else
                    {
                        if (par.Current == SongLineParser.Token.End) break;
                        tpos += Options.HTextSpace;
                        lastspace = par.State;
                        par.Read();
                    }
                    if (par.Current == SongLineParser.Token.End) break;
                }
                else if (par.Current == SongLineParser.Token.Chord)
                {
                    if (tpos < apos) tpos = apos; // aby nebyly 2 akordy pres sebe
                    apos = tpos;
                    float chordwi = (float)gfx.MeasureString(par.Data, Options.ChordFont).Width;
                    apos += chordwi + Options.HChordSpace;
                    par.Read();
                }
            }
            yield return par.Original.Substring(lastflushed.Position);
        }

        public override float Draw(Graphics gfx, PointF pt, bool dorender)
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
                        float wordwi = (float)gfx.MeasureString(par.Data, Options.TextFont).Width;
                        if (dorender) gfx.DrawString(par.Data, Options.TextFont, Options.TextColor, pt.X + tpos, acty + pt.Y + Options.ChordHeight);
                        tpos += wordwi;
                    }
                    if (par.Current == SongLineParser.Token.Space)
                    {
                        tpos += Options.HTextSpace;
                    }
                    if (par.Current == SongLineParser.Token.Chord)
                    {
                        if (tpos < apos) tpos = apos; // aby nebyly 2 akordy pres sebe
                        apos = tpos;
                        float chordwi = (float)gfx.MeasureString(par.Data, Options.ChordFont).Width;
                        if (dorender) gfx.DrawString(par.Data, Options.ChordFont, Options.ChordColor, pt.X + apos, acty + pt.Y);
                        apos += chordwi + Options.HChordSpace;
                    }
                    par.Read();
                }
                acty += Options.ChordHeight + Options.TextHeight;
            }
            DrawLabel(gfx, pt, Options.ChordHeight + Options.TextHeight);

            return acty;
        }
    }

    public class SongFormatter
    {
        PaneGrp m_panegrp;
        string m_text;
        SongFormatOptions m_options;

        public SongFormatter(string text, SongFormatOptions options)
        {
            m_panegrp = new PaneGrp();
            m_text = text;
            m_options = options;
        }

        public void Run()
        {
            string pending_label = null;
            float x0 = 0;
            foreach (string line0 in m_text.Split('\n'))
            {
                string line = line0.Trim();
                if (SongTool.IsLabelLine(line))
                {
                    if (pending_label != null) m_panegrp.Add(new LabelLinePane(m_options, pending_label));
                    pending_label = line.Substring(1);
                    x0 = (float)m_options.InfoContext.MeasureString(pending_label, m_options.LabelFont).Width;
                }
                else if (SongTool.IsChordLine(line))
                {
                    m_panegrp.Add(new ChordLinePane(line, m_options, x0, pending_label));
                    pending_label = null;
                }
                else if (!SongTool.IsEmptyLine(line))
                {
                    m_panegrp.Add(new TextLinePane(line, m_options, x0, pending_label));
                    pending_label = null;
                }
                else
                {
                    // zrusit odsazeni
                    x0 = 0;
                    m_panegrp.Add(new ParagraphSeparatorPane(m_options));
                }
            }
        }

        public PaneGrp Result { get { return m_panegrp; } }
    }
}
