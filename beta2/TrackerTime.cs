using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIV_Speedkill_Tracker
{
    public class TrackerTime
    {
        private readonly int MINUTE_IN_SECONDS = 60;

        private int _duration = -1;
        private Color _color = Color.Black;

        public TrackerTime(int duration)
        {
            this._duration = duration;
            _color = SetColor(duration);
        }
        public TrackerTime(String durationmmss)
        {
            this._duration = this.timeStringToSeconds(durationmmss);
        }

        private int timeStringToSeconds(String durationmmss)
        {
            String[] timeParsed = durationmmss.Split(':');

            int minInSecond = Int32.Parse(timeParsed[0]) * MINUTE_IN_SECONDS;
            int second = Int32.Parse(timeParsed[1]);

            return minInSecond + second;
        }
        private Color SetColor(int duration)
        {
            if (duration > 0)
            {
                return Color.Red;
            }
            else if (duration == 0)
            {
                return Color.Black;
            }
            else
            {
                return Color.Green;
            }
        }

        override public String ToString()
        {
            return Sign(_duration) + TimeFormatter.Format(_duration);
        }

        private String Sign(int seconds)
        {
            if (seconds > 0) return "+";
            else if (seconds == 0) return "";
            else return "-";
        }

        public int Duration
        {
            get => _duration;
            set
            {
                _duration = value;
            }
        }
        public Color Color
        {
            get => _color;
        }


        public TrackerTime TimeDifference(TrackerTime comparingLogTime)
        {
            return new TrackerTime(this.Duration - comparingLogTime.Duration);
        }
    }
}
