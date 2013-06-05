using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoStepsAuthenticator
{
    // http://www.rudyhuyn.com/blog/2012/04/02/convertir-un-temps-unix-vers-un-datetime-et-inversement/
    internal static class UnixTimeHelper
    {
        internal static DateTime ToDateTime(this long unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime).ToLocalTime();
        }

        internal static long ToUnixTime(this DateTime datetime)
        {
            TimeSpan ts = (datetime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return (long)ts.TotalSeconds;
        }
    }
}
