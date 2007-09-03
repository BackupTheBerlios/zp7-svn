using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace DAIntf
{
    public enum ConnectionStatus { Closed, Open };

    public interface IConnectionHooks
    {
        void BeforeOpen();
        void AfterOpen();
        void BeforeClose();
        void AfterClose();
    }

    public class TableDataProperties
    {
    }

    public interface ICommonConnection
    {
        void Open();
        void Close();
        ConnectionStatus State { get;}
        IConnectionHooks Hooks { get;set;}

        DbConnection SystemConnection { get;}
        DbProviderFactory DbFactory { get;}
    }

    public interface IServerConnection : ICommonConnection
    {
        IEnumerable<string> Databases { get;}
        IDatabaseConnection GetDatabase(string name);
    }

    public interface IDatabaseConnection : ICommonConnection
    {
        IEnumerable<string> Tables { get;}
        ITableConnection GetTable(string name);
    }

    public interface ITableConnection : ICommonConnection
    {
        string TableName { get;}
        DataTable GetTabularData(TableDataProperties props);
    }

    public class HooksBase : IConnectionHooks
    {
        #region IConnectionHooks Members

        public virtual void BeforeOpen()
        {
        }

        public virtual void AfterOpen()
        {
        }

        public virtual void BeforeClose()
        {
        }

        public virtual void AfterClose()
        {
        }

        #endregion
    }
}
