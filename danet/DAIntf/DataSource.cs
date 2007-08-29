using System;
using System.Collections.Generic;
using System.Text;

namespace DAIntf
{
    public interface IServerConnection
    {
        IEnumerable<string> Databases { get;}
        IDatabaseConnection GetDatabase(string name);
    }

    public interface IDatabaseConnection
    {
        IEnumerable<string> Tables { get;}
        ITableConnection GetTable(string name);
    }

    public interface ITableConnection
    {

    }
}
