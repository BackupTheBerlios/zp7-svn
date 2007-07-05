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

    public class FontEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                if (!context.PropertyDescriptor.IsReadOnly)
                {
                    return UITypeEditorEditStyle.Modal;
                }
            }
            return UITypeEditorEditStyle.None;
        }

        [RefreshProperties(RefreshProperties.All)]
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            if (context == null || provider == null || context.Instance == null)
            {
                return base.EditValue(provider, value);
            }
            FontDialog dlg = new FontDialog();
            PersistentFont src = (PersistentFont)value;
            dlg.ShowColor = true;
            dlg.Font = src.ToFont();
            dlg.Color = src.FontColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return PersistentFont.FromFont(dlg.Font, dlg.Color);
            }
            else
            {
                return value;
            }
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            if (context == null || context.Instance == null)
            {
                return base.GetPaintValueSupported(context);
            }
            return true;
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value != null)
            {
                PersistentFont font = (PersistentFont)e.Value;
                using (Brush color = new SolidBrush(font.FontColor))
                {
                    GraphicsState state = e.Graphics.Save();
                    e.Graphics.FillRectangle(color, e.Bounds);
                    e.Graphics.Restore(state);
                }
            }
            else
            {
                base.PaintValue(e);
            }
        }
    }


    [Editor(typeof(FontEditor), typeof(UITypeEditor))]
    public class PersistentFont
    {
        private static readonly XPdfFontOptions XFontOptions = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
        private String m_fontName = "Arial";
        public String FontName
        {
            get { return m_fontName; }
            set { m_fontName = value; }
        }

        private Single m_fontSize = 10;
        public Single FontSize
        {
            get { return m_fontSize; }
            set { m_fontSize = value; }
        }

        private FontStyle m_fontStyle = FontStyle.Regular;
        public FontStyle FontStyle
        {
            get { return m_fontStyle; }
            set { m_fontStyle = value; }
        }

        private Color m_fontColor = Color.Black;
        [System.Xml.Serialization.XmlIgnore]
        public Color FontColor
        {
            get { return m_fontColor; }
            set { m_fontColor = value; }
        }

        [System.Xml.Serialization.XmlElement("FontColor")]
        [Browsable(false)]
        public string _ColorName
        {
            get { return m_fontColor.Name; }
            set { m_fontColor = Color.FromName(value); }
        }


        public static PersistentFont FromFont(Font font)
        {
            return FromFont(font, Color.Black);
        }

        public static PersistentFont FromFont(Font font, Color color)
        {
            PersistentFont result = new PersistentFont();
            result.FontName = font.Name;
            result.FontSize = font.Size;
            result.FontStyle = font.Style;
            result.FontColor = color;
            return result;
        }

        public Font ToFont()
        {
            return new Font(FontName, FontSize, FontStyle);
        }

        public XFont ToXFont()
        {
            XFont res = new XFont(FontName, FontSize, ToXFontStyle(this), XFontOptions);
            return res;
        }
        public bool Bold { get { return (m_fontStyle & FontStyle.Bold) == FontStyle.Bold; } }
        public bool Italic { get { return (m_fontStyle & FontStyle.Italic) == FontStyle.Italic; } }
        public bool Underline { get { return (m_fontStyle & FontStyle.Underline) == FontStyle.Underline; } }
        public bool Strikeout { get { return (m_fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout; } }
        private static XFontStyle ToXFontStyle(PersistentFont font)
        {
            return
              (font.Bold ? XFontStyle.Bold : 0) |
              (font.Italic ? XFontStyle.Italic : 0) |
              (font.Strikeout ? XFontStyle.Strikeout : 0) |
              (font.Underline ? XFontStyle.Underline : 0);
        }

        public string GetTitle()
        {
            return String.Format("{0} {1}pt", FontName, FontSize);
        }

        public override string ToString()
        {
            return GetTitle();
        }
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
