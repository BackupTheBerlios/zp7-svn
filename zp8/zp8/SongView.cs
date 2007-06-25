using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class SongView : UserControl
    {
        ISongSource m_source;
        BindingSource m_bsrc;

        public SongView()
        {
            InitializeComponent();
        }
        public ISongSource SongSource
        {
            get { return m_source; }
            set
            {
                if (m_bsrc != null) m_bsrc.PositionChanged -= src_PositionChanged;
                m_source = value;
                m_bsrc = m_source.GetBindingSource();
                m_bsrc.PositionChanged += src_PositionChanged;
            }
        }

        private void src_PositionChanged(object sender, EventArgs e)
        {
            int index = m_bsrc.Position;
            SongDatabase sdb = m_source.GetDataSet();
            if (sdb != null) textBox1.Text = sdb.DataSet.song[index].songtext;
        }
    }
}
