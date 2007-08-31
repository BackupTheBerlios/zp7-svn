using System;
using System.Collections.Generic;
using System.Text;

namespace zp8
{
    public static class Templates
    {
        public static string MakeTemplate(string tpl)
        {
            tpl = tpl.Replace("$[NL]", "\r\n");
            return tpl;
        }

        public static string MakeTemplate(string tpl, ISongRow song)
        {
            tpl = tpl.Replace("$[TITLE]", song.Title);
            tpl = tpl.Replace("$[AUTHOR]", song.Author);
            tpl = tpl.Replace("$[GROUP]", song.GroupName);
            tpl = tpl.Replace("$[REMARK]", song.Remark);
            return MakeTemplate(tpl);
        }
    }
}
