using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.ComponentModel;

using PdfSharp;
using PdfSharp.Pdf;

namespace zp8
{
    public class SongBookManager
    {
        List<SongBook> m_books = new List<SongBook>();

        public List<SongBook> SongBooks
        {
            get { return m_books; }
        }

        public SongBook CreateNew()
        {
            SongBook book = new SongBook();
            m_books.Add(book);
            return book;
        }

        public SongBook LoadExisting(string filename)
        {
            SongBook book = new SongBook();
            m_books.Add(book);
            book.Load(filename);
            return book;
        }
    }

    public class SongBookFonts
    {
        PersistentFont m_textFont = new PersistentFont();
        PersistentFont m_chordFont = new PersistentFont();
        PersistentFont m_labelFont = new PersistentFont();

        [Description("Font textu")]
        public PersistentFont TextFont { get { return m_textFont; } set { m_textFont = value; } }
        [Description("Font akordù")]
        public PersistentFont ChordFont { get { return m_chordFont; } set { m_chordFont = value; } }
        [Description("Font Návìští")]
        public PersistentFont LabelFont { get { return m_labelFont; } set { m_labelFont = value; } }
    }

    public class SongBook : AbstractSongDatabase
    {
        public static readonly SongBookManager Manager = new SongBookManager();
        string m_filename;
        BookLayout m_layout = new BookLayout();
        BookSequence m_sequence;
        IPrintTarget m_printTarget;
        SongBookFonts m_fonts = new SongBookFonts();
        Dictionary<int, PaneGrp> m_formatted = new Dictionary<int, PaneGrp>();
        
        protected override void WantOpen() { }

        public SongBook()
        {
            m_dataset = new SongDb();
            //m_dataset.song.TableNewRow += song_TableNewRow;
            m_dataset.song.songRowDeleted += song_songRowChanged;
            m_dataset.song.songRowChanged += song_songRowChanged;

            m_sequence = new BookSequence();
            m_sequence.Items.Add(new AllSongsSequenceItem());

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.AddPage();
            PrintTarget = new PdfPrintTarget(page);
        }

        void song_songRowChanged(object sender, SongDb.songRowChangeEvent e)
        {
            if (Changed != null) Changed(sender, e);
        }

        /*
        void song_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            if (Changed != null) Changed(sender, e);
        }
        */

        public string Title
        {
            get
            {
                if (m_filename == null) return "[Beze jména]";
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
                xw.WriteStartElement(XmlNamespaces.SongBook_Prefix, "SongBook", XmlNamespaces.SongBook);
                xw.WriteStartElement(XmlNamespaces.SongBook_Prefix, "Songs", XmlNamespaces.SongBook);
                m_dataset.WriteXml(xw);
                xw.WriteEndElement();
                Options.SaveOptions(xw, this);
                xw.WriteEndElement();
            }
        }
        [PropertyPage(Name = "fonts", Title = "Fonty")]
        public SongBookFonts Fonts { get { return m_fonts; } set { m_fonts = value; } }

        [PropertyPage(Name = "layout", Title = "Vzhled stránky")]
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
        public void Reformat()
        {
            m_formatted.Clear();
        }
        public event EventHandler Changed;
        public IPrintTarget PrintTarget
        {
            get { return m_printTarget; }
            set
            {
                m_printTarget = value;
                Layout.Target = value;
                Reformat();                
            }
        }
        public void Load(string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNamespaceManager mgr = XmlNamespaces.CreateManager(doc.NameTable);

            XmlNode songs = doc.DocumentElement.SelectSingleNode("sb:Songs", mgr);
            m_dataset.ReadXml(new XmlNodeReader(songs.FirstChild));

            XmlNode options = doc.DocumentElement.SelectSingleNode("opt:Options", mgr);
            Options.LoadOptions((XmlElement)options, this);

            m_filename = filename;
            PrintTarget = m_printTarget;
        }
    }
}
