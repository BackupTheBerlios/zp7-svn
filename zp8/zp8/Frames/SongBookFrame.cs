using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace zp8
{
    public partial class SongBookFrame : UserControl
    {
        SongBook m_book;
        FormattedBook m_fbook;

        public SongBookFrame()
        {
            InitializeComponent();
        }
        public SongBook SongBook
        {
            get { return m_book; }
            set
            {
                if (m_book != null) m_book.Changed -= m_book_Changed;
                m_book = value;
                if (m_book != null) m_book.Changed += m_book_Changed;
                Format();
            }
        }

        void m_book_Changed(object sender, EventArgs e)
        {
            Format();
        }

        private void Format()
        {
            if (m_book == null)
            {
                m_fbook = null;
            }
            else
            {
                m_fbook = m_book.Format();
            }
            if (m_fbook != null) previewFrame1.Source = m_fbook.GetPreview();
            else previewFrame1.Source = null;
        }


        public void PropertiesDialog()
        {
            if (m_book != null)
            {
                OptionsForm.Run(m_book);
                m_book.ClearCaches();
                Format();
            }
        }

        public void ExportAsPDF()
        {
            if (m_book != null && savepdf.ShowDialog() == DialogResult.OK)
            {
                m_book.ExportAsPDF(savepdf.FileName);
            }
        }

        public void ChangeBookStyle()
        {
            if (m_book==null) return;
            string newstyle = ChangeBookStyleForm.Run();
            if (newstyle != null)
            {
                m_book.SetBookStyle(newstyle);
                m_book.ClearCaches();
                Format();
            }
        }
    }
}
