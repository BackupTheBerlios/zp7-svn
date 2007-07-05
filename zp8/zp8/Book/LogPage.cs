using System;
using System.Collections.Generic;
using System.Text;
using PdfSharp.Drawing;
using System.Drawing;

namespace zp8
{
    public class LogPage
    {
        List<Pane> m_panes = new List<Pane>();

        float m_heightWithDelim = 0;
        float m_heightWithoutDelim = 0;
        float m_maxPageHeight;
        int m_noDelimPageCount = 0;

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
            if (!pane.IsDelimiter)
            {
                m_heightWithoutDelim = m_heightWithDelim;
                m_noDelimPageCount = m_panes.Count;
            }
        }

        public void DrawPage(XGraphics gfx, PointF pagepos)
        {
            float acty = 0;
            for (int i = 0; i < m_noDelimPageCount; i++)
            {
                Pane pane = m_panes[i];
                pane.Draw(gfx, new PointF(pagepos.X, pagepos.X + acty));
                acty += pane.Height;
            }
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
        public IEnumerable<LogPage> Pages { get { return m_pages; } }
        //public IEnumerable<LogPage> ReversedPages()
        //{
        //    for (int i = m_pages.Count - 1; i >= 0; i--) yield return m_pages[i];
        //}
        public int Count { get { return m_pages.Count; } }
    }

    public interface IDistribAlg
    {
        void Run(LogPages pages, IEnumerable<PaneGrp> panegrps);
        string Name { get;}
    }

    public static class DistribAlgs
    {
        public static readonly SimpleDistribAlg Simple = new SimpleDistribAlg();
        public static IDistribAlg FromName(string name)
        {
            switch (name)
            {
                case "simple": return Simple;
            }
            throw new Exception("Unknown distrib algorithm:" + name);
        }
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
        public string Name { get { return "simple"; } }
    }
}
