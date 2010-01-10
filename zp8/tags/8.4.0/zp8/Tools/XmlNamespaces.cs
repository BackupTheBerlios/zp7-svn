using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace zp8
{
    public static class XmlNamespaces
    {
        public static string InetSongDb = "http://zpevnik.net/InetSongDb.xsd";
        public static string SongDb = "http://zpevnik.net/SongDb.xsd";
        public static string Options = "http://zpevnik.net/Options.xsd";
        public static string Options_Prefix = "opt";
        public static string SongBook = "http://zpevnik.net/SongBook.xsd";
        public static string SongBook_Prefix = "sb";
        public static string BookStyle = "http://zpevnik.net/BookStyle.xsd";
        public static string BookStyle_Prefix = "bs";

        public static XmlNamespaceManager CreateManager(XmlNameTable table)
        {
            XmlNamespaceManager mgr = new XmlNamespaceManager(table);
            mgr.AddNamespace(Options_Prefix, Options);
            mgr.AddNamespace(SongBook_Prefix, SongBook);
            return mgr;
        }
    }
}
