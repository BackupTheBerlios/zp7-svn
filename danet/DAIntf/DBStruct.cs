using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DAIntf
{
    public interface IDBStruct
    {
    }

    public interface ITableStruct
    {
        string Name { get;}
        IColumnStruct[] Columns { get;}
    }

    public interface IColumnStruct
    {
        string Name { get;}
    }
}
