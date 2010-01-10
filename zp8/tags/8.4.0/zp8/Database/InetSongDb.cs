using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using System.Linq;

namespace zp8
{
    public class InetSongDb
    {
        public List<SongData> Songs = new List<SongData>();
        public static string XMLNS = "http://zpevnik.net/InetSongDb.xsd";

        private int m_lastNetId = 0;

        public DataTable GetAsTable()
        {
            var res = new DataTable();
            foreach (string col in SongAccessor.SONG_DATA_TITLES)
            {
                res.Columns.Add(col);
            }
            foreach (var song in Songs)
            {
                var row = res.NewRow();
                for (int i = 0; i < SongAccessor.SONG_DATA_COLUMNS.Length; i++)
                {
                    row[i] = song[SongAccessor.SONG_DATA_COLUMNS[i]];
                }
                res.Rows.Add(row);
            }
            return res;
        }

        public void Save(Stream fw)
        {
            using (XmlWriter xw = XmlWriter.Create(fw))
            {
                xw.WriteStartDocument(true);
                xw.WriteStartElement("InetSongDb", XMLNS);
                foreach (var song in Songs)
                {
                    song.Save(xw);
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
            }
        }

        public void Load(Stream fr)
        {
            Load(XmlTextReader.Create(fr));
        }

        public void Load(XmlReader reader)
        {
            Songs.Clear();
            XmlDocument doc = new XmlDocument();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ns", XMLNS);
            doc.Load(reader);
            foreach (XmlElement xsong in doc.SelectNodes("/ns:InetSongDb/ns:song", nsmgr))
            {
                SongData song = new SongData();
                song.Load(xsong);
                Songs.Add(song);
                if (!String.IsNullOrEmpty(song.NetID) && Int32.Parse(song.NetID) > m_lastNetId)
                {
                    m_lastNetId = Int32.Parse(song.NetID);
                }
            }
        }

        public int FindNetIdIndex(string netid)
        {
            return Songs.FindIndex(song => song.NetID == netid);
        }

        public void UpdateSongByNetID(SongData song)
        {
            int index = FindNetIdIndex(song.NetID);
            if (index >= 0)
            {
                Songs[index] = song.Clone();
            }
        }

        public void AddSongWithNewNetID(SongData song)
        {
            m_lastNetId += 1;
            song.NetID = m_lastNetId.ToString();
            Songs.Add(song.Clone());
        }

        public void DeleteSongByNetID(string netid)
        {
            int index = FindNetIdIndex(netid);
            if (index >= 0)
            {
                Songs.RemoveAt(index);
            }
        }
    }
}
