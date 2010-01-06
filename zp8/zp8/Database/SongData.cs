using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml;

namespace zp8
{
    //public interface SongData
    //{
    //    int LocalID { get; }
    //    string NetID { get; set; }
    //    string Title { get; set; }
    //    string Author { get; set; }
    //    string GroupName { get; set; }
    //    int Transp { get; }
    //    string OrigText { get; }
    //    string SongText { get; }
    //    string Lang { get; set; }
    //    string Remark { get; set; }
    //}

    public enum SongDataType : int
    {
        Text = 1,
        Notation = 2,
        Link = 3,
    }

    public class SongDataItem
    {
        public SongDataType DataType;
        public string Label;
        public string TextData;
    }

    public class SongData
    {
        public List<SongDataItem> Items = new List<SongDataItem>();

        int m_localID = 0;
        public int LocalID
        {
            get { return m_localID; }
            set { m_localID = value; }
        }

        string m_netID = "0";
        public string NetID
        {
            get { return m_netID; }
            set { m_netID = value; }
        }

        string m_title = "";
        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        string m_author = "";
        public string Author
        {
            get { return m_author; }
            set { m_author = value; }
        }

        string m_groupname = "";
        public string GroupName
        {
            get { return m_groupname; }
            set { m_groupname = value; }
        }

        int m_transp;
        public int Transp
        {
            get { return m_transp; }
            set { m_transp = value; }
        }

        public string SongText
        {
            get
            {
                return Chords.Transpose(OrigText, Transp);
            }
        }

        public string OrigText
        {
            get
            {
                var item = (from it in Items where it.DataType == SongDataType.Text && it.Label == null select it).FirstOrDefault();
                if (item != null) return item.TextData;
                return null;
            }
            set
            {
                Items.RemoveAll(it => it.DataType == SongDataType.Text && it.Label == null);
                Items.Add(new SongDataItem { DataType = SongDataType.Text, TextData = value });
            }

        }

        string m_lang = "";
        public string Lang
        {
            get { return m_lang; }
            set { m_lang = value; }
        }

        string m_remark = "";
        public string Remark
        {
            get { return m_remark; }
            set { m_remark = value; }
        }

        DateTime? m_published;
        public DateTime? Published
        {
            get { return m_published; }
            set { m_published = value; }
        }

        public string this[string name]
        {
            get
            {
                switch (name)
                {
                    case "title": return Title;
                    case "groupname": return GroupName;
                    case "author": return Author;
                    case "lang": return Lang;
                    case "netID": return NetID;
                    case "transp": return Transp.ToString();
                    case "remark": return Remark;
                }
                return "";
            }
        }

        public void Save(XmlWriter xw)
        {
            xw.WriteStartElement("song");
            if (NetID != null) xw.WriteElementString("ID", NetID.ToString());
            if (Lang != null) xw.WriteElementString("lang", Lang);
            if (SongText != null) xw.WriteElementString("songtext", SongText);
            if (Author != null) xw.WriteElementString("author", Author);
            if (GroupName != null) xw.WriteElementString("groupname", GroupName);
            if (Title != null) xw.WriteElementString("title", Title);
            if (Published != null) xw.WriteElementString("published", Published.Value.ToString("s"));
            xw.WriteEndElement();
        }

        public void DeleteData(SongDataType datatype)
        {
            Items.RemoveAll(it => it.DataType == datatype);
        }

        public void AddLink(string link)
        {
            Items.Add(new SongDataItem { DataType = SongDataType.Link, TextData = link });
        }

        public IEnumerable<SongDataItem> GetData(SongDataType datatype)
        {
            foreach (var data in Items)
            {
                if (data.DataType == datatype) yield return data;
            }
        }
    }

}
