using System;
using System.Collections.Generic;
using System.Text;

namespace zp8
{
    public static class SongTool
    {
        public static bool IsChordLine(string line)
        {
            return line.IndexOf(']') >= 0;
        }
        public static bool IsLabelLine(string line)
        {
            return line.StartsWith(".");
        }
    }

    public class SongLineParser
    {
        public struct ParserState
        {
            public readonly Token Token;
            public readonly int Position;
            public readonly string Data;
            public ParserState(SongLineParser parser)
            {
                Token = parser.m_token;
                Position = parser.m_index;
                Data = parser.m_data;
            }
        }
        public enum Token { Word, Chord, Space, End };
        string m_line;
        Token m_token;
        int m_index = 0;
        string m_data;
        public readonly ParserState InitState;

        public SongLineParser(string line)
        {
            m_line = line;
            while (AreData && Char.IsWhiteSpace(CurCh)) m_index++;
            InitState = State;
            Read();
        }
        public Token Current { get { return m_token; } }
        public string Data { get { return m_data; } }
        public string Original { get { return m_line; } }
        private char CurCh { get { return m_line[m_index]; } }
        private bool AreData { get { return m_index < m_line.Length; } }
        public void Read()
        {
            m_data = null;
            if (m_token == Token.End) throw new Exception("End of line");
            if (!AreData) m_token = Token.End;
            else if (Char.IsWhiteSpace(CurCh))
            {
                while (AreData && Char.IsWhiteSpace(CurCh)) m_index++;
                if (AreData) m_token = Token.Space;
                else m_token = Token.End;
            }
            else if (CurCh == '[')
            {
                m_index++;
                m_data = "";
                while (AreData && CurCh != ']')
                {
                    m_data += CurCh;
                    m_index++;
                }
                if (AreData) m_index++;
                m_token = Token.Chord;
            }
            else
            {
                m_data = "";
                while (AreData && CurCh != '[' && !Char.IsWhiteSpace(CurCh))
                {
                    m_data += CurCh;
                    m_index++;
                }
                m_token = Token.Word;
            }
        }
        public int Position { get { return m_index; } set { m_index = value; } }
        public ParserState State
        {
            get
            {
                return new ParserState(this);
            }
            set
            {
                m_index = value.Position;
                m_token = value.Token;
                m_data = value.Data;
            }
        }
    }
}
