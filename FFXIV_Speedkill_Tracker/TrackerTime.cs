using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FFXIV_Speedkill_Tracker
{
    public class TrackerTime: TrackerData
    {
        public TrackerTime(int duration, Boolean showSign, String checkPointEventString = "", String checkPointName = "")
        {
            _type = DataType.kDataTypeTime;

            CheckPointName = checkPointName;
            if (!checkPointEventString.Equals(""))
            {
                CheckPointEventString = new Regex(checkPointEventString);
            }

            Value = duration;
            ShowSign = showSign;
            TextColor = SetTextColor();
        }

        public TrackerTime(String durationmmss, Boolean showSign=false, String checkPointEventString = "", String checkPointName = "")
        {
            Value = this.timeStringToSeconds(durationmmss);
            CheckPointName = checkPointName;
            CheckPointEventString = new Regex(checkPointEventString);
            ShowSign = showSign;
            TextColor = SetTextColor();
        }

        private int timeStringToSeconds(String durationmmss)
        {
            String[] timeParsed = durationmmss.Split(':');

            int minInSecond = Int32.Parse(timeParsed[0]) * MINUTE_IN_SECONDS;
            int second = Int32.Parse(timeParsed[1]);

            return minInSecond + second;
        }

        protected Color SetTextColor()
        {
            if (Value > 0)
            {
                return Color.Red;
            }
            else if (Value == 0)
            {
                return Color.Black;
            }
            else
            {
                return Color.Green;
            }
        }

        protected String Sign()
        {
            if (Value > 0) return "+";
            else if (Value == 0) return "";
            else return "-";
        }
        override public String ToString()
        {
            if (ShowSign)
            {
                return Sign() + TimeFormatter.Format(Value);
            }
            else
            {
                return TimeFormatter.Format(Value);
            }
        }


        public TrackerTime Difference(TrackerTime comparingLogTime)
        {
            return new TrackerTime(this.Value - comparingLogTime.Value, true);
        }
    }
}
