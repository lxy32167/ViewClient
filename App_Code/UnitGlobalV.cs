using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.App_Code
{
    public static class UnitGlobalV
    {
     
        public const int WindowCount = 5;

        static public DateTime javaTime = new DateTime(1970, 1, 1);
        static public DateTime delphiTime = new DateTime(1899, 12, 30);
        static public TimeSpan ts = javaTime - delphiTime;
    }
}