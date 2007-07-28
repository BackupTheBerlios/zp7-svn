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


    [ConfigurableSongFilter(Name = "Export dle �ablony")]
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

        [Category("Cel� soubor")]
        [DisplayName("Za��tek")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string FileHeader
        {
            get { return m_fileHeader; }
            set { m_fileHeader = value; }
        }
        [Category("Cel� soubor")]
        [DisplayName("Konec")]
        [Editor(typeof(TemplateTextEditor),typeof(UITypeEditor))]
        public string FileFooter
        {
            get { return m_fileFooter; }
            set { m_fileFooter = value; }
        }

        [Category("Cel� soubor")]
        [DisplayName("Odd�lova� p�sn�")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string SongSeparator
        {
            get { return m_songSeparator; }
            set { m_songSeparator = value; }
        }

        [Category("P�se�")]
        [DisplayName("Za��tek")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string SongHeader
        {
            get { return m_songHeader; }
            set { m_songHeader = value; }
        }
        [Category("P�se�")]
        [DisplayName("Konec")]
        [Editor(typeof(TemplateTextEditor), typeof(UITypeEditor))]
        public string SongFooter
        {
            get { return m_songFooter; }
            set { m_songFooter = value; }
        }

        [Category("��dka textu")]
        [DisplayName("Za��tek textov� ��dky")]
        public string TextLineBegin
        {
            get { return m_textLineBegin; }
            set { m_textLineBegin = value; }
        }

        [Category("��dka textu")]
        [DisplayName("Konec textov� ��dky")]
        public string TextLineEnd
        {
            get { return m_textLineEnd; }
            set { m_textLineEnd = value; }
        }

        [Category("��dka textu")]
        [DisplayName("Za��tek akordov� ��dky")]
        public string ChordLineBegin
        {
            get { return m_chordLineBegin; }
            set { m_chordLineBegin = value; }
        }

        [Category("��dka textu")]
        [DisplayName("Konec akordov� ��dky")]
        public string ChordLineEnd
        {
            get { return m_chordLineEnd; }
            set { m_chordLineEnd = value; }
        }

        [Category("��dka textu")]
        [DisplayName("Za��tek sm�en� ��dky")]
        public string MixedLineBegin
        {
            get { return m_mixedLineBegin; }
            set { m_mixedLineBegin = value; }
        }

        [Category("��dka textu")]
        [DisplayName("Konec sm�en� ��dky")]
        public string MixedLineEnd
        {
            get { return m_mixedLineEnd; }
            set { m_mixedLineEnd = value; }
        }

        [Category("��dka textu")]
        [DisplayName("Za��tek ��dky s n�v�t�m")]
        public string LabelBegin
        {
            get { return m_labelBegin; }
            set { m_labelBegin = value; }
        }

        [Category("��dka textu")]
        [DisplayName("Konec ��dky s n�v�t�m")]
        public string LabelEnd
        {
            get { return m_labelEnd; }
            set { m_labelEnd = value; }
        }

        [Category("��dka textu")]
        [DisplayName("Za��tek akordu")]
        public string ChordBegin
        {
            get { return m_chordBegin; }
            set { m_chordBegin = value; }
        }

        [Category("��dka textu")]
        [DisplayName("Konec akordu")]
        public string ChordEnd
        {
            get { return m_chordEnd; }
            set { m_chordEnd = value; }
        }

        [Category("��dka textu")]
        [DisplayName("Za��tek n�v�t� v textu")]
        public string LabelInTextBegin
        {
            get { return m_labelInTextBegin; }
            set { m_labelInTextBegin = value; }
        }

        [Category("��dka textu")]
        [DisplayName("Konec n�v�t� v textu")]
        public string LabelInTextEnd
        {
            get { return m_labelInTextEnd; }
            set { m_labelInTextEnd = value; }
        }


        [DisplayName("K�dov�n�")]
        public string Encoding
        {
            get { return m_encoding.WebName; }
            set { m_encoding = System.Text.Encoding.GetEncoding(value); }
        }

        /*
        [XmlIgnore]
        [DisplayName("K�dov�n�")]
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

        private static string ModifyTemplate(string tpl)
        {
            tpl = tpl.Replace("$[NL]", "\r\n");
            return tpl;
        }

        private static string ModifyTemplate(string tpl, InetSongDb.songRow song)
        {
            tpl = tpl.Replace("$[TITLE]", song.title);
            tpl = tpl.Replace("$[AUTHOR]", song.author);
            tpl = tpl.Replace("$[GROUP]", song.groupname);
            return ModifyTemplate(tpl);
        }

        protected override void DumpSongBegin(InetSongDb.songRow song, TextWriter fw)
        {
            fw.Write(ModifyTemplate(m_songHeader, song));
        }

        protected override void DumpSongEnd(InetSongDb.songRow song, TextWriter fw)
        {
            fw.Write(ModifyTemplate(m_songFooter, song));
        }

        protected override void BeginLine(string label, TextWriter fw, LineType type)
        {
            if (type == LineType.TEXT) fw.Write(ModifyTemplate(TextLineBegin));
            if (type == LineType.CHORD) fw.Write(ModifyTemplate(ChordLineBegin));
            if (type == LineType.MIXED) fw.Write(ModifyTemplate(MixedLineBegin));

            if (m_textProps.TextLabels)
            {
                if (IsLabel(label)) fw.Write(ModifyTemplate(LabelInTextBegin));
                fw.Write(label);
                if (IsLabel(label)) fw.Write(ModifyTemplate(LabelInTextEnd));
            }
        }

        protected override void EndLine(TextWriter fw, LineType type)
        {
            if (type == LineType.TEXT) fw.Write(ModifyTemplate(TextLineEnd));
            if (type == LineType.CHORD) fw.Write(ModifyTemplate(ChordLineEnd));
            if (type == LineType.MIXED) fw.Write(ModifyTemplate(MixedLineEnd));
        }

        protected override void DumpLabel(string label, TextWriter fw)
        {
            fw.Write(ModifyTemplate(LabelBegin));
            fw.Write(label);
            fw.Write(ModifyTemplate(LabelEnd));
        }

        protected override void DumpText(string text, TextWriter fw)
        {
            fw.Write(text);
        }

        protected override void DumpChord(string chord, TextWriter fw, ref int reallen)
        {
            fw.Write(ModifyTemplate(ChordBegin));
            fw.Write(chord);
            fw.Write(ModifyTemplate(ChordEnd));
        }

        protected override void DumpFileBegin(TextWriter fw)
        {
            fw.Write(ModifyTemplate(FileHeader));
        }

        protected override void DumpFileEnd(TextWriter fw)
        {
            fw.Write(ModifyTemplate(FileFooter));
        }

        protected override void DumpSongSeparator(TextWriter fw)
        {
            fw.Write(ModifyTemplate(SongSeparator));
        }

        public override string Title
        {
            get { return m_name; }
        }

        public override string Description
        {
            get { return "Export podle vlastn� �ablony"; }
        }

        [Browsable(false)]
        public override string FileDialogFilter
        {
            get { return String.Format("{0} soubory (*.{1})|*.{1}", m_ext.ToLower(), m_ext.ToUpper()); }
        }

        #region ICustomSongFilter Members

        public void SetName(string name)
        {
            m_name = name;
        }

        #endregion
    }

}
