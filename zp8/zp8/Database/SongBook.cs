using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.ComponentModel;

namespace zp8
{
    public class SongBookManager
    {
        List<SongBook> m_books = new List<SongBook>();

        public List<SongBook> SongBooks
        {
            get { return m_books; }
        }

        public void CreateNew()
        {
            m_books.Add(new SongBook());
        }
    }

    public class SongBookFonts
    {
        PersistentFont m_textFont = new PersistentFont();
        PersistentFont m_chordFont = new PersistentFont();
        PersistentFont m_labelFont = new PersistentFont();

        [Description("Font textu")]
        public PersistentFont TextFont { get { return m_textFont; } set { m_textFont = value; } }
        [Description("Font akord�")]
        public PersistentFont ChordFont { get { return m_chordFont; } set { m_chordFont = value; } }
        [Description("Font N�v�t�")]
        public PersistentFont LabelFont { get { return m_labelFont; } set { m_labelFont = value; } }
    }

    public class SongBook : AbstractSongDatabase
    {
        public static readonly SongBookManager Manager = new SongBookManager();
        string m_filename;
        BookLayout m_layout;
        BookSequence m_sequence;
        IPrintTarget m_printTarget;
        SongBookFonts m_fonts = new SongBookFonts();
        Dictionary<int, PaneGrp> m_formatted = new Dictionary<int, PaneGrp>();
        
        protected override void WantOpen() { }

        public SongBook()
        {
            m_dataset = new SongDb();
        }

        public string Title
        {
            get
            {
                if (m_filename == null) return "[Beze jm�na]";
                return Path.GetFileName(m_filename);
            }
        }

        public bool Modified
        {
            get
            {
                return m_dataset.HasChanges();
            }
        }
        public string FileName { get { return m_filename; } set { m_filename = value; } }
        public void Save()
        {
            using (XmlWriter xw = XmlWriter.Create(m_filename))
            {
                xw.WriteStartElement("SongBook", "http://zpevnik.net/SongBook.xsd");
                m_dataset.WriteXml(xw);
                xw.WriteEndElement();
            }
        }
        public SongBookFonts Fonts { get { return m_fonts; } }
        public BookLayout Layout { get { return m_layout; } set { m_layout = value; } }

        public FormattedBook Format()
        {
            LogPages pages = m_sequence.CreateLogPages(this);
            return new FormattedBook(pages, Layout);
        }
        public FormatOptions GetFormatOptions()
        {
            return new FormatOptions(m_layout.SmallPageWidth, m_fonts.TextFont, m_fonts.ChordFont, m_fonts.LabelFont);
        }
        public PaneGrp FormatSong(int songid)
        {
            if (m_formatted.ContainsKey(songid)) return m_formatted[songid];
            SongDb.songRow row = DataSet.song.FindByID(songid);

            SongFormatter fmt = new SongFormatter(row.songtext, GetFormatOptions());
            fmt.Run();
            m_formatted[songid] = fmt.Result;

            return m_formatted[songid];
        }
    }
}
