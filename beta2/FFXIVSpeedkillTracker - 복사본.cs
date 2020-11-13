using Advanced_Combat_Tracker;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;

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

        int checkpointCount = 4;

        private DataGridView checkpointTable;

        int currentPhase = 0;

        String[] wrCheckpoints = { "01:27", "04:23", "08:51", "05:10" };

        Regex[] checkPointLine = {
                                    new Regex("순항추격기:코드네임 '블래스티'! 계차 폐쇄 우주를 위협하는 적 발견…… 격파하겠다!"),
                                    new Regex("알렉산더 프라임:나는 알렉산더…… 기계장치 신……. 이상향으로 가는 길을 인도하리니…… 나의 심판을 받으라……."),
                                    new Regex("신성한 심판 10초 전")
                                 };


        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            initStartTrackerButton();

            pluginScreenSpace.Controls.Add(startTrackerButton);


            ActGlobals.oFormActMain.OnCombatStart += new CombatToggleEventDelegate(oFormActMain_OnCombatStart);
            ActGlobals.oFormActMain.OnCombatEnd += new CombatToggleEventDelegate(oFormActMain_OnCombatEnd);
            ActGlobals.oFormActMain.OnLogLineRead += new LogLineEventDelegate(oFormActMain_OnLogLineRead);
            
        }


        void initStartTrackerButton()
        {
            startTrackerButton = new Button();
            startTrackerButton.Text = START_TRACKER_TEXT;
            startTrackerButton.Click += new EventHandler(startTrackerButtonEvent);
        }

        void startTrackerButtonEvent(Object sender, EventArgs e)
        {
            if(trackerOpen)
            {
                deInitTracker();

                startTrackerButton.Text = START_TRACKER_TEXT;
            }
            else
            {
                initTracker();

                startTrackerButton.Text = END_TRACKER_TEXT;
            }


            trackerOpen = !trackerOpen;
        }

        void initTracker()
        {
            trackerPage = new Form();


            trackerPage.TopMost = true;
            trackerPage.ControlBox = false;
            trackerPage.Text = "   ";

            trackerPage.Size = new Size(520, 150);
            trackerPage.Location = new Point(0, 0);

            checkpointTable = new DataGridView();
            initCheckPointTable();

            List<String> checkpoints = InterfaceFactory.createInterface("");


            for(int i = 0; i < checkpoints.Count; i++)
            {
                checkpointTable.Rows.Add(checkpoints[i]);
                checkpointTable.Rows[i].Cells[2].Value = wrCheckpoints[i];
            }


            trackerPage.Controls.Add(checkpointTable);


            trackerPage.Show();
        }

        void initCheckPointTable()
        {
            checkpointTable.ColumnCount = 4;
            checkpointTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            checkpointTable.Columns[0].Name = "Checkpoint";
            checkpointTable.Columns[1].Name = "Current";
            checkpointTable.Columns[2].Name = "WR";
            checkpointTable.Columns[3].Name = "Diff";

            checkpointTable.Location = new Point(0, 0);
            checkpointTable.Size = new Size(520, 150);
        }

        void deInitTracker()
        {
            trackerPage.Hide();
            trackerPage = null;
        }


        void oFormActMain_OnCombatStart(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            currentFightData = encounterInfo.encounter;

        }

        void oFormActMain_OnCombatEnd(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            currentFightData = null;
            currentPhase = 0;

            for(int i = 0; i < checkpointCount; i++)
            {
                checkpointTable.Rows[i].Cells[1].Value = "";
                checkpointTable.Rows[i].Cells[3].Value = "";
            }
        }

        void oFormActMain_OnLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {
            try
            {
                if (currentFightData != null)
                {
                    duration = currentFightData.Duration;
                }

            
                checkpointTable.Rows[currentPhase].Cells[1].Value = format(duration);



                String logLine = logInfo.logLine;


                if (checkPointLine[currentPhase].IsMatch(logLine))
                {
                    int diff = (timeStringToSeconds(format(duration)) - timeStringToSeconds(wrCheckpoints[currentPhase]));
                    Color color = Color.Black;

                    if (diff > 0)
                    {
                        color = Color.Red;
                    }
                    else if (diff == 0)
                    {
                        color = Color.Black;
                    }
                    else
                    {
                        color = Color.Green;
                    }

                    checkpointTable.Rows[currentPhase].Cells[3].Style = new DataGridViewCellStyle { ForeColor = color };
                    checkpointTable.Rows[currentPhase].Cells[3].Value = sign(diff) + format(diff);

                    currentPhase++;
                }
            } catch (Exception e)
            {
                Console.WriteLine("Error");
            }
        }

        String sign(int seconds)
        {
            if (seconds > 0) return "+";
            else if (seconds == 0) return "";
            else return "-";
        }

        String format(TimeSpan duration)
        {
            return padZero(duration.Minutes) + ":" + padZero(duration.Seconds);
        }

        String format(int seconds)
        {
            if (seconds > 0)
            {
                return padZero(seconds / 60) + ":" + padZero(seconds % 60);
            }
            else
            {
                return padZero(-seconds / 60) + ":" + padZero(-seconds % 60);
            }
        }

        String padZero(int time)
        {
            if(time < 10)
            {
                return "0" + time.ToString();
            }
            else
            {
                return time.ToString();
            }
        }


        int timeStringToSeconds(String time)
        {
            String[] timeParsed = time.Split(':');
            int minInSecond = Int32.Parse(timeParsed[0]) * 60;
            int second = Int32.Parse(timeParsed[1]);

            return minInSecond + second;
        }


        public void DeInitPlugin()
        {
            this.trackerPage = null;
            this.currentFightData = null;

            duration = new TimeSpan(0);
        }
    }
}
