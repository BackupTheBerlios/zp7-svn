using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Printing;
using PdfSharp.Drawing;

namespace zp8
{
    public class SongPrinter
    {
        SongDb.songRow m_song;
        PaneGrp m_panegrp;
        LogPages m_pages;
        IEnumerator<LogPage> m_actpage;
        PrinterSettings m_settings;

        public SongPrinter(SongDb.songRow song, PrinterSettings settings)
        {
            m_song = song;
            m_settings = settings;
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
            if (m_panegrp == null)
            {
                SongPrintFormatOptions opt = CfgTools.CreateSongPrintFormatOptions(e.PageBounds.Width);
                SongFormatter fmt = new SongFormatter(m_song.songtext, opt.SongOptions);
                fmt.Run();
                m_panegrp = fmt.Result;
                m_panegrp.Insert(new SongHeaderPane(opt, m_song.title, m_song.author));

                m_pages = new LogPages(e.PageBounds.Height);
                m_pages.AddPaneGrp(m_panegrp);

                m_actpage = m_pages.Pages.GetEnumerator();
                m_actpage.MoveNext();
            }
            m_actpage.Current.DrawPage(
                XGraphics.FromGraphics(e.Graphics, new XSize(e.PageBounds.Width, e.PageBounds.Height)),
                new PointF(0, 0),
                null);
            e.HasMorePages = m_actpage.MoveNext();
        }
    }
}
