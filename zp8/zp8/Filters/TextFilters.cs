using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace zp8
{
    public class SongTextFormatterBase : TextFormatter, ISongFormatter
    {
        protected override void DumpChord(string chord, TextWriter fw, ref int reallen)
        {
            if (m_textProps.ChordsInText) fw.Write('[');
            fw.Write(chord);
            if (m_textProps.ChordsInText) fw.Write(']');
        }

        #region ISongFormatter Members

        public void Format(InetSongDb db, Stream fw)
        {
            using (StreamWriter sw = new StreamWriter(fw))
            {
                foreach (InetSongDb.songRow row in db.song.Rows)
                {
                    sw.WriteLine(row.title);
                    sw.WriteLine(row.author);
                    RunTextFormatting(row.songtext, sw);
                    sw.WriteLine("");
                    sw.WriteLine("");
                }
            }
        }

        #endregion

        #region ISongFilter Members

        public virtual string Title
        {
            get { return "Textový soubor"; }
        }

        public string Description
        {
            get { return "Textový soubor"; }
        }

        [Browsable(false)]
        public string FileDialogFilter
        {
            get { return "Textové soubory (*.txt)|*.txt"; }
        }

        #endregion
    }

    [StaticSongFilter]
    public class StaticTextFormatter : SongTextFormatterBase
    { }

    [ConfigurableSongFilter(Name = "Textový soubor")]
    public class ConfigurableTextFormatter : SongTextFormatterBase, ICustomSongFilter
    {
        string m_name;

        [DisplayName("Vlastnosti formátování")]
        public TextFormatProps FormatProperties
        {
            get { return m_textProps; }
            set { m_textProps = value; }
        }

        public override string Title { get { return m_name; } }

        #region ICustomSongFilter Members

        public void SetName(string name)
        {
            m_name = name;
        }

        #endregion
    }
}
