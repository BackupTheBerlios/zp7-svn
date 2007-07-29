using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace zp8
{
    public class TextParserBase : MultipleStreamImporter
    {
        Encoding m_encoding = System.Text.Encoding.UTF8;
        [DisplayName("Kódování")]
        public string Encoding
        {
            get { return m_encoding.WebName; }
            set { m_encoding = System.Text.Encoding.GetEncoding(value); }
        }

        SongTextAnalyseProperties m_textProps = new SongTextAnalyseProperties();
        [DisplayName("Zpracování textu písnì")]
        public SongTextAnalyseProperties TextProps
        {
            get { return m_textProps; }
            set { m_textProps = value; }
        }
        SongStreamSplitProperties m_splitProps = new SongStreamSplitProperties();
        [DisplayName("Rozdìlení na pisnì")]
        public SongStreamSplitProperties SplitProps
        {
            get { return m_splitProps; }
            set { m_splitProps = value; }
        }

        SongDataAnalyseProperties m_dataProps = new SongDataAnalyseProperties();
        [DisplayName("Analýza atributù písnì")]
        public SongDataAnalyseProperties DataProps
        {
            get { return m_dataProps; }
            set { m_dataProps = value; }
        }

        public override void Parse(Stream fr, InetSongDb db, IWaitDialog wait)
        {
            using (StreamReader sr = new StreamReader(fr, m_encoding))
            {
                foreach (string fulltext in SongStreamSplitter.SplitSongs(sr, m_splitProps))
                {
                    if (fulltext.Trim() == "") continue;
                    SongData song = SongDataAnalyser.AnalyseSongData(fulltext, m_dataProps);
                    song.songtext = SongTextAnalyser.NormalizeSongText(song.songtext, m_textProps);
                    DbTools.AddSongRow(song, db);
                    if (wait.Canceled) return;
                    wait.Message("Zpracována píseò " + song.title);
                }
                /*
                List<string> lines = new List<string>();
                while (!sr.EndOfStream) lines.Add(sr.ReadLine().TrimEnd());
                int i = 0;
                while (lines.Count > 0 && lines[lines.Count - 1] == "") lines.RemoveAt(lines.Count - 1);

                while (i < lines.Count)
                {
                    List<string> songlines = new List<string>();
                    while (i < lines.Count && lines[i] == "") i++; // preskoc prazdne
                    for (; ; )
                    {
                        if (i + 1 < lines.Count && lines[i] == "" && lines[i + 1] == "") break;
                        if (i >= lines.Count) break;
                        songlines.Add(lines[i]);
                        i++;                      
                    }
                    if (songlines.Count > 0)
                    {
                        InetSongDb.songRow song = db.song.NewsongRow();
                        song.author = song.title = song.groupname = song.songtext = "";
                        song.lang = "cz";
                        SongTextAnalyser.AnalyseSongHeader(songlines, song);
                        song.songtext = SongTextAnalyser.NormalizeSongText(String.Join("\n", songlines.ToArray()));
                        db.song.AddsongRow(song);
                    }
                }
                */
            }
        }

        public override string Title
        {
            get { return "Import z textu"; }
        }

        public override string Description
        {
            get { return "Importuje písnì z textového souboru"; }
        }

        [Browsable(false)]
        public override string FileDialogFilter
        {
            get { return "Textové soubory (*.txt)|*.txt|Všechny soubory|*.*"; }
        }
    }

    [StaticSongFilter]
    public class StaticTextParser : TextParserBase
    {
    }

    [ConfigurableSongFilter(Name = "Import z textu")]
    public class ConfigurableTextParse : TextParserBase, ICustomSongFilter
    {
        string m_name;

        [Browsable(false)]
        [XmlIgnore]
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public override string Title
        {
            get { return m_name; }
        }
    }
}
