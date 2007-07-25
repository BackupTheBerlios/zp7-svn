using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Xsl;
using System.Xml;
using System.IO;

namespace zp8
{
    public interface IDbExportType
    {
        string Name { get;}
        string Title { get;}
        string Description { get;}
        string FileDialogFilter { get;}
        void Run(InetSongDb xmldb, string filename);
    }

    public class InetDbExportType : IDbExportType
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

        public void Run(InetSongDb xmldb, string filename)
        {
            xmldb.song.WriteXml(filename);
        }

        #endregion
    }

    public static class DbExport
    {
        static Dictionary<string, IDbExportType> m_exports = new Dictionary<string, IDbExportType>();
        static DbExport()
        {
            RegisterExportType(new InetDbExportType());
        }
        public static void RegisterExportType(IDbExportType type)
        {
            m_exports[type.Name] = type;
        }
        public static IEnumerable<IDbExportType> Types { get { return m_exports.Values; } }
    }
}
