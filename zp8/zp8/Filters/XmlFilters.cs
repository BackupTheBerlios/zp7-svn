using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace zp8
{
    [StaticSongFilter]
    public class InetDbSongFormatter : ISongFormatter
    {
        public string Title
        {
            get { return "Internetová databáze"; }
        }

        public string Description
        {
            get { return "Soubor XML s písnìmi ve stejném formátu, jako je uložen v internetové databázi"; }
        }

        public string FileDialogFilter
        {
            get { return "XML soubory (*.xml)|*.xml"; }
        }

        public void Format(InetSongDb xmldb, Stream fw)
        {
            xmldb.song.WriteXml(fw);
        }
    }

    [StaticSongFilter]
    public class Zp6SongParser : ISongParser
    {
        public string Title
        {
            get { return "Databáze zpìvníkátoru 6.0"; }
        }

        public string FileDialogFilter
        {
            get { return "XML soubory (*.xml)|*.xml"; }
        }

        public string Description
        {
            get { return "Databáze zpìvníkátoru 6.0 ve formátu XML"; }
        }

        //public void Run(AbstractSongDatabase db, string filename, int? serverid)
        public void Parse(Stream fr, InetSongDb xmldb)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(XmlReader.Create(new StringReader(xsls.zp6_to_zp8)));
            XmlDocument result = new XmlDocument();
            StringBuilder sb = new StringBuilder();
            XmlDocument zp6doc = new XmlDocument();
            zp6doc.Load(fr);
            xslt.Transform(zp6doc, XmlWriter.Create(sb));
            using (StringReader sr = new StringReader(sb.ToString()))
            {
                xmldb.song.ReadXml(sr);
            }
            //db.ImportSongs(sr, serverid);
        }
    }

    [StaticSongFilter]
    public class InetDbSongParser : ISongParser
    {
        public string Title
        {
            get { return "Internetová databáze"; }
        }

        public string Description
        {
            get { return "Soubor XML s písnìmi ve stejném formátu, jako je uložen v internetové databázi"; }
        }

        public string FileDialogFilter
        {
            get { return "XML soubory (*.xml)|*.xml"; }
        }

        public void Parse(Stream fr, InetSongDb xmldb)
        {
            xmldb.ReadXml(fr);
        }
    }

}
