using System;
using System.Collections.Generic;
using System.Text;

namespace zp8
{
    public static class Sorting
    {
        public static int CompareTitleGroup(SongDb.songRow a, SongDb.songRow b)
        {
            int rt = String.Compare(a.title, b.title, true);
            if (rt != 0) return rt;
            int rg = String.Compare(a.groupname, b.groupname, true);
            if (rg != 0) return rg;
            return a.ID - b.ID;
        }
        public static int CompareGroupTitle(SongDb.songRow a, SongDb.songRow b)
        {
            int rg = String.Compare(a.groupname, b.groupname, true);
            if (rg != 0) return rg;
            int rt = String.Compare(a.title, b.title, true);
            if (rt != 0) return rt;
            return a.ID - b.ID;
        }
        public static int CompareDatabase(SongDb.songRow a, SongDb.songRow b)
        {
            return a.ID - b.ID;
        }
        public static Comparison<SongDb.songRow> GetComparison(SongOrder order)
        {
            switch (order)
            {
                case SongOrder.TitleGroup:
                    return CompareTitleGroup;
                case SongOrder.GroupTitle:
                    return CompareTitleGroup;
                case SongOrder.Database:
                    return CompareDatabase;
            }
            throw new Exception("Unsupported order:" + order.ToString());
        }
    }
}
