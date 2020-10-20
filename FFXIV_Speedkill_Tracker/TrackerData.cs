using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;

namespace FFXIV_Speedkill_Tracker
{
    abstract public class TrackerData
    {
        protected readonly int MINUTE_IN_SECONDS = 60;

        protected string _checkPointName;
        protected Regex _checkPointEventString;
        protected Color _textColor = Color.Black;

        protected DataType _type;
        private int _value = -1;
        private Boolean _showSign = false;

        public Boolean ShowSign
        {
            get => _showSign;
            set
            {
                _showSign = value;
            }
        }

        public Color TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
            }
        }
        public String CheckPointName
        {
            get => _checkPointName;
            set
            {
                _checkPointName = value;
            }
        }

        public Regex CheckPointEventString
        {
            get => _checkPointEventString;
            set
            {
                _checkPointEventString = value;
            }
        }
    }
}
