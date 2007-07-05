using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace zp8
{
    public enum DistribType { Book, Lines };

    public class BookLayout
    {
        IPrintTarget m_printTarget;
        int m_hcnt = 1;
        int m_vcnt = 1;

        int m_dleft;
        int m_dright;
        int m_dtop;
        int m_dbottom;

        float? m_smallWidth;
        float? m_smallHeight;

        DistribType m_dtype = DistribType.Book;

        public int HorizontalCount { get { return m_hcnt; } set { m_hcnt = value; } }
        public int VerticalCount { get { return m_vcnt; } set { m_vcnt = value; } }
        public DistribType DistribType { get { return m_dtype; } set { m_dtype = value; } }

        [System.Xml.Serialization.XmlIgnore]
        public IPrintTarget Target
        {
            get { return m_printTarget; }
            set
            {
                m_printTarget = value;
                if (m_printTarget != null)
                {
                    m_smallWidth = m_printTarget.Width / m_hcnt - (m_hcnt - 1) * (m_dleft + m_dright);
                    m_smallHeight = m_printTarget.Height / m_vcnt - (m_vcnt - 1) * (m_dtop + m_dbottom);
                }
                else
                {
                    m_smallWidth = null;
                    m_smallHeight = null;
                }
            }
        }

        public float SmallPageWidth { get { return m_smallWidth.Value; } }
        public float SmallPageHeight { get { return m_smallHeight.Value; } }

        public PointF GetPagePos(int x, int y)
        {
            float w0 = m_printTarget.Width / m_hcnt;
            float h0 = m_printTarget.Height / m_vcnt;
            return new PointF(x * w0 + m_dleft, y * h0 + m_dtop);
        }
    }
}
