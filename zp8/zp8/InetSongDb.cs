using System;

namespace zp8 {

    public partial class InetSongDb {
        partial class songDataTable
        {
            public songRow FindByID(int id)
            {
                foreach (songRow row in Rows) if (row.ID == id) return row;
                throw new Exception("Row not found");
            }
        }
    }
}
