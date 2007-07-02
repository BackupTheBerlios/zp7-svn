using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

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
        string URL { get;}
        string Type { get;}
        string Config { get;}
        void DownloadNew(SongDatabase db, int serverid);
        void UploadChanges(SongDatabase songDatabase, int serverid);
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
            RegisterSongServer(new FtpSongServerType());
            RegisterFactory(new XmlSongServerFactoryType());
            RegisterFactory(new FtpSongServerFactoryType());
        }
    }

    public abstract class BaseSongServer : ISongServer
    {
        protected string m_url;
        protected string m_type;
        protected string m_config;

        public BaseSongServer(string type, string url, string config)
        {
            m_url = url;
            m_type = type;
            m_config = config;
        }

        #region ISongServer Members

        public string URL
        {
            get { return m_url; }
        }

        public string Type
        {
            get { return m_type; }
        }

        public string Config
        {
            get { return m_config; }
        }

        public abstract void DownloadNew(SongDatabase db, int serverid);

        public virtual void UploadChanges(SongDatabase songDatabase, int serverid)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
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

    public class XmlSongServer : BaseSongServer
    {
        public XmlSongServer(string url) : base("xml", url, null) { m_url = url; }

        public override void DownloadNew(SongDatabase db, int serverid)
        {
            WebRequest req = WebRequest.Create(m_url);
            WebResponse resp = req.GetResponse();
            db.DeleteSongsFromServer(serverid);
            using (Stream fr = resp.GetResponseStream())
            {
                db.ImportSongs(fr, serverid);
                //db.DataSet.song.Merge(xmldb.song);
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

    public class FtpSongServerType : ISongServerType
    {
        #region ISongServerType Members

        public ISongServer Load(string url, string config)
        {
            return new FtpSongServer(url, config);
        }

        public string Name
        {
            get { return "ftp"; }
        }

        public bool Readonly
        {
            get { return false; }
        }

        #endregion
    }

    public class FtpAccess
    {
        public string Host;
        public string Login;
        public string Password;
        public string Path;

        public static FtpAccess Load(string xml)
        {
            StringReader sr = new StringReader(xml);
            XmlSerializer xs = new XmlSerializer(typeof(FtpAccess));
            return (FtpAccess)xs.Deserialize(sr);
        }
        public override string ToString()
        {
            XmlSerializer xs = new XmlSerializer(typeof(FtpAccess));
            StringWriter sw = new StringWriter();
            xs.Serialize(sw, this);
            return sw.ToString();
        }
        public string MakeUrl()
        {
            return String.Format("ftp://{0}@{1}{2}", Login, Host, Path);
        }

    }

    public class FtpSongServer : BaseSongServer
    {
        FtpAccess m_access;

        public FtpSongServer(string url, string config)
            : base("ftp", url, config)
        {
            m_access = FtpAccess.Load(config);
        }

        public override void DownloadNew(SongDatabase db, int serverid)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(String.Format("ftp://{0}{1}", m_access.Host, m_access.Path));
            req.Credentials = new NetworkCredential(m_access.Login, m_access.Password);
            req.Method = WebRequestMethods.Ftp.DownloadFile;

            WebResponse resp = req.GetResponse();
            db.DeleteSongsFromServer(serverid);
            using (Stream fr = resp.GetResponseStream())
            {
                db.ImportSongs(fr, serverid);
            }
            resp.Close();
        }
        public override void UploadChanges(SongDatabase songDatabase, int serverid)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(String.Format("ftp://{0}{1}", m_access.Host, m_access.Path));
            req.Credentials = new NetworkCredential(m_access.Login, m_access.Password);
            req.Method = WebRequestMethods.Ftp.UploadFile;
            using (Stream fw = req.GetRequestStream())
            {
                songDatabase.GetSongsAsInetXml(serverid, fw);
            }
        }
    }

    public class FtpSongServerFactory : ISongServerFactory
    {
        FtpAccess m_access = new FtpAccess();

        public string Host { get { return m_access.Host; } set { m_access.Host = value; } }
        public string Login { get { return m_access.Login; } set { m_access.Login = value; } }
        public string Password { get { return m_access.Password; } set { m_access.Password = value; } }
        public string Path { get { return m_access.Path; } set { m_access.Path = value; } }

        #region ISongServerFactory Members

        public bool Work(List<ISongServer> servers, out string message)
        {
            servers.Add(new FtpSongServer(m_access.MakeUrl(), m_access.ToString()));
            message = "Pøidáno:" + m_access.ToString();
            return true;
        }

        #endregion
    }

    public class FtpSongServerFactoryType : ISongServerFactoryType
    {
        #region ISongServerFactoryType Members

        public ISongServerFactory CreateFactory()
        {
            return new FtpSongServerFactory();
        }

        public string Name
        {
            get { return "ftp"; }
        }

        public string Description
        {
            get { return "XML soubor uložený na FTP (ètení/zápis)"; }
        }

        #endregion
    }

}
