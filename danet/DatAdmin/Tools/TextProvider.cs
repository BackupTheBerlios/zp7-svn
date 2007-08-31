using System;
using System.Collections.Generic;
using System.Text;
using DAIntf;
using System.IO;

namespace DatAdmin
{
    public class FileTextProvider : ITextProvider
    {
        Dictionary<string, string> m_texts = new Dictionary<string, string>();
        public void AddFile(string file, string lang)
        {
            using (StreamReader fr = new StreamReader(file))
            {
                while (!fr.EndOfStream)
                {
                    string line = fr.ReadLine();
                    if (line.StartsWith("#")) continue;
                    string[] arr = line.Split(new char[] { '=' }, 2);

                    if (arr.Length == 2)
                    {
                        m_texts[arr[0] + "@" + lang] = arr[1];
                    }
                }
            }
        }

        #region ITextProvider Members

        public string GetText(string name, string lang)
        {
            string key = name + "@" + lang;
            if (m_texts.ContainsKey(key)) return m_texts[key];
            return null;
        }

        #endregion

        public static void LoadStdTexts()
        {
            FileTextProvider provider = new FileTextProvider();
            foreach (string file in Directory.GetFiles(Core.LangDirectory))
            {
                string ext = Path.GetExtension(file).ToLower();
                if (ext.StartsWith(".")) ext = ext.Substring(1);
                provider.AddFile(file, ext);
            }
            Texts.RegisterTextProvider(provider);
        }
    }
}
