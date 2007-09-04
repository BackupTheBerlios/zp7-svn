using System;
using System.Collections.Generic;
using System.Text;

namespace DatAdmin
{
    public static class ConnTools
    {
        public static void InvokeVoid(IPhysicalConnection conn, SimpleCallback callback, IInvoker invoker, SimpleCallback onfinish)
        {
            IAsyncVoid async = conn.Invoker.InvokeVoid(callback);
            async.OnFinish(onfinish, invoker);
        }
        public static void InvokeVoid(ICommonSource conn, SimpleCallback callback, IInvoker invoker, SimpleCallback onfinish)
        {
            InvokeVoid(conn.Connection, callback, invoker, onfinish);
        }
    }
}
