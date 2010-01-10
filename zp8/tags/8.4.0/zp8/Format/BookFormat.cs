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
    public class SongPrintFormatOptions : FormatOptions
    {
        public readonly XFont TitleFont;
        public readonly XFont AuthorFont;
        public readonly XBrush TitleColor;
        public readonly XBrush AuthorColor;

        public readonly float TitleHeight;
        public readonly float AuthorHeight;
        public readonly float HeaderHeight;

        public readonly SongFormatOptions SongOptions;

        public SongPrintFormatOptions(float pgwi, XGraphics infoContext, PersistentFont titleFont, PersistentFont authorFont, SongFormatOptions songOptions, float mmky)
            : base(pgwi, infoContext)
        {
            SongOptions = songOptions;

            ConvertFont(titleFont, out TitleFont, out TitleColor, mmky);
            ConvertFont(authorFont, out AuthorFont, out AuthorColor, mmky);
            TitleHeight = (float)InfoContext.MeasureString("M", TitleFont).Height;
            AuthorHeight = (float)InfoContext.MeasureString("M", AuthorFont).Height;
            HeaderHeight = TitleHeight + AuthorHeight;
        }
    }

    public class BookFormatOptions : SongPrintFormatOptions
    {
        public readonly float SongSpaceHeight;
        public readonly bool PrintSeparatorLines;


        public BookFormatOptions(float pgwi, XGraphics infoContext, SongBookFonts fonts, SongBookFormatting formatting, SongFormatOptions songOptions, float mmky)
            : base(pgwi, infoContext, fonts.TitleFont, fonts.AuthorFont, songOptions, mmky)
        {

            PrintSeparatorLines = formatting.PrintSongDividers;
            SongSpaceHeight = formatting.SongSpaceHeight * SongOptions.TextHeight / 100;
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

    public abstract class SongPrintFormatPane : Pane
    {
        protected SongPrintFormatOptions Options { get { return (SongPrintFormatOptions)m_options; } }

        public SongPrintFormatPane(SongPrintFormatOptions options)
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

    public class SongHeaderPane : SongPrintFormatPane
    {
        string m_title;
        string m_author;

        public SongHeaderPane(SongPrintFormatOptions options, string title, string author)
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
