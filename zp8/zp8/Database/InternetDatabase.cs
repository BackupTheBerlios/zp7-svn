using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml;
using System.Resources;
using System.Windows.Forms;
using System.Data;

using System.Data.SQLite;

namespace zp8
{
    public abstract partial class AbstractSongDatabase
    {
        public void DeleteSongsFromServer(int server)
        {
            List<SongDb.songRow> rows = new List<SongDb.songRow>();
            foreach (SongDb.songRow row in EnumSongs()) rows.Add(row);
            foreach (SongDb.songRow row in rows)
            {
                if (!row.Isserver_idNull() && row.server_id == server) row.Delete();
            }
        }

        /*
        public void ImportSongs(Stream fr, int? serverid)
        {
            InetSongDb xmldb = new InetSongDb();
            xmldb.ReadXml(fr);
            ImportSongs(serverid, xmldb);
        }

        public void ImportSongs(TextReader fr, int? serverid)
        {
            InetSongDb xmldb = new InetSongDb();
            xmldb.ReadXml(fr);
            ImportSongs(serverid, xmldb);
        }
        */

        public void ImportSongs(InetSongDb xmldb, int? serverid)
        {
            UnInstallTriggers();
            foreach (InetSongDb.songRow row in xmldb.song.Rows)
            {
                SongDb.songRow newrow = DataSet.song.NewsongRow();
                DbTools.InetSongRowToLocalSongRow(row, newrow);
                if (serverid.HasValue) newrow.server_id = serverid.Value;
                DataSet.song.AddsongRow(newrow);
            }
            InstallTriggers();
        }

        public void MergeInternetXml(int serverid, Stream fr)
        {
            InetSongDb xmldb = new InetSongDb();
            xmldb.ReadXml(fr);
            MergeInternetXml(serverid, xmldb);
        }

        public void MergeInternetXml(int serverid, InetSongDb xmldb)
        {
            UnInstallTriggers();

            // indexed by netid
            Dictionary<int, SongDb.songRow> locals = new Dictionary<int, SongDb.songRow>();
            foreach (SongDb.songRow song in EnumSongs())
            {
                if (!song.Isserver_idNull() && song.server_id == serverid)
                {
                    if (!song.IsnetIDNull()) locals[song.netID] = song;
                }
            }

            // nejdriv provedeme merge existujicich/pridani novych pisni
            foreach (InetSongDb.songRow src in xmldb.song)
            {
                if (locals.ContainsKey(src.ID))
                {
                    SongDb.songRow locsong = locals[src.ID];
                    if (locsong.IslocalmodifiedNull() || !locsong.localmodified)
                    {
                        DbTools.InetSongRowToLocalSongRow(src, locsong);
                        locsong.published = src.published;
                    }
                    locals.Remove(src.ID);
                }
                else
                {
                    SongDb.songRow newrow = DataSet.song.NewsongRow();
                    DbTools.InetSongRowToLocalSongRow(src, newrow);
                    newrow.server_id = serverid;
                    newrow.netID = src.ID;
                    newrow.published = src.published;
                    m_dataset.song.AddsongRow(newrow);
                }
            }

            // pak smazeme vsechny pisne k danemu serveru bez lokalnich modifikaci
            foreach (SongDb.songRow song in locals.Values)
            {
                if (song.IslocalmodifiedNull() || !song.localmodified)
                {
                    song.Delete();
                }
            }

            InstallTriggers();
        }

        public IEnumerable<SongDb.songRow> EnumSongsWithLocalModifications(int serverid)
        {
            foreach (SongDb.songRow song in EnumSongs())
            {
                if (!song.Isserver_idNull() && song.server_id == serverid)
                {
                    if (!song.IslocalmodifiedNull() && song.localmodified)
                    {
                        yield return song;
                    }
                }
            }
        }

        public List<SongDb.songRow> GetConflictingSongs(int serverid, InetSongDb xmldb)
        {
            List<SongDb.songRow> res = new List<SongDb.songRow>();
            foreach (SongDb.songRow row in EnumSongsWithLocalModifications(serverid))
            {
                if (!row.IsnetIDNull())
                {
                    InetSongDb.songRow inet = xmldb.song.FindByID(row.netID);
                    if (row.published != inet.published) res.Add(row);
                }
            }
            return res;
        }

        public void UpdateInternetXml(int serverid, InetSongDb xmldb)
        {
            UnInstallTriggers();
            foreach (SongDb.songRow local in EnumSongsWithLocalModifications(serverid))
            {
                if (local.IsnetIDNull())
                {
                    // nova pisen
                    InetSongDb.songRow newrow = xmldb.song.NewsongRow();
                    DbTools.LocalSongRowToInetSongRow(local, newrow);
                    local.netID = newrow.ID;
                    local.published = DateTime.UtcNow;
                    local.localmodified = false;
                    newrow.published = local.published;
                    xmldb.song.AddsongRow(newrow);
                }
                else
                {
                    InetSongDb.songRow inet = xmldb.song.FindByID(local.netID);
                    DbTools.LocalSongRowToInetSongRow(local, inet);
                    local.localmodified = false;
                    local.published = DateTime.UtcNow;
                    inet.published = local.published;
                }
            }

            DeleteMarkedSongs(serverid, xmldb);
            InstallTriggers();
        }

        protected void DeleteMarkedSongs(int serverid, InetSongDb xmldb)
        {
            List<SongDb.deletedsongRow> todel = new List<SongDb.deletedsongRow>();
            foreach (SongDb.deletedsongRow delete in m_dataset.deletedsong.Rows)
            {
                if (delete.server_id == serverid)
                {
                    if (xmldb != null) xmldb.song.FindByID(delete.song_netID).Delete();
                    todel.Add(delete);
                }
            }
            foreach (SongDb.deletedsongRow del in todel) del.Delete();
        }

        public void CreateInternetXml(int serverid, Stream fw)
        {
            UnInstallTriggers();
            InetSongDb xmldb = new InetSongDb();
            foreach (SongDb.songRow local in EnumSongs())
            {
                if (!local.Isserver_idNull() && local.server_id == serverid)
                {
                    InetSongDb.songRow newrow = xmldb.song.NewsongRow();
                    DbTools.LocalSongRowToInetSongRow(local, newrow);
                    local.netID = newrow.ID;
                    local.published = DateTime.UtcNow;
                    local.localmodified = false;
                    newrow.published = local.published;
                    xmldb.song.AddsongRow(newrow);
                }
            }
            xmldb.WriteXml(fw);
            InstallTriggers();
        }

        /*
        public void GetSongsAsInetXml(int serverid, Stream fw)
        {
            InetSongDb xmldb = new InetSongDb();
            foreach (SongDb.songRow row in EnumSongs())
            {
                if (row.server_id == serverid)
                {
                    InetSongDb.songRow newrow = xmldb.song.NewsongRow();
                    DbTools.LocalSongRowToInetSongRow(row, newrow);
                    if (!row.IsnetIDNull()) newrow.ID = row.netID;
                    xmldb.song.AddsongRow(newrow);
                }
            }
            xmldb.WriteXml(fw);
        }
        */
    }
}
