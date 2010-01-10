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
    public class FormatOptions
    {
        public readonly XGraphics InfoContext;
        public readonly float PageWidth;

        public FormatOptions(float pgwi, XGraphics infoContext)
        {
            //PdfDocument doc = new PdfDocument();
            //PdfPage page = doc.AddPage();
            //InfoContext = XGraphics.FromPdfPage(page);
            InfoContext = infoContext;
            PageWidth = pgwi;
        }

        public static void ConvertFont(PersistentFont pfont, out XFont xfont, out XBrush xcolor, float mmky)
        {
            xfont = pfont.ToXFont(mmky);
            using (Brush br = new SolidBrush(pfont.FontColor)) xcolor = (XBrush)br;
        }
    }

    public abstract class Pane
    {
        protected FormatOptions m_options;
        protected float? m_height;

        protected Pane(FormatOptions options)
        {
            m_options = options;
        }
        public abstract float Draw(XGraphics gfx, PointF pt, bool dorender);
        public abstract bool IsDelimiter { get;}

        public float Height
        {
            get
            {
                if (!m_height.HasValue) m_height = Draw(m_options.InfoContext, new PointF(0, 0), false);
                return m_height.Value;
            }
        }
    }

    public class PaneGrp
    {
        List<Pane> m_panes = new List<Pane>();
        public void Draw(XGraphics gfx)
        {
            float y = 0;
            foreach (Pane pane in m_panes)
            {
                pane.Draw(gfx, new PointF(0, y), true);
                y += pane.Height;
            }
        }
        public float FullHeight
        {
            get
            {
                float res = 0;
                foreach (Pane pane in m_panes) res += pane.Height;
                return res;
            }
        }

        public void Add(Pane pane)
        {
            m_panes.Add(pane);
        }
        public void Insert(Pane pane)
        {
            m_panes.Insert(0, pane);
        }

        public IEnumerable<Pane> Panes { get { return m_panes; } }

        public int CountExtraSheets(float heightWithDelims, float heightWithoutDelims, float maxPageHeight)
        {
            int res = 0;
            foreach (Pane pane in m_panes)
            {
                if (heightWithDelims + pane.Height > maxPageHeight)
                {
                    res += 1;
                    heightWithDelims = 0;
                    heightWithoutDelims = 0;
                }
                heightWithDelims += pane.Height;
                if (!pane.IsDelimiter) heightWithoutDelims = heightWithDelims;
            }
            return res;
        }
        public int SheetCount(float maxPageHeight)
        {
            return CountExtraSheets(maxPageHeight, maxPageHeight, maxPageHeight);
        }
        public Pane FirstPane { get { return m_panes[0]; } }

    }
}
