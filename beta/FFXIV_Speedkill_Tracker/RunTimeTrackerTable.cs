using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FFXIV_Speedkill_Tracker
{
    public class RunTimeTrackerTable : DataGridView
    {
        public readonly int CHECKPOINT_COLUMN = 0;
        public readonly int CURRENT_RUN_COLUMN = 1;
        public readonly int WORLD_RECORD_COLUMN = 2;
        public readonly int TIME_DIFFERENCE_COLUMN = 3;

        public readonly int COLUMN_COUNT = 4;

        private CheckPointDataTable checkPointDataTable = null;

        public RunTimeTrackerTable(CheckPointDataTable checkPointDataTable)
        {
            this.checkPointDataTable = checkPointDataTable;

            ColumnCount = COLUMN_COUNT;
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            Location = new Point(0, 0);
            Size = new Size(520, 150);

            UpdateTitles();
            UpdateCheckPointDataCells();
        }

        private void UpdateTitles()
        {
            Columns[CHECKPOINT_COLUMN].Name = "Checkpoint";
            Columns[CURRENT_RUN_COLUMN].Name = "Current";
            Columns[WORLD_RECORD_COLUMN].Name = "WR";
            Columns[TIME_DIFFERENCE_COLUMN].Name = "Diff";
        }
        private void UpdateCheckPointDataCells()
        {
            for (int i = 0; i < checkPointDataTable.Count; i++)
            {
                Rows.Add(checkPointDataTable.CheckPointNames[i]);
                Rows[i].Cells[WORLD_RECORD_COLUMN].Value = checkPointDataTable.WorldRecordCheckPointTimes[i];
            }
        }

        public void Reset()
        {
            for (int i = 0; i < checkPointDataTable.Count; i++)
            {
                Rows[i].Cells[CURRENT_RUN_COLUMN].Value = "";
                Rows[i].Cells[TIME_DIFFERENCE_COLUMN].Value = "";
            }
        }

        public void UpdateCurrentRunTime(int currentPhase, TimeSpan duration)
        {
            Rows[currentPhase].Cells[CURRENT_RUN_COLUMN].Value = TimeFormatter.Format(duration);
        }

        public void UpdateCurrentRunWorldRecordCheckPointTimeDifference(int currentPhase, TrackerTime currentRunWorldRecordRunTimeDifference)
        {
            Rows[currentPhase].Cells[TIME_DIFFERENCE_COLUMN].Style = new DataGridViewCellStyle { ForeColor = currentRunWorldRecordRunTimeDifference.Color };
            Rows[currentPhase].Cells[TIME_DIFFERENCE_COLUMN].Value = currentRunWorldRecordRunTimeDifference.ToString();
        }
    }
}
