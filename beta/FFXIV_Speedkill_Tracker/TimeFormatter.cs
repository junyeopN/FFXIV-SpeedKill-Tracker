using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIV_Speedkill_Tracker
{
    public class TimeFormatter
    {
        public static readonly int NUMBER_OF_DIGITS = 10;
        private static String PadZero(int time)
        {
            if (time < NUMBER_OF_DIGITS)
            {
                return "0" + time.ToString();
            }
            else
            {
                return time.ToString();
            }
        }

        public static String Format(TimeSpan duration)
        {
            return PadZero(duration.Minutes) + ":" + PadZero(duration.Seconds);
        }

        public static String Format(int seconds)
        {
            return PadZero(Math.Abs(seconds) / 60) + ":" + PadZero(Math.Abs(seconds) % 60);       
        }
    }
}
