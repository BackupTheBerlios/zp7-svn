using System;
using System.Collections.Generic;
using System.Text;

namespace zp8
{
    public interface IWaitDialog : IDisposable
    {
        bool Canceled { get;}
        void Message(string msg);
    }
}
