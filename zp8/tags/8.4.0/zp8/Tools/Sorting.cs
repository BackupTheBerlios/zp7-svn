using System;
using System.Collections.Generic;
using System.Text;

namespace zp8
{
    public static class Sorting
    {
        public static int CompareTitleGroup(SongData a, SongData b)
        {
            int rt = String.Compare(a.Title, b.Title, true);
            if (rt != 0) return rt;
            int rg = String.Compare(a.GroupName, b.GroupName, true);
            if (rg != 0) return rg;
            return a.LocalID - b.LocalID;
        }
        public static int CompareGroupTitle(SongData a, SongData b)
        {
            int rg = String.Compare(a.GroupName, b.GroupName, true);
            if (rg != 0) return rg;
            int rt = String.Compare(a.Title, b.Title, true);
            if (rt != 0) return rt;
            return a.LocalID - b.LocalID;
        }
        public static int CompareDatabase(SongData a, SongData b)
        {
            return a.LocalID - b.LocalID;
        }
        public static Comparison<SongData> GetComparison(SongOrder order)
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
        public static void Sort(List<SongData> rows, SongOrder order)
        {
            List<SongData> irows = new List<SongData>();
            foreach (SongData row in rows) irows.Add(row);
            irows.Sort(GetComparison(order));
            rows.Clear();
            foreach (SongData row in irows) rows.Add(row);
        }
    }
}
