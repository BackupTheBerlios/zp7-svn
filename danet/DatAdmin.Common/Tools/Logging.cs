using System;
using System.Collections.Generic;
using System.Text;

namespace DAIntf
{
    public static class Logging
    {
        public static void Debug(string message)
        {
            Toolkit.LogToolkit.LogMessage("debug", message);
        }
        public static void Debug(string message, params object[] args)
        {
            Toolkit.LogToolkit.LogMessage("debug", String.Format(message, args));
        }

        public static void Info(string message)
        {
            Toolkit.LogToolkit.LogMessage("info", message);
        }
        public static void Info(string message, params object[] args)
        {
            Toolkit.LogToolkit.LogMessage("info", String.Format(message, args));
        }

        public static void Warning(string message)
        {
            Toolkit.LogToolkit.LogMessage("warning", message);
        }
        public static void Warning(string message, params object[] args)
        {
            Toolkit.LogToolkit.LogMessage("warning", String.Format(message, args));
        }

        public static void Error(string message)
        {
            Toolkit.LogToolkit.LogMessage("error", message);
        }
        public static void Error(string message, params object[] args)
        {
            Toolkit.LogToolkit.LogMessage("error", String.Format(message, args));
        }
    }
}
