using System;
using System.Collections.Generic;
using System.Text;

namespace Wallace.Core.Helpers.Format {
    public static class DateTimeHelper {

        public static long ToUnix(this DateTime date)
           => (long)Math.Round((date.ToUniversalTime() - BeginOffset()).TotalSeconds);

        public static DateTime BeginPoint()
            => new DateTime(1970, 1, 1, 0, 0, 0);

        public static DateTimeOffset BeginOffset()
           => new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

    }
}
