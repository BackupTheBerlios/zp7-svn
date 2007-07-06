using System;
using System.Collections.Generic;
using System.Text;

namespace zp8
{
    public abstract class SequenceItem
    {
        public abstract void AddToPages(LogPages pages, SongBook book);
    }

    public class AllSongsSequenceItem : SequenceItem
    {
        /*
        IDistribAlg m_distrib = DistribAlgs.Complex;
        public string DistribAlg
        {
            get { return m_distrib.Name; }
            set { m_distrib = DistribAlgs.FromName(value); }
        }
        */
        public override void AddToPages(LogPages pages, SongBook book)
        {
            List<PaneGrp> grps = new List<PaneGrp>();
            foreach (SongDb.songRow row in book.GetSongs(book.Formatting.Order))
            {
                grps.Add(book.FormatSong(row.ID));
            }
            IDistribAlg alg = DistribAlgs.FromEnum(book.Layout.DistribAlg);
            alg.Run(pages, grps, book.Layout);
        }
    }

    public class OutlineSequenceItem : SequenceItem
    {
        public override void AddToPages(LogPages pages, SongBook book)
        {
            PaneGrp outline = book.FormatOutline();
            DistribAlgs.Simple.Run(pages, new PaneGrp[] { outline }, book.Layout);
        }
    }

    public class BookSequence
    {
        List<SequenceItem> m_items = new List<SequenceItem>();

        public LogPages CreateLogPages(SongBook book)
        {
            LogPages pages = new LogPages(book.Layout.SmallPageHeight);
            foreach (SequenceItem item in m_items) item.AddToPages(pages, book);
            return pages;
        }
        public IList<SequenceItem> Items { get { return m_items; } }
    }
}
