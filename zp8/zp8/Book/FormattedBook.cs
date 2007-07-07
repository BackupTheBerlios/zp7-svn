using System;
using System.Collections.Generic;
using System.Text;
using PdfSharp.Drawing;
using System.Drawing;

namespace zp8
{
    public interface IPreviewSource
    {
        float PageWidth { get;}
        float PageHeight { get;}
        int PageCount { get;}
        void DrawPage(XGraphics gfx, int index);
        string PageTitle(int index);
    }

    public class FormattedBook
    {
        BookLayout m_layout;

        int m_pagesPerSheet;
        int m_sheetCount;
        int m_hcnt;
        int m_vcnt;
        int m_smallPageCount;

        PageDrawOptions m_opts;

        LogPage[] m_pages; // importonce sequence: sheet,row,col,side

        LogPage[] m_originalPages;

        public FormattedBook(LogPages pages, BookLayout layout, PageDrawOptions opts)
        {
            m_layout = layout;
            m_hcnt = layout.HorizontalCount;
            m_vcnt = layout.VerticalCount;
            m_pagesPerSheet = 2 * m_vcnt * m_hcnt;
            m_opts = opts;

            List<LogPage> tmp = new List<LogPage>();
            tmp.AddRange(pages.Pages);
            m_originalPages = tmp.ToArray();

            m_smallPageCount = pages.Count;

            // logicke stranky doplnene o null
            List<LogPage> virtpages = new List<LogPage>();
            virtpages.AddRange(pages.Pages);

            while (m_smallPageCount % m_pagesPerSheet > 0)
            {
                m_smallPageCount++;
                virtpages.Add(null);
            }

            // logicke stranky doplnene o null obracene
            List<LogPage> revvirtpages = new List<LogPage>();
            revvirtpages.AddRange(virtpages);
            revvirtpages.Reverse();

            m_sheetCount = m_smallPageCount / m_pagesPerSheet;

            m_pages = new LogPage[m_smallPageCount];

            if (layout.DistribType == DistribType.Lines)
            {
                IEnumerator<LogPage> actpage = virtpages.GetEnumerator();
                for (int i = 0; i < m_sheetCount; i++)
                {
                    for (int l = 0; l < m_vcnt; l++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            for (int k = 0; k < m_hcnt; k++)
                            {
                                actpage.MoveNext();
                                m_pages[PageIndex(i, k, l, j)] = actpage.Current;
                            }
                        }
                    }
                }
            }
            if (layout.DistribType == DistribType.Book)
            {
                if (m_hcnt % 2 == 1) // liche horizontalne
                {
                    for (int i = 0; i < m_smallPageCount; i++) m_pages[i] = virtpages[i];
                }
                else // sude horizontalne
                {
                    IEnumerator<LogPage> incer = virtpages.GetEnumerator();
                    IEnumerator<LogPage> decer = revvirtpages.GetEnumerator();
                    for (int i = 0; i < m_smallPageCount; i += 4)
                    {
                        incer.MoveNext(); m_pages[i + 0] = incer.Current;
                        incer.MoveNext(); m_pages[i + 1] = incer.Current;
                        decer.MoveNext(); m_pages[i + 2] = decer.Current;
                        decer.MoveNext(); m_pages[i + 3] = decer.Current; 
                    }
                }
            }
        }

        int PageIndex(int sheet, int x, int y, int side)
        {
            if (side == 1) return m_pagesPerSheet * sheet + y * m_hcnt * 2 + x * 2 + side;
            if (side == 0) return m_pagesPerSheet * sheet + y * m_hcnt * 2 + (m_hcnt - x - 1) * 2 + side;
            throw new Exception("Bad side:" + side.ToString());
        }


        public int A4SheetCount { get { return m_sheetCount; } }
        public int SmallPageCount { get { return m_smallPageCount; } }
        public int FreePageCount { get { return m_sheetCount * 2 * m_hcnt * m_vcnt - m_smallPageCount; } }
        public BookLayout Layout { get { return m_layout; } }

        public void DrawBigPage(XGraphics gfx, int sheet, int side, PageDrawOptions opts)
        {
            for (int x = 0; x < m_hcnt; x++)
            {
                for (int y = 0; y < m_vcnt; y++)
                {
                    LogPage page = m_pages[PageIndex(sheet, x, y, side)];
                    if (page != null)
                    {
                        PointF pagepos = m_layout.GetPagePos(x, y);
                        page.DrawPage(gfx, pagepos, opts);
                    }
                }
            }
        }

        public LogPage[] OriginalPages { get { return m_originalPages; } }

        public IPreviewSource GetPreview() { return new NormalPreviewSource(this, m_opts); }
    }

    public class NormalPreviewSource : IPreviewSource
    {
        FormattedBook m_book;
        PageDrawOptions m_opts;
        public NormalPreviewSource(FormattedBook book, PageDrawOptions opts)
        {
            m_book = book;
            m_opts = opts;
        }
        #region IPreviewSource Members

        public float PageWidth
        {
            get { return m_book.Layout.BigPageWidth; }
        }

        public float PageHeight
        {
            get { return m_book.Layout.BigPageHeight; }
        }

        public int PageCount
        {
            get { return m_book.A4SheetCount * 2; }
        }

        public void DrawPage(XGraphics gfx, int index)
        {
            m_book.DrawBigPage(gfx, index / 2, index % 2, m_opts);
        }

        public string PageTitle(int index)
        {
            return String.Format("Strana {0}/{1}", index + 1, m_book.A4SheetCount * 2);
        }

        #endregion
    }
}
