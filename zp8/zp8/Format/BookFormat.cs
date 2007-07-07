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
    public class BookFormatOptions : FormatOptions
    {
        public readonly XFont TitleFont;
        public readonly XFont AuthorFont;
        public readonly XBrush TitleColor;
        public readonly XBrush AuthorColor;

        public readonly float TitleHeight;
        public readonly float AuthorHeight;
        public readonly float HeaderHeight;

        public readonly float SongSpaceHeight;
        public readonly bool PrintSeparatorLines;

        SongFormatOptions m_songOptions;

        public BookFormatOptions(float pgwi, SongBookFonts fonts, SongBookFormatting formatting, SongFormatOptions songOptions)
            : base(pgwi)
        {
            ConvertFont(fonts.TitleFont, out TitleFont, out TitleColor);
            ConvertFont(fonts.AuthorFont, out AuthorFont, out AuthorColor);
            m_songOptions = songOptions;

            PrintSeparatorLines = formatting.PrintSongDividers;
            TitleHeight = (float)DummyGraphics.MeasureString("M", TitleFont).Height;
            AuthorHeight = (float)DummyGraphics.MeasureString("M", AuthorFont).Height;
            HeaderHeight = TitleHeight + AuthorHeight;
            SongSpaceHeight = formatting.SongSpaceHeight * m_songOptions.TextHeight / 100;
        }
    }

    public abstract class BookFormatPane : Pane
    {
        protected BookFormatOptions Options { get { return (BookFormatOptions)m_options; } }

        public BookFormatPane(BookFormatOptions options)
            : base(options)
        {
        }
    }

    public class SongSeparatorPane : BookFormatPane
    {
        public SongSeparatorPane(BookFormatOptions options) : base(options) { }
        public override float Draw(XGraphics gfx, PointF pt, bool dorender)
        {
            if (Options.PrintSeparatorLines)
            {
                float y = pt.Y + Options.SongSpaceHeight / 2;
                if (dorender) gfx.DrawLine(XPens.Black, pt.X, y, pt.X + Options.PageWidth, y);
            }
            return Options.SongSpaceHeight;
        }

        public override bool IsDelimiter { get { return true; } }
    }

    public class SongHeaderPane : BookFormatPane
    {
        string m_title;
        string m_author;

        public SongHeaderPane(BookFormatOptions options, string title, string author)
            : base(options)
        {
            m_title = title;
            m_author = author;
        }

        public override float Draw(XGraphics gfx, PointF pt, bool dorender)
        {
            if (dorender)
            {
                gfx.DrawString(m_title, Options.TitleFont, Options.TitleColor, pt, XStringFormat.TopLeft);
                gfx.DrawString(m_author, Options.AuthorFont, Options.AuthorColor, new PointF(pt.X, pt.Y + Options.TitleHeight), XStringFormat.TopLeft);
            }
            return Options.HeaderHeight;
        }

        public override bool IsDelimiter { get { return false; } }
    }

}
