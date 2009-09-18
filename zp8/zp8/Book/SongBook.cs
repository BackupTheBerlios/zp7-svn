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

    public class SongBookFonts : DatAdmin.PropertyPageBase
    {
        PersistentFont m_textFont = new PersistentFont();
        PersistentFont m_chordFont = new PersistentFont();
        PersistentFont m_labelFont = new PersistentFont();
        PersistentFont m_titleFont = new PersistentFont();
        PersistentFont m_authorFont = new PersistentFont();
        PersistentFont m_outlineTitleFont = new PersistentFont();
        PersistentFont m_pageNumberFont = new PersistentFont();
        PersistentFont m_headerFont = new PersistentFont();
        PersistentFont m_footerFont = new PersistentFont();

        [Category("P�se�")]
        [DisplayName("Text")]
        public PersistentFont TextFont { get { return m_textFont; } set { m_textFont = value; } }
        [Category("P�se�")]
        [DisplayName("Akordy")]
        public PersistentFont ChordFont { get { return m_chordFont; } set { m_chordFont = value; } }
        [Category("P�se�")]
        [DisplayName("N�v�t�")]
        public PersistentFont LabelFont { get { return m_labelFont; } set { m_labelFont = value; } }
        [Category("P�se�")]
        [DisplayName("N�zev")]
        public PersistentFont TitleFont { get { return m_titleFont; } set { m_titleFont = value; } }
        [Category("P�se�")]
        [DisplayName("Autor")]
        public PersistentFont AuthorFont { get { return m_authorFont; } set { m_authorFont = value; } }

        [Category("Obsah")]
        [DisplayName("N�zev")]
        [Description("Font n�zvu p�sn� v obsahu")]
        public PersistentFont OutlineTitleFont { get { return m_outlineTitleFont; } set { m_outlineTitleFont = value; } }

        [Category("Obsah")]
        [DisplayName("��slo ��dky")]
        [Description("Font ��sla ��dky v obsahu")]
        public PersistentFont PageNumberFont { get { return m_pageNumberFont; } set { m_pageNumberFont = value; } }

        [Category("Str�nka")]
        [DisplayName("Z�hlav�")]
        public PersistentFont HeaderFont { get { return m_headerFont; } set { m_headerFont = value; } }

        [Category("Str�nka")]
        [DisplayName("Z�pat�")]
        public PersistentFont FooterFont { get { return m_footerFont; } set { m_footerFont = value; } }
    }

    public class SongBookFormatting : DatAdmin.PropertyPageBase
    {
        int m_songSpaceHeight = 100;
        bool m_printSongDividers = true;
        SongOrder m_order;
        string m_header = "";
        string m_footer = "";

        [DisplayName("��ry mezi p�sn�mi")]
        [Description("Tisknout ��ry mezi p�sn�mi")]
        [TypeConverter(typeof(DatAdmin.YesNoTypeConverter))]
        public bool PrintSongDividers { get { return m_printSongDividers; } set { m_printSongDividers = value; } }

        [DisplayName("V��ka mezery")]
        [Description("V��ka mezery mezi p�sn�mi v procentech v��ky ��dku")]
        public int SongSpaceHeight { get { return m_songSpaceHeight; } set { m_songSpaceHeight = value; } }

        [DisplayName("Po�ad� p�sn�")]
        [TypeConverter(typeof(DatAdmin.EnumDescConverter))]
        public SongOrder Order { get { return m_order; } set { m_order = value; } }

        [DisplayName("Text z�hlav�")]
        [Description("%c bude nahrazeno ��slem str�nky")]
        public string Header { get { return m_header; } set { m_header = value; } }

        [DisplayName("Text z�pat�")]
        [Description("%c bude nahrazeno ��slem str�nky")]
        public string Footer { get { return m_footer; } set { m_footer = value; } }
    }

    public enum OutlinePosition 
    { 
        [Description("Na za��tku")]
        Begin,
        [Description("Na konci")]
        End
    };
    public enum SongOrder 
    {
        [Description("Podle n�zvu, pak skupiny")]
        TitleGroup,
        [Description("Podle skupiny, pak n�zvu")]
        GroupTitle,
        [Description("Stejn� jako v datab�zi")]
        Database 
    };

    public class OutlineProperties : DatAdmin.PropertyPageBase
    {
        bool m_printOutline = false;
        int m_columns = 2;
        OutlinePosition m_position = OutlinePosition.Begin;
        SongOrder m_order = SongOrder.TitleGroup;

        [DisplayName("Tisknout obsah")]
        [TypeConverter(typeof(DatAdmin.YesNoTypeConverter))]
        public bool PrintOutline { get { return m_printOutline; } set { m_printOutline = value; } }

        [DisplayName("Po�et sloupc�")]
        public int Columns { get { return m_columns; } set { m_columns = value; } }

        [DisplayName("Um�st�n� obsahu")]
        [TypeConverter(typeof(DatAdmin.EnumDescConverter))]
        public OutlinePosition Position { get { return m_position; } set { m_position = value; } }

        [DisplayName("Po�ad� p�sn�")]
        [TypeConverter(typeof(DatAdmin.EnumDescConverter))]
        public SongOrder Order { get { return m_order; } set { m_order = value; } }
    }

    public class SongBook : AbstractSongDatabase
    {
        public static readonly SongBookManager Manager = new SongBookManager();
        string m_filename;
        bool m_modifyFlag;
        BookLayout m_layout = new BookLayout();
        SongBookFormatting m_formatting = new SongBookFormatting();
        //BookSequence m_sequence;
        IPrintTarget m_printTarget;
        SongBookFonts m_fonts = new SongBookFonts();
        OutlineProperties m_outlineProperties = new OutlineProperties();
        Dictionary<int, PaneGrp> m_formatted = new Dictionary<int, PaneGrp>();
        SongFormatOptions m_songFormatOptions;
        BookFormatOptions m_bookFormatOptions;
        PageDrawOptions m_pageDrawOptions;
        FormattedBook m_fbook;
        
        protected override void WantOpen() { }

        public SongBook()
        {
            m_dataset = new SongDb();
            //m_dataset.song.TableNewRow += song_TableNewRow;
            m_dataset.song.songRowDeleted += song_songRowChanged;
            m_dataset.song.songRowChanged += song_songRowChanged;

            //m_sequence = new BookSequence();
            //m_sequence.Items.Add(new AllSongsSequenceItem());

            //PdfDocument doc = new PdfDocument();
            //PdfPage page = doc.AddPage();
            PrintTarget = new PdfPrintTarget();
            InstallTriggers();
        }

        void song_songRowChanged(object sender, SongDb.songRowChangeEvent e)
        {
            m_fbook = null;
            if (BookChanged != null) BookChanged(sender, e);
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
                if (m_filename == null) return "[Beze jm�na]";
                return Path.GetFileName(m_filename);
            }
        }

        public bool Modified
        {
            get
            {
                return m_dataset.HasChanges() || m_modifyFlag;
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
            m_modifyFlag = false;
        }

        [PropertyPage(Name = "fonts", Title = "Fonty")]
        public SongBookFonts Fonts { get { return m_fonts; } set { m_fonts = value; } }

        [PropertyPage(Name = "layout", Title = "Vzhled str�nky")]
        public BookLayout Layout { get { return m_layout; } set { m_layout = value; } }

        [PropertyPage(Name = "formatting", Title = "Form�tov�n�")]
        public SongBookFormatting Formatting { get { return m_formatting; } set { m_formatting = value; } }

        [PropertyPage(Name = "outline", Title = "Obsah")]
        public OutlineProperties OutlineProperties { get { return m_outlineProperties; } set { m_outlineProperties = value; } }

        public FormattedBook Book
        {
            get
            {
                if (m_fbook == null)
                {
                    using (IWaitDialog wait = WaitForm.Show("Form�tuji zp�vn�k", false))
                    {
                        LogPages pages = Sequence.CreateLogPages(this);
                        m_fbook = new FormattedBook(pages, Layout, PageDrawOptions);
                    }
                }
                return m_fbook;
            }
        }
        /*
        public FormattedBook Format()
        {
            LogPages pages = m_sequence.CreateLogPages(this);
            return new FormattedBook(pages, Layout);
        }
        */

        public SongFormatOptions SongFormatOptions
        {
            get
            {
                if (m_songFormatOptions == null) m_songFormatOptions = new SongFormatOptions(m_layout.SmallPageWidth, PrintTarget.GetInfoContext(Layout.Orientation), m_fonts.TextFont, m_fonts.ChordFont, m_fonts.LabelFont, PrintTarget.mmky);
                return m_songFormatOptions;
            }
        }
        public BookFormatOptions BookFormatOptions
        {
            get
            {
                if (m_bookFormatOptions == null) m_bookFormatOptions = new BookFormatOptions(m_layout.SmallPageWidth, PrintTarget.GetInfoContext(Layout.Orientation), Fonts, Formatting, SongFormatOptions, PrintTarget.mmky);
                return m_bookFormatOptions;
            }
        }
        public PageDrawOptions PageDrawOptions
        {
            get
            {
                if (m_pageDrawOptions == null) m_pageDrawOptions = new PageDrawOptions(Layout.SmallPageWidth, Layout.SmallPageHeight, Fonts.HeaderFont, Fonts.FooterFont, Formatting.Header, Formatting.Footer, PrintTarget.mmky);
                return m_pageDrawOptions;
            }
        }

        public PaneGrp FormatSong(int songid)
        {
            if (m_formatted.ContainsKey(songid)) return m_formatted[songid];
            SongDb.songRow row = DataSet.song.FindByID(songid);

            SongFormatter fmt = new SongFormatter(row.SongText, SongFormatOptions);
            fmt.Run();
            PaneGrp grp = fmt.Result;
            grp.Insert(new SongHeaderPane(BookFormatOptions, row.Title, row.Author));
            grp.Add(new SongSeparatorPane(BookFormatOptions));
            m_formatted[songid] = grp;

            return m_formatted[songid];
        }
        public void ClearCaches()
        {
            m_formatted.Clear();
            m_songFormatOptions = null;
            m_bookFormatOptions = null;
            m_pageDrawOptions = null;
            m_fbook = null;
        }
        public event EventHandler BookChanged;
        public IPrintTarget PrintTarget
        {
            get { return m_printTarget; }
            set { SetPrintTarget(value, true); }
        }
        public void SetPrintTarget(IPrintTarget value, bool dispose)
        {
            if (dispose && m_printTarget != null) m_printTarget.Dispose();
            m_printTarget = value;
            Layout.Target = value;
            ClearCaches();
            if (BookChanged != null) BookChanged(this, new EventArgs());
        }
        public void Load(string filename)
        {
            UnInstallTriggers();
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNamespaceManager mgr = XmlNamespaces.CreateManager(doc.NameTable);

            XmlNode songs = doc.DocumentElement.SelectSingleNode("sb:Songs", mgr);
            m_dataset.ReadXml(new XmlNodeReader(songs.FirstChild));
            m_dataset.AcceptChanges();

            XmlNode options = doc.DocumentElement.SelectSingleNode("opt:Options", mgr);
            Options.LoadOptions((XmlElement)options, this);

            m_filename = filename;
            PrintTarget = m_printTarget;
            InstallTriggers();
        }

        public void ExportAsPDF(string filename)
        {
            FormattedBook fbook = Book;
            PdfDocument doc = new PdfDocument();
            for (int i = 0; i < fbook.A4SheetCount * 2; i++)
            {
                PdfPage page = doc.AddPage();
                page.Orientation = Layout.Orientation;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                fbook.DrawBigPage(gfx, i / 2, i % 2, PageDrawOptions);
            }
            doc.Save(filename);
        }

        public void SetBookStyle(string newstyle)
        {
            BookStyle style = BookStyle.LoadBookStyle(newstyle);
            Layout = style.Layout;
            Fonts = style.Fonts;
            Formatting = style.Formatting;
            this.OutlineProperties = style.OutlineProperties;
            PrintTarget = m_printTarget;
        }
        public int? FindPageNumber(int songid)
        {
            FormattedBook book = Book;
            PaneGrp grp = m_formatted[songid];
            Pane header = grp.FirstPane;
            foreach (LogPage page in book.OriginalPages)
            {
                if (page.HasPane(header)) return page.PageNumber;
            }
            return null;
        }

        protected BookSequence Sequence
        {
            get
            {
                BookSequence seq = new BookSequence();
                if (OutlineProperties.PrintOutline && OutlineProperties.Position == OutlinePosition.Begin)
                    seq.Items.Add(new OutlineSequenceItem());
                seq.Items.Add(new AllSongsSequenceItem());
                if (OutlineProperties.PrintOutline && OutlineProperties.Position == OutlinePosition.End)
                    seq.Items.Add(new OutlineSequenceItem());
                return seq;
            }
        }

        public IEnumerable<ISongRow> GetSongs(SongOrder order)
        {
            List<ISongRow> rows = new List<ISongRow>();
            foreach (ISongRow row in EnumSongs())
            {
                rows.Add(row);
            }
            rows.Sort(Sorting.GetComparison(order));
            return rows;
        }

        public PaneGrp FormatOutline()
        {
            OutlineFormatOptions opt = new OutlineFormatOptions(
                Layout.SmallPageWidth, 
                Layout.SmallPageHeight, 
                PrintTarget.GetInfoContext(Layout.Orientation),
                Fonts.OutlineTitleFont, 
                Fonts.PageNumberFont, 
                OutlineProperties.Columns, 
                this,
                PrintTarget.mmky);
            OutlineFormatter fmt = new OutlineFormatter(opt, GetSongs(OutlineProperties.Order));
            fmt.Run();
            return fmt.Result;
        }

        public void SetModifyFlag()
        {
            m_modifyFlag = true;
        }
    }
}
