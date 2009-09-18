using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace zp8
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class StringSearchDefinition : DatAdmin.PropertyPageBase
    {
        bool m_useRegex = false;
        [DisplayName("Použít regulární výraz")]
        public bool UseRegex
        {
            get { return m_useRegex; }
            set { m_useRegex = value; }
        }

        string m_begin = "";
        [DisplayName("Zaèátek")]
        public string Begin
        {
            get { return m_begin; }
            set { m_begin = value; }
        }

        string m_end = "";
        [DisplayName("Konec")]
        public string End
        {
            get { return m_end; }
            set { m_end = value; }
        }

        string m_pattern = "";
        [DisplayName("Reg. výraz")]
        public string Pattern
        {
            get { return m_pattern; }
            set { m_pattern = value; }
        }

        string m_replacement = "";
        [DisplayName("Výsledek reg. výrazu")]
        [Description("Mùže obsahovat zpìtné odkazy do vyhledávaného textu, napø. $1 vrátí obsah první závorky")]
        public string Replacement
        {
            get { return m_replacement; }
            set { m_replacement = value; }
        }

        public string Search(string fulltext)
        {
            if (UseRegex)
            {
                Match m = Regex.Match(fulltext, Pattern);
                return m.Result(Replacement);
            }
            else
            {
                if (Begin != "" && End != "")
                {
                    int index = fulltext.IndexOf(m_begin);
                    if (index >= 0)
                    {
                        index += m_begin.Length;
                        int endindex = fulltext.IndexOf(m_end, index);
                        if (endindex >= 0) return fulltext.Substring(index, endindex - index);
                        return fulltext.Substring(index);
                    }
                }
            }
            return "";
        }
        public override string ToString()
        {
            if (m_useRegex) return "REGEX:" + m_pattern;
            return "STR:" + Begin + "-" + End;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SongDataAnalyseProperties : DatAdmin.PropertyPageBase
    {
        bool m_lineAnalysing = true;
        [DisplayName("Analyzovat po øádcích")]
        public bool LineAnalysing
        {
            get { return m_lineAnalysing; }
            set { m_lineAnalysing = value; }
        }

        StringSearchDefinition m_searchTitle = new StringSearchDefinition();
        [DisplayName("Název")]
        [Category("Jak vyhledat")]
        public StringSearchDefinition SearchTitle
        {
            get { return m_searchTitle; }
            set { m_searchTitle = value; }
        }

        StringSearchDefinition m_searchAuthor = new StringSearchDefinition();
        [DisplayName("Autor")]
        [Category("Jak vyhledat")]
        public StringSearchDefinition SearchAuthor
        {
            get { return m_searchAuthor; }
            set { m_searchAuthor = value; }
        }

        StringSearchDefinition m_searchGroup = new StringSearchDefinition();
        [DisplayName("Skupina")]
        [Category("Jak vyhledat")]
        public StringSearchDefinition SearchGroup
        {
            get { return m_searchGroup; }
            set { m_searchGroup = value; }
        }

        StringSearchDefinition m_searchText = new StringSearchDefinition();
        [DisplayName("Text")]
        [Category("Jak vyhledat")]
        public StringSearchDefinition SearchText
        {
            get { return m_searchText; }
            set { m_searchText = value; }
        }

        public override string ToString()
        {
            if (LineAnalysing) return "Po øádcích";
            else return "Vyhledáváním";
        }
    }

    public static class SongDataAnalyser
    {
        public static bool LooksLikeTextLine(string line)
        {
            line = line.Trim();
            if (SongTextAnalyser.IsChordLine(line)) return true;
            if (SongTextAnalyser.LabelLength(line) > 0) return true;
            if (line.Split(' ').Length > 4) return true;
            return false;
        }

        public static SongData AnalyseSongData(string fulltext, SongDataAnalyseProperties props)
        {
            SongData res = new SongData();
            if (props.LineAnalysing)
            {
                List<string> lines = new List<string>();
                lines.AddRange(fulltext.Split('\n'));
                AnalyseSongHeaderLines(lines, res);
                res.SongText = String.Join("\n", lines.ToArray());
            }
            else
            {
                res.Title = props.SearchTitle.Search(fulltext);
                res.Author = props.SearchAuthor.Search(fulltext);
                res.GroupName = props.SearchGroup.Search(fulltext);
                res.SongText = props.SearchText.Search(fulltext);
            }
            return res;
        }

        private static void AnalyseSongHeaderLines(List<string> songlines, ISongRow song)
        {
            if (songlines.Count == 0) return;
            string line0 = songlines[0];
            if (line0.IndexOf(" - ") >= 0)
            {
                int i = line0.IndexOf(" - ");
                song.Title = line0.Substring(0, i).Trim();
                song.Author = line0.Substring(i + 3).Trim();
                songlines.RemoveAt(0);
            }
            else if (line0.IndexOf("    ") >= 0)
            {
                int i = line0.IndexOf("    ");
                song.Title = line0.Substring(0, i).Trim();
                song.Author = line0.Substring(i + 4).Trim();
                songlines.RemoveAt(0);
            }
            else
            {
                song.Title = line0.Trim();
                songlines.RemoveAt(0);
                if (songlines.Count == 0) return;
                line0 = songlines[0];
                if (line0 == "")
                {
                    songlines.RemoveAt(0);
                    return;
                }
                if (LooksLikeTextLine(line0)) return;
                song.Author = line0.Trim();
                songlines.RemoveAt(0);
            }
        }
    }
}
