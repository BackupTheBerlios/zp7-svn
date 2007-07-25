using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace zp8
{
    public interface ISongFilter
    {
    }

    public struct SongData
    {
        public string Title;
        public string Author;
        public string Group;
        public string Text;
        public string Language;
    }

    public interface ISongParser : ISongFilter
    {
        IEnumerable<SongData> Parse(Stream fr);
    }

    public interface ISongFormatter : ISongFilter
    {
        void Format(IEnumerable<SongData> songs, Stream fw);
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ConfigurableSongFilterAttribute : Attribute
    {
        public string Name;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StaticSongFilterAttribute : Attribute
    {
    }

    public struct ConfigurableSongFilterStruct
    {
        public string Name;
        public Type Type;
    }

    public static class SongFilters
    {
        public static readonly List<ConfigurableSongFilterStruct> FilterTypes = new List<ConfigurableSongFilterStruct>();
        static List<ISongFilter> m_staticFilters = new List<ISongFilter>();

        static SongFilters()
        {
            ScanAssembly(Assembly.GetAssembly(typeof(SongFilters)));

            try { Directory.CreateDirectory(CustomFiltersDirectory); }
            catch (Exception) { }
        }

        public static void ScanAssembly(Assembly asm)
        {
            foreach (Type t in asm.GetTypes())
            {
                foreach (ConfigurableSongFilterAttribute attr in t.GetCustomAttributes(typeof(ConfigurableSongFilterAttribute), true))
                {
                    ConfigurableSongFilterStruct s;
                    s.Name = attr.Name;
                    s.Type = t;
                    FilterTypes.Add(s);
                }
                foreach (StaticSongFilterAttribute attr in t.GetCustomAttributes(typeof(StaticSongFilterAttribute), true))
                {
                    m_staticFilters.Add((ISongFilter)t.GetConstructor(new Type[] { }).Invoke(new object[] { }));
                }
            }
        }

        public static IEnumerable<T> EnumFilters<T>() where T : class
        {
            foreach (ISongFilter flt in m_staticFilters) if (flt is T) yield return (T)flt;
        }

        public static string CustomFiltersDirectory { get { return Path.Combine(Options.CfgDirectory, "filters"); } }

        public static IEnumerable<string> GetCustomFilters()
        {
            foreach (string file in Directory.GetFiles(CustomFiltersDirectory))
            {
                string name = Path.GetFileName(file);
                if (Path.GetExtension(name).ToLower() == ".flt")
                    yield return Path.ChangeExtension(name, null);
            }
        }

    }
}
