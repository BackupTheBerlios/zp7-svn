using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace zp8
{
    public enum LineType { TEXT, CHORD, MIXED };

    public class TextFormatProps
    {
        bool m_textLabels = true; // navesti je az pred textem (tj. nevola se DumpLabel, ale BeginLine s label!="")
        bool m_chordsInText = false; // akordy uvnitr textu
        bool m_chordsOut = false; // vyhodit akordy

        public bool TextLabels { get { return m_textLabels; } set { m_textLabels = value; } }
        public bool ChordsOut { get { return m_chordsOut; } set { m_chordsOut = value; } }
        public bool ChordsInText { get { return m_chordsInText; } set { m_chordsInText = value; } }
    }

    public abstract class TextFormatter
    {
        bool m_waslabel;
        string m_label;
        string m_labelsp;
        TextFormatProps m_textProps = new TextFormatProps();

        public void Run(string text, TextWriter fw)
        {
            foreach (string line0 in text.Split('\n'))
            {
                string line = line0.Trim();
                if (line.StartsWith("."))
                {
                    if (m_waslabel) DumpLabel(m_label, fw);
                    m_label = line.Substring(1);
                    m_labelsp = "";
                    for (int i = 0; i < m_label.Length; i++) m_labelsp += " ";
                    m_waslabel = true;
                    if (!m_textProps.TextLabels)
                    {
                        DumpLabel(m_labelsp, fw);
                        m_waslabel = false;
                    }
                }
                else if (SongTool.IsChordLine(line))
                {
                    MakeChordLine(line, fw);
                }
                else
                {
                    MakeTextLine(line, fw);
                }
            }
        }

        private void MakeTextLine(string line, TextWriter fw)
        {
            BeginLine(GetLabel(), fw, LineType.TEXT);
            DumpText(line, fw);
            EndLine(fw, LineType.TEXT);
        }

        private void MakeChordLine(string line, TextWriter fw)
        {
            if (m_textProps.ChordsOut) MakeTextLine(SongTool.RemoveChords(line), fw);
            else if (m_textProps.ChordsInText) MakeInChordLine(line, fw);
            else MakeNormalChordLine(line, fw);
        }

        private void MakeNormalChordLine(string line, TextWriter fw)
        {
            StringBuilder tline = new StringBuilder(), chline = new StringBuilder(), chordsp = new StringBuilder();
            int tpos = 0, apos = 0;//pozice, kam se pise text,akord

            BeginLine(m_labelsp, fw, LineType.CHORD);
            SongLineParser par = new SongLineParser(line);
            while (par.Current != SongLineParser.Token.End)
            {
                switch (par.Current)
                {
                    case SongLineParser.Token.Word:
                        tline.Append(par.Data);
                        tpos += par.Data.Length;
                        break;
                    case SongLineParser.Token.Chord:
                        while (apos < tpos)
                        {
                            chordsp.Append(' ');
                            apos++;
                        }
                        DumpChordSpace(chordsp.ToString(), fw);
                        chordsp = new StringBuilder();

                        int reallen = par.Data.Length;
                        DumpChord(par.Data, fw, ref reallen);
                        apos += reallen;
                        chordsp.Append(' ');
                        apos++;
                        break;
                    case SongLineParser.Token.Space:
                        tline.Append(" ");
                        tpos += 1;
                        break;
                }
                par.Read();
            }
            EndLine(fw, LineType.CHORD);

            BeginLine(GetLabel(), fw, LineType.TEXT);
            DumpText(tline.ToString(), fw);
            EndLine(fw, LineType.TEXT);
        }

        private void MakeInChordLine(string line, TextWriter fw)
        {
            BeginLine(GetLabel(), fw, LineType.MIXED);
            SongLineParser par = new SongLineParser(line);
            while (par.Current != SongLineParser.Token.End)
            {
                switch (par.Current)
                {
                    case SongLineParser.Token.Word:
                        DumpText(par.Data, fw);
                        break;
                    case SongLineParser.Token.Chord:
                        int reallen = par.Data.Length;
                        DumpChord(par.Data, fw, ref reallen);
                        break;
                    case SongLineParser.Token.Space:
                        DumpText(" ", fw);
                        break;
                }
                par.Read();
            }
            EndLine(fw, LineType.MIXED);
        }

        private string GetLabel()
        {
            if (m_waslabel)
            {
                m_waslabel = false;
                return m_label;
            }
            return m_labelsp;
        }

        protected virtual void DumpChord(string chord, TextWriter fw, ref int reallen) { fw.Write(chord); }
        protected virtual void DumpChordSpace(string space, TextWriter fw) { fw.Write(space); }
        protected virtual void DumpText(string text, TextWriter fw) { fw.Write(text); }
        protected virtual void DumpLabel(string label, TextWriter fw) { fw.Write(label); }
        protected virtual void BeginLine(string label, TextWriter fw, LineType type) { fw.Write(label); }
        protected virtual void EndLine(TextWriter fw, LineType type) { fw.Write('\n'); }
        protected virtual void BeginText(string text, TextWriter fw) { }
        protected virtual void EndText(TextWriter fw) { }
    }
}
