using System;

namespace FFXIV_Speedkill_Tracker
{
    public class FightData
    {
        private TimeSpan _duration;
        private int _totalDamageDoneToBoss;


        public FightData(TimeSpan duration, int totalDamageDoneToBoss) {
            _duration = duration;
            _totalDamageDoneToBoss = totalDamageDoneToBoss; 
        }

        
        public TimeSpan Duration {
            get => _duration;
            set
            {
                _duration = value;
            }
        }

        public int TotalDamageDoneToBoss {
            get => _totalDamageDoneToBoss;
            set
            {
                _totalDamageDoneToBoss = value;
            }
        }
    }
}
