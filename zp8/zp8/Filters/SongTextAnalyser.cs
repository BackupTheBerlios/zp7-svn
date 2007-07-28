using System;
using System.Collections.Generic;
using System.Text;

namespace zp8
{
    public static class SongTextAnalyser
    {
        static string[] m_chordBegins = new string[] { "Ces", "Des", "Es", "Ges", "As", "Hes", "Cb", "Db", "Eb", "Gbs", "Ab", "Bb" };
        static string[] m_chordTypes = new string[] { "+", "1", "2", "4", "5", "6", "7", "9", "dim", "mi", "moll", "dim", "maj", "add" };
        static char[] m_chordEnd = new char[] { '[', ']', '(', ')', ',', '/' };

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

        private static int LabelLength(string line)
        {
            if (line.Length >= 2 && Char.IsDigit(line[0]) && line[1] == '.') return 2;
            if (line.Length >= 3 && Char.IsDigit(line[0]) && Char.IsDigit(line[1]) && line[2] == '.') return 3;
            if (line.StartsWith("Ref.:")) return 5;
            if (line.StartsWith("Rec.:")) return 5;
            if (line.StartsWith("Rec:")) return 4;
            if (line.StartsWith("Ref:")) return 4;
            if (line.StartsWith("R.")) return 2;
            if (line.StartsWith("R.:")) return 3;
            if (line.StartsWith("R:")) return 2;
            if (line.StartsWith("*:")) return 2;
            if (line.Length >= 3 && line[0] == 'R' && Char.IsDigit(line[1]) && line[2] == ':') return 3;
            if (line.Length >= 3 && line[0] == 'R' && Char.IsDigit(line[1]) && line[2] == '.') return 3;
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

        public static string NormalizeSongText(string text)
        {
            text = text.Replace("[:", "/:").Replace(":]", ":/");
            text = ConvertChords(text);
            text = ConvertLabels(text);
            return text;
        }

        public static void AnalyseSongHeader(List<string> songlines, ISongRow song)
        {
            if (songlines.Count == 0) return;
            string line0 = songlines[0];
            if (line0.IndexOf(" - ") >= 0)
            {
                int i = line0.IndexOf(" - ");
                song.title = line0.Substring(0, i).Trim();
                song.author = line0.Substring(i + 3).Trim();
                songlines.RemoveAt(0);
            }
            else if (line0.IndexOf("    ") >= 0)
            {
                int i = line0.IndexOf("    ");
                song.title = line0.Substring(0, i).Trim();
                song.author = line0.Substring(i + 4).Trim();
                songlines.RemoveAt(0);
            }
            else
            {
                song.title = line0.Trim();
                songlines.RemoveAt(0);
                if (songlines.Count == 0) return;
                line0 = songlines[0];
                if (IsChordLine(line0)) return;
                if (LabelLength(line0.Trim()) > 0) return;
                if (line0.Split(' ').Length > 4) return;
                song.author = line0.Trim();
                songlines.RemoveAt(0);
            }
        }
    }
}
