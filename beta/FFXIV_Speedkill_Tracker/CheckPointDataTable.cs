using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FFXIV_Speedkill_Tracker
{
    public class CheckPointDataTable
    {

        private int _count = 4;

        private String[] _worldRecordCheckPointTimes = { "01:27", "04:23", "08:51", "15:09" };

        private Regex[] _checkPointEventStrings = 
        {
            new Regex("순항추격기:코드네임 '블래스티'! 계차 폐쇄 우주를 위협하는 적 발견…… 격파하겠다!"),
            new Regex("알렉산더 프라임:나는 알렉산더…… 기계장치 신……. 이상향으로 가는 길을 인도하리니…… 나의 심판을 받으라……."),
            new Regex("신성한 심판 10초 전")
        };

        private String[] _checkPointNames =
        {
            "Phase 1 End",
            "Phase 2 End",
            "Phase 3 End",
            "Clear"
        };


        public CheckPointDataTable()
        {
            
        }

        public int Count
        {
            get => _count;
        }

        public String[] WorldRecordCheckPointTimes
        {
            get => _worldRecordCheckPointTimes;
        }

        public Regex[] CheckPointEventStrings
        {
            get => _checkPointEventStrings;
        }


        public String[] CheckPointNames
        {
            get => _checkPointNames;
        }
    }
}
