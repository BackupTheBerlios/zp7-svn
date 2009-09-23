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
    public class SongBookHolder
    {
        public int ID;
        public string Name;
        public override string ToString()
        {
            return Name;
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

        [Category("Píseò")]
        [DisplayName("Text")]
        public PersistentFont TextFont { get { return m_textFont; } set { m_textFont = value; } }
        [Category("Píseò")]
        [DisplayName("Akordy")]
        public PersistentFont ChordFont { get { return m_chordFont; } set { m_chordFont = value; } }
        [Category("Píseò")]
        [DisplayName("Návìští")]
        public PersistentFont LabelFont { get { return m_labelFont; } set { m_labelFont = value; } }
        [Category("Píseò")]
        [DisplayName("Název")]
        public PersistentFont TitleFont { get { return m_titleFont; } set { m_titleFont = value; } }
        [Category("Píseò")]
        [DisplayName("Autor")]
        public PersistentFont AuthorFont { get { return m_authorFont; } set { m_authorFont = value; } }

        [Category("Obsah")]
        [DisplayName("Název")]
        [Description("Font názvu písnì v obsahu")]
        public PersistentFont OutlineTitleFont { get { return m_outlineTitleFont; } set { m_outlineTitleFont = value; } }

        [Category("Obsah")]
        [DisplayName("Èíslo øádky")]
        [Description("Font èísla øádky v obsahu")]
        public PersistentFont PageNumberFont { get { return m_pageNumberFont; } set { m_pageNumberFont = value; } }

        [Category("Stránka")]
        [DisplayName("Záhlaví")]
        public PersistentFont HeaderFont { get { return m_headerFont; } set { m_headerFont = value; } }

        [Category("Stránka")]
        [DisplayName("Zápatí")]
        public PersistentFont FooterFont { get { return m_footerFont; } set { m_footerFont = value; } }
    }

    public class SongBookFormatting : DatAdmin.PropertyPageBase
    {
        int m_songSpaceHeight = 100;
        bool m_printSongDividers = true;
        SongOrder m_order;
        string m_header = "";
        string m_footer = "";

        [DisplayName("Èáry mezi písnìmi")]
        [Description("Tisknout èáry mezi písnìmi")]
        [TypeConverter(typeof(DatAdmin.YesNoTypeConverter))]
        public bool PrintSongDividers { get { return m_printSongDividers; } set { m_printSongDividers = value; } }

        [DisplayName("Výška mezery")]
        [Description("Výška mezery mezi písnìmi v procentech výšky øádku")]
        public int SongSpaceHeight { get { return m_songSpaceHeight; } set { m_songSpaceHeight = value; } }

        [DisplayName("Poøadí písní")]
        [TypeConverter(typeof(DatAdmin.EnumDescConverter))]
        public SongOrder Order { get { return m_order; } set { m_order = value; } }

        [DisplayName("Text záhlaví")]
        [Description("%c bude nahrazeno èíslem stránky")]
        public string Header { get { return m_header; } set { m_header = value; } }

        [DisplayName("Text zápatí")]
        [Description("%c bude nahrazeno èíslem stránky")]
        public string Footer { get { return m_footer; } set { m_footer = value; } }
    }

    public enum OutlinePosition 
    { 
        [Description("Na zaèátku")]
        Begin,
        [Description("Na konci")]
        End
    };
    public enum SongOrder 
    {
        [Description("Podle názvu, pak skupiny")]
        TitleGroup,
        [Description("Podle skupiny, pak názvu")]
        GroupTitle,
        [Description("Stejné jako v databázi")]
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

        [DisplayName("Poèet sloupcù")]
        public int Columns { get { return m_columns; } set { m_columns = value; } }

        [DisplayName("Umístìní obsahu")]
        [TypeConverter(typeof(DatAdmin.EnumDescConverter))]
        public OutlinePosition Position { get { return m_position; } set { m_position = value; } }

        [DisplayName("Poøadí písní")]
        [TypeConverter(typeof(DatAdmin.EnumDescConverter))]
        public SongOrder Order { get { return m_order; } set { m_order = value; } }
    }

    public class SongBook
    {
        //public static readonly SongBookManager Manager = new SongBookManager();
        //string m_filename;
        //bool m_modifyFlag;
        int m_id;
        string m_name;
        SongDatabase m_db;
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
        
        public SongBook(SongDatabase db, int id)
        {
            PrintTarget = new PdfPrintTarget();
            m_db = db;
            m_name = m_db.ExecuteScalar("select name from songlist where id=@id", "id", id).ToString();
        }

        public int ID { get { return m_id; } }

        void DispatchRowChanged()
        {
            m_fbook = null;
            if (BookChanged != null) BookChanged(this, EventArgs.Empty);
        }

        /*
        void song_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            if (Changed != null) Changed(sender, e);
        }
        */

        public string Title
        {
            get { return m_name; }
        }

        //public bool Modified
        //{
        //    get
        //    {
        //        return m_dataset.HasChanges() || m_modifyFlag;
        //    }
        //}
        //public string FileName { get { return m_filename; } set { m_filename = value; } }

        private void SaveOptions()
        {
            StringWriter sw = new StringWriter();
            using (XmlWriter xw = XmlWriter.Create(sw))
            {
                Options.SaveOptions(xw, this);
                xw.Flush();
            }
            m_db.ExecuteNonQuery("update songlist set options=@opt where id=@id", "opt", sw.ToString(), "id", m_id);
        }

        [PropertyPage(Name = "fonts", Title = "Fonty")]
        public SongBookFonts Fonts { get { return m_fonts; } set { m_fonts = value; } }

        [PropertyPage(Name = "layout", Title = "Vzhled stránky")]
        public BookLayout Layout { get { return m_layout; } set { m_layout = value; } }

        [PropertyPage(Name = "formatting", Title = "Formátování")]
        public SongBookFormatting Formatting { get { return m_formatting; } set { m_formatting = value; } }

        [PropertyPage(Name = "outline", Title = "Obsah")]
        public OutlineProperties OutlineProperties { get { return m_outlineProperties; } set { m_outlineProperties = value; } }

        public FormattedBook Book
        {
            get
            {
                if (m_fbook == null)
                {
                    using (IWaitDialog wait = WaitForm.Show("Formátuji zpìvník", false))
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

        public PaneGrp FormatSong(SongData song)
        {
            if (m_formatted.ContainsKey(song.LocalID)) return m_formatted[song.LocalID];

            SongFormatter fmt = new SongFormatter(song.SongText, SongFormatOptions);
            fmt.Run();
            PaneGrp grp = fmt.Result;
            grp.Insert(new SongHeaderPane(BookFormatOptions, song.Title, song.Author));
            grp.Add(new SongSeparatorPane(BookFormatOptions));
            m_formatted[song.LocalID] = grp;

            return m_formatted[song.LocalID];
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
        //public void Load(string filename)
        //{
        //    UnInstallTriggers();
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(filename);
        //    XmlNamespaceManager mgr = XmlNamespaces.CreateManager(doc.NameTable);

        //    XmlNode songs = doc.DocumentElement.SelectSingleNode("sb:Songs", mgr);
        //    m_dataset.ReadXml(new XmlNodeReader(songs.FirstChild));
        //    m_dataset.AcceptChanges();

        //    XmlNode options = doc.DocumentElement.SelectSingleNode("opt:Options", mgr);
        //    Options.LoadOptions((XmlElement)options, this);

        //    //m_filename = filename;
        //    PrintTarget = m_printTarget;
        //    //InstallTriggers();
        //}

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

        public IEnumerable<SongData> EnumSongs()
        {
            return m_db.LoadSongList(m_id);
        }

        public IEnumerable<SongData> GetSongs(SongOrder order)
        {
            List<SongData> rows = new List<SongData>();
            foreach (SongData row in EnumSongs())
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

        public void Modified()
        {
            SaveOptions();
        }
    }
}
