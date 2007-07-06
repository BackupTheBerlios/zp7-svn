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

        [DisplayName("Po�et mal�ch str�nek horizont�ln�")]
        public int HorizontalCount { get { return m_hcnt; } set { m_hcnt = value; } }
        [DisplayName("Po�et mal�ch str�nek vertik�ln�")]
        public int VerticalCount { get { return m_vcnt; } set { m_vcnt = value; } }
        [DisplayName("Zp�sob rozm�st�n� na str�nky")]
        [Description("Book - rozm�s�uje str�nky pro tisk kn�ek, Lines - rozmis�uje str�nky vodorovn� do ��dek (nap�. styl zp�vn�ku \"Kapela\")")]
        public DistribType DistribType { get { return m_dtype; } set { m_dtype = value; } }

        [DisplayName("Rozm�s�ovac� algoritmus")]
        [Description("Simple - jednduch�, Complex - slo�it�, sna�� se, aby p�se� nebyla p�es v�ce str�nek")]
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

        [DisplayName("Odsazen� zleva")]
        [Description("Odsazen� mal� str�nky v milimetrech")]
        public int DistLeftMM { get { return m_dleft; } set { m_dleft = value; } }

        [DisplayName("Odsazen� zezhora")]
        [Description("Odsazen� mal� str�nky v milimetrech")]
        public int DistTopMM { get { return m_dtop; } set { m_dtop = value; } }

        [DisplayName("Odsazen� zprava")]
        [Description("Odsazen� mal� str�nky v milimetrech")]
        public int DistRightMM { get { return m_dright; } set { m_dright = value; } }

        [DisplayName("Odsazen� zezdola")]
        [Description("Odsazen� mal� str�nky v milimetrech")]
        public int DistBottomMM { get { return m_dbottom; } set { m_dbottom = value; } }

        public PointF GetPagePos(int x, int y)
        {
            float w0 = m_printTarget.Width / m_hcnt;
            float h0 = m_printTarget.Height / m_vcnt;
            return new PointF(x * w0 + m_dleft, y * h0 + m_dtop);
        }
    }
}
