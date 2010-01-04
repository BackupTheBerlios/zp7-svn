using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace zp8
{
    public class InetSongDb
    {
        public List<SongData> Songs = new List<SongData>();

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
    }
}
