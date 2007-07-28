using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace zp8
{
    public class SongTextFormatterBase : AbstractSongsTextFormatter
    {
        protected override void DumpChord(string chord, TextWriter fw, ref int reallen)
        {
            if (m_textProps.ChordsInText) fw.Write('[');
            fw.Write(chord);
            if (m_textProps.ChordsInText) fw.Write(']');
        }

        protected override void DumpSongBegin(InetSongDb.songRow song, TextWriter fw)
        {
            fw.WriteLine(song.title);
            fw.WriteLine(song.author);
        }

        protected override void DumpSongSeparator(TextWriter fw)
        {
            fw.WriteLine("");
            fw.WriteLine("");
        }

        public override string Title
        {
            get { return "Textový soubor"; }
        }

        public override string Description
        {
            get { return "Textový soubor"; }
        }

        [Browsable(false)]
        public override string FileDialogFilter
        {
            get { return "Textové soubory (*.txt)|*.txt"; }
        }
    }

    [StaticSongFilter]
    public class StaticTextFormatter : SongTextFormatterBase
    { }

    [ConfigurableSongFilter(Name = "Textový soubor")]
    public class ConfigurableTextFormatter : SongTextFormatterBase, ICustomSongFilter
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
