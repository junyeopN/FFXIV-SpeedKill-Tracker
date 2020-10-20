using System;
using System.IO;
using System.Reflection;

namespace FFXIV_Speedkill_Tracker
{
    class CheckPointDataTableFactory: ZoneFileReader 
    {
        static private readonly String ZONE_DATA_FILE_NAME = DLL_FOLDER_DIRECTORY + "\\Plugins\\FFXIV_Speedkill_Tracker\\FFXIV_Speedkill_Tracker\\ZoneData.txt";


        static public CheckPointDataTable CreateCheckPointDataTable(String zoneName)
        {
            CheckPointDataTable zoneSpeedRunData = null;
            StreamReader dataFileReader = new StreamReader(ZONE_DATA_FILE_NAME);

            zoneSpeedRunData = GetZoneSpeedRunData(zoneName, dataFileReader);

            dataFileReader.Close();

            return zoneSpeedRunData;
        }

        static private CheckPointDataTable GetZoneSpeedRunData(String zoneName, StreamReader dataFileReader)
        {
            if (FindCurrentZone(zoneName, dataFileReader))
            { 
                return new CheckPointDataTable(dataFileReader);
            }
            else
            {
                return null;
            }
        }
    }
}
