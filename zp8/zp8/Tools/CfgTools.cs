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
            SongFormatOptions res = new SongFormatOptions(pgwi, PdfPrintTarget.InfoContext, pg.TextFont, pg.ChordFont, pg.LabelFont);
            return res;
        }

        public static SongPrintFormatOptions CreateSongPrintFormatOptions(float pgwi, XGraphics infoContext)
        {
            SongPrintPropertyPage pg = GlobalOpts.Default.SongPrint;
            SongFormatOptions sopt = new SongFormatOptions(pgwi, infoContext, pg.TextFont, pg.ChordFont, pg.LabelFont);
            SongPrintFormatOptions res = new SongPrintFormatOptions(pgwi, infoContext, pg.TitleFont, pg.AuthorFont, sopt);
            return res;
        }
    }
}
