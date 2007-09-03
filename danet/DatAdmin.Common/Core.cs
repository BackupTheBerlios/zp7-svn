using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DatAdmin
{
    public static class Core
    {
        static Core()
        {
            try { Directory.CreateDirectory(ConfigDirectory); }
            catch (Exception) { }
            try { Directory.CreateDirectory(DataDirectory); }
            catch (Exception) { }
            try { Directory.CreateDirectory(PluginsDirectory); }
            catch (Exception) { }
        }
        public static string BaseDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }
        public static string DataDirectory
        {
            get { return Path.Combine(BaseDirectory, "data"); }
        }
        public static string PluginsDirectory
        {
            get { return Path.Combine(BaseDirectory, "Plugins"); }
        }
        public static string ConfigDirectory
        {
            get { return Path.Combine(BaseDirectory, "cfg"); }
        }
        public static string LangDirectory
        {
            get { return Path.Combine(BaseDirectory, "lang"); }
        }
        public static bool IsGUIDatAdmin;
    }
}
