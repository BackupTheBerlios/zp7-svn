using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace zp8
{
    public class PropertyPage
    {
        internal string m_name;
        internal string m_title;
    }

    public class SongViewPropertyPage : PropertyPage
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

    public class Options
    {
        List<PropertyPage> m_pages = new List<PropertyPage>();
        public readonly static Options GlobalOpts = new Options();

        public static string OptionsFile { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "options.xml"); } }

        public List<PropertyPage> Pages { get { return m_pages; } }

        public SongViewPropertyPage songview { get { return (SongViewPropertyPage)m_pages[0]; } }

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
                page = (PropertyPage)xser.Deserialize(new XmlNodeReader(xml.FirstChild));
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
            using (XmlWriter xw = XmlWriter.Create(OptionsFile))
            {
                //xw.WriteStartElement("opt", "Options", "http://zpevnik.net/Options.xsd");
                xw.WriteStartElement("Options");
                foreach (PropertyPage page in m_pages)
                {
                    XmlSerializer xser = new XmlSerializer(page.GetType());
                    xw.WriteStartElement(page.m_name);
                    xser.Serialize(xw, page);
                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
            }
        }
    }
}
