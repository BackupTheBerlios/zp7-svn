using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace DAIntf
{
    public enum ConnectionStatus { Closed, Open };

    public interface ICommonConnection
    {
        void Open();
        void Close();
        ConnectionStatus State { get;}
        DbConnection SystemConnection { get;}
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

    }
}
