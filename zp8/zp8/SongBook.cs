using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

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

        protected override void WantOpen() { }

        public SongBook()
        {
            m_dataset = new SongDb();
        }

        public string Title
        {
            get
            {
                if (m_filename == null) return "[Beze jména]";
                return Path.GetFileName(m_filename);
            }
        }

        public bool Modified
        {
            get
            {
                return m_dataset.HasChanges();
            }
        }
        public string FileName { get { return m_filename; } set { m_filename = value; } }
        public void Save()
        {
            using (XmlWriter xw = XmlWriter.Create(m_filename))
            {
                xw.WriteStartElement("SongBook", "http://zpevnik.net/SongBook.xsd");
                m_dataset.WriteXml(xw);
                xw.WriteEndElement();
            }
        }
    }
}
