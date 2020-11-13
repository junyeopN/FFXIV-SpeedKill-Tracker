using Advanced_Combat_Tracker;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace FFXIV_Speedkill_Tracker
{
    public class FFXIVSpeedkillTracker : IActPluginV1
    {
        public readonly String START_TRACKER_TEXT = "Start Tracker";
        public readonly String END_TRACKER_TEXT = "Close Tracker";


        private Form trackerPage = null;
        private Boolean trackerOpen = false;

        private Button startTrackerButton = null;

        private EncounterData currentFightData = null;

        private TimeSpan duration = new TimeSpan(0);

        private RunTimeTrackerTable runTimeTrackerTable;
        private readonly CheckPointDataTable checkPointDataTable = new CheckPointDataTable();

        int currentPhase = 0;



        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            InitStartTrackerButton();

            pluginScreenSpace.Controls.Add(startTrackerButton);

            AddEventHandlers(); 
        }

        private void AddEventHandlers()
        {
            ActGlobals.oFormActMain.OnCombatStart += new CombatToggleEventDelegate(SpeedKillTrackerOnCombatStartEventHandler);
            ActGlobals.oFormActMain.OnCombatEnd += new CombatToggleEventDelegate(SpeedKillTrackerOnCombatEndEventHandler);
            ActGlobals.oFormActMain.OnLogLineRead += new LogLineEventDelegate(SpeedKillTrackerOnLogLineReadEventHandler);
        }


        private void InitStartTrackerButton()
        {
            startTrackerButton = new Button();
            startTrackerButton.Text = START_TRACKER_TEXT;
            startTrackerButton.Click += new EventHandler(StartTrackerButtonEvent);
        }

        private void StartTrackerButtonEvent(Object sender, EventArgs e)
        {
            if(!trackerOpen)
            {
                InitTracker();

                startTrackerButton.Text = END_TRACKER_TEXT;
            }
            else
            {
                DeInitTracker();

                startTrackerButton.Text = START_TRACKER_TEXT;
            }


            trackerOpen = !trackerOpen;
        }

        void InitTracker()
        {
            trackerPage = new Form();


            trackerPage.TopMost = true;
            trackerPage.ControlBox = false;
            trackerPage.Text = "   ";

            trackerPage.Size = new Size(520, 150);
            trackerPage.Location = new Point(0, 0);

            runTimeTrackerTable = new RunTimeTrackerTable(checkPointDataTable);

            trackerPage.Controls.Add(runTimeTrackerTable);


            trackerPage.Show();
        }


        void DeInitTracker()
        {
            trackerPage.Hide();
            trackerPage = null;
        }


        void SpeedKillTrackerOnCombatStartEventHandler(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            currentFightData = encounterInfo.encounter;
        }

        void SpeedKillTrackerOnCombatEndEventHandler(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            currentFightData = null;
            currentPhase = 0;

            runTimeTrackerTable.Reset();          
        }

        void SpeedKillTrackerOnLogLineReadEventHandler(bool isImport, LogLineEventArgs logInfo)
        {
            try
            {
                String logLine = logInfo.logLine;

                if (currentFightData != null)
                {
                    duration = currentFightData.Duration;
                }


                runTimeTrackerTable.UpdateCurrentRunTime(currentPhase, duration);

                if (CheckPointDetected(logLine))
                { 
                
                    runTimeTrackerTable.UpdateCurrentRunWorldRecordCheckPointTimeDifference(currentPhase, CalculateCurrentRunWorldRecordRunTimeDifference());
                    currentPhase++;
                }

            } catch (Exception e)
            {
                Console.WriteLine("Error");
            }
        }

        private Boolean CheckPointDetected(String logLine)
        {
            return checkPointDataTable.CheckPointEventStrings[currentPhase].IsMatch(logLine);
        }

        private TrackerTime CalculateCurrentRunWorldRecordRunTimeDifference()
        {
            TrackerTime currentRunTrackerTime = new TrackerTime(TimeFormatter.Format(duration));
            TrackerTime worldRecordRunTrackerTime = new TrackerTime(checkPointDataTable.WorldRecordCheckPointTimes[currentPhase]);

            return currentRunTrackerTime.TimeDifference(worldRecordRunTrackerTime);
        }

        public void DeInitPlugin()
        {
            this.trackerPage = null;
            this.currentFightData = null;

            duration = new TimeSpan(0);
        }
    }
}
