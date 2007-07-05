using System;
using System.Collections.Generic;
using System.Text;

using PdfSharp.Pdf;
using PdfSharp;

namespace zp8
{
    public interface IPrintTarget
    {
        float Width { get;}
        float Height { get;}
    }
    public class PdfPrintTarget : IPrintTarget
    {
        PdfPage m_page;
        public PdfPrintTarget(PdfPage page)
        {
            m_page = page;
        }

        #region IPrintTarget Members

        public float Width
        {
            get { return PageSizeConverter.ToSize(m_page.Size).Width; }
        }

        public float Height
        {
            get { return PageSizeConverter.ToSize(m_page.Size).Height; }
        }

        #endregion
    }
}
