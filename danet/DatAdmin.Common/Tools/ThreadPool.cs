using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DatAdmin
{
    public static class ThreadPool
    {
        public static void Invoke(ThreadStart proc)
        {
            new Thread(proc).Start();
        }
    }
}
