using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DatAdmin
{
    public class ControlInvoker : IInvoker
    {
        Control m_ctrl;
        public ControlInvoker(Control ctrl)
        {
            m_ctrl = ctrl;
        }
        #region IInvoker Members

        public IAsyncVoid InvokeVoid(SimpleCallback callback)
        {
            return new AsyncVoidResult(m_ctrl.BeginInvoke(callback));
        }

        public IAsyncValue<T> InvokeValue<T>(ReturnValueCallback<T> callback)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    public class AsyncVoidResult : IAsyncVoid
    {
        IAsyncResult m_result;
        public AsyncVoidResult(IAsyncResult result)
        {
            m_result = result;
        }
        #region IAsyncBase Members

        public void Wait()
        {
            m_result.AsyncWaitHandle.WaitOne();
        }

        public bool IsCompleted
        {
            get { return m_result.IsCompleted; }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public void OnFinish(SimpleCallback callback, IInvoker invoker)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
