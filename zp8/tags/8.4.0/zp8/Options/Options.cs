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
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public class PropertyPageAttribute : Attribute
    {
        public string Name;
        public string Title;
    }

    public struct PropertyPageReference
    {
        public readonly string Name;
        public readonly string Title;
        public readonly object PageObject;
        public readonly PropertyInfo Property;
        public PropertyPageReference(string name, string title, object page, PropertyInfo property)
        {
            Name = name;
            Title = title;
            PageObject = page;
            Property = property;
        }
    }

    public static class Options
    {
        public static string CfgDirectory { get { return Directories.CfgDirectory; } }

        public static IEnumerable<PropertyPageReference> GetPropertyPages(object obj)
        {
            foreach (PropertyInfo info in obj.GetType().GetProperties())
            {
                foreach (PropertyPageAttribute attr in info.GetCustomAttributes(typeof(PropertyPageAttribute), true))
                {
                    object page = info.GetGetMethod().Invoke(obj, new object[] { });
                    yield return new PropertyPageReference(attr.Name, attr.Title, page, info);
                }
            }
        }

        public static void SaveOptions(XmlWriter xw, object obj)
        {
            xw.WriteStartElement(XmlNamespaces.Options_Prefix, "Options", XmlNamespaces.Options);
            foreach (PropertyPageReference page in GetPropertyPages(obj))
            {
                XmlSerializer xser = new XmlSerializer(page.PageObject.GetType());
                xw.WriteStartElement(XmlNamespaces.Options_Prefix, "Page", XmlNamespaces.Options);
                xw.WriteAttributeString("name", page.Name);
                xser.Serialize(xw, page.PageObject);
                xw.WriteEndElement();
            }
            xw.WriteEndElement();
        }

        public static void LoadOptions(XmlElement root, object obj)
        {
            foreach (PropertyPageReference page in GetPropertyPages(obj))
            {
                XmlElement xml = root == null ? null : (XmlElement)root.SelectSingleNode(String.Format("*[@name='{0}']", page.Name));

                if (xml != null)
                {
                    XmlSerializer xser = new XmlSerializer(page.Property.PropertyType);
                    object newpage = xser.Deserialize(new XmlNodeReader(xml.FirstChild));
                    page.Property.GetSetMethod().Invoke(obj, new object[] { newpage });
                }
                else
                {
                    object newpage = page.Property.PropertyType.GetConstructor(new Type[] { }).Invoke(new object[] { });
                    page.Property.GetSetMethod().Invoke(obj, new object[] { newpage });
                }

            }
        }

        static Options()
        {
            try { Directory.CreateDirectory(CfgDirectory); }
            catch (Exception) { }
        }

        /*
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
        */
    }
}
