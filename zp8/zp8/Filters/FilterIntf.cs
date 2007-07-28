using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;

namespace zp8
{
    public interface ISongFilter
    {
        string Title { get;}
        string Description { get;}
        //string FileDialogFilter { get;}
        object CreateDynamicProperties();
    }

    public interface ICustomSongFilter
    {
        void SetName(string name);
    }

    /*
    public struct SongData
    {
        public string Title;
        public string Author;
        public string Group;
        public string Text;
        public string Language;
    }
    */

    public interface ISongParser : ISongFilter
    {
        //IEnumerable<SongData> Parse(Stream fr);
        void Parse(object props, InetSongDb db);
    }

    public interface IStreamSongParser
    {
        //IEnumerable<SongData> Parse(Stream fr);
        void Parse(Stream fr, InetSongDb db);
    }

    public interface ISongFormatter : ISongFilter
    {
        //void Format(IEnumerable<SongData> songs, Stream fw);
        void Format(InetSongDb db, object props);
    }

    public interface IStreamSongFormatter
    {
        //void Format(IEnumerable<SongData> songs, Stream fw);
        void Format(InetSongDb db, Stream fw);
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
            foreach (string cf in GetCustomFilters())
            {
                ISongFilter flt = LoadCustomFilter(cf);
                if (flt is T) yield return (T)flt;
            }
        }

        public static string CustomFiltersDirectory { get { return Path.Combine(Options.CfgDirectory, "filters"); } }
        public static string FilterPath(string name) { return Path.Combine(CustomFiltersDirectory, name + ".flt"); }

        public static IEnumerable<string> GetCustomFilters()
        {
            foreach (string file in Directory.GetFiles(CustomFiltersDirectory))
            {
                string name = Path.GetFileName(file);
                if (Path.GetExtension(name).ToLower() == ".flt")
                    yield return Path.ChangeExtension(name, null);
            }
        }

        public static bool ExistsCustomFilter(string name)
        {
            return File.Exists(FilterPath(name));
        }

        public static void DeleteFilter(string name)
        {
            File.Delete(FilterPath(name));
        }

        public static void SaveCustomFilter(string name, ISongFilter flt)
        {
            Type type = flt.GetType();
            XmlSerializer ser = new XmlSerializer(type);
            using (FileStream fw = new FileStream(FilterPath(name), FileMode.Create))
            {
                using (XmlWriter xw = XmlWriter.Create(fw))
                {
                    xw.WriteStartElement("CustomFilter");
                    xw.WriteAttributeString("type", type.FullName);
                    ser.Serialize(xw, flt);
                    xw.WriteEndElement();
                }
            }
        }

        public static ISongFilter LoadCustomFilter(string name)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FilterPath(name));
            string typename = doc.DocumentElement.GetAttribute("type");
            Type type = Type.GetType(typename);
            XmlSerializer ser = new XmlSerializer(type);
            using (XmlNodeReader xr = new XmlNodeReader(doc.DocumentElement.LastChild))
            {
                object res = ser.Deserialize(xr);
                ((ICustomSongFilter)res).SetName(name);
                return (ISongFilter)res;
            }
        }
    }
}
