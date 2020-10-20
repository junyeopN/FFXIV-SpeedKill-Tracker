using System;

namespace FFXIV_Speedkill_Tracker
{
    static class DifferenceCalculator
    {
        public static TrackerTime CalculateCurrentRunWorldRecordRunClearTimeDifference(FightData fightData, CheckPointDataTable checkPointDataTable)
        {
            TrackerTime currentRunClearTime = new TrackerTime(TimeFormatter.Format(fightData.Duration), false);
            TrackerTime worldRecordClearTime = checkPointDataTable.WorldRecordClearTime;

            return currentRunClearTime.Difference(worldRecordClearTime);
        }
        public static TrackerTime CalculateCurrentRunWorldRecordRunCheckPointTimeDifference(FightData fightData, CheckPointDataTable checkPointDataTable, int currentPhase)
        {
            TrackerTime currentRunTrackerTime = new TrackerTime(TimeFormatter.Format(fightData.Duration), false);
            TrackerTime worldRecordRunTrackerTime = (TrackerTime)checkPointDataTable.CheckPointDatas[currentPhase];

            return currentRunTrackerTime.Difference(worldRecordRunTrackerTime);
        }

        public static TrackerHP CalculateCurrentRunWorldRecordRunCheckPointHPDifference(FightData fightData, CheckPointDataTable checkPointDataTable, int currentPhase)
        {
            int bossTotalHP = checkPointDataTable.BossTotalHP;

            TrackerHP currentRunCheckPointHP = new TrackerHP(bossTotalHP, fightData.TotalDamageDoneToBoss, false);
            TrackerHP worldRecordCheckPointHP = (TrackerHP)checkPointDataTable.CheckPointDatas[currentPhase];

            return currentRunCheckPointHP.Difference(worldRecordCheckPointHP);      
        }
    }
}
