using System;
using System.Collections.Generic;
using System.Text;
using PdfSharp.Drawing;

namespace zp8
{
    public static class CfgTools
    {
        public static FormatOptions CreateSongViewFormatOptions(float pgwi)
        {
            SongViewPropertyPage pg = GlobalOpts.Default.SongView;
            FormatOptions res = new FormatOptions(pgwi, pg.TextFont, pg.ChordFont, pg.LabelFont);
            return res;
        }
    }
}
