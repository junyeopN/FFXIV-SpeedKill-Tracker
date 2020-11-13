using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.IO;
using System.Reflection;

namespace FFXIV_Speedkill_Tracker
{
    public class CheckPointDataTable
    {
        static public readonly String DLL_FOLDER_DIRECTORY = Path.GetDirectoryName(Uri.UnescapeDataString(new System.Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath));
        static public readonly String ZONE_DATA_FILE_NAME = DLL_FOLDER_DIRECTORY + "\\Plugins\\FFXIV_Speedkill_Tracker\\FFXIV_Speedkill_Tracker\\ZoneData.txt";

        static public readonly String TIME_CHECKPOINT_DATA_TYPE = "TIMECHECKPOINTS";
        static public readonly String HP_CHECKPOINT_DATA_TYPE = "HPCHECKPOINTS";

        private int _count = -1;

        String _checkPointDataType = "";
        List<String> _checkPointNames = null;
        List<String> _worldRecordCheckPointValues = null;
        List<Regex> _checkPointEventStrings = null;

        private StreamReader dataFileReader = null;
        private String zoneName = "";

        public CheckPointDataTable(String zoneName, RunTimeTrackerTable table)
        {
            this.zoneName = zoneName;

            try
            {
                dataFileReader = new StreamReader(ZONE_DATA_FILE_NAME);

                FillZoneData();

                dataFileReader.Close();
            }
            catch (Exception e)
            {
                table.Rows[2].Cells[2].Value = e.Message;
                
            }
        }

        public String CheckPointDataType
        {
            get => _checkPointDataType;
            set
            {
                _checkPointDataType = value;
            }
        }

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
            }
        }


        public List<String> CheckPointNames
        {
            get => _checkPointNames;
            set
            {
                _checkPointNames = value;
            }
        }
        public List<String> WorldRecordCheckPointValues
        {
            get => _worldRecordCheckPointValues;
            set
            {
                _worldRecordCheckPointValues = value;
            }
        }

        public List<Regex> CheckPointEventStrings
        {
            get => _checkPointEventStrings;
            set
            {
                _checkPointEventStrings = value;
            }
        }


        private void FillZoneData()
        {
            if (FindCurrentZone(zoneName))
            {
                CheckPointDataType = FindCheckPointDataType();
                Count = FindCount();
                CheckPointNames = FindCheckPointNames();
                WorldRecordCheckPointValues = FindWorldRecordCheckPointValues();
                CheckPointEventStrings = FindCheckPointEventStrings();
            }
            else
            {
                InitDefaultDataTable();
            }
        }


        protected void InitDefaultDataTable()
        {
            _count = 0;
            _checkPointDataType = TIME_CHECKPOINT_DATA_TYPE;
            _checkPointNames = new List<String>();
            _worldRecordCheckPointValues = new List<String>();
            _checkPointEventStrings = new List<Regex>();
        }

        private Boolean FindCurrentZone(String zoneName)
        {
            while (dataFileReader.Peek() >= 0)
            {
                String currentLine = dataFileReader.ReadLine();

                if (currentLine.Equals(zoneName))
                {
                    return true;
                }
            }

            return false;
        }


        private String FindCheckPointDataType()
        {
            return dataFileReader.ReadLine();
        }

        private int FindCount()
        {
            return Int32.Parse(dataFileReader.ReadLine());
        }

        private List<String> FindCheckPointNames()
        {
            List<String> checkPointNames = new List<String>();

            for (int i = 0; i < Count; i++)
            {
                checkPointNames.Add(dataFileReader.ReadLine());
            }

            return checkPointNames;
        }

        private List<String> FindWorldRecordCheckPointValues()
        {
            List<String> worldRecordCheckPointValues = new List<String>();

            for (int i = 0; i < Count; i++)
            {
                worldRecordCheckPointValues.Add(dataFileReader.ReadLine());
            }

            return worldRecordCheckPointValues;
        }

        private List<Regex> FindCheckPointEventStrings()
        {
            List<Regex> checkPointEventStrings = new List<Regex>();

            for (int i = 0; i < Count - 1; i++)
            {
                checkPointEventStrings.Add(new Regex(dataFileReader.ReadLine()));
            }

            return checkPointEventStrings;
        }
    }
}

