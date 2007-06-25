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
        string Description { get;}
        bool Readonly { get;}
    }

    public interface ISongServer
    {
        void DownloadNew(SongDatabase db, int serverid);
    }

    public static class SongServer
    {
        static Dictionary<string, ISongServerType> m_types = new Dictionary<string, ISongServerType>();

        public static ISongServer LoadSongServer(string type, string url, string config)
        {
            return m_types[type].Load(url, config);
        }
        public static void RegisterSongServer(ISongServerType type)
        {
            m_types[type.Name] = type;
        }

        public static IEnumerable<ISongServerType> GetTypes()
        {
            return m_types.Values;
        }

        static SongServer()
        {
            RegisterSongServer(new XmlSongServerType());
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

        public string Description
        {
            get { return "XML soubor nìkde na internetu"; }
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
}
