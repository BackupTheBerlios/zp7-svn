using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DAIntf
{
    internal class AsyncAction
    {
        SimpleCallback m_callback;
        AsyncVoid m_async;

        private void DoRun()
        {
            try
            {
                m_callback();
                m_async.DispatchFinished();
            }
            catch (Exception e)
            {
                m_async.DispatchException(e);
            }
        }
        public AsyncAction(SimpleCallback callback)
        {
            m_callback = callback;
        }
        public IAsyncVoid GetAsync()
        {
            m_async = new AsyncVoid();
            new Thread(DoRun).Start();
            return m_async;
        }
    }

    internal class AsyncResultAction<T>
    {
        ReturnValueCallback<T> m_callback;
        AsyncValue<T> m_async;

        private void DoRun()
        {
            try
            {
                T result = m_callback();
                m_async.DispatchFinished(result);
            }
            catch (Exception e)
            {
                m_async.DispatchException(e);
            }
        }
        public AsyncResultAction(ReturnValueCallback<T> callback)
        {
            m_callback = callback;
        }
        public IAsyncValue<T> GetAsync()
        {
            m_async = new AsyncValue<T>();
            new Thread(DoRun).Start();
            return m_async;
        }
    }


    public static partial class Async
    {
        public static IAsyncVoid InvokeVoid(SimpleCallback proc)
        {
            AsyncAction res = new AsyncAction(proc);
            return res.GetAsync();
        }
        public static IAsyncValue<T> InvokeValue<T>(ReturnValueCallback<T> proc)
        {
            AsyncResultAction<T> res = new AsyncResultAction<T>(proc);
            return res.GetAsync();
        }
    }
}
