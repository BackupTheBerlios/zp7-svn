using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace DatAdmin
{
    public class ConnectionException : Exception
    {
        public ConnectionException(string message)
            : base(message)
        {
        }
    }

    public delegate void PhysicalConnectionDelegate(IPhysicalConnection conn);
    //public enum ConnectionStatus { Closed, Open };

    //public interface IConnectionHooks
    //{
    //    void BeforeOpen();
    //    void AfterOpen();
    //    void BeforeClose();
    //    void AfterClose();
    //}

    public class TableDataProperties
    {
    }

    /// connection, should autamatically open/close/reopen connection, when needed
    public interface IPhysicalConnection
    {
        IAsyncVoid Open();
        IAsyncVoid Close();
        //ConnectionStatus State { get;}
        //public IInvoker EventsInvoker { get;set;}
        event PhysicalConnectionDelegate BeforeOpen;
        event PhysicalConnectionDelegate AfterOpen;
        event PhysicalConnectionDelegate BeforeClose;
        event PhysicalConnectionDelegate AfterClose;
        //IAsyncVoid InvokeVoid(SimpleCallback func);
        //IAsyncValue<T> InvokeValue(ReturnValueCallback<T> func);
        bool IsOpened { get;}
        IInvoker Invoker { get;}
        //IConnectionHooks Hooks { get;set;}

        DbConnection SystemConnection { get;}
        DbProviderFactory DbFactory { get;}
    }

    public interface ICommonSource
    {
        IPhysicalConnection Connection { get;}
    }

    public interface IServerSource : ICommonSource
    {
        IEnumerable<string> Databases { get;}
        IDatabaseSource GetDatabase(string name);
    }

    public interface IDatabaseSource : ICommonSource
    {
        IEnumerable<string> Tables { get;}
        ITableSource GetTable(string name);
    }

    public interface ITableSource : ICommonSource
    {
        string TableName { get;}
        DataTable GetTabularData(TableDataProperties props);
    }

    //public class HooksBase : IConnectionHooks
    //{
    //    #region IConnectionHooks Members

    //    public virtual void BeforeOpen()
    //    {
    //    }

    //    public virtual void AfterOpen()
    //    {
    //    }

    //    public virtual void BeforeClose()
    //    {
    //    }

    //    public virtual void AfterClose()
    //    {
    //    }

    //    #endregion
    //}
}
