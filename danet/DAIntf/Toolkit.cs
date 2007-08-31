using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace DAIntf
{
    public interface IWindowToolkit
    {
        void OpenSchemaWindow(DbConnection conn);
    }
    public static class Toolkit
    {
        public static IWindowToolkit WindowToolkit;
    }
}
