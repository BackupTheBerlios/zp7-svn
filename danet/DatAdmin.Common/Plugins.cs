using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace DatAdmin
{
    public static class Plugins
    {
        public static void AddAssembly(Assembly assembly)
        {
            CreateFactory.AddAssembly(assembly);
            NodeFactory.AddAssembly(assembly);
        }
    }
}
