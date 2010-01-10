using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace pocketzp
{
    public static class Tools
    {
        public static string DbPath
        {
            get
            {
                System.Reflection.Module[] modules = System.Reflection.Assembly.GetExecutingAssembly().GetModules();
                string basepath = Path.GetDirectoryName(modules[0].FullyQualifiedName);
                return Path.Combine(basepath, "db");
            }
        }

        public static string RemoveExtension(string file)
        {
            file = Path.ChangeExtension(file, "");
            if (file.EndsWith(".")) file = file.Substring(0, file.Length - 1);
            return file;
        }

        public static string PureName(string fn)
        {
            return RemoveExtension(Path.GetFileName(fn));
        }
    }
}
