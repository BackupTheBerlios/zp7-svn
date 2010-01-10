using System;
using System.Collections.Generic;
using System.Text;

namespace zp8
{
    public class SearchIndex
    {
        SongDatabase m_db;
        Dictionary<int, string> m_searchTexts = new Dictionary<int, string>();

        public SearchIndex(SongDatabase db)
        {
            m_db = db;

            using (IWaitDialog dlg = WaitForm.Show("Vytvářím index pro vyhledávání", false))
            {
                LoadSongs("1=1", "1=1");
            }
        }

        private void LoadSongs(string song_condition, string songdata_condition)
        {
            using (var reader = m_db.ExecuteReader("select song_id, textdata from songdata where datatype_id=1 and " + songdata_condition))
            {
                while (reader.Read())
                {
                    int id = reader.SafeInt(0);
                    string text = reader.SafeString(1);
                    AddText(id, SongTool.RemoveChords(text));
                }
            }

            using (var reader = m_db.ExecuteReader("select ID, title, author, groupname, remark from song where " + song_condition))
            {
                while (reader.Read())
                {
                    int id = reader.SafeInt(0);
                    string title = reader.SafeString(1);
                    string author = reader.SafeString(2);
                    string groupname = reader.SafeString(3);
                    string remark = reader.SafeString(4);
                    AddText(id, title);
                    AddText(id, author);
                    AddText(id, groupname);
                    AddText(id, remark);
                }
            }
        }

        private void AddText(int id, string text)
        {
            if (String.IsNullOrEmpty(text)) return;
            if (!m_searchTexts.ContainsKey(id)) m_searchTexts[id] = "";
            m_searchTexts[id] += Searching.MakeSearchText(text);
        }

        public void UpdatedSong(int id)
        {
            m_searchTexts[id] = "";
            LoadSongs("ID=" + id.ToString(), "song_id=" + id.ToString());
        }

        public List<int> SearchText(string text)
        {
            var res = new List<int>();
            text = Searching.MakeSearchText(text);
            foreach (var tuple in m_searchTexts)
            {
                if (tuple.Value.Contains(text)) res.Add(tuple.Key);
            }
            return res;
        }
    }
}
