using System;
using System.Collections.Generic;
using System.Text;

namespace zp8
{
    public class LogPage
    {
        List<Pane> m_panes = new List<Pane>();

        float m_heightWithDelim = 0;
        float m_heightWithoutDelim = 0;
        float m_maxPageHeight;

        public LogPage(float maxPageHeight)
        {
            m_maxPageHeight=maxPageHeight;
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
            if (!pane.IsDelimiter) m_heightWithoutDelim = m_heightWithDelim;
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
            m_pages.Add(new LogPage(m_maxPageHeight));
        }
        public LogPage LastPage
        {
            get
            {
                if (m_pages.Count == 0) AddPage();
                return m_pages[m_pages.Count - 1];
            }
        }
    }

    public interface IDistribAlg
    {
        void Run(LogPages pages, IEnumerable<PaneGrp> panegrps);
    }

    public class SimpleDistribAlg : IDistribAlg
    {
        public void Run(LogPages pages, IEnumerable<PaneGrp> panegrps)
        {
            foreach (PaneGrp grp in panegrps)
            {
                pages.AddPaneGrp(grp);
            }
        }
    }
}
