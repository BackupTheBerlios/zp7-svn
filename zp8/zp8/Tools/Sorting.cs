using System;
using System.Collections.Generic;
using System.Text;

namespace zp8
{
    public static class Sorting
    {
        public static int CompareTitleGroup(ISongRow a, ISongRow b)
        {
            int rt = String.Compare(a.Title, b.Title, true);
            if (rt != 0) return rt;
            int rg = String.Compare(a.GroupName, b.GroupName, true);
            if (rg != 0) return rg;
            return a.ID - b.ID;
        }
        public static int CompareGroupTitle(ISongRow a, ISongRow b)
        {
            int rg = String.Compare(a.GroupName, b.GroupName, true);
            if (rg != 0) return rg;
            int rt = String.Compare(a.Title, b.Title, true);
            if (rt != 0) return rt;
            return a.ID - b.ID;
        }
        public static int CompareDatabase(ISongRow a, ISongRow b)
        {
            return a.ID - b.ID;
        }
        public static Comparison<ISongRow> GetComparison(SongOrder order)
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
        public static void Sort(List<SongDb.songRow> rows, SongOrder order)
        {
            List<ISongRow> irows = new List<ISongRow>();
            foreach (ISongRow row in rows) irows.Add(row);
            irows.Sort(GetComparison(order));
            rows.Clear();
            foreach (SongDb.songRow row in irows) rows.Add(row);
        }
    }
}
