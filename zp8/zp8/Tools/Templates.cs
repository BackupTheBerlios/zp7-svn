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
            tpl = tpl.Replace("$[TITLE]", song.title);
            tpl = tpl.Replace("$[AUTHOR]", song.author);
            tpl = tpl.Replace("$[GROUP]", song.groupname);
            tpl = tpl.Replace("$[REMARK]", song.remark);
            return MakeTemplate(tpl);
        }
    }
}
