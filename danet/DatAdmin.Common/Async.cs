using System;
using System.Collections.Generic;
using System.Text;

namespace DAIntf
{
    public delegate void SimpleCallback();
    public delegate void ValueCallback<T>(T value);
    public delegate T ReturnValueCallback<T>();

    public class AsyncException : Exception
    {
    }

    public class WaitAbortException : Exception
    {
    }

    public interface IAsyncBase
    {
        /// wait to complete request
        void Wait();
        /// returns whether async call is completed
        bool IsCompleted { get;}
        /// returns whether is completed synchonously, true eg. when small IO reads
        bool CompletedSynchronously { get;}
        /// registers calls callback, when finished, 
        /// callback is sent to called thread, when possible
        /// (if it is GUI thread)
        void OnFinish(SimpleCallback callback);
    }

    public interface IAsyncVoid : IAsyncBase
    {
    }

    public interface IInvoker
    {
        void DoInvoke(SimpleCallback callback);
    }

    public interface IAsyncValue<T> : IAsyncBase
    {
        /// gets result of call, if not finished, throws AsyncException
        T Result { get;}
        void OnFinish(ValueCallback<T> callback);
    }

    public interface IWaitDialog
    {
        void Show();
        void Hide();
        bool Canceled { get;}
    }
}
