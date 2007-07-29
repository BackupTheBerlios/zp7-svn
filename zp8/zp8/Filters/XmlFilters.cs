using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace zp8
{
    [StaticSongFilter]
    public class InetDbSongFormatter : SingleFileExporter
    {
        public override string Title
        {
            get { return "Internetov� datab�ze"; }
        }

        public override string Description
        {
            get { return "Soubor XML s p�sn�mi ve stejn�m form�tu, jako je ulo�en v internetov� datab�zi"; }
        }

        public override string FileDialogFilter
        {
            get { return "XML soubory (*.xml)|*.xml"; }
        }

        public override void Format(InetSongDb xmldb, Stream fw, IWaitDialog wait)
        {
            xmldb.song.WriteXml(fw);
        }
    }

    [StaticSongFilter]
    public class Zp6SongParser : MultipleStreamImporter
    {
        public override string Title
        {
            get { return "Datab�ze zp�vn�k�toru 6.0"; }
        }

        public override string FileDialogFilter
        {
            get { return "XML soubory (*.xml)|*.xml"; }
        }

        public override string Description
        {
            get { return "Datab�ze zp�vn�k�toru 6.0 ve form�tu XML"; }
        }

        //public void Run(AbstractSongDatabase db, string filename, int? serverid)
        public override void Parse(Stream fr, InetSongDb xmldb, IWaitDialog wait)
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
    public class InetDbSongParser : MultipleStreamImporter
    {
        public override string Title
        {
            get { return "Internetov� datab�ze"; }
        }

        public override string Description
        {
            get { return "Soubor XML s p�sn�mi ve stejn�m form�tu, jako je ulo�en v internetov� datab�zi"; }
        }

        public override string FileDialogFilter
        {
            get { return "XML soubory (*.xml)|*.xml"; }
        }

        public override void Parse(Stream fr, InetSongDb xmldb, IWaitDialog wait)
        {
            xmldb.ReadXml(fr);
        }
    }

}
