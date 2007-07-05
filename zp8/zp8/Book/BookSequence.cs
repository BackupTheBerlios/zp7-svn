using System;
using System.Collections.Generic;
using System.Text;

namespace zp8
{
    public abstract class SequenceItem
    {
    }

    public class BookSequence
    {
        List<SequenceItem> m_items = new List<SequenceItem>();

        public FormattedBook FormatBook(BookLayout layout)
        {
            return null;
        }
    }
}
