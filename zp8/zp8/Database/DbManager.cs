using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace zp8
{
    public class DbManager
    {
        Dictionary<string, SongDatabase> m_dbs = new Dictionary<string, SongDatabase>();
        public static DbManager Manager = new DbManager();
        public static string DbPath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db"); } }

        public void Refresh()
        {
            foreach (string path in Directory.GetFiles(DbPath))
            {
                if (m_dbs.ContainsKey(SongDatabase.ExtractDbName(path))) continue;
                string file = Path.GetFileName(path);
                if (Path.GetExtension(file).ToLower() == ".db")
                {
                    SongDatabase db = new SongDatabase(path);
                    m_dbs[db.Name] = db;
                }
            }
        }

        public SongDatabase this[string name]
        {
            get { return m_dbs[name]; }
        }
        public IEnumerable<SongDatabase> GetDatabases()
        {
            return m_dbs.Values;
        }

        public SongDatabase CreateDatabase(string dbname)
        {
            dbname = dbname.ToLower();
            if (!dbname.EndsWith(".db")) dbname += ".db";
            if (m_dbs.ContainsKey(dbname)) throw new Exception("Duplicate db name");
            SongDatabase db = new SongDatabase(Path.Combine(DbPath, dbname));
            m_dbs[dbname] = db;
            db.WantOpen();
            return db;
        }

        static DbManager()
        {
            if (!Directory.Exists(DbPath)) Directory.CreateDirectory(DbPath);
        }
    }
}
