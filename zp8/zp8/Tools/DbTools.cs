using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace zp8
{
    public static class DbTools
    {
        //public static void CopySong(InetSongDb.songRow src, SongDb.songRow dst)
        //{
        //    dst.title = src.title;
        //    dst.groupname = src.groupname;
        //    dst.author = src.author;
        //    dst.songtext = src.songtext;
        //    dst.lang = src.lang;
        //    dst.remark = src.remark;
        //    //dst.netID = src.ID;
        //}
        //public static void CopySong(SongDb.songRow src, InetSongDb.songRow dst)
        //{
        //    dst.title = src.title;
        //    dst.groupname = src.groupname;
        //    dst.author = src.author;
        //    dst.songtext = src.songtext;
        //    dst.lang = src.lang;
        //    dst.remark = src.remark;
        //    //dst.ID = src.netID;
        //}


        public static void AddSongRow(SongDb.songRow src, AbstractSongDatabase db)
        {
            SongDb.songRow dst = db.DataSet.song.NewsongRow();
            CopySong(src, dst);
            if (!src.IstranspNull()) dst.transp = src.transp;
            db.DataSet.song.AddsongRow(dst);
        }

        public static void CopySong(ISongRow src, ISongRow dst)
        {
            dst.title = src.title;
            dst.groupname = src.groupname;
            dst.author = src.author;
            dst.songtext = src.songtext;
            dst.lang = src.lang;
            try
            {
                dst.remark = src.remark;
            }
            catch (Exception)
            {
                dst.remark = "";
            }
        }

        public static void AddSongRow(ISongRow src, InetSongDb db)
        {
            InetSongDb.songRow dst = db.song.NewsongRow();
            CopySong(src, dst);
            //dst.title = src.title;
            //dst.groupname = src.groupname;
            //dst.author = src.author;
            //dst.songtext = src.songtext;
            //dst.lang = src.lang;
            //dst.remark = src.remark;
            db.song.AddsongRow(dst);
        }
    }
}
