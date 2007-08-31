namespace zp8 {
    
    
    public partial class SongDb {
        public partial class songDataTable
        {
        }

        public partial class songRow : ISongRow
        {
            public string Title
            {
                get
                {
                    try
                    {
                        return title;
                    }
                    catch (System.Data.StrongTypingException)
                    {
                        return "";
                    }
                }
                set
                {
                    title = value;
                }
            }
            public string GroupName
            {
                get
                {
                    try
                    {
                        return groupname;
                    }
                    catch (System.Data.StrongTypingException)
                    {
                        return "";
                    }
                }
                set
                {
                    groupname = value;
                }
            }
            public string Author
            {
                get
                {
                    try
                    {
                        return author;
                    }
                    catch (System.Data.StrongTypingException)
                    {
                        return "";
                    }
                }
                set
                {
                    author = value;
                }
            }
            public string SongText
            {
                get
                {
                    try
                    {
                        return songtext;
                    }
                    catch (System.Data.StrongTypingException)
                    {
                        return "";
                    }
                }
                set
                {
                    songtext = value;
                }
            }
            public string Lang
            {
                get
                {
                    try
                    {
                        return lang;
                    }
                    catch (System.Data.StrongTypingException)
                    {
                        return "";
                    }
                }
                set
                {
                    lang = value;
                }
            }
            public string Remark
            {
                get
                {
                    try
                    {
                        return remark;
                    }
                    catch (System.Data.StrongTypingException)
                    {
                        return "";
                    }
                }
                set
                {
                    remark = value;
                }
            }
        }
    }
}
