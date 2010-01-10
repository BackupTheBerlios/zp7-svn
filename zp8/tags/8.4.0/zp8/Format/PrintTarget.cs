using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;

using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp;

namespace zp8
{
    public interface IPrintTarget : IDisposable
    {
        SizeF GetPageSize(PageOrientation orientation);
        XGraphics GetInfoContext(PageOrientation orientation);
        float mmkx { get;} // milimeters * mmkx = page coords
        float mmky { get;} // milimeters * mmkx = page coords
        //float Width { get;}
        //float Height { get;}
    }

    public class PdfPrintTarget : IPrintTarget
    {
        static PdfDocument m_doc;
        static PdfPage m_page;
        static XGraphics m_info;
        static Size m_size;
        static float m_mmkx;
        static float m_mmky;

        public static XGraphics InfoContext { get { return m_info; } }

        static PdfPrintTarget()
        {
            m_doc = new PdfDocument();
            m_page = m_doc.AddPage();
            m_info = XGraphics.FromPdfPage(m_page);
            m_size = PageSizeConverter.ToSize(m_page.Size);
            m_mmkx = m_size.Width / 210.0f;
            m_mmky = m_size.Height / 297.0f;
        }
        public static float getmmkx() { return m_mmkx; }
        public static float getmmky() { return m_mmky; }

        //PdfPage m_page;
        //public PdfPrintTarget(PdfPage page)
        //{
        //    m_page = page;
        //}

        /*
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
        */

        #region IPrintTarget Members

        public SizeF GetPageSize(PageOrientation orientation)
        {
            switch (orientation)
            {
                case PageOrientation.Portrait:
                    return new SizeF(m_size.Width, m_size.Height);
                case PageOrientation.Landscape:
                    return new SizeF(m_size.Height, m_size.Width);
            }
            throw new Exception("The method or operation is not implemented.");
        }

        public XGraphics GetInfoContext(PageOrientation orientation)
        {
            return m_info;
        }

        public float mmkx
        {
            get { return m_mmkx; }
        }

        public float mmky
        {
            get { return m_mmky; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose() { }

        #endregion
    }

    public class PrinterPrintTarget : IPrintTarget
    {
        PrinterSettings m_settings;
        Graphics m_infoDC;
        XGraphics m_info;
        float m_w0, m_h0;
        float m_mmkx;
        float m_mmky;

        public PrinterPrintTarget(PrinterSettings settings)
        {
            m_settings = (PrinterSettings)settings.Clone();
            m_settings.DefaultPageSettings.Landscape = false;
            m_infoDC = m_settings.CreateMeasurementGraphics();
            m_w0 = m_infoDC.VisibleClipBounds.Width;
            m_h0 = m_infoDC.VisibleClipBounds.Height;
            m_info = XGraphics.FromGraphics(m_infoDC, new XSize(m_w0, m_h0));
            m_mmkx = m_w0 / 210.0f;
            m_mmky = m_h0 / 297.0f;
        }

        #region IPrintTarget Members

        public SizeF GetPageSize(PageOrientation orientation)
        {
            switch (orientation)
            {
                case PageOrientation.Portrait:
                    return new SizeF(m_w0, m_h0);
                case PageOrientation.Landscape:
                    return new SizeF(m_h0, m_w0);
            }
            throw new Exception("The method or operation is not implemented.");
        }

        public XGraphics GetInfoContext(PageOrientation orientation)
        {
            return m_info;
        }

        public float mmkx
        {
            get { return m_mmkx; }
        }

        public float mmky
        {
            get { return m_mmky; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            m_infoDC.Dispose();
            m_info.Dispose();
        }

        #endregion

        /*
        Rectangle m_bounds;
        public PrinterPrintTarget(Rectangle bounds)
        {
            m_bounds = bounds;
        }

        #region IPrintTarget Members

        public float Width
        {
            get { return m_bounds.Width; }
        }

        public float Height
        {
            get { return m_bounds.Height; }
        }

        #endregion
        */

    }


}
