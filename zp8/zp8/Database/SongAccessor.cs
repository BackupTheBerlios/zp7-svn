using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.Common;

namespace zp8
{
    public static class SongAccessor
    {
        public static string[] SONG_DATA_COLUMNS = new string[] { 
            "title", "groupname", "author", "lang", "netID", 
            "transp", "remark" };

        public static string[] SONG_DATA_TITLES = new string[] {
            "Název", "Skupina", "Autor", "Jazyk", "NetID", 
            null, "Poznámka"
        };

        public static IEnumerable<SongData> LoadSongList(this SongDatabase db, int id)
        {
            var res = new Dictionary<int, SongData>();
            using (var reader = db.ExecuteReader("select song.id,songlistitem.transp," + String.Join(",", (from s in SONG_DATA_COLUMNS select "song." + s).ToArray()) + " from song "
                + "inner join songlistitem on songlistitem.song_id = song.id "
                + "where songlistitem.songlist_id = @id", "id", id))
            {
                while (reader.Read())
                {
                    SongData song = new SongData();
                    LoadSongDataColumns(song, reader, 2);
                    if (!reader.IsDBNull(1)) song.Transp = reader.SafeInt(1);
                    song.LocalID = reader.SafeInt(0);
                    res[song.LocalID] = song;
                }
            }
            using (var reader = db.ExecuteReader("select songdata.song_id,datatype_id,label,textdata from songdata "
                + "inner join songlistitem on songdata.song_id = songlistitem.song_id "
                + "where songlistitem.songlist_id = @id", "id", id))
            {
                while (reader.Read())
                {
                    SongData song = res[reader.SafeInt(0)];
                    song.Items.Add(new SongDataItem
                    {
                        DataType = (SongDataType)reader.SafeInt(1),
                        Label = reader.SafeString(2),
                        TextData = reader.SafeString(3)
                    });
                }
            }
            return res.Values;
        }

        private static void LoadSongDataColumns(SongData song, DbDataReader reader, int ofs)
        {
            song.Title = reader.SafeString(ofs + 0);
            song.GroupName = reader.SafeString(ofs + 1);
            song.Author = reader.SafeString(ofs + 2);
            song.Lang = reader.SafeString(ofs + 3);
            song.NetID = reader.SafeString(ofs + 4);
            song.Transp = reader.SafeInt(ofs + 5);
            song.Remark = reader.SafeString(ofs + 6);
        }

        public static SongData LoadSong(this SongDatabase db, int id)
        {
            var res = new SongData();
            using (var reader = db.ExecuteReader("select id," + String.Join(",", SONG_DATA_COLUMNS) + " from song "
                + "where ID = @id", "id", id))
            {
                if (reader.Read())
                {
                    LoadSongDataColumns(res, reader, 1);
                    res.LocalID = reader.SafeInt(0);
                }
                else
                {
                    return null;
                }
            }
            using (var reader = db.ExecuteReader("select song_id,datatype_id,label,textdata from songdata "
                + "where songdata.song_id = @id", "id", id))
            {
                while (reader.Read())
                {
                    res.Items.Add(new SongDataItem
                    {
                        DataType = (SongDataType)reader.SafeInt(1),
                        Label = reader.SafeString(2),
                        TextData = reader.SafeString(3)
                    });
                }
            }
            return res;
        }

        public static string GetSongFields(bool wantid)
        {
            var res = new List<string>();
            if (wantid) res.Add("ID");
            for (int i = 0; i < SONG_DATA_COLUMNS.Length; i++)
            {
                if (SONG_DATA_TITLES[i] == null) continue;
                res.Add(String.Format("{0} AS \"{1}\"", SONG_DATA_COLUMNS[i], SONG_DATA_TITLES[i]));
            }
            return String.Join(",", res.ToArray());
        }
    }
}
