using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace zp8
{
    public static class DataReaderExetension
    {
        public static string SafeString(this IDataRecord row, string field)
        {
            int ord = row.GetOrdinal(field);
            return row.SafeString(ord);
        }
        public static string SafeString(this IDataRecord row, int ord)
        {
            if (ord >= 0 && !row.IsDBNull(ord)) return row[ord].ToString();
            return null;
        }
        public static int SafeInt(this IDataRecord row, int ord)
        {
            try
            {
                if (ord >= 0 && !row.IsDBNull(ord)) return Int32.Parse(row[ord].ToString());
                return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}
