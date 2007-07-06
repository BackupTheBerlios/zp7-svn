using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace zp8
{
    public enum DistribType { Book, Lines };

    public class BookLayout : PropertyPageBase
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
        DistribAlg m_distribAlg = DistribAlg.Complex;

        [DisplayName("Poèet malıch stránek horizontálnì")]
        public int HorizontalCount { get { return m_hcnt; } set { m_hcnt = value; } }
        [DisplayName("Poèet malıch stránek vertikálnì")]
        public int VerticalCount { get { return m_vcnt; } set { m_vcnt = value; } }
        [DisplayName("Zpùsob rozmístìní na stránky")]
        [Description("Book - rozmísuje stránky pro tisk kníek, Lines - rozmisuje stránky vodorovnì do øádek (napø. styl zpìvníku \"Kapela\")")]
        public DistribType DistribType { get { return m_dtype; } set { m_dtype = value; } }

        [DisplayName("Rozmísovací algoritmus")]
        [Description("Simple - jednduchı, Complex - sloitı, snaí se, aby píseò nebyla pøes více stránek")]
        public DistribAlg DistribAlg { get { return m_distribAlg; } set { m_distribAlg = value; } }

        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public IPrintTarget Target
        {
            get { return m_printTarget; }
            set
            {
                m_printTarget = value;
                if (m_printTarget != null)
                {
                    m_smallWidth = m_printTarget.Width / m_hcnt - m_dleft - m_dright;
                    m_smallHeight = m_printTarget.Height / m_vcnt - m_dtop - m_dbottom;
                }
                else
                {
                    m_smallWidth = null;
                    m_smallHeight = null;
                }
            }
        }

        [Browsable(false)]
        public float SmallPageWidth { get { return m_smallWidth.Value; } }
        [Browsable(false)]
        public float SmallPageHeight { get { return m_smallHeight.Value; } }
        [Browsable(false)]
        public float BigPageWidth { get { return m_printTarget.Width; } }
        [Browsable(false)]
        public float BigPageHeight { get { return m_printTarget.Height; } }

        [DisplayName("Odsazení zleva")]
        [Description("Odsazení malé stránky v milimetrech")]
        public int DistLeftMM { get { return m_dleft; } set { m_dleft = value; } }

        [DisplayName("Odsazení zezhora")]
        [Description("Odsazení malé stránky v milimetrech")]
        public int DistTopMM { get { return m_dtop; } set { m_dtop = value; } }

        [DisplayName("Odsazení zprava")]
        [Description("Odsazení malé stránky v milimetrech")]
        public int DistRightMM { get { return m_dright; } set { m_dright = value; } }

        [DisplayName("Odsazení zezdola")]
        [Description("Odsazení malé stránky v milimetrech")]
        public int DistBottomMM { get { return m_dbottom; } set { m_dbottom = value; } }

        public PointF GetPagePos(int x, int y)
        {
            float w0 = m_printTarget.Width / m_hcnt;
            float h0 = m_printTarget.Height / m_vcnt;
            return new PointF(x * w0 + m_dleft, y * h0 + m_dtop);
        }
    }
}
