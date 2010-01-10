using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace zp8.install
{
    class Program
    {
        static string Combine(string arg0, string arg1, params string[] nextargs)
        {
            string res = Path.Combine(arg0, arg1);
            foreach (string n in nextargs)
            {
                res = Path.Combine(res, n);
            }
            return res;
        }

        private static void SaveFile(string fn, string data)
        {
            string basedir = AppDomain.CurrentDomain.BaseDirectory;
            using (StreamWriter sw = new StreamWriter(Path.Combine(basedir, fn)))
            {
                sw.Write(data);
            }
        }

        static void Main(string[] args)
        {
            string basedir = AppDomain.CurrentDomain.BaseDirectory;
            try { File.Delete(Path.Combine(basedir, "System.Data.SQLite.dll")); }
            catch { }

            if (Type.GetType("Mono.Runtime") == null)
            {
                if (IntPtr.Size == 4)
                {
                    SaveFile("zp8.exe.config", InstFiles.exe32);
                }
                if (IntPtr.Size == 8)
                {
                    SaveFile("zp8.exe.config", InstFiles.exe64);
                }
            }
        }
    }
}
