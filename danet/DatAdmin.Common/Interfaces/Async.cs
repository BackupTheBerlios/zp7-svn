using System;
using System.Collections.Generic;
using System.Text;

namespace DatAdmin
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
        /// callback is sent to invoker
        void OnFinish(SimpleCallback callback, IInvoker invoker);
    }

    public interface IAsyncVoid : IAsyncBase
    {
    }

    public interface IInvoker
    {
        IAsyncVoid InvokeVoid(SimpleCallback callback);
        IAsyncValue<T> InvokeValue<T>(ReturnValueCallback<T> callback);
        //void DoInvoke(SimpleCallback callback);
    }

    public interface IAsyncValue<T> : IAsyncBase
    {
        /// gets result of call, if not finished, throws AsyncException
        T Result { get;}
        void OnFinish(ValueCallback<T> callback, IInvoker invoker);
    }

    public interface IWaitDialog
    {
        void Show();
        void Hide();
        bool Canceled { get;}
    }
}
