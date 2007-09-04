using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DatAdmin
{
    public class AsyncBase : IAsyncBase
    {
        bool m_completed;
        SimpleCallback m_simpleCallback;
        //Thread m_simpleDestThread;
        protected IInvoker m_invoker;
        AutoResetEvent m_event = new AutoResetEvent(false);
        Exception m_error;

        #region IAsyncBase Members

        public void Wait()
        {
            if (Async.IsMainThread)
            {
                // show dialog and wait active
                Async.WaitDialog.Show();
                while (!m_completed)
                {
                    if (Async.WaitDialog.Canceled)
                    {
                        Async.WaitDialog.Hide();
                        throw new WaitAbortException();
                    }
                    Application.DoEvents();
                }
                Async.WaitDialog.Hide();
            }
            else
            {
                m_event.WaitOne();
            }
            if (m_error != null) throw m_error;
        }

        public bool IsCompleted
        {
            get { return m_completed; }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public void OnFinish(SimpleCallback callback, IInvoker invoker)
        {
            m_invoker = invoker;
            lock (this)
            {
                if (IsCompleted)
                {
                    m_invoker.InvokeVoid(callback);
                }
                else
                {
                    m_simpleCallback = callback;
                }
            }
        }

        #endregion

        protected virtual void PerformCallbacks()
        {
            if (m_simpleCallback != null) m_invoker.InvokeVoid(m_simpleCallback);
        }

        /// called from any thread, call callback from apropriate thread
        protected void DoDispatchFinished()
        {
            lock (this)
            {
                m_completed = true;
                PerformCallbacks();
                m_event.Set();
            }
        }

        public void DispatchException(Exception e)
        {
            lock (this)
            {
                m_error = e;
                m_completed = true;
                m_event.Set();
            }
        }
    }
    public class AsyncVoid : AsyncBase, IAsyncVoid
    {
        ///  call this when finished
        public void DispatchFinished()
        {
            DoDispatchFinished();
        }
    }

    public class AsyncValue<T> : AsyncBase, IAsyncValue<T>
    {
        T m_value;
        ValueCallback<T> m_valueCallback;

        #region IAsyncValue<T> Members

        public T Result
        {
            get
            {
                if (!IsCompleted) throw new AsyncException();
                return m_value;
            }
        }

        public void OnFinish(ValueCallback<T> callback, IInvoker invoker)
        {
            m_invoker = invoker;
            lock (this)
            {
                if (IsCompleted)
                {
                    callback(m_value);
                }
                else
                {
                    m_valueCallback = callback;
                }
            }
        }

        #endregion

        private void DoValueCall()
        {
            m_valueCallback(m_value);
        }

        protected override void PerformCallbacks()
        {
            base.PerformCallbacks();
            if (m_valueCallback != null) m_invoker.InvokeVoid(DoValueCall);
        }

        ///  call this when finished
        public void DispatchFinished(T value)
        {
            m_value = value;
            DoDispatchFinished();
        }
    }
    public class AsyncAction
    {
        SimpleCallback m_callback;
        AsyncVoid m_async;

        public void DoRun()
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
            m_async = new AsyncVoid();
        }

        public IAsyncVoid Async { get { return m_async; } }

        //public IAsyncVoid GetAsync()
        //{
        //    m_async = new AsyncVoid();
        //    new Thread(DoRun).Start();
        //    return m_async;
        //}
    }

    public class AsyncResultAction<T>
    {
        ReturnValueCallback<T> m_callback;
        AsyncValue<T> m_async;

        public void DoRun()
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
            m_async = new AsyncValue<T>();
        }
        public IAsyncValue<T> Async { get { return m_async; } }
    }
}
