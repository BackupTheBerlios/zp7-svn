using System;
using System.Collections.Generic;
using System.Text;

namespace zp8.Database
{
    public interface ISongServer
    {
        string URL { get;}
        string Type { get;}
        string Config { get;}
        void DownloadNew(AbstractSongDatabase db, int serverid);
        void UploadChanges(AbstractSongDatabase db, int serverid);
        void UploadWhole(AbstractSongDatabase db, int serverid);
    }

    public class BaseSongServer : zp8.PropertyPageBase, ISongServer
    {
    }

    public class ReadOnlySongServer : ISongServer
    {
    }
}
