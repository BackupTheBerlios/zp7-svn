using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Printing;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp;

namespace zp8
{
    public class SongPrinter
    {
        SongData m_song;
        IEnumerator<LogPage> m_actpage;
        PrinterSettings m_settings;
        PrinterPrintTarget m_target;

        public SongPrinter(SongData song, PrinterSettings settings)
        {
            m_song = song;
            m_settings = settings;
            m_target = new PrinterPrintTarget(m_settings);
        }

        public void Run()
        {
            PrintDocument doc = new PrintDocument();
            doc.PrinterSettings = m_settings;
            doc.PrintPage += doc_PrintPage;
            doc.Print();
        }

        void doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            XGraphics gfx = XGraphics.FromGraphics(e.Graphics, new XSize(e.PageBounds.Width, e.PageBounds.Height));
            if (m_actpage == null)
            {
                LogPages pages = FormatSongForPrinting(m_song, e.PageBounds.Width, gfx, e.PageBounds.Height, m_target.mmky);
                m_actpage = pages.Pages.GetEnumerator();
                m_actpage.MoveNext();
            }
            m_actpage.Current.DrawPage(gfx, new PointF(0, 0), null);
            e.HasMorePages = m_actpage.MoveNext();
        }

        public static LogPages FormatSongForPrinting(SongData song, float pgwi, XGraphics infoContext, float pghi, float mmky)
        {
            //PrinterPrintTarget target=new PrinterPrintTarget(
            SongPrintFormatOptions opt = CfgTools.CreateSongPrintFormatOptions(pgwi, infoContext, mmky);
            SongFormatter fmt = new SongFormatter(song.SongText, opt.SongOptions);
            fmt.Run();
            PaneGrp grp = fmt.Result;
            grp.Insert(new SongHeaderPane(opt, song.Title, song.Author != "" ? song.Author : song.GroupName));

            LogPages pages = new LogPages(pghi);
            pages.AddPaneGrp(grp);
            return pages;
        }
    }

    public static class SongPDFPrinter
    {
        public static void Print(SongData song, string filename)
        {
            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.AddPage();
            LogPages pages = SongPrinter.FormatSongForPrinting(song, PageSizeConverter.ToSize(page.Size).Width, PdfPrintTarget.InfoContext, PageSizeConverter.ToSize(page.Size).Height, PdfPrintTarget.getmmky());
            foreach (LogPage lp in pages.Pages)
            {
                lp.DrawPage(XGraphics.FromPdfPage(page), new PointF(0, 0), null);
                if (pages.LastPage != lp) page = doc.AddPage();
            }
            doc.Save(filename);
        }
    }

    public class SongBookPrinter
    {
        SongBook m_book;
        PrinterSettings m_settings;
        IPrintTarget m_lastTarget;
        IPreviewSource m_preview;
        int m_pageIndex;

        public SongBookPrinter(SongBook book, PrinterSettings settings)
        {
            m_book = book;
            m_settings = settings;
            m_lastTarget = m_book.PrintTarget;
            if (book.Layout.Orientation == PageOrientation.Landscape)
            {
                m_settings.DefaultPageSettings.Landscape = true;
            }
        }

        public void Run()
        {
            PrintDocument doc = new PrintDocument();
            doc.PrinterSettings = m_settings;
            doc.PrintPage += doc_PrintPage;
            doc.Print();
        }

        void doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            XGraphics gfx = XGraphics.FromGraphics(e.Graphics, new XSize(e.PageBounds.Width, e.PageBounds.Height));
            if (m_preview == null)
            {
                // uchovame stary PrintTarget: dispose=false
                m_book.SetPrintTarget(new PrinterPrintTarget(m_settings), false);
                m_preview = m_book.Book.GetPreview();
            }

            if (m_pageIndex < m_preview.PageCount)
            {
                m_preview.DrawPage(gfx, m_pageIndex);
            }
            m_pageIndex++;

            e.HasMorePages = m_pageIndex < m_preview.PageCount;
            if (m_pageIndex >= m_preview.PageCount)
            {
                m_book.PrintTarget = m_lastTarget;
            }
        }
    }
}
