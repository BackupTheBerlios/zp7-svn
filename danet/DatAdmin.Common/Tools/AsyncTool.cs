using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DatAdmin
{
    public static class Async
    {
        public static Thread MainThread;
        public static IWaitDialog WaitDialog;
        public static bool IsMainThread
        {
            get
            {
                return Thread.CurrentThread == MainThread;
            }
        }

        public static IAsyncVoid InvokeVoid(SimpleCallback proc)
        {
            AsyncAction res = new AsyncAction(proc);
            ThreadPool.Invoke(res.DoRun);
            return res.Async;
        }
        public static IAsyncValue<T> InvokeValue<T>(ReturnValueCallback<T> proc)
        {
            AsyncResultAction<T> res = new AsyncResultAction<T>(proc);
            ThreadPool.Invoke(res.DoRun);
            return res.Async;
        }
    }
}
