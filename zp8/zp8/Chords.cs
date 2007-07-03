using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace zp8
{
    public struct ChordElement
    {
        int hi;
        string type;
    }
    public static class Chords
    {
        static Dictionary<string, int> m_tonNames = new Dictionary<string, int>();
        static Chords()
        {
            m_tonNames["C"] = 0;

            m_tonNames["C#"] = 1;
            m_tonNames["Des"] = 1;
            m_tonNames["Db"] = 1;

            m_tonNames["D"] = 2;

            m_tonNames["Es"] = 3;
            m_tonNames["D#"] = 3;
            m_tonNames["Eb"] = 3;

            m_tonNames["E"] = 4;
            m_tonNames["Fes"] = 4;
            m_tonNames["Fb"] = 4;

            m_tonNames["F"] = 5;
            m_tonNames["E#"] = 5;

            m_tonNames["F#"] = 6;
            m_tonNames["Gb"] = 6;
            m_tonNames["Ges"] = 6;

            m_tonNames["G"] = 7;

            m_tonNames["G#"] = 8;
            m_tonNames["Ab"] = 8;
            m_tonNames["As"] = 8;

            m_tonNames["A"] = 9;

            m_tonNames["B"] = 10;
            m_tonNames["Bb"] = 10;
            m_tonNames["A#"] = 10;
            m_tonNames["Hes"] = 10;

            m_tonNames["H"] = 11;
            m_tonNames["Ces"] = 11;
            m_tonNames["Cb"] = 11;
        }

        static ChordElement SplitChord_Simple(string chord)
        {
            throw new Exception();
        }
        public static IEnumerable<ChordElement> SplitChord(string chord)
        {
            if (chord.IndexOf('/') >= 0)
            {
                yield return SplitChord_Simple(chord.Substring(0, chord.IndexOf('/')));
                yield return SplitChord_Simple(chord.Substring(chord.IndexOf('/') + 1));
            }
            else
            {
                yield return SplitChord_Simple(chord);
            }
        }
        public static string TransposeChord(string chord, int d)
        {
            throw new Exception();
        }
        public static string Transpose(string text, int d)
        {
            return Regex.Replace(text, @"\[[^]]*\]", delegate(Match m)
            {
                return "[" + TransposeChord(m.Value.Substring(1, m.Value.Length - 2), d) + "]";
            });
        }
    }
}
