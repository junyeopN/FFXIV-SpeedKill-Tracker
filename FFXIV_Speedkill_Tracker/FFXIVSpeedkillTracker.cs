using Advanced_Combat_Tracker;
using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;

namespace FFXIV_Speedkill_Tracker
{
    public class FFXIVSpeedkillTracker : IActPluginV1
    {
        private static readonly string RAID_ZONE_TITLE_PREFIX = "Current Raid: ";

        private static readonly Point TRACKER_PAGE_SIZE_SETTER_INITIAL_LOCATION = new Point(300, 100);
        private static readonly Size TRACKER_PAGE_SIZE_SETTER_INITIAL_SIZE = new Size(550, 360);

        private static readonly Point SPEED_RUN_TRACKER_TABLE_SIZE_SETTER_INITIAL_LOCATION = new Point(300, 250);
        private static readonly Size SPEED_RUN_TRACKER_TABLE_SIZE_SETTER_INITIAL_SIZE = new Size(520, 340);

        public SizeSetter trackerPageSizeSetter = null;
        public SizeSetter speedRunTrackerTableSizeSetter = null;

        public TrackerPage trackerPage = null;
        private Boolean trackerOpen = false;

        private StartTrackerButton startTrackerButton = null;
        private RaidZoneTitleTextBox raidZoneTitleTextBox = null;

        private EncounterData currentFightData = null;

        private FightDataFactory fightDataFactory = null;
        private FightData fightData = null;

        private SpeedRunTrackerTable speedRunTrackerTable;
        private CheckPointDataTable checkPointDataTable = null;


        private int currentPhase = 0;


        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            InitStartTrackerButton();

            trackerPageSizeSetter = new SizeSetter("Tracker Window Size: ", TRACKER_PAGE_SIZE_SETTER_INITIAL_LOCATION, TRACKER_PAGE_SIZE_SETTER_INITIAL_SIZE);
            speedRunTrackerTableSizeSetter = new SizeSetter("Table Size: ", SPEED_RUN_TRACKER_TABLE_SIZE_SETTER_INITIAL_LOCATION, SPEED_RUN_TRACKER_TABLE_SIZE_SETTER_INITIAL_SIZE);

            pluginScreenSpace.Controls.Add(startTrackerButton);
            pluginScreenSpace.Controls.Add(trackerPageSizeSetter);
            pluginScreenSpace.Controls.Add(speedRunTrackerTableSizeSetter);

            AddEventHandlers();
        }

        private void InitStartTrackerButton()
        {
            startTrackerButton = new StartTrackerButton();
            startTrackerButton.Click += new EventHandler(StartTrackerButtonEvent);
        }

        private void StartTrackerButtonEvent(Object sender, EventArgs e)
        {
            if (!trackerOpen)
            {
                InitTracker();
            }
            else
            {
                DeInitTracker();
            }

            startTrackerButton.ChangeText();

            trackerOpen = !trackerOpen;
        }


        void InitTracker()
        {
            trackerPage = new TrackerPage(trackerPageSizeSetter.GetWidth(), trackerPageSizeSetter.GetHeight());

            AddRaidZoneTitleTextBox();
            AddSpeedRunTrackerTable();

            trackerPage.Show();
        }

        private void AddRaidZoneTitleTextBox()
        {
            raidZoneTitleTextBox = new RaidZoneTitleTextBox();

            trackerPage.Controls.Add(raidZoneTitleTextBox);
        }

        private void AddSpeedRunTrackerTable()
        {
            speedRunTrackerTable = new SpeedRunTrackerTable(speedRunTrackerTableSizeSetter.GetWidth(), speedRunTrackerTableSizeSetter.GetHeight());

            trackerPage.Controls.Add(speedRunTrackerTable);
        }


        private void DeInitTracker()
        {
            trackerPage.Hide();
            trackerPage = null;
        }


        private void AddEventHandlers()
        {
            ActGlobals.oFormActMain.OnCombatStart += new CombatToggleEventDelegate(SpeedKillTrackerOnCombatStartEventHandler);
            ActGlobals.oFormActMain.OnCombatEnd += new CombatToggleEventDelegate(SpeedKillTrackerOnCombatEndEventHandler);
            ActGlobals.oFormActMain.OnLogLineRead += new LogLineEventDelegate(SpeedKillTrackerOnLogLineReadEventHandler);
        }


