using System;
using System.Collections.Generic;
using System.Text;

namespace DatAdmin
{
    public interface ITextProvider
    {
        string GetText(string name, string lang);
    }

    public static class Texts
    {
        static List<ITextProvider> m_textProviders = new List<ITextProvider>();
        static string m_lang = "en";

        public static void RegisterTextProvider(ITextProvider provider)
        {
            m_textProviders.Add(provider);
        }

        public static void SetLang(string lang)
        {
            m_lang = lang;
        }

        public static string Get(string name)
        {
            foreach (ITextProvider provider in m_textProviders)
            {
                string res = provider.GetText(name, m_lang);
                if (res != null) return res;
            }
            return name;
        }
    }
}
