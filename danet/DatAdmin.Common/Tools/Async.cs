using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DatAdmin
{
    public delegate IAsyncResult BeginInvokeDelegate(Delegate proc);

    public static partial class Async
    {
        public static Thread MainThread;
        public static BeginInvokeDelegate MainThreadInvoke;
        public static IWaitDialog WaitDialog;
        public static bool IsMainThread
        {
            get
            {
                return Thread.CurrentThread == MainThread;
            }
        }
    }

    public class AsyncBase : IAsyncBase
    {
        bool m_completed;
        SimpleCallback m_simpleCallback;
        Thread m_simpleDestThread;
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

        public void OnFinish(SimpleCallback callback)
        {
            lock (this)
            {
                if (IsCompleted)
                {
                    callback();
                }
                else
                {
                    m_simpleDestThread = Thread.CurrentThread;
                    m_simpleCallback = callback;
                }
            }
        }

        #endregion

        protected void DoCallback(SimpleCallback callback, Thread dstthread)
        {
            if (dstthread == Async.MainThread)
            {
                Async.MainThreadInvoke(callback);
            }
            else
            {
                callback();
            }
        }

        protected virtual void PerformCallbacks()
        {
            if (m_simpleCallback != null) DoCallback(m_simpleCallback, m_simpleDestThread);
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
        Thread m_valueDestThread;

        #region IAsyncValue<T> Members

        public T Result
        {
            get
            {
                if (!IsCompleted) throw new AsyncException();
                return m_value;
            }
        }

        public void OnFinish(ValueCallback<T> callback)
        {
            lock (this)
            {
                if (IsCompleted)
                {
                    callback(m_value);
                }
                else
                {
                    m_valueDestThread = Thread.CurrentThread;
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
            if (m_valueCallback != null) DoCallback(DoValueCall, m_valueDestThread);
        }

        ///  call this when finished
        public void DispatchFinished(T value)
        {
            m_value = value;
            DoDispatchFinished();
        }
    }

}