        private void SpeedKillTrackerOnCombatStartEventHandler(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            try
            {
                speedRunTrackerTable.Reset();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            currentFightData = encounterInfo.encounter;

            try
            {
                LoadFightCheckpoints();
                fightDataFactory = new FightDataFactory(ActGlobals.oFormActMain.CurrentZone, checkPointDataTable.PhaseCount);

                raidZoneTitleTextBox.Text = RAID_ZONE_TITLE_PREFIX + ActGlobals.oFormActMain.CurrentZone;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void LoadFightCheckpoints()
        {
            String zoneName = ActGlobals.oFormActMain.CurrentZone;
           
            checkPointDataTable = CheckPointDataTableFactory.CreateCheckPointDataTable(zoneName);
            speedRunTrackerTable.UpdateCheckPointDataCells(checkPointDataTable);
        }


        private void SpeedKillTrackerOnCombatEndEventHandler(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            try
            {
                if (IsCleared())
                {
                    TrackerTime currentRunWorldRecordRunClearTimeDifference = DifferenceCalculator.CalculateCurrentRunWorldRecordRunClearTimeDifference(fightData, checkPointDataTable);
                    speedRunTrackerTable.UpdateCurrentRunWorldRecordDifference(currentPhase, currentRunWorldRecordRunClearTimeDifference);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("check");
            }

            currentFightData = null;
            currentPhase = 0;

            fightDataFactory = null;
            fightData = null;
        }

        private Boolean IsCleared()
        {
            return (currentPhase >= checkPointDataTable.PhaseCount - 1);
        }


        private void SpeedKillTrackerOnLogLineReadEventHandler(bool isImport, LogLineEventArgs logInfo)
        {
            try
            {
                String logLine = logInfo.logLine;

                if (currentFightData != null)
                {
                    fightData = fightDataFactory.CreateCurrentRunFightData(currentFightData, currentPhase);
                }

                UpdateCurrentRunDataToTracker();

                if (CheckPointDetected(logLine))
                {
                    UpdateCurrentRunWorldRecordRunDifferenceToTracker();
                    currentPhase++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void UpdateCurrentRunDataToTracker()
        {
            if (checkPointDataTable.CheckPointTypes[currentPhase] == DataType.kDataTypeTime)
            {
                speedRunTrackerTable.UpdateCurrentRunData(currentPhase, fightData.Duration);
            }
            else
            {
                int bossTotalHP = checkPointDataTable.BossTotalHP;
                speedRunTrackerTable.UpdateCurrentRunData(currentPhase, bossTotalHP, fightData.TotalDamageDoneToBoss);
            }
        }


        private Boolean CheckPointDetected(String logLine)
        {
            TrackerData checkPointData = checkPointDataTable.CheckPointDatas[currentPhase];
            return checkPointData.CheckPointEventString.IsMatch(logLine);
        }

        private void UpdateCurrentRunWorldRecordRunDifferenceToTracker()
        {
            TrackerData checkPointData = checkPointDataTable.CheckPointDatas[currentPhase];

            if (checkPointDataTable.CheckPointTypes[currentPhase] == DataType.kDataTypeTime)
            {
                TrackerTime currentRunWorldRecordRunCheckPointTimeDifference = DifferenceCalculator.CalculateCurrentRunWorldRecordRunCheckPointTimeDifference(fightData, checkPointDataTable, currentPhase);
                speedRunTrackerTable.UpdateCurrentRunWorldRecordDifference(currentPhase, currentRunWorldRecordRunCheckPointTimeDifference);
            }
            else
            {
                TrackerHP currentRunWorldRecordRunCheckPointHPDifference = DifferenceCalculator.CalculateCurrentRunWorldRecordRunCheckPointHPDifference(fightData, checkPointDataTable, currentPhase);
                speedRunTrackerTable.UpdateCurrentRunWorldRecordDifference(currentPhase, currentRunWorldRecordRunCheckPointHPDifference);
            }
        }


        public void DeInitPlugin()
        {
            trackerPage = null;
            currentFightData = null;

            fightDataFactory = null;
            fightData = null;

            checkPointDataTable = null;
            speedRunTrackerTable = null;
        }
    }
}
