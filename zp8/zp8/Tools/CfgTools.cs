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

        public static SongPrintFormatOptions CreateSongPrintFormatOptions(float pgwi)
        {
            SongPrintPropertyPage pg = GlobalOpts.Default.SongPrint;
            SongFormatOptions sopt = new SongFormatOptions(pgwi, pg.TextFont, pg.ChordFont, pg.LabelFont);
            SongPrintFormatOptions res = new SongPrintFormatOptions(pgwi, pg.TitleFont, pg.AuthorFont, sopt);
            return res;
        }
    }
}
