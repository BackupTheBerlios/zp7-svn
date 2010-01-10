using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace zp8
{
    public static class DataRowExtension
    {
        public static int GetOrdinal(this DataColumnCollection cols, string name)
        {
            int i = 0;
            foreach (DataColumn col in cols)
            {
                if (col.ColumnName == name) return i;
                i++;
            }
            return -1;
        }
        public static string SafeString(this DataRow row, string field)
        {
            int ord = row.Table.Columns.GetOrdinal(field);
            return row.SafeString(ord);
        }
        public static string SafeString(this DataRow row, int ord)
        {
            if (ord >= 0 && row[ord] != null && row[ord] != DBNull.Value) return row[ord].ToString();
            return null;
        }
        public static int SafeInt(this DataRow row, int ord)
        {
            try
            {
                if (ord >= 0 && row[ord] != null && row[ord] != DBNull.Value) return Int32.Parse(row[ord].ToString());
                return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}
