using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;

namespace zp8
{
    public class InetSongDb
    {
        public List<SongData> Songs = new List<SongData>();
        public static string XMLNS = "http://zpevnik.net/InetSongDb.xsd";

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
    }
}
