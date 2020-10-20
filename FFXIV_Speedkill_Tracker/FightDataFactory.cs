using Advanced_Combat_Tracker;
using System;
using System.IO;
using System.Collections.Generic;

namespace FFXIV_Speedkill_Tracker
{
    public class FightDataFactory: ZoneFileReader 
    {
        static private readonly String ZONE_BOSS_FILE_DIRECTORY = DLL_FOLDER_DIRECTORY + "\\Plugins\\FFXIV_Speedkill_Tracker\\FFXIV_Speedkill_Tracker\\ZoneBoss.txt";

        private List<String> _bossNameList = null;
        private List<List<String>> _bossListOfEachPhase = null;
        private Dictionary<String, int> _damageDoneToEachBoss = null;
        private int currentPhase = -1;

        public FightDataFactory(String zoneName, int totalPhaseCount) {
            StreamReader zoneBossTextFileReader = new StreamReader(ZONE_BOSS_FILE_DIRECTORY);
            _bossNameList = new List<String>();
            _bossListOfEachPhase = new List<List<String>>();
            _damageDoneToEachBoss = new Dictionary<String, int>();

            if (FindCurrentZone(zoneName, zoneBossTextFileReader)) {
                AddBossNames(zoneBossTextFileReader);
                AddBossOfEachPhaseNames(zoneBossTextFileReader, totalPhaseCount);
            }

            InitDamageDoneToEachBossTable();
        }

        private void AddBossNames(StreamReader zoneBossTextFileReader) 
        {
            int total_boss_count = Int32.Parse(zoneBossTextFileReader.ReadLine());

            for (int i = 0; i < total_boss_count; i++) {
                String bossName = zoneBossTextFileReader.ReadLine();
                _bossNameList.Add(bossName);               
            }
        }
        private void InitDamageDoneToEachBossTable()
        {
            foreach (String name in _bossNameList)
            {
                _damageDoneToEachBoss.Add(name, 0);
            }
        }

        private void AddBossOfEachPhaseNames(StreamReader zoneBossTextFileReader, int totalPhaseCount)
        {
            for (int i = 0; i < totalPhaseCount - 1; i++)
            {
                List<String> phaseBossList = new List<String>();
                int bossCount = Int32.Parse(zoneBossTextFileReader.ReadLine());

                for(int j = 0; j < bossCount; j++)
                {
                    phaseBossList.Add(zoneBossTextFileReader.ReadLine());
                }

                _bossListOfEachPhase.Add(phaseBossList);
            }
        }

        public FightData CreateCurrentRunFightData(EncounterData currentFightData, int phase) 
        {
            TimeSpan fightDuration = currentFightData.Duration;
            int totalDamageDoneToBossInCurrentPhase = 0;

            foreach(String bossName in _bossListOfEachPhase[phase])
            {
                int damageDoneToBoss = (int)currentFightData.GetCombatant(bossName).DamageTaken;

                totalDamageDoneToBossInCurrentPhase += damageDoneToBoss;
                _damageDoneToEachBoss[bossName] = damageDoneToBoss;
            }

            if (currentPhase != phase)
            {
                currentPhase++;
            }

            return (new FightData(fightDuration, GetTotalDamageDoneToBoss(currentPhase))); 
        }

        private int GetTotalDamageDoneToBoss(int currentPhase) 
        {
            int totalDamageDoneToBoss = 0;

            foreach(String bossName in _bossListOfEachPhase[currentPhase]) {
                totalDamageDoneToBoss += _damageDoneToEachBoss[bossName];
            }

            return totalDamageDoneToBoss;
        }


        public List<String> BossNameList {
            get => _bossNameList;
            set 
            {
                _bossNameList = value;
            }
        }
    }
}
