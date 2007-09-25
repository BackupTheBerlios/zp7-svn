using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using PdfSharp.Drawing;

namespace zp8
{
    public class OutlineFormatOptions : FormatOptions
    {
        public readonly XFont TitleFont;
        public readonly XBrush TitleColor;
        public readonly XFont NumberFont;
        public readonly XBrush NumberColor;
        public readonly float PageHeight;
        public readonly int Columns;
        public readonly float ColumnWidth;
        public readonly float ColumnWidthForText;
        public readonly float NumberLeft;
        public readonly float TitleHeight;
        public readonly float NumberHeight;
        public readonly SongBook SongBook;

        public OutlineFormatOptions(float pgwi, float pghi, XGraphics infoContext, PersistentFont titleFont, PersistentFont numberFont, int columns, SongBook songBook, float mmky)
            : base(pgwi, infoContext)
        {
            ConvertFont(titleFont, out TitleFont, out TitleColor, mmky);
            ConvertFont(numberFont, out NumberFont, out NumberColor, mmky);
            PageHeight = pghi;
            Columns = columns;

            ColumnWidth = PageWidth / Columns;
            float numwi = (float)InfoContext.MeasureString("99999", NumberFont).Width;
            ColumnWidthForText = ColumnWidth - numwi;
            NumberLeft = ColumnWidthForText + numwi / 4;
            TitleHeight = (float)InfoContext.MeasureString("M", TitleFont).Height;
            NumberHeight = (float)InfoContext.MeasureString("M", NumberFont).Height;
            SongBook = songBook;
        }
    }

    public abstract class OutlineFormatPane : Pane
    {
        protected OutlineFormatOptions Options { get { return (OutlineFormatOptions)m_options; } }

        public OutlineFormatPane(OutlineFormatOptions options)
            : base(options)
        {
        }
    }

    internal delegate void DrawTextDelegate(string text, PointF pt);

    public class OutlinePane : OutlineFormatPane
    {
        List<Item> m_items = new List<Item>();
        abstract class Item
        {
            protected PointF m_pt;
            protected OutlineFormatOptions m_options;
            public Item(OutlineFormatOptions options, PointF pt)
            {
                m_pt = pt;
                m_options = options;
            }
            public abstract void Draw(XGraphics gfx, PointF pt);
        }
        class TextItem : Item
        {
            string m_text;
            public TextItem(OutlineFormatOptions options, string text, PointF pt)
                : base(options, pt)
            {
                m_text = text;
            }
            public override void Draw(XGraphics gfx, PointF pt)
            {
                gfx.DrawString(m_text, m_options.TitleFont, m_options.TitleColor, new PointF(pt.X + m_pt.X, pt.Y + m_pt.Y), XStringFormat.TopLeft);
            }
        }
        class NumberItem : Item
        {
            int m_songid;
            public NumberItem(OutlineFormatOptions options, int songid, PointF pt)
                : base(options, pt)
            {
                m_songid = songid;
            }
            public override void Draw(XGraphics gfx, PointF pt)
            {
                int? linenum = m_options.SongBook.FindPageNumber(m_songid);
                if (linenum.HasValue)
                    gfx.DrawString(linenum.ToString(), m_options.NumberFont, m_options.NumberColor, new PointF(pt.X + m_pt.X, pt.Y + m_pt.Y), XStringFormat.TopLeft);
            }
        }
        public void DrawText(string text, PointF pt)
        {
            m_items.Add(new TextItem(Options, text, pt));
        }
        public void DrawNumber(int songid, PointF pt)
        {
            m_items.Add(new NumberItem(Options, songid, pt));
        }

        public override float Draw(XGraphics gfx, PointF pt, bool dorender)
        {
            if (dorender)
            {
                foreach (Item item in m_items) item.Draw(gfx, pt);
            }
            return Options.PageHeight;
        }

        public override bool IsDelimiter { get { return false; } }

        public OutlinePane(OutlineFormatOptions options)
            : base(options)
        {
        }
    }

    public class OutlineFormatter
    {
        OutlineFormatOptions m_options;
        IEnumerable<ISongRow> m_songs;
        OutlinePane m_actpane;
        public readonly PaneGrp Result;

        public OutlineFormatter(OutlineFormatOptions options, IEnumerable<ISongRow> songs)
        {
            m_options = options;
            m_songs = songs;
            Result = new PaneGrp();
        }
        public void Run()
        {
            int actcol = 0;
            float acty = 0;
            foreach (SongDb.songRow song in m_songs)
            {
                float hi = MeasureWrappedTextHeight(m_options.InfoContext, song.Title, m_options.ColumnWidthForText, m_options.TitleFont);
                if (acty + hi > m_options.PageHeight)
                {
                    actcol++;
                    acty = 0;
                    if (actcol >= m_options.Columns)
                    {
                        actcol = 0;
                        m_actpane = null;
                    }
                }
                WantPane();

                float x0 = actcol * m_options.ColumnWidth;
                WrapText(
                    m_options.InfoContext,
                    delegate(string txt, PointF pt) { m_actpane.DrawText(txt, new PointF(pt.X + x0, pt.Y + acty)); },
                    song.Title,
                    m_options.ColumnWidthForText,
                    m_options.TitleFont);
                m_actpane.DrawNumber(song.ID, new PointF(m_options.NumberLeft + x0, acty + hi - m_options.NumberHeight));
                acty += hi;
            }
        }
        private static float MeasureWrappedTextHeight(XGraphics gfx, string text, float wi, XFont font)
        {
            return WrapText(gfx, delegate(string t, PointF p) { }, text, wi, font);
        }
        private static float WrapText(XGraphics gfx, DrawTextDelegate drawtext, string text, float wi, XFont font)
        {
            bool wasword = false;
            float actx = 0;
            float acty = 0;
            float space = (float)gfx.MeasureString("i", font).Width;
            float linehi = (float)gfx.MeasureString("M", font).Height;
            foreach (string wrd0 in text.Split(' '))
            {
                string wrd = wrd0.Trim();
                if (wasword) actx += space;
                float wordwi = (float)gfx.MeasureString(wrd, font).Width;
                if (actx + wordwi > wi && actx > 0) // slovo se nevejde na radku
                { // odradkujeme
                    actx = 0;
                    acty += linehi;
                }
                drawtext(wrd0, new PointF(actx, acty));
                actx += wordwi;
                wasword = true;
            }
            if (actx > 0) acty += linehi;
            return acty;
        }
        private void WantPane()
        {
            if (m_actpane == null)
            {
                m_actpane = new OutlinePane(m_options);
                Result.Add(m_actpane);
            }
        }
    }
}
