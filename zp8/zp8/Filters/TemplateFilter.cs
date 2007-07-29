using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing.Design;

namespace zp8
{
    public class TemplateTextEditor : UITypeEditor
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
            return TemplateTextForm.Run((string)value);
        }
    }


    [ConfigurableSongFilter(Name = "Export dle šablony")]
    public class TemplateFormatter : AbstractSongsTextFormatter, ICustomSongFilter
    {
        string m_name;
        string m_ext = "html";
        string m_fileHeader = "";
        string m_fileFooter = "";
        string m_songHeader = "#TITLE#\r\n#AUTHOR\r\n";
        string m_songFooter = "";
        string m_textLineBegin = "";
        string m_textLineEnd = "";
        string m_chordLineBegin = "";
        string m_chordLineEnd = "";
        string m_mixedLineBegin = "";
        string m_mixedLineEnd = "";
        string m_labelBegin = "";
        string m_labelEnd = "";
        string m_chordBegin = "";
        string m_chordEnd = "";
        string m_songSeparator = "";
        string m_labelInTextBegin = "";
        string m_labelInTextEnd = "";

        [Category("Celý soubor")]
        [DisplayName("Zaèátek")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string FileHeader
        {
            get { return m_fileHeader; }
            set { m_fileHeader = value; }
        }
        [Category("Celý soubor")]
        [DisplayName("Konec")]
        [Editor(typeof(TemplateTextEditor),typeof(UITypeEditor))]
        public string FileFooter
        {
            get { return m_fileFooter; }
            set { m_fileFooter = value; }
        }

        [Category("Celý soubor")]
        [DisplayName("Oddìlovaè písní")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string SongSeparator
        {
            get { return m_songSeparator; }
            set { m_songSeparator = value; }
        }

        [Category("Píseò")]
        [DisplayName("Zaèátek")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string SongHeader
        {
            get { return m_songHeader; }
            set { m_songHeader = value; }
        }
        [Category("Píseò")]
        [DisplayName("Konec")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string SongFooter
        {
            get { return m_songFooter; }
            set { m_songFooter = value; }
        }

        [Category("Øádka textu")]
        [DisplayName("Zaèátek textové øádky")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string TextLineBegin
        {
            get { return m_textLineBegin; }
            set { m_textLineBegin = value; }
        }

        [Category("Øádka textu")]
        [DisplayName("Konec textové øádky")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string TextLineEnd
        {
            get { return m_textLineEnd; }
            set { m_textLineEnd = value; }
        }

        [Category("Øádka textu")]
        [DisplayName("Zaèátek akordové øádky")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string ChordLineBegin
        {
            get { return m_chordLineBegin; }
            set { m_chordLineBegin = value; }
        }

        [Category("Øádka textu")]
        [DisplayName("Konec akordové øádky")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string ChordLineEnd
        {
            get { return m_chordLineEnd; }
            set { m_chordLineEnd = value; }
        }

        [Category("Øádka textu")]
        [DisplayName("Zaèátek smíšené øádky")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string MixedLineBegin
        {
            get { return m_mixedLineBegin; }
            set { m_mixedLineBegin = value; }
        }

        [Category("Øádka textu")]
        [DisplayName("Konec smíšené øádky")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string MixedLineEnd
        {
            get { return m_mixedLineEnd; }
            set { m_mixedLineEnd = value; }
        }

        [Category("Øádka textu")]
        [DisplayName("Zaèátek øádky s návìštím")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string LabelBegin
        {
            get { return m_labelBegin; }
            set { m_labelBegin = value; }
        }

        [Category("Øádka textu")]
        [DisplayName("Konec øádky s návìštím")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string LabelEnd
        {
            get { return m_labelEnd; }
            set { m_labelEnd = value; }
        }

        [Category("Øádka textu")]
        [DisplayName("Zaèátek akordu")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string ChordBegin
        {
            get { return m_chordBegin; }
            set { m_chordBegin = value; }
        }

        [Category("Øádka textu")]
        [DisplayName("Konec akordu")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string ChordEnd
        {
            get { return m_chordEnd; }
            set { m_chordEnd = value; }
        }

        [Category("Øádka textu")]
        [DisplayName("Zaèátek návìští v textu")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string LabelInTextBegin
        {
            get { return m_labelInTextBegin; }
            set { m_labelInTextBegin = value; }
        }

        [Category("Øádka textu")]
        [DisplayName("Konec návìští v textu")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string LabelInTextEnd
        {
            get { return m_labelInTextEnd; }
            set { m_labelInTextEnd = value; }
        }


        [DisplayName("Kódování")]
        public string Encoding
        {
            get { return m_encoding.WebName; }
            set { m_encoding = System.Text.Encoding.GetEncoding(value); }
        }

        /*
        [XmlIgnore]
        [DisplayName("Kódování")]
        public Encoding Encoding
        {
            get { return m_encoding; }
            set { m_encoding = value; }
        }
        */

        /*
        [XmlElement("Encoding")]
        [Browsable(false)]
        public string EncodingName
        {
            get { return m_encoding.WebName; }
            set { m_encoding = Encoding.GetEncoding(value); }
        }
        */

        private static string MakeTemplate(string tpl)
        {
            return Templates.MakeTemplate(tpl);
        }

        private static string MakeTemplate(string tpl, InetSongDb.songRow song)
        {
            return Templates.MakeTemplate(tpl, song);
        }

        protected override void DumpSongBegin(InetSongDb.songRow song, TextWriter fw)
        {
            fw.Write(MakeTemplate(m_songHeader, song));
        }

        protected override void DumpSongEnd(InetSongDb.songRow song, TextWriter fw)
        {
            fw.Write(MakeTemplate(m_songFooter, song));
        }

        protected override void BeginLine(string label, TextWriter fw, LineType type)
        {
            if (type == LineType.TEXT) fw.Write(MakeTemplate(TextLineBegin));
            if (type == LineType.CHORD) fw.Write(MakeTemplate(ChordLineBegin));
            if (type == LineType.MIXED) fw.Write(MakeTemplate(MixedLineBegin));

            if (m_textProps.TextLabels)
            {
                if (IsLabel(label)) fw.Write(MakeTemplate(LabelInTextBegin));
                fw.Write(label);
                if (IsLabel(label)) fw.Write(MakeTemplate(LabelInTextEnd));
            }
        }

        protected override void EndLine(TextWriter fw, LineType type)
        {
            if (type == LineType.TEXT) fw.Write(MakeTemplate(TextLineEnd));
            if (type == LineType.CHORD) fw.Write(MakeTemplate(ChordLineEnd));
            if (type == LineType.MIXED) fw.Write(MakeTemplate(MixedLineEnd));
        }

        protected override void DumpLabel(string label, TextWriter fw)
        {
            fw.Write(MakeTemplate(LabelBegin));
            fw.Write(label);
            fw.Write(MakeTemplate(LabelEnd));
        }

        protected override void DumpText(string text, TextWriter fw)
        {
            fw.Write(text);
        }

        protected override void DumpChord(string chord, TextWriter fw, ref int reallen)
        {
            fw.Write(MakeTemplate(ChordBegin));
            fw.Write(chord);
            fw.Write(MakeTemplate(ChordEnd));
        }

        protected override void DumpFileBegin(TextWriter fw)
        {
            fw.Write(MakeTemplate(FileHeader));
        }

        protected override void DumpFileEnd(TextWriter fw)
        {
            fw.Write(MakeTemplate(FileFooter));
        }

        protected override void DumpSongSeparator(TextWriter fw)
        {
            fw.Write(MakeTemplate(SongSeparator));
        }

        public override string Title
        {
            get { return m_name; }
        }

        public override string Description
        {
            get { return "Export podle vlastní šablony"; }
        }

        [Browsable(false)]
        public override string FileDialogFilter
        {
            get { return String.Format("{0} soubory (*.{1})|*.{1}", m_ext.ToLower(), m_ext.ToUpper()); }
        }

        [Browsable(false)]
        [XmlIgnore]
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
    }

}
