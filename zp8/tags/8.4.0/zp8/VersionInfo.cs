using System;
using System.Collections.Generic;
using System.Text;

namespace zp8
{
    public static class VersionInfo
    {
        public static string BUILT_AT = "#BUILT_AT#";
        public static string SVN_REVISION = "#SVN_REVISION#";
        public static string VERSION = "#VERSION#";
        //public static string VERSION = "3.5.1";

        public static bool IsDevVersion { get { return VERSION.StartsWith("#"); } }

        public static string VersionTypeName
        {
            get
            {
                try
                {
                    var ar = VERSION.Split('.');
                    if (ar.Length == 3)
                    {
                        if (Int32.Parse(ar[1]) % 2 == 0) return "";
                        else return "BETA";
                    }
                    if (ar.Length == 4)
                    {
                        if (Int32.Parse(ar[1]) % 2 == 0) return "GAMMA";
                        else return "ALPHA";
                    }
                }
                catch { }
                return null;
            }
        }

        public static bool IsSnapshot { get { return VERSION.Split('.').Length == 4; } }
        public static bool IsRelease
        {
            get { return VersionTypeName == ""; }
        }
        public static bool IsBeta
        {
            get { return VersionTypeName == "BETA"; }
        }
        public static bool IsGamma
        {
            get { return VersionTypeName == "GAMMA"; }
        }
        public static bool IsAlpha
        {
            get { return VersionTypeName == "ALPHA"; }
        }
    }
}
