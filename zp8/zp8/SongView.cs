using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace zp8
{
    public partial class SongView : UserControl
    {
        //ISongSource m_source;

        BindingSource m_bsrc;
        SongDatabaseWrapper m_dbwrap;
        SongDatabase m_db;
        PaneGrp m_panegrp;

        public SongView()
        {
            InitializeComponent();
        }

        public SongDatabaseWrapper SongDb
        {
            get { return m_dbwrap; }
            set
            {
                if (m_dbwrap != null) m_dbwrap.ChangedSongDatabase -= m_dbwrap_ChangedSongDatabase;
                if (m_bsrc != null) m_bsrc.PositionChanged -= src_PositionChanged;
                m_dbwrap = value;
                if (m_dbwrap != null)
                {
                    m_dbwrap.ChangedSongDatabase += m_dbwrap_ChangedSongDatabase;
                    m_bsrc = m_dbwrap.SongBindingSource;
                    m_bsrc.PositionChanged += src_PositionChanged;
                }
            }
        }

        void m_dbwrap_ChangedSongDatabase(SongDatabase db)
        {
            m_db = db;
            m_panegrp = null;
            panel1.Invalidate();
            //textBox1.Text = "";
        }

        /*
        public ISongSource SongSource
        {
            get { return m_source; }
            set
            {
                m_source = value;
                m_bsrc = m_source.GetBindingSource();
                m_bsrc.PositionChanged += src_PositionChanged;
            }
        }
        */

        private void src_PositionChanged(object sender, EventArgs e)
        {
            string text;
            try
            {
                text = m_db.DataSet.song[m_bsrc.Position].songtext;
            }
            catch (Exception)
            {
                text = null;                
                //textBox1.Text = "";
            }
            if (text != null)
            {
                SongFormatter fmt = new SongFormatter(text);
                fmt.Run();
                m_panegrp = fmt.Result;
            }
            else
            {
                m_panegrp = null;
            }

            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (m_panegrp != null) m_panegrp.Draw(XGraphics.FromGraphics(e.Graphics, new XSize(panel1.Width, panel1.Height)));
        }
    }
}
