using System;
using System.Collections.Generic;
using System.Text;
using PdfSharp.Drawing;
using System.Drawing;

namespace zp8
{
    public class PageDrawOptions
    {
        public readonly XFont HeaderFont;
        public readonly XBrush HeaderColor;
        public readonly XFont FooterFont;
        public readonly XBrush FooterColor;
        public readonly string Header;
        public readonly string Footer;
        public readonly float PageWidth;
        public readonly float PageHeight;

        public PageDrawOptions(float pgwi, float pghi, PersistentFont headerFont, PersistentFont footerFont, string header, string footer, float mmky)
        {
            FormatOptions.ConvertFont(headerFont, out HeaderFont, out HeaderColor, mmky);
            FormatOptions.ConvertFont(footerFont, out FooterFont, out FooterColor, mmky);
            Header = header;
            Footer = footer;
            PageWidth = pgwi;
            PageHeight = pghi;
        }
    }

    public class LogPage
    {
        List<Pane> m_panes = new List<Pane>();

        float m_heightWithDelim = 0;
        float m_heightWithoutDelim = 0;
        float m_maxPageHeight;
        int m_noDelimPageCount = 0;
        int m_pageNumber;

        public LogPage(float maxPageHeight, int pageNumber)
        {
            m_maxPageHeight = maxPageHeight;
            m_pageNumber = pageNumber;
        }

        public bool CanAddPane(Pane pane)
        {
            if (pane.IsDelimiter) return true;
            return pane.Height + m_heightWithDelim <= m_maxPageHeight;
        }

        public void AddPane(Pane pane)
        {
            m_panes.Add(pane);
            m_heightWithDelim += pane.Height;
            if (!pane.IsDelimiter)
            {
                m_heightWithoutDelim = m_heightWithDelim;
                m_noDelimPageCount = m_panes.Count;
            }
        }

        public void DrawPage(XGraphics gfx, PointF pagepos, PageDrawOptions opts)
        {
            float acty = 0;

            for (int i = 0; i < m_noDelimPageCount; i++)
            {
                Pane pane = m_panes[i];
                pane.Draw(gfx, new PointF(pagepos.X, pagepos.Y + acty), true);
                acty += pane.Height;
            }

            if (opts != null)
            {
                string header = opts.Header.Replace("%c", PageNumber.ToString());
                string footer = opts.Footer.Replace("%c", PageNumber.ToString());

                if (header != "")
                {
                    float hdrwi = (float)gfx.MeasureString(header, opts.HeaderFont).Width;
                    float hdrhi = (float)gfx.MeasureString(header, opts.HeaderFont).Height;
                    gfx.DrawString(header, opts.HeaderFont, opts.HeaderColor, new PointF(pagepos.X + opts.PageWidth / 2 - hdrwi / 2, pagepos.Y - hdrhi), XStringFormat.TopLeft);
                }
                if (footer != "")
                {
                    float ftrwi = (float)gfx.MeasureString(footer, opts.FooterFont).Width;
                    gfx.DrawString(footer, opts.FooterFont, opts.FooterColor, new PointF(pagepos.X + opts.PageWidth / 2 - ftrwi / 2, pagepos.Y + opts.PageHeight), XStringFormat.TopLeft);
                }
            }
        }

        public int ExtraPageCount(PaneGrp paneGrp)
        {
            return paneGrp.CountExtraSheets(m_heightWithDelim, m_heightWithoutDelim, m_maxPageHeight);            
        }

        public int PageNumber { get { return m_pageNumber; } }
        public bool HasPane(Pane pane)
        {
            return m_panes.Contains(pane);
        }
    }

    public class LogPages
    {
        List<LogPage> m_pages = new List<LogPage>();
        float m_maxPageHeight;

        public LogPages(float maxPageHeight)
        {
            m_maxPageHeight = maxPageHeight;
        }

        public void AddPaneGrp(PaneGrp grp)
        {
            foreach (Pane pane in grp.Panes)
            {
                if (!LastPage.CanAddPane(pane)) AddPage();
                LastPage.AddPane(pane);
            }
        }
        public void AddPage()
        {
            m_pages.Add(new LogPage(m_maxPageHeight, m_pages.Count + 1));
        }
        public LogPage LastPage
        {
            get
            {
                if (m_pages.Count == 0) AddPage();
                return m_pages[m_pages.Count - 1];
            }
        }
        public IEnumerable<LogPage> Pages { get { return m_pages; } }
        //public IEnumerable<LogPage> ReversedPages()
        //{
        //    for (int i = m_pages.Count - 1; i >= 0; i--) yield return m_pages[i];
        //}
        public int Count { get { return m_pages.Count; } }
        public float MaxPageHeight { get { return m_maxPageHeight; } }
    }

}
