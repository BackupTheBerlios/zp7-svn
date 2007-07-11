using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Xsl;
using System.Xml;
using System.IO;

namespace zp8
{
    public interface IDbImportType
    {
        string Name { get;}
        string Title { get;}
        string Description { get;}
        string FileDialogFilter { get;}
        //void Run(AbstractSongDatabase db, string filename, int? serverid);
        void Run(string filename, InetSongDb xmldb);
    }

    public class Zp6ImportType : IDbImportType
    {
        #region IDbImportType Members

        public string Name
        {
            get { return "zp6"; }
        }

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
        public void Run(string filename, InetSongDb xmldb)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(XmlReader.Create(new StringReader(xsls.zp6_to_zp8)));
            XmlDocument result = new XmlDocument();
            StringBuilder sb = new StringBuilder();
            XmlDocument zp6doc = new XmlDocument();
            zp6doc.Load(filename);
            xslt.Transform(zp6doc, XmlWriter.Create(sb));
            using (StringReader sr = new StringReader(sb.ToString()))
            {
                xmldb.song.ReadXml(sr);
            }
            //db.ImportSongs(sr, serverid);
        }

        #endregion
    }

    public class InetDbImportType : IDbImportType
    {
        #region IDbExportType Members

        public string Name
        {
            get { return "inetdb"; }
        }

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

        public void Run(string filename, InetSongDb xmldb)
        {
        }

        #endregion
    }

    public static class DbImport
    {
        static Dictionary<string, IDbImportType> m_imports = new Dictionary<string, IDbImportType>();
        static DbImport()
        {
            RegisterImportType(new Zp6ImportType());
        }
        public static void RegisterImportType(IDbImportType type)
        {
            m_imports[type.Name] = type;
        }
        public static IEnumerable<IDbImportType> Types { get { return m_imports.Values; } }
    }
}
