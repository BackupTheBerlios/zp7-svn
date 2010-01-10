using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml;

namespace zp8
{
    // byvaly typ zpevniku
    public class BookStyle
    {
        public static string SbTypesDirectory { get { return Path.Combine(Options.CfgDirectory, "bookstyles"); } }

        public readonly string Name;

        SongBookFonts m_fonts = new SongBookFonts();
        BookLayout m_layout = new BookLayout();
        SongBookFormatting m_formatting = new SongBookFormatting();
        OutlineProperties m_outlineProperties = new OutlineProperties();

        [PropertyPage(Name = "fonts", Title = "Fonty")]
        public SongBookFonts Fonts { get { return m_fonts; } set { m_fonts = value; } }

        [PropertyPage(Name = "layout", Title = "Vzhled stránky")]
        public BookLayout Layout { get { return m_layout; } set { m_layout = value; } }

        [PropertyPage(Name = "formatting", Title = "Formátování")]
        public SongBookFormatting Formatting { get { return m_formatting; } set { m_formatting = value; } }

        [PropertyPage(Name = "outline", Title = "Obsah")]
        public OutlineProperties OutlineProperties { get { return m_outlineProperties; } set { m_outlineProperties = value; } }

        static BookStyle()
        {
            try { Directory.CreateDirectory(SbTypesDirectory); }
            catch (Exception) { }
        }

        public static IEnumerable<string> GetSbTypes()
        {
            foreach (string file in Directory.GetFiles(SbTypesDirectory))
            {
                string name = Path.GetFileName(file);
                if (Path.GetExtension(name).ToLower() == ".bs")
                    yield return Path.ChangeExtension(name, null);
            }
        }

        public BookStyle(string name)
        {
            Name = name;
        }

        public string FileName { get { return Path.Combine(SbTypesDirectory, Name + ".bs"); } }

        public bool Load()
        {
            if (File.Exists(FileName))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(FileName);
                XmlNamespaceManager mgr = XmlNamespaces.CreateManager(doc.NameTable);
                Options.LoadOptions((XmlElement)doc.DocumentElement.SelectSingleNode("opt:Options", mgr), this);
                return true;
            }
            return false;
        }

        public void Save()
        {
            using (XmlWriter xw = XmlWriter.Create(FileName))
            {
                xw.WriteStartElement(XmlNamespaces.BookStyle_Prefix, "BookStyle", XmlNamespaces.BookStyle);
                Options.SaveOptions(xw, this);
                xw.WriteEndElement();
            }
        }

        public static BookStyle LoadBookStyle(string name)
        {
            BookStyle res = new BookStyle(name);
            res.Load();
            return res;
        }

        public static bool Exists(string name)
        {
            BookStyle bs = new BookStyle(name);
            return File.Exists(bs.FileName);
        }
    }
}
