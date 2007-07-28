using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace zp8
{
    public static class DbTools
    {
        public static void InetSongRowToLocalSongRow(InetSongDb.songRow src, SongDb.songRow dst)
        {
            dst.title = src.title;
            dst.groupname = src.groupname;
            dst.author = src.author;
            dst.songtext = src.songtext;
            dst.lang = src.lang;
            //dst.netID = src.ID;
        }
        public static void LocalSongRowToInetSongRow(SongDb.songRow src, InetSongDb.songRow dst)
        {
            dst.title = src.title;
            dst.groupname = src.groupname;
            dst.author = src.author;
            dst.songtext = src.songtext;
            dst.lang = src.lang;
            //dst.ID = src.netID;
        }


        public static void AddSongRow(SongDb.songRow src, AbstractSongDatabase db)
        {
            SongDb.songRow dst = db.DataSet.song.NewsongRow();
            dst.title = src.title;
            dst.groupname = src.groupname;
            dst.author = src.author;
            dst.songtext = src.songtext;
            dst.lang = src.lang;
            if (!src.IstranspNull()) dst.transp = src.transp;
            db.DataSet.song.AddsongRow(dst);
        }

        public static void AddSongRow(ISongRow src, InetSongDb db)
        {
            InetSongDb.songRow dst = db.song.NewsongRow();
            dst.title = src.title;
            dst.groupname = src.groupname;
            dst.author = src.author;
            dst.songtext = src.songtext;
            dst.lang = src.lang;
            db.song.AddsongRow(dst);
        }
    }
}
