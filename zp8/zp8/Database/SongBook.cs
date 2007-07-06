using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.ComponentModel;

using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

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
        PersistentFont m_titleFont = new PersistentFont();
        PersistentFont m_authorFont = new PersistentFont();

        [Description("Font textu")]
        public PersistentFont TextFont { get { return m_textFont; } set { m_textFont = value; } }
        [Description("Font akordù")]
        public PersistentFont ChordFont { get { return m_chordFont; } set { m_chordFont = value; } }
        [Description("Font Návìští")]
        public PersistentFont LabelFont { get { return m_labelFont; } set { m_labelFont = value; } }
        [Description("Font Názvu")]
        public PersistentFont TitleFont { get { return m_titleFont; } set { m_titleFont = value; } }
        [Description("Font Autora")]
        public PersistentFont AuthorFont { get { return m_authorFont; } set { m_authorFont = value; } }
    }

    public class SongBookFormatting
    {
        int m_songSpaceHeight = 100;
        bool m_printSongDividers = true;

        [Description("Tisknout èáry mezi písnìmi")]
        public bool PrintSongDividers { get { return m_printSongDividers; } set { m_printSongDividers = value; } }
        [Description("Výška mezery mezi písnìmi v procentech výšky øádku")]
        public int SongSpaceHeight { get { return m_songSpaceHeight; } set { m_songSpaceHeight = value; } }
    }

    public class SongBook : AbstractSongDatabase
    {
        public static readonly SongBookManager Manager = new SongBookManager();
        string m_filename;
        BookLayout m_layout = new BookLayout();
        SongBookFormatting m_formatting = new SongBookFormatting();
        BookSequence m_sequence;
        IPrintTarget m_printTarget;
        SongBookFonts m_fonts = new SongBookFonts();
        Dictionary<int, PaneGrp> m_formatted = new Dictionary<int, PaneGrp>();
        SongFormatOptions m_songFormatOptions;
        BookFormatOptions m_bookFormatOptions;
        
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

        [PropertyPage(Name = "formatting", Title = "Formátování")]
        public SongBookFormatting Formatting { get { return m_formatting; } set { m_formatting = value; } }

        public FormattedBook Format()
        {
            LogPages pages = m_sequence.CreateLogPages(this);
            return new FormattedBook(pages, Layout);
        }
        public SongFormatOptions SongFormatOptions
        {
            get
            {
                if (m_songFormatOptions == null) m_songFormatOptions = new SongFormatOptions(m_layout.SmallPageWidth, m_fonts.TextFont, m_fonts.ChordFont, m_fonts.LabelFont);
                return m_songFormatOptions;
            }
        }
        public BookFormatOptions BookFormatOptions
        {
            get
            {
                if (m_bookFormatOptions == null) m_bookFormatOptions = new BookFormatOptions(m_layout.SmallPageWidth, Fonts, Formatting, SongFormatOptions);
                return m_bookFormatOptions;
            }
        }

        public PaneGrp FormatSong(int songid)
        {
            if (m_formatted.ContainsKey(songid)) return m_formatted[songid];
            SongDb.songRow row = DataSet.song.FindByID(songid);

            SongFormatter fmt = new SongFormatter(row.songtext, SongFormatOptions);
            fmt.Run();
            PaneGrp grp = fmt.Result;
            grp.Insert(new SongHeaderPane(BookFormatOptions, row.title, row.author));
            grp.Add(new SongSeparatorPane(BookFormatOptions));
            m_formatted[songid] = grp;

            return m_formatted[songid];
        }
        public void ClearCaches()
        {
            m_formatted.Clear();
            m_songFormatOptions = null;
            m_bookFormatOptions = null;
        }
        public event EventHandler Changed;
        public IPrintTarget PrintTarget
        {
            get { return m_printTarget; }
            set
            {
                m_printTarget = value;
                Layout.Target = value;
                ClearCaches();                
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

        public void ExportAsPDF(string filename)
        {
            FormattedBook fbook = Format();
            PdfDocument doc = new PdfDocument();
            for (int i = 0; i < fbook.A4SheetCount * 2; i++)
            {
                PdfPage page = doc.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                fbook.DrawBigPage(gfx, i / 2, i % 2);
            }
            doc.Save(filename);
        }

        public void SetBookStyle(string newstyle)
        {
            BookStyle style = BookStyle.LoadBookStyle(newstyle);
            Layout = style.Layout;
            Fonts = style.Fonts;
            Formatting = style.Formatting;
            PrintTarget = m_printTarget;
        }
    }
}
