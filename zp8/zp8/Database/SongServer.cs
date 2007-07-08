using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;

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
        void DownloadNew(AbstractSongDatabase db, int serverid);
        void UploadChanges(AbstractSongDatabase db, int serverid);
        void UploadWhole(AbstractSongDatabase db, int serverid);
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
        public static ISongServerType ServerType(string name) { return m_types[name]; }

        static SongServer()
        {
            RegisterSongServer(new XmlSongServerType());
            RegisterSongServer(new FtpSongServerType());
            RegisterSongServer(new FileSongServerType());
            RegisterFactory(new XmlSongServerFactoryType());
            RegisterFactory(new FtpSongServerFactoryType());
            RegisterFactory(new FileSongServerFactoryType());
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

        public abstract void DownloadNew(AbstractSongDatabase db, int serverid);

        public virtual void UploadChanges(AbstractSongDatabase songDatabase, int serverid)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void UploadWhole(AbstractSongDatabase db, int serverid)
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

        public override void DownloadNew(AbstractSongDatabase db, int serverid)
        {
            WebRequest req = WebRequest.Create(m_url);
            WebResponse resp = req.GetResponse();
            db.DeleteSongsFromServer(serverid);
            using (Stream fr = resp.GetResponseStream())
            {
                InetSongDb xmldb = new InetSongDb();
                xmldb.ReadXml(fr);
                db.MergeInternetXml(serverid, xmldb);
            }
            resp.Close();
        }
    }

    public class XmlSongServerFactory : PropertyPageBase, ISongServerFactory
    {
        string m_url;

        [Description("Internetová adresa XML souboru, napø. http://nejakyweb.cz/muj_zpevnik.xml")]
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
        public FtpWebRequest CreateRequest()
        {
            string p = Path;
            if (!p.StartsWith("/")) p = "/" + p;
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(String.Format("ftp://{0}{1}", Host, p));
            req.Credentials = new NetworkCredential(Login, Password);
            return req;
        }
        public Stream UploadFile()
        {
            FtpWebRequest req = CreateRequest();
            req.Method = WebRequestMethods.Ftp.UploadFile;
            return req.GetRequestStream();
        }
        public Stream DownloadFile(out WebResponse resp)
        {
            FtpWebRequest req = CreateRequest();
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            resp = req.GetResponse();
            return resp.GetResponseStream();
        }
    }

    public abstract class ReadWriteServer : BaseSongServer
    {
        public ReadWriteServer(string type, string url, string config)
            : base(type, url, config)
        {
        }

        protected abstract Stream Read(ref object request);
        protected virtual void CloseRead(object request) { }
        protected abstract Stream Write(ref object request);
        protected virtual void CloseWrite(object request) { }

        public override void DownloadNew(AbstractSongDatabase db, int serverid)
        {
            //db.DeleteSongsFromServer(serverid);
            object request = null;
            using (Stream fr = Read(ref request))
            {
                InetSongDb xmldb = new InetSongDb();
                xmldb.ReadXml(fr);
                db.MergeInternetXml(serverid, xmldb);
            }
            CloseRead(request);
        }
        public override void UploadChanges(AbstractSongDatabase db, int serverid)
        {
            InetSongDb xmldb = new InetSongDb();
            object req1 = null;
            using (Stream fr = Read(ref req1)) xmldb.ReadXml(fr);
            CloseRead(req1);

            db.UpdateInternetXml(serverid, xmldb);

            object req2 = null;
            using (Stream fw = Write(ref req2))
            {
                xmldb.WriteXml(fw);
            }
            CloseWrite(req2);
        }
        public override void UploadWhole(AbstractSongDatabase db, int serverid)
        {
            object request = null;
            using (Stream fw = Write(ref request))
            {
                db.CreateInternetXml(serverid, fw);
            }
            CloseWrite(request);
        }

    }

    public class FtpSongServer : ReadWriteServer
    {
        FtpAccess m_access;

        public FtpSongServer(string url, string config)
            : base("ftp", url, config)
        {
            m_access = FtpAccess.Load(config);
        }

        protected override Stream Read(ref object request)
        {
            WebResponse resp;
            Stream fr = m_access.DownloadFile(out resp);
            request = resp;
            return fr;
        }
        protected override void CloseRead(object request)
        {
            ((WebResponse)request).Close();
        }
        protected override Stream Write(ref object request)
        {
            return m_access.UploadFile();
        }

    }

    public class FtpSongServerFactory : PropertyPageBase, ISongServerFactory
    {
        FtpAccess m_access = new FtpAccess();

        [DisplayName("Hostitel")]
        [Description("Jméno FTP serveru, napø. ftp.webzdarma.cz")]
        public string Host { get { return m_access.Host; } set { m_access.Host = value; } }
        [DisplayName("Jméno uživatele")]
        public string Login { get { return m_access.Login; } set { m_access.Login = value; } }
        [DisplayName("Heslo")]
        public string Password { get { return m_access.Password; } set { m_access.Password = value; } }
        [DisplayName("Cesta k souboru")]
        [Description("Cesta k souboru na FTP serveru, vìtšinou staèí uvést jméno souboru, napø. muj_zpevnik.xml")]
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

    public class FileSongServerType : ISongServerType
    {
        #region ISongServerType Members

        public ISongServer Load(string url, string config)
        {
            return new FileSongServer(url);
        }

        public string Name
        {
            get { return "file"; }
        }

        public bool Readonly
        {
            get { return false; }
        }

        #endregion
    }

    public class FileSongServer : ReadWriteServer
    {
        public FileSongServer(string fileName)
            : base("file", fileName, null)
        {
        }

        protected override Stream Read(ref object request)
        {
            return new FileStream(URL, FileMode.Open);
        }

        protected override Stream Write(ref object request)
        {
            return new FileStream(URL, FileMode.Create);
        }
    }

    public class FileSongServerFactory : PropertyPageBase, ISongServerFactory
    {
        string m_fileName;

        [DisplayName("Jméno souboru")]
        [Description("Úplná cesta k souboru na disku, napø. c:/dokumenty/muj_zpevnik.xml")]
        public string FileName { get { return m_fileName; } set { m_fileName = value; } }

        #region ISongServerFactory Members

        public bool Work(List<ISongServer> servers, out string message)
        {
            servers.Add(new FileSongServer(m_fileName));
            message = "Server pøidán";
            return true;
        }

        #endregion
    }

    public class FileSongServerFactoryType : ISongServerFactoryType
    {
        #region ISongServerFactoryType Members

        public ISongServerFactory CreateFactory()
        {
            return new FileSongServerFactory();
        }

        public string Name
        {
            get { return "file"; }
        }

        public string Description
        {
            get { return "Soubor uložený na disku, hlavnì pro testovací úèely, chová se stejnì jako FTP song-server"; }
        }

        #endregion
    }
}
