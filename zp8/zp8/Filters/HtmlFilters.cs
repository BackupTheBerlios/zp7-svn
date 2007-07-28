using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Drawing;

namespace zp8
{
    public static class HtmlTools
    {
        public static void WriteSpan(string text, string cls, TextWriter fw)
        {
            fw.Write(String.Format("<span class='{0}'>", cls));
            fw.Write(text);
            fw.Write("</span>");
        }
        public static void WriteDiv(string text, string cls, TextWriter fw)
        {
            fw.Write(String.Format("<div class='{0}'>", cls));
            fw.Write(text);
            fw.Write("</div>");
        }
        /*
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
        */

        public static void WriteFontDef(string cls, PersistentFont font, TextWriter fw)
        {
            fw.Write("." + cls + " { ");
            fw.Write("font-family: " + font.FontName.ToString() + ";");
            fw.Write("font-size: " + ((int)font.FontSize).ToString() + "pt;");
            fw.Write("color: " + HtmlColorName(font.FontColor) + ";");
            if (font.Bold) fw.Write("font-weight: bold;");
            if (font.Italic) fw.Write("font-style: italic;");
            if (font.Underline) fw.Write("text-decoration: underline;");
            fw.Write("}\n");
        }

        public static string HtmlColorName(Color color)
        {
            return String.Format("rgb({0},{1},{2})", color.R, color.G, color.B);
        }
    }

    public class HtmlTextFormatterBase : AbstractSongsTextFormatter
    {
        ExportFontsPropertyPage m_fonts = new ExportFontsPropertyPage();

        private bool UsePre {get{return !m_textProps.ChordsInText && !m_textProps.ChordsOut;}}

        protected override void DumpChord(string chord, TextWriter fw, ref int reallen)
        {
            if (!UsePre) fw.Write("<sup>");
            HtmlTools.WriteSpan(chord, "chord", fw);
            if (!UsePre) fw.Write("</sup>");
        }

        [DisplayName("Fonty")]
        public ExportFontsPropertyPage Fonts
        {
            get { return m_fonts; }
            set { m_fonts = value; }
        }

        protected override void DumpSongBegin(InetSongDb.songRow song, TextWriter fw)
        {
            HtmlTools.WriteDiv(song.title, "title", fw);
            HtmlTools.WriteDiv(song.author, "author", fw);

            if (UsePre) fw.Write("<pre>");
        }

        protected override void DumpSongEnd(InetSongDb.songRow song, TextWriter fw)
        {
            if (UsePre) fw.Write("</pre>");
        }

        protected override void DumpSongSeparator(TextWriter fw)
        {
            fw.Write("<hr />");
        }

        protected override void BeginLine(string label, TextWriter fw, LineType type)
        {
            if (m_textProps.TextLabels)
            {
                if (TextFormatter.IsLabel(label))
                {
                    HtmlTools.WriteSpan(label, "label", fw);
                }
                else
                {
                    HtmlTools.WriteSpan(label, "text", fw);
                }
            }
        }

        protected override void DumpText(string text, TextWriter fw)
        {
            HtmlTools.WriteSpan(text, "text", fw);
        }

        protected override void DumpChordSpace(string space, TextWriter fw)
        {
            HtmlTools.WriteSpan(space, "chord", fw);
        }

        protected override void EndLine(TextWriter fw, LineType type)
        {
            DumpEndLine(fw);
        }

        private void DumpEndLine(TextWriter fw)
        {
            if (UsePre) fw.Write("\n");
            else fw.Write("<br />\n");
        }

        protected override void DumpLabel(string label, TextWriter fw)
        {
            HtmlTools.WriteSpan(label, "label", fw);
            DumpEndLine(fw);
        }

        protected override void DumpFileBegin(TextWriter fw)
        {
            fw.Write("<html>\n<head>\n");
            fw.Write("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">\n");
            fw.Write("<meta name=\"generator\" content=\"Zpevnikator 8.0\">\n");
            //fw.Write("<title>???</title>");
            fw.Write("<style type=\"text/css\">\n<!--\n");
            HtmlTools.WriteFontDef("text", Fonts.TextFont, fw);
            HtmlTools.WriteFontDef("chord", Fonts.ChordFont, fw);
            HtmlTools.WriteFontDef("label", Fonts.LabelFont, fw);
            HtmlTools.WriteFontDef("title", Fonts.TitleFont, fw);
            HtmlTools.WriteFontDef("author", Fonts.AuthorFont, fw);
            fw.Write("//-->\n</style>\n");
            fw.Write("</head><body>");
        }

        protected override void DumpFileEnd(TextWriter fw)
        {
            fw.Write("</body><html>");
        }

        #region ISongFilter Members

        public override string Title
        {
            get { return "HTML soubor"; }
        }

        public override string Description
        {
            get { return "HTML soubor"; }
        }

        [Browsable(false)]
        public override string FileDialogFilter
        {
            get { return "HTML soubory (*.html)|*.html"; }
        }

        #endregion
    }

    [StaticSongFilter]
    public class StaticHtmlFormatter : HtmlTextFormatterBase
    { }

    [ConfigurableSongFilter(Name = "HTML soubor")]
    public class ConfigurableHtmlFormatter : HtmlTextFormatterBase, ICustomSongFilter
    {
        string m_name;

        public override string Title { get { return m_name; } }

        #region ICustomSongFilter Members

        public void SetName(string name)
        {
            m_name = name;
        }

        #endregion
    }
}
