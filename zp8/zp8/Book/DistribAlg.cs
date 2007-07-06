using System;
using System.Collections.Generic;
using System.Text;

using PdfSharp.Drawing;
using System.Drawing;

namespace zp8
{
    public enum DistribAlg { Complex, Simple };

    public interface IDistribAlg
    {
        void Run(LogPages pages, IEnumerable<PaneGrp> panegrps, BookLayout layout);
    }

    public static class DistribAlgs
    {
        public static readonly SimpleDistribAlg Simple = new SimpleDistribAlg();
        public static readonly ComplexDistribAlg Complex = new ComplexDistribAlg();
        public static IDistribAlg FromEnum(DistribAlg alg)
        {
            switch (alg)
            {
                case DistribAlg.Simple: return Simple;
                case DistribAlg.Complex: return Complex;
            }
            throw new Exception("Unknown distrib algorithm:" + alg.ToString());
        }
    }

    public class SimpleDistribAlg : IDistribAlg
    {
        #region IDistribAlg Members

        public void Run(LogPages pages, IEnumerable<PaneGrp> panegrps, BookLayout layout)
        {
            foreach (PaneGrp grp in panegrps)
            {
                pages.AddPaneGrp(grp);
            }
        }
        public string Name { get { return "simple"; } }

        #endregion
    }

    public class ComplexDistribAlg : IDistribAlg
    {
        #region IDistribAlg Members

        public void Run(LogPages pages, IEnumerable<PaneGrp> panegrps, BookLayout layout)
        {
            DistribAlgImpl impl = null;
            switch (layout.DistribType)
            {
                case DistribType.Book:
                    impl = new BookDistribAlgImpl(pages, panegrps, layout);
                    break;
                case DistribType.Lines:
                    impl = new LinesDistribAlgImpl(pages, panegrps, layout);
                    break;
            }
            impl.Run();
        }

        public string Name { get { return "complex"; } }

        #endregion
    }

    public abstract class DistribAlgImpl
    {
        protected LogPages m_pages;
        protected List<PaneGrp> m_stack = new List<PaneGrp>();
        protected BookLayout m_layout;
        public DistribAlgImpl(LogPages pages, IEnumerable<PaneGrp> panegrps, BookLayout layout)
        {
            m_pages = pages;
            m_stack.AddRange(panegrps);
            m_layout = layout;
        }
        public abstract void Run();
        protected LogPage LastPage { get { return m_pages.LastPage; } }
        protected bool IsLeftPage { get { return m_pages.Count % 2 == 1; } }
        protected float MaxPageHeight { get { return m_pages.MaxPageHeight; } }
        protected void AddPage() { m_pages.AddPage(); }
        protected void AddPaneGrp(PaneGrp grp)
        {
            m_pages.AddPaneGrp(grp);
            m_stack.Remove(grp);
        }
        protected PaneGrp FindPassingPaneGrp()
        {
            foreach (PaneGrp grp in m_stack)
            {
                if (LastPage.ExtraPageCount(grp) <= 1) return grp;
            }
            return null;
        }
    }

    public class BookDistribAlgImpl : DistribAlgImpl
    {
        public BookDistribAlgImpl(LogPages pages, IEnumerable<PaneGrp> panegrps, BookLayout layout)
            : base(pages, panegrps, layout)
        {
        }
        public override void Run()
        {
            bool wasAddedOnLeftPage = false;
            while (m_stack.Count > 0)
            {
                int rest = LastPage.ExtraPageCount(m_stack[0]);
                if (IsLeftPage)
                {
                    if (rest <= 1)
                    {
                        if (m_stack[0].SheetCount(MaxPageHeight) == 1) AddPage(); // vejde se na stranku, zacit vpravo
                        AddPaneGrp(m_stack[0]);
                        wasAddedOnLeftPage = true;
                    }
                    else
                    {
                        if (!wasAddedOnLeftPage)
                        {
                            // smula, pisen se nevejde ani na dvojstranu, je potreba rozdelit
                            AddPaneGrp(m_stack[0]);
                        }
                        else
                        {
                            if (m_stack[0].SheetCount(MaxPageHeight) < 2)
                            {
                                PaneGrp other = FindPassingPaneGrp();
                                if (other != null)
                                {
                                    if (other.SheetCount(MaxPageHeight) == 1) AddPage();
                                    AddPaneGrp(other);
                                }
                                else
                                {
                                    AddPage();
                                    AddPage();
                                    AddPaneGrp(m_stack[0]);
                                }
                            }
                            else
                            {
                                AddPaneGrp(m_stack[0]);
                            }
                        }
                    }
                }
                else // right side
                {
                    if (rest == 0)
                    {
                        AddPaneGrp(m_stack[0]);
                        wasAddedOnLeftPage = true;
                    }
                    else
                    {
                        AddPage();
                        AddPaneGrp(m_stack[0]);
                    }
                }
            }
        }
    }

    public class LinesDistribAlgImpl : DistribAlgImpl
    {
        public LinesDistribAlgImpl(LogPages pages, IEnumerable<PaneGrp> panegrps, BookLayout layout)
            : base(pages, panegrps, layout)
        {
        }
        public override void Run()
        {
            // nejdrive dame ty, ktere se nevejdou do jedne rady
            List<PaneGrp> copy = new List<PaneGrp>();
            copy.AddRange(m_stack);
            foreach (PaneGrp grp in copy)
            {
                if (grp.SheetCount(MaxPageHeight) > m_layout.HorizontalCount)
                    AddPaneGrp(grp);
            }

            while (m_stack.Count > 0)
            {
                // zkusime najit prvni, ktera se vejde
                bool worked = false;
                int acthpage = m_pages.Count % m_layout.HorizontalCount;
                if (acthpage == 0) acthpage = m_pages.Count;
                copy.Clear(); copy.AddRange(m_stack);
                foreach (PaneGrp grp in copy)
                {
                    if (LastPage.ExtraPageCount(grp) <= m_layout.HorizontalCount - acthpage)
                    {
                        AddPaneGrp(grp);
                        worked = true;
                        break;
                    }
                }
                // zadna pisen nebyla pridana
                if (!worked)
                {
                    if (m_pages.Count % m_layout.HorizontalCount == 1) AddPage();
                    while (m_pages.Count % m_layout.HorizontalCount != 1) AddPage();
                }
            }
        }
    }

}
