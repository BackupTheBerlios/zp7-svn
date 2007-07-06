using System;
using System.Collections.Generic;
using System.Text;
using PdfSharp.Drawing;

namespace zp8
{
    public static class CfgTools
    {
        public static SongFormatOptions CreateSongViewFormatOptions(float pgwi)
        {
            SongViewPropertyPage pg = GlobalOpts.Default.SongView;
            SongFormatOptions res = new SongFormatOptions(pgwi, pg.TextFont, pg.ChordFont, pg.LabelFont);
            return res;
        }
    }
}
