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
            try
            {
                dst.title = src.title;
            }
            catch (Exception)
            {
                dst.title = "";
            }
            try
            {
                dst.groupname = src.groupname;
            }
            catch (Exception)
            {
                dst.groupname = "";
            }
            try
            {
                dst.author = src.author;
            }
            catch (Exception)
            {
                dst.author = "";
            }
            try
            {
                dst.songtext = src.songtext;
            }
            catch (Exception)
            {
                dst.songtext = "";
            }
            try
            {
                dst.lang = src.lang;
            }
            catch (Exception)
            {
                dst.lang = "";
            }
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
