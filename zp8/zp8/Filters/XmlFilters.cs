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
            get { return "Internetová databáze"; }
        }

        public override string Description
        {
            get { return "Soubor XML s písnìmi ve stejném formátu, jako je uložen v internetové databázi"; }
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
            get { return "Databáze zpìvníkátoru 6.0"; }
        }

        public override string FileDialogFilter
        {
            get { return "XML soubory (*.xml)|*.xml|Komprimované XML soubory (*.xgz)|*.xgz"; }
        }

        public override string Description
        {
            get { return "Databáze zpìvníkátoru 6.0 ve formátu XML"; }
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
            if (zp6doc.DocumentElement.LocalName != "zpevnik_data") throw new Exception("Špatný formát vstupního souboru");
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
            get { return "Internetová databáze"; }
        }

        public override string Description
        {
            get { return "Soubor XML s písnìmi ve stejném formátu, jako je uložen v internetové databázi"; }
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
