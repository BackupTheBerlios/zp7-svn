using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.ComponentModel;

namespace zp8
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class SongServerAttribute : Attribute
    {
        public string Title;
        public string Description;
        public string Name;
        public bool ReadOnly = true;
    }

    public class SongServerType
    {
        public Type Type;
        public string Title;
        public string Description;
        public string Name;
        public bool ReadOnly;

        public ISongServer CreateServer()
        {
            return (ISongServer)Type.GetConstructor(new Type[] { }).Invoke(new object[] { });
        }
    }

    /*
    public struct SongServerInfo
    {
        public string URL;
        public string Type;
        public string Config;
    }
    */

    public interface ISongServer
    {
        //string URL { get;}
        //string Type { get;}
        //string Config { get;}
        void DownloadNew(AbstractSongDatabase db, int serverid);
        void UploadChanges(AbstractSongDatabase db, int serverid);
        void UploadWhole(AbstractSongDatabase db, int serverid);
        //void Save(out SongServerInfo dst);
        //void Load(SongServerInfo src);
    }

    public abstract class BaseSongServer : DatAdmin.PropertyPageBase, ISongServer
    {
        #region ISongServer Members

        public virtual void DownloadNew(AbstractSongDatabase db, int serverid)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void UploadChanges(AbstractSongDatabase db, int serverid)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void UploadWhole(AbstractSongDatabase db, int serverid)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    public abstract class ReadOnlySongServer : BaseSongServer
    {
        protected abstract Stream Read(ref object request);
        protected virtual void CloseRead(object request) { }

        public override void DownloadNew(AbstractSongDatabase db, int serverid)
        {
            //db.DeleteSongsFromServer(serverid);
            object request = null;
            using (Stream fr = Read(ref request))
            {
                db.MergeInternetXml(serverid, fr);
            }
            CloseRead(request);
        }
    }

    public abstract class ReadWriteSongServer : ReadOnlySongServer
    {
        protected abstract Stream Write(ref object request);
        protected virtual void CloseWrite(object request) { }

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

    public static class SongServer
    {
        static Dictionary<string, SongServerType> m_types = new Dictionary<string, SongServerType>();
        static Dictionary<Type, SongServerType> m_invtypes = new Dictionary<Type, SongServerType>();

        static SongServer()
        {
            foreach (Type type in Assembly.GetAssembly(typeof(SongServer)).GetTypes())
            {
                foreach(SongServerAttribute attr in type.GetCustomAttributes(typeof(SongServerAttribute), true))
                {
                    SongServerType st = new SongServerType();
                    st.Type = type;
                    st.Description = attr.Description;
                    st.Name = attr.Name;
                    st.Title = attr.Title;
                    st.ReadOnly = attr.ReadOnly;
                    m_types[attr.Name] = st;
                    m_invtypes[type] = st;
                }
            }
        }

        public static ISongServer Load(string type, string config)
        {
            SongServerType st = m_types[type];
            XmlSerializer xs = new XmlSerializer(st.Type);
            using (StringReader sr = new StringReader(config))
            {
                return (ISongServer)xs.Deserialize(sr);
            }
        }

        public static string Save(ISongServer server)
        {
            XmlSerializer xs = new XmlSerializer(server.GetType());
            using (StringWriter sw = new StringWriter())
            {
                xs.Serialize(sw, server);
                return sw.ToString();
            }
        }
        public static IEnumerable<SongServerType> GetServerTypes()
        {
            return m_types.Values;
        }
        public static SongServerType GetServerName(ISongServer server)
        {
            return m_invtypes[server.GetType()];
        }
    }

    [SongServer(Name = "xml", Title = "XML", Description = "XML soubor nìkde na internetu", ReadOnly = true)]
    public class XmlSongServer : ReadOnlySongServer
    {
        string m_url;

        [Description("Internetová adresa XML souboru, napø. http://nejakyweb.cz/muj_zpevnik.xml")]
        public string URL { get { return m_url; } set { m_url = value; } }

        protected override Stream Read(ref object request)
        {
            WebRequest req = WebRequest.Create(m_url);
            WebResponse resp = req.GetResponse();
            request = resp;
            return resp.GetResponseStream();
        }

        protected override void CloseRead(object request)
        {
            ((WebResponse)request).Close();
        }
        public override string ToString()
        {
            return m_url;
        }
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

    [SongServer(Name = "ftp", Title = "FTP", Description = "XML soubor uložený na FTP (ètení/zápis)", ReadOnly=false)]
    public class FtpSongServer : ReadWriteSongServer
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
        public override string ToString()
        {
            return m_access.MakeUrl();
        }
    }

    [SongServer(Name = "file", Title = "Soubor na disku", Description = "Soubor uložený na disku, hlavnì pro testovací úèely, chová se stejnì jako FTP song-server", ReadOnly=false)]
    public class FileSongServer : ReadWriteSongServer
    {
        string m_fileName;

        [DisplayName("Jméno souboru")]
        [Description("Úplná cesta k souboru na disku, napø. c:/dokumenty/muj_zpevnik.xml")]
        public string FileName { get { return m_fileName; } set { m_fileName = value; } }

        protected override Stream Read(ref object request)
        {
            return new FileStream(FileName, FileMode.Open);
        }

        protected override Stream Write(ref object request)
        {
            return new FileStream(FileName, FileMode.Create);
        }
        public override string ToString()
        {
            return m_fileName;
        }
    }
}
