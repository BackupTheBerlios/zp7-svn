using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace zp8
{
    public interface ISongServerType
    {
        ISongServer Load(string url, string config);
        string Name { get;}
        bool Readonly { get;}
    }

    public interface ISongServer
    {
        void DownloadNew(SongDatabase db, int serverid);
    }

    public interface ISongServerFactoryType
    {
        ISongServerFactory CreateFactory();
        string Name { get;}
        string Description { get;}
    }

    public interface ISongServerFactory
    {
        bool Work(List<ISongServer> servers, out string message);
    }


    public static class SongServer
    {
        static Dictionary<string, ISongServerType> m_types = new Dictionary<string, ISongServerType>();
        static Dictionary<string, ISongServerFactoryType> m_ftypes = new Dictionary<string, ISongServerFactoryType>();

        public static ISongServer LoadSongServer(string type, string url, string config)
        {
            return m_types[type].Load(url, config);
        }
        public static void RegisterSongServer(ISongServerType type)
        {
            m_types[type.Name] = type;
        }
        public static void RegisterFactory(ISongServerFactoryType type)
        {
            m_ftypes[type.Name] = type;
        }

        public static IEnumerable<ISongServerType> GetTypes()
        {
            return m_types.Values;
        }
        public static IEnumerable<ISongServerFactoryType> GetFactoryTypes()
        {
            return m_ftypes.Values;
        }

        static SongServer()
        {
            RegisterSongServer(new XmlSongServerType());
            RegisterFactory(new XmlSongServerFactoryType());
        }
    }

    public class XmlSongServerType : ISongServerType
    {
        #region ISongServerType Members

        public ISongServer Load(string url, string config)
        {
            return new XmlSongServer(url);
        }

        public string Name
        {
            get { return "xml"; }
        }

        public bool Readonly
        {
            get { return true; }
        }

        #endregion
    }

    public class XmlSongServer : ISongServer
    {
        string m_url;

        public XmlSongServer(string url) { m_url = url; }

        public void DownloadNew(SongDatabase db, int serverid)
        {
            WebRequest req = WebRequest.Create(m_url);
            WebResponse resp = req.GetResponse();
            db.DeleteSongsFromServer(serverid);
            using (Stream fr = resp.GetResponseStream())
            {
                SongDb xmldb = new SongDb();
                xmldb.ReadXml(fr);
                foreach (SongDb.songRow row in xmldb.song.Rows)
                {
                    row.server_id = serverid;
                }
                db.DataSet.song.Merge(xmldb.song);
            }
            resp.Close();
        }
    }

    public class XmlSongServerFactory : ISongServerFactory
    {
        string m_url;

        public string URL { get { return m_url; } set { m_url = value; } }
        #region ISongServerFactory Members

        public bool Work(List<ISongServer> servers, out string message)
        {
            servers.Add(new XmlSongServer(URL));
            message = "Pøidáno:" + URL;
            return true;
        }

        #endregion
    }

    public class XmlSongServerFactoryType : ISongServerFactoryType
    {
        #region ISongServerFactoryType Members

        public ISongServerFactory CreateFactory()
        {
            return new XmlSongServerFactory();
        }

        public string Name
        {
            get { return "xml"; }
        }

        public string Description
        {
            get { return "XML soubor nìkde na internetu"; }
        }

        #endregion
    }
}
