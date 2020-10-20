using System;
using System.Windows.Forms;
using System.Drawing;

namespace FFXIV_Speedkill_Tracker
{
    public class SpeedRunTrackerTable : DataGridView
    {
        public static readonly int X = 0;
        public static readonly int Y = 20;

        public readonly int CHECKPOINT_NAME_COLUMN = 0;
        public readonly int CURRENT_RUN_COLUMN = 1;
        public readonly int WORLD_RECORD_COLUMN = 2;
        public readonly int TIME_DIFFERENCE_COLUMN = 3;

        public readonly int MAX_COLUMN_COUNT = 10;


        public SpeedRunTrackerTable(int width, int height)
        {
            ColumnCount = MAX_COLUMN_COUNT;
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            Size = new Size(width, height);
            Location = new Point(X, Y);

            UpdateTitles();
            AddRows();
        }


        private void UpdateTitles()
        {
            Columns[CHECKPOINT_NAME_COLUMN].Name = "Checkpoint";
            Columns[CURRENT_RUN_COLUMN].Name = "Current";
            Columns[WORLD_RECORD_COLUMN].Name = "WR";
            Columns[TIME_DIFFERENCE_COLUMN].Name = "Diff";
        }

        private void AddRows()
        {
            for (int i = 0; i < MAX_COLUMN_COUNT; i++)
            {
                Rows.Add();
            }
        }

        public void UpdateCheckPointDataCells(CheckPointDataTable checkPointDataTable)
        {
            for (int i = 0; i < checkPointDataTable.PhaseCount - 1; i++)
            {
                if(checkPointDataTable.CheckPointTypes[i] == DataType.kDataTypeTime)
                {
                    TrackerTime checkPointData = (TrackerTime)checkPointDataTable.CheckPointDatas[i];

                    Rows[i].Cells[CHECKPOINT_NAME_COLUMN].Value = checkPointData.CheckPointName;
                    Rows[i].Cells[WORLD_RECORD_COLUMN].Value = checkPointData.ToString();
                }
                else
                {
                    TrackerHP checkPointData = (TrackerHP)checkPointDataTable.CheckPointDatas[i];

                    Rows[i].Cells[CHECKPOINT_NAME_COLUMN].Value = checkPointData.CheckPointName;
                    Rows[i].Cells[WORLD_RECORD_COLUMN].Value = checkPointData.ToString();
                }
                
            }

            Rows[checkPointDataTable.PhaseCount - 1].Cells[CHECKPOINT_NAME_COLUMN].Value = "Clear";
            Rows[checkPointDataTable.PhaseCount - 1].Cells[WORLD_RECORD_COLUMN].Value = checkPointDataTable.WorldRecordClearTime.ToString();
        }

        public void Reset()
        {
            for (int i = 0; i < MAX_COLUMN_COUNT; i++)
            {
                Rows[i].Cells[CURRENT_RUN_COLUMN].Value = "";
                Rows[i].Cells[TIME_DIFFERENCE_COLUMN].Value = "";
            }
        }

        public void UpdateCurrentRunData(int currentPhase, TimeSpan duration)
        {
            Rows[currentPhase].Cells[CURRENT_RUN_COLUMN].Value = TimeFormatter.Format(duration);
        }

        public void UpdateCurrentRunData(int currentPhase, int bossTotalHP, int totalDamageDone)
        {
            TrackerHP currentRunTrackerHP = new TrackerHP(bossTotalHP, totalDamageDone, false);
            Rows[currentPhase].Cells[CURRENT_RUN_COLUMN].Value = currentRunTrackerHP.ToString();
        }

        public void UpdateCurrentRunWorldRecordDifference(int currentPhase, TrackerTime currentRunWorldRecordRunTimeDifference)
        {
            Rows[currentPhase].Cells[TIME_DIFFERENCE_COLUMN].Style = new DataGridViewCellStyle { ForeColor = currentRunWorldRecordRunTimeDifference.TextColor };
            Rows[currentPhase].Cells[TIME_DIFFERENCE_COLUMN].Value = currentRunWorldRecordRunTimeDifference.ToString();
        }

        public void UpdateCurrentRunWorldRecordDifference(int currentPhase, TrackerHP currentRunWorldRecordRunHPDifference)
        {
            Rows[currentPhase].Cells[TIME_DIFFERENCE_COLUMN].Style = new DataGridViewCellStyle { ForeColor = currentRunWorldRecordRunHPDifference.TextColor };
            Rows[currentPhase].Cells[TIME_DIFFERENCE_COLUMN].Value = currentRunWorldRecordRunHPDifference.ToString();
        }
    }
}
