using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace zp8
{
    public class SongBookManager
    {
        List<SongBook> m_books = new List<SongBook>();

        public List<SongBook> SongBooks
        {
            get { return m_books; }
        }

        public void CreateNew()
        {
            m_books.Add(new SongBook());
        }
    }

    public class SongBook : AbstractSongDatabase
    {
        public static readonly SongBookManager Manager = new SongBookManager();
        string m_filename;

        protected override void WantOpen()
        {
        }
        public string Title
        {
            get
            {
                if (m_filename == null) return "[Beze jm�na]";
                return Path.GetFileName(m_filename);
            }
        }
    }
}
