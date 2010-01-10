using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DatAdmin;

namespace zp8
{
    public static class Directories
    {
        public static string AppDataDirectory
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Zpevnikator");
            }
        }

        public static string DbDirectory
        {
            get
            {
                return Path.Combine(AppDataDirectory, "db");
            }
        }

        public static string CfgDirectory
        {
            get
            {
                return Path.Combine(AppDataDirectory, "cfg");
            }
        }

        static Directories()
        {
            CreateWantedDirs();
        }

        public static bool IsMono { get { return Type.GetType("Mono.Runtime") != null; } }

        public static void CopyDefaultData()
        {
            if (!IsMono) return;
            try
            {
                IOTool.CopyDirectory("/etc/zpevnikator/.config", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
            catch (Exception e)
            {
            }
        }

        private static void CreateWantedDirs()
        {
            try { Directory.CreateDirectory(AppDataDirectory); }
            catch (Exception) { }
            try { Directory.CreateDirectory(CfgDirectory); }
            catch (Exception) { }
            try { Directory.CreateDirectory(DbDirectory); }
            catch (Exception) { }
        }    
    }
}
