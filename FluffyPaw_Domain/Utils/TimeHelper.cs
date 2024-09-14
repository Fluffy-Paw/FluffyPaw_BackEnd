using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Utils
{
    public static class TimeHelper
    {
        public static DateTimeOffset ConvertToUtcPlus7(DateTimeOffset dateTimeOffset)
        {
            // UTC+7 is 7 hours ahead of UTC
            TimeSpan utcPlus7Offset = new(7, 0, 0);
            return dateTimeOffset.ToOffset(utcPlus7Offset);
        }

        public static DateTimeOffset ConvertToUtcPlus7NotChanges(DateTimeOffset dateTimeOffset)
        {
            // UTC+7 is 7 hours ahead of UTC
            TimeSpan utcPlus7Offset = new(7, 0, 0);
            return dateTimeOffset.ToOffset(utcPlus7Offset).AddHours(-7);
        }
    }
}
