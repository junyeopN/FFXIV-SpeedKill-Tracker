using System;
using System.IO;
using System.Reflection;

namespace FFXIV_Speedkill_Tracker
{
    public class ZoneFileReader
    {
        static public readonly String DLL_FOLDER_DIRECTORY = Path.GetDirectoryName(Uri.UnescapeDataString(new System.Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath));

        static public Boolean FindCurrentZone(String zoneName, StreamReader dataFileReader)
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

   }
}
