using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace DatAdmin
{
    public static class PluginTools
    {
        public static void LoadPlugins()
        {
            foreach (string file in Directory.GetFiles(Core.PluginsDirectory))
            {
                string name = Path.GetFileName(file);
                if (name.ToLower().StartsWith("plugin.") && name.ToLower().EndsWith(".dll"))
                {
                    Assembly asm = Assembly.LoadFile(file);
                    Plugins.AddAssembly(asm);
                }
            }
        }
    }
}
