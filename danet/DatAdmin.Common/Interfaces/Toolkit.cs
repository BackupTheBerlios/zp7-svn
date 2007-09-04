using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace DatAdmin
{
    public interface IWindowToolkit
    {
        void OpenSchemaWindow(IPhysicalConnection conn);
        void OpenTable(ITableSource conn);
    }
    public interface ILogToolkit
    {
        void LogMessage(string type, string message);
    }
    public static class Toolkit
    {
        public static IWindowToolkit WindowToolkit;
        public static ILogToolkit LogToolkit;
    }
}
