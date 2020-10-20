using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;

namespace FFXIV_Speedkill_Tracker
{
    public class TrackerHP: TrackerData
    {
        private int bossTotalHP = -1;
        private int bossRemainingHP = -1;
        public TrackerHP(int bossTotalHP, int totalDamageDone, Boolean showSign = false, String checkPointEventString = "", String checkPointName = "") {
            _type = DataType.kDataTypeHP;
            Value = totalDamageDone;
            CheckPointName = checkPointName;

            if (!checkPointEventString.Equals(""))
            {
                CheckPointEventString = new Regex(checkPointEventString);
            }

            this.bossTotalHP = bossTotalHP;
            this.bossRemainingHP = bossTotalHP - totalDamageDone;
            ShowSign = showSign;
            TextColor = SetTextColor();
        }

        protected Color SetTextColor()
        {
            if (bossRemainingHP > 0)
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
            if (bossRemainingHP > 0) return "+";
            else if (bossRemainingHP == 0) return "";
            else return "";
        }

        public override string ToString()
        {
            double bossRemainingHPPercentage = Math.Round((((double)bossRemainingHP) / ((double)bossTotalHP)) * 100, 2);

            if (ShowSign)
            {
                return Sign() + bossRemainingHPPercentage.ToString() + "%";
            } 
            else
            {
                return bossRemainingHPPercentage.ToString() + "%";
            }
        }

        public int BossRemainingHP
        {
            get => bossRemainingHP;
            set
            {
                bossRemainingHP = value;
            }
        }

        public TrackerHP Difference(TrackerHP comparingLogTime)
        {
            return new TrackerHP(bossTotalHP, bossTotalHP - (comparingLogTime.Value - this.Value), true);
        }
    }
}
