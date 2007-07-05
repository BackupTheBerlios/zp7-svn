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
        IDistribAlg m_distrib = DistribAlgs.Simple;
        public string DistribAlg
        {
            get { return m_distrib.Name; }
            set { m_distrib = DistribAlgs.FromName(value); }
        }
        public override void AddToPages(LogPages pages, SongBook book)
        {
            List<PaneGrp> grps = new List<PaneGrp>();
            foreach (SongDb.songRow row in book.DataSet.song)
            {
                grps.Add(book.FormatSong(row.ID));
            }
            m_distrib.Run(pages, grps);
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
    }
}
