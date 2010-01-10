using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace zp8
{
    public struct ChordElement
    {
        public readonly int Height;
        public readonly string Type;
        public ChordElement(int height, string type)
        {
            Height = height;
            Type = type;
        }
        public override string ToString()
        {
            return Chords.m_tonNames[Height] + Type;
        }
        public ChordElement Transpose(int d)
        {
            if (Height < 0) return this;
            return new ChordElement((Height + d) % 12, Type);
        }
    }
    public static class Chords
    {
        public static Dictionary<string, int> m_tonHeights = new Dictionary<string, int>();
        public static Dictionary<int, string> m_tonNames = new Dictionary<int, string>();
        public static List<string> m_sortedTonNames = new List<string>();
        static Chords()
        {
            m_tonHeights["C"] = 0;

            m_tonHeights["C#"] = 1;
            m_tonHeights["Des"] = 1;
            m_tonHeights["Db"] = 1;

            m_tonHeights["D"] = 2;

            m_tonHeights["Es"] = 3;
            m_tonHeights["D#"] = 3;
            m_tonHeights["Eb"] = 3;

            m_tonHeights["E"] = 4;
            m_tonHeights["Fes"] = 4;
            m_tonHeights["Fb"] = 4;

            m_tonHeights["F"] = 5;
            m_tonHeights["E#"] = 5;

            m_tonHeights["F#"] = 6;
            m_tonHeights["Gb"] = 6;
            m_tonHeights["Ges"] = 6;

            m_tonHeights["G"] = 7;

            m_tonHeights["G#"] = 8;
            m_tonHeights["Ab"] = 8;
            m_tonHeights["As"] = 8;

            m_tonHeights["A"] = 9;

            m_tonHeights["B"] = 10;
            m_tonHeights["Bb"] = 10;
            m_tonHeights["A#"] = 10;
            m_tonHeights["Hes"] = 10;

            m_tonHeights["H"] = 11;
            m_tonHeights["Ces"] = 11;
            m_tonHeights["Cb"] = 11;

            m_tonNames[-1] = "";
            m_tonNames[0] = "C";
            m_tonNames[1] = "C#";
            m_tonNames[2] = "D";
            m_tonNames[3] = "D#";
            m_tonNames[4] = "E";
            m_tonNames[5] = "F";
            m_tonNames[6] = "F#";
            m_tonNames[7] = "G";
            m_tonNames[8] = "G#";
            m_tonNames[9] = "A";
            m_tonNames[10] = "Bb";
            m_tonNames[11] = "H";

            m_sortedTonNames.AddRange(m_tonHeights.Keys);
            m_sortedTonNames.Sort(delegate(string a, string b) { return b.Length - a.Length; });
        }

        static ChordElement SplitChord_Simple(string chord)
        {
            foreach (string beg in m_sortedTonNames)
            {
                if (chord.ToUpper().StartsWith(beg.ToUpper()))
                {
                    int ton = m_tonHeights[beg];
                    return new ChordElement(ton, chord.Substring(beg.Length));
                }
            }
            return new ChordElement(-1, chord);
        }
        public static IEnumerable<ChordElement> SplitChord(string chord)
        {
            if (chord.IndexOf('/') >= 0)
            {
                yield return SplitChord_Simple(chord.Substring(0, chord.IndexOf('/') + 1));
                yield return SplitChord_Simple(chord.Substring(chord.IndexOf('/') + 1));
            }
            else
            {
                yield return SplitChord_Simple(chord);
            }
        }
        public static string TransposeChord(string chord, int d)
        {
            string res = "";
            foreach (ChordElement elem in SplitChord(chord))
            {
                res += elem.Transpose(d);
            }
            return res;
        }
        public static string Transpose(string text, int d)
        {
            if (d == 0) return text;
            return Regex.Replace(text, @"\[[^]]*\]", delegate(Match m)
            {
                return "[" + TransposeChord(m.Value.Substring(1, m.Value.Length - 2), d) + "]";
            });
        }
        // vraci ton prvniho akordu nebo -1, kdyz se to nepovede zjistit
        public static int GetBaseTone(string text)
        {
            Match m = Regex.Match(text, @"\[[^]]*\]");
            if (m != null && m.Length > 0)
            {
                ChordElement ch = SplitChord_Simple(m.Value.Substring(1, m.Value.Length - 2));
                return ch.Height;
            }
            return -1;
        }
    }
}
