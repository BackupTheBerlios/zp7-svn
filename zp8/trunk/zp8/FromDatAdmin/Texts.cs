using System;
using System.Collections.Generic;
using System.Text;

namespace DatAdmin
{
    // dummy class for compatibility with DatAdmin
    public static class Texts
    {
        static Dictionary<string, string> m_texts = new Dictionary<string, string>();
        static Texts()
        {
            m_texts["s_yes"] = "Ano";
            m_texts["s_no"] = "Ne";
        }
        public static string Get(string text)
        {
            if (m_texts.ContainsKey(text)) return m_texts[text];
            return text;
        }
    }
}
