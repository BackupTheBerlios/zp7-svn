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
        void Run(SongDatabase db, string filename, int? serverid);
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

        public string Description
        {
            get { return "Databáze zpìvníkátoru 6.0 ve formátu XML"; }
        }

        public void Run(SongDatabase db, string filename, int? serverid)
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
                db.ImportSongs(sr, serverid);
            }
        }

        #endregion
    }

    public static class DbImport
    {
        static Dictionary<string, IDbImportType> m_imports = new Dictionary<string, IDbImportType>();
        static DbImport()
        {
            RegisterImprotType(new Zp6ImportType());
        }
        public static void RegisterImprotType(IDbImportType type)
        {
            m_imports[type.Name] = type;
        }
        public static IEnumerable<IDbImportType> Types { get { return m_imports.Values; } }
    }
}
