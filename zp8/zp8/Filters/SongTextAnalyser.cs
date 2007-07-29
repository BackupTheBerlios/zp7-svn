using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Text.RegularExpressions;

namespace zp8
{
    public class ReplacementProperties : PropertyPageBase
    {
        string m_pattern;
        [DisplayName("Co hledat")]
        public string Pattern
        {
            get { return m_pattern; }
            set { m_pattern = value; }
        }

        string m_replacement;
        [DisplayName("Èím nahradit")]
        public string Replacement
        {
            get { return m_replacement; }
            set { m_replacement = value; }
        }

        bool m_useRegex;
        [DisplayName("Použit regulární výrazy")]
        public bool UseRegex
        {
            get { return m_useRegex; }
            set { m_useRegex = value; }
        }

        public override string ToString()
        {
            return "Textová substituce";
        }

        public string Run(string input)
        {
            if (UseRegex) return Regex.Replace(input, Pattern, Replacement);
            return input.Replace(Pattern, Templates.MakeTemplate(Replacement));
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SongTextAnalyseProperties : PropertyPageBase
    {
        bool m_convertChords = true;
        [Category("Text písnì")]
        [DisplayName("Analyzovat akordy")]
        public bool ConvertChords
        {
            get { return m_convertChords; }
            set { m_convertChords = value; }
        }

        bool m_convertLabels = true;
        [Category("Text písnì")]
        [DisplayName("Analyzovat návìští")]
        public bool ConvertLabels
        {
            get { return m_convertLabels; }
            set { m_convertLabels = value; }
        }

        bool m_cutHtml = false;
        [Category("Text písnì")]
        [DisplayName("Odstranit HTML tagy")]
        public bool CutHtml
        {
            get { return m_cutHtml; }
            set { m_cutHtml = value; }
        }

        List<ReplacementProperties> m_replacements = new List<ReplacementProperties>();
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        [Category("Text písnì")]
        [DisplayName("Seznam textových zámìn")]
        public List<ReplacementProperties> Replacements
        {
            get { return m_replacements; }
            set { m_replacements = value; }
        }

        public override string ToString()
        {
            return String.Format("Akordy:{0}, Návìští:{1}, HTML:{2}", m_convertChords, m_convertLabels, m_cutHtml);
            
        }
    }

    public static class SongTextAnalyser
    {
        static string[] m_chordBegins = new string[] { "Ces", "Des", "Es", "Ges", "As", "Hes", "Cb", "Db", "Eb", "Gbs", "Ab", "Bb", "C#", "D#", "F#", "G#", "A#" };
        static string[] m_chordTypes = new string[] { "+", "1", "2", "4", "5", "6", "7", "9", "dim", "mi", "moll", "dim", "maj", "add" };
        static char[] m_chordEnd = new char[] { '[', ']', '(', ')', ',', '/' };
        static string[] m_Labels = new string[] {
            "Ref.:", "Rec.:", "Rec:", "Ref:", "Solo:", "Solo.", "Sólo:", "Sólo.",
            "R.", "R.:", "R:", "*:", "*.", "Instr.", "Instr:",
            "Pøedehra:", "Pøedehra.", "Mezihra:", "Mezihra.", "Dohra:", "Dohra."
        };

        public static bool IsNote(string note)
        {
            return note.Length == 1 && note[0] >= 'A' && note[0] <= 'H';
        }
        public static bool StartsWith(string tested, string[] variants)
        {
            foreach (string var in variants) if (tested.StartsWith(var)) return true;
            return false;
        }
        public static bool IsChord(string chord)
        {
            if (chord.StartsWith("(")) chord = chord.Substring(1);

            if (IsNote(chord)) return true;
            if (StartsWith(chord, m_chordBegins)) return true;

            if (!IsNote(chord.Substring(0, 1))) return false;
            chord = chord.Substring(1);

            // zkoumame typ akordy, pismeno akordu je dobre

            if (StartsWith(chord, m_chordTypes)) return true;
            if (chord.StartsWith("m") && (chord.Length == 1 || StartsWith(chord.Substring(1), m_chordTypes))) return true;
            return false;
        }
        public static bool IsChordLine(string line)
        {
            int acnt = 0, wcnt = 0;
            foreach (string item0 in line.Split(' '))
            {
                string item = item0;
                item = item.Trim();
                int idx = item.IndexOfAny(m_chordEnd);
                if (idx >= 0) item = item.Substring(0, idx);
                if (item == "") continue;
                if (IsChord(item)) acnt++; else wcnt++;
            }
            return acnt > wcnt;
        }

        private static void WriteChordLine(string chordline, string textline, StringBuilder sb)
        {
            int i = 0;
            while (i < chordline.Length)
            {
                // mezera pred akordem
                while (i < chordline.Length && chordline[i] == ' ')
                {
                    if (i < textline.Length) sb.Append(textline[i]);
                    i++;
                }
                if (i < chordline.Length)
                {
                    int i0 = i;
                    sb.Append('[');
                    //akord
                    while (i < chordline.Length && chordline[i] != ' ')
                    {
                        sb.Append(chordline[i]);
                        i++;
                    }
                    sb.Append(']');
                    while (i0 < i)
                    {
                        if (i0 < textline.Length) sb.Append(textline[i0]);
                        i0++;
                    }
                }
            }
            if (i < textline.Length) sb.Append(textline.Substring(i));
            sb.Append('\n');
        }

        private static string ConvertChords(string text)
        {
            string[] lines = text.Split('\n');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (IsChordLine(line))
                {
                    if (i + 1 >= lines.Length)
                    {
                        WritePureChordLine(line, sb);
                        break;
                    }
                    string nextline = lines[i + 1];
                    if (IsChordLine(nextline))
                    {
                        WritePureChordLine(line, sb);
                        continue;
                    }
                    i++;
                    WriteChordLine(line, nextline, sb);
                }
                else
                {
                    sb.Append(line);
                    sb.Append('\n');
                }
            }
            return sb.ToString();
        }

        public static int LabelLength(string line)
        {
            if (line.Length >= 2 && Char.IsDigit(line[0]) && line[1] == '.') return 2;
            if (line.Length >= 3 && Char.IsDigit(line[0]) && Char.IsDigit(line[1]) && line[2] == '.') return 3;
            if (line.Length >= 3 && line[0] == 'R' && Char.IsDigit(line[1]) && line[2] == ':') return 3;
            if (line.Length >= 3 && line[0] == 'R' && Char.IsDigit(line[1]) && line[2] == '.') return 3;
            foreach (string label in m_Labels) if (line.StartsWith(label)) return label.Length;
            return 0;
        }

        private static void WritePureChordLine(string line, StringBuilder sb)
        {
            foreach (string chord in line.Split(' '))
            {
                if (chord.Trim() != "")
                {
                    sb.Append("[");
                    sb.Append(chord.Trim());
                    sb.Append("]");
                }
            }
            sb.Append('\n');
        }

        private static string ConvertLabels(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string line0 in text.Split('\n'))
            {
                string line = line0.Trim();
                int label = LabelLength(line);
                if (label > 0)
                {
                    sb.Append('.');
                    sb.Append(line.Substring(0, label));
                    sb.Append('\n');
                    line = line.Substring(label);
                }
                sb.Append(line);
                sb.Append('\n');
            }
            return sb.ToString();
        }

        public static string CutHtml(string text)
        {
            return Regex.Replace(text, @"<[^>]+>", "");
        }

        public static string RunReplacements(string text, SongTextAnalyseProperties props)
        {
            foreach (ReplacementProperties repl in props.Replacements)
            {
                text = repl.Run(text);
            }
            return text;
        }

        public static string NormalizeSongText(string text, SongTextAnalyseProperties props)
        {
            text = RunReplacements(text, props);
            text = text.Replace("[:", "/:").Replace(":]", ":/");
            if (props.CutHtml) text = CutHtml(text);
            if (props.ConvertChords) text = ConvertChords(text);
            if (props.ConvertLabels) text = ConvertLabels(text);
            return text;
        }
    }
}
