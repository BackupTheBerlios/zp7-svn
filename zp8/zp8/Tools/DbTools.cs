using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
