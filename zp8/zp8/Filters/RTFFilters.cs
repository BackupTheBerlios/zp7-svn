using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Drawing;

namespace zp8
{
    public static class RtfTools
    {
        public static void SetFont(TextWriter fw, int index, string style, int size)
        {
            fw.Write("\\plain\\f");
            fw.Write(index);
            fw.Write("\\fs");
            fw.Write(size);
            fw.Write(style);
            fw.Write(" ");
        }

        public static string FontToRtfStyle(PersistentFont font, int fontindex)
        {
            string res = "";
            res += "\\cf";
            res += fontindex.ToString();
            if (font.Italic) res += "\\i";
            if (font.Underline) res += "\\ul";
            if (font.Bold) res += "\\b";
            return res;
        }

        public static string ColorToRtfColor(Color color)
        {
            string res = "";
            res += "\\red"; res += color.R;
            res += "\\green"; res += color.G;
            res += "\\blue"; res += color.B;
            res += ";";
            return res;
        }

        public static void SetFont(TextWriter fw, PersistentFont font, int fontindex)
        {
            SetFont(fw, fontindex, FontToRtfStyle(font, fontindex), (int)(font.FontSize * 2));
        }
    }

    public class RtfTextFormatterBase : TextFormatter, ISongFormatter
    {
        ExportFontsPropertyPage m_fonts = new ExportFontsPropertyPage();

        protected override void DumpChord(string chord, TextWriter fw, ref int reallen)
        {
            if (m_textProps.ChordsInText) fw.Write('[');
            fw.Write(chord);
            if (m_textProps.ChordsInText) fw.Write(']');
        }

        [DisplayName("Fonty")]
        public ExportFontsPropertyPage Fonts
        {
            get { return m_fonts; }
            set { m_fonts = value; }
        }

        private void DumpSong(InetSongDb.songRow song, TextWriter fw)
        {
            RtfTools.SetFont(fw, Fonts.TitleFont, 4);
            fw.Write(song.title);
            fw.Write("\\par ");

            RtfTools.SetFont(fw, Fonts.AuthorFont, 5);
            fw.Write(song.author);
            fw.Write("\\par ");

            RunTextFormatting(song.songtext,fw);
        }

        protected override void BeginLine(string label, TextWriter fw, LineType type)
        {
            if (m_textProps.TextLabels)
            {
                if (TextFormatter.IsLabel(label))
                {
                    RtfTools.SetFont(fw, Fonts.LabelFont, 3);
                    fw.Write(label);
                }
                else
                {
                    RtfTools.SetFont(fw, Fonts.TextFont, 1);
                    fw.Write(label);
                }
            }
            switch (type)
            {
                case LineType.TEXT:
                    RtfTools.SetFont(fw, Fonts.TextFont, 1);
                    break;
                case LineType.CHORD:
                    RtfTools.SetFont(fw, Fonts.ChordFont, 2);
                    break;
            }
        }

        protected override void EndLine(TextWriter fw, LineType type)
        {
            fw.Write("\\par ");
        }

        protected override void DumpLabel(string label, TextWriter fw)
        {
            RtfTools.SetFont(fw, Fonts.LabelFont, 4);
            fw.Write(label);
            fw.Write("\\par ");
        }

        private void DumpFileBegin(TextWriter fw)
        {
            fw.Write("{\\rtf1\\ansi\\deff0\\deftab720{\\fonttbl{\\f0\\fnil MS Sans Serif;}");
            fw.Write("{\\f1\\fnil "); fw.Write(Fonts.TextFont.FontName); fw.Write(";}");
            fw.Write("{\\f2\\fnil "); fw.Write(Fonts.ChordFont.FontName); fw.Write(";}");
            fw.Write("{\\f3\\fnil "); fw.Write(Fonts.LabelFont.FontName); fw.Write(";}");
            fw.Write("{\\f4\\fnil "); fw.Write(Fonts.TitleFont.FontName); fw.Write(";}");
            fw.Write("{\\f5\\fnil "); fw.Write(Fonts.AuthorFont.FontName); fw.Write(";}");
            fw.Write("}{\\colortbl;");
            //fw.Write("\\red0\\green0\\blue0;");
            fw.Write(RtfTools.ColorToRtfColor(Fonts.TextFont.FontColor));
            fw.Write(RtfTools.ColorToRtfColor(Fonts.ChordFont.FontColor));
            fw.Write(RtfTools.ColorToRtfColor(Fonts.LabelFont.FontColor));
            fw.Write(RtfTools.ColorToRtfColor(Fonts.TitleFont.FontColor));
            fw.Write(RtfTools.ColorToRtfColor(Fonts.AuthorFont.FontColor));
            fw.Write("}");
        }

        private void DumpFileEnd(TextWriter fw)
        {
            fw.Write("\\par }");
        }

        #region ISongFormatter Members

        public void Format(InetSongDb db, Stream fw)
        {
            using (StreamWriter sw = new StreamWriter(fw, Encoding.GetEncoding(1250)))
            {
                DumpFileBegin(sw);
                foreach (InetSongDb.songRow row in db.song.Rows)
                {
                    DumpSong(row, sw);
                }
                DumpFileEnd(sw);
            }
        }

        #endregion

        #region ISongFilter Members

        public virtual string Title
        {
            get { return "RTF soubor"; }
        }

        public string Description
        {
            get { return "RTF soubor"; }
        }

        [Browsable(false)]
        public string FileDialogFilter
        {
            get { return "RTF soubory (*.rtf)|*.rtf"; }
        }

        #endregion
    }

    [StaticSongFilter]
    public class StaticRtfFormatter : RtfTextFormatterBase
    { }

    [ConfigurableSongFilter(Name = "RTF soubor")]
    public class ConfigurableRtfFormatter : RtfTextFormatterBase, ICustomSongFilter
    {
        string m_name;

        public override string Title { get { return m_name; } }

        [DisplayName("Vlastnosti formátování")]
        public TextFormatProps FormatProperties
        {
            get { return m_textProps; }
            set { m_textProps = value; }
        }

        #region ICustomSongFilter Members

        public void SetName(string name)
        {
            m_name = name;
        }

        #endregion
    }
}
