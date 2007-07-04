using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.ComponentModel;

namespace zp8
{
    public class PropertyPage
    {
        internal string m_name;
        internal string m_title;
    }

    public class SongViewPropertyPage : PropertyPage
    {
        Font m_titleFont;
        Font m_chordFont;
        Font m_textFont;

        [Description("Font názvu písnì")]
        public Font TitleFont { get { return m_titleFont; } set { m_titleFont = value; } }
        [Description("Font akordù")]
        public Font ChordFont { get { return m_chordFont; } set { m_chordFont = value; } }
        [Description("Font textu")]
        public Font TextFont { get { return m_textFont; } set { m_textFont = value; } }
    }

    public class Options
    {
        List<PropertyPage> m_pages = new List<PropertyPage>();
        public readonly static Options GlobalOpts = new Options();

        public static string OptionsFile { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "options.xml"); } }

        public List<PropertyPage> Pages { get { return m_pages; } }


        public Options()
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
            LoadOption(root, "songview", "Prohlížení písnì", typeof(SongViewPropertyPage));
        }

        private void LoadOption(XmlElement root, string name, string title, Type type)
        {
            PropertyPage page;
            XmlElement xml = root == null ? null : (XmlElement)root.SelectSingleNode(name);
            if (xml != null)
            {
                XmlSerializer xser = new XmlSerializer(type);
                page = (PropertyPage)xser.Deserialize(new XmlNodeReader(xml));
            }
            else
            {
                page = (PropertyPage)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
            }
            page.m_name = name;
            page.m_title = title;
            m_pages.Add(page);
        }

        public void Save()
        {
        }
    }
}
