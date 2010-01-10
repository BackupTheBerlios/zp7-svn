using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Xml;
using System.IO;
using System.Drawing;

namespace zp8
{
    /*
    public class PropertyPage
    {
        internal string m_name;
        internal string m_title;
    }
    */

    public abstract class SongFontsPropertyPage : DatAdmin.PropertyPageBase
    {
        PersistentFont m_textFont = new PersistentFont();
        PersistentFont m_chordFont = new PersistentFont();
        PersistentFont m_labelFont = new PersistentFont();

        [DisplayName("Font textu")]
        public PersistentFont TextFont { get { return m_textFont; } set { m_textFont = value; } }
        [DisplayName("Font akordù")]
        public PersistentFont ChordFont { get { return m_chordFont; } set { m_chordFont = value; } }
        [DisplayName("Font Návìští")]
        public PersistentFont LabelFont { get { return m_labelFont; } set { m_labelFont = value; } }
    }

    public class SongViewPropertyPage : SongFontsPropertyPage
    {
    }

    public class SongPrintPropertyPage : SongFontsPropertyPage
    {
        PersistentFont m_titleFont = new PersistentFont();
        PersistentFont m_authorFont = new PersistentFont();

        [DisplayName("Font názvu")]
        public PersistentFont TitleFont { get { return m_titleFont; } set { m_titleFont = value; } }
        [DisplayName("Font autora")]
        public PersistentFont AuthorFont { get { return m_authorFont; } set { m_authorFont = value; } }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ExportFontsPropertyPage : SongPrintPropertyPage
    {
        public ExportFontsPropertyPage()
        {
            TextFont.FontName = "Courier New";
            LabelFont.FontName = "Courier New";
            ChordFont.FontName = "Courier New";
            LabelFont.Underline = true;
            ChordFont.Bold = true;
            TitleFont.FontSize = 15;
            AuthorFont.FontSize = 12;
            AuthorFont.Italic = true;
            ChordFont.FontColor = Color.Blue;
        }
    }

    public class GlobalOpts
    {
        SongViewPropertyPage m_songview;
        SongPrintPropertyPage m_songprint;

        public static readonly GlobalOpts Default = new GlobalOpts();
        public static string OptionsFile { get { return Path.Combine(Options.CfgDirectory, "options.xml"); } }

        [PropertyPage(Name="songview",Title="Prohlížení písnì")]
        public SongViewPropertyPage SongView { get { return m_songview; } set { m_songview = value; } }

        [PropertyPage(Name = "songprint", Title = "Tisk písnì")]
        public SongPrintPropertyPage SongPrint { get { return m_songprint; } set { m_songprint = value; } }

        private GlobalOpts()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root;
            try
            {
                doc.Load(OptionsFile);
                root = doc.DocumentElement;
            }
            catch (Exception)
            {
                root = null;
            }
            Options.LoadOptions(root, this);
        }

        public void Save()
        {
            using (XmlWriter xw = XmlWriter.Create(OptionsFile))
            {
                Options.SaveOptions(xw, this);
            }
        }

    }
}
