using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace FFXIV_Speedkill_Tracker
{
    public class CheckPointDataTable
    {
        private int _phaseCount = -1;
        private int _bossTotalHP = -1;
        
        private List<TrackerData> _checkPointDatas;
        private List<DataType> _checkPointTypes;

        private TrackerTime _worldRecordClearTime = null;

        public CheckPointDataTable(StreamReader dataFileReader)
        {
            _worldRecordClearTime = FindWorldRecordClearTime(dataFileReader);
            _bossTotalHP = FindBossTotalHP(dataFileReader);
            _phaseCount = FindPhaseCount(dataFileReader);

            _checkPointDatas = new List<TrackerData>();
            _checkPointTypes = new List<DataType>();

            SetCheckPointDatas(dataFileReader);
        }

        public int BossTotalHP
        {
            get => _bossTotalHP;
        }

        public int PhaseCount
        {
            get => _phaseCount;
            set
            {
                _phaseCount = value;
            }
        }

        public TrackerTime WorldRecordClearTime
        {
            get => _worldRecordClearTime;
            set
            {
                _worldRecordClearTime = value;
            }
        }

        public List<TrackerData> CheckPointDatas
        {
            get => _checkPointDatas;
            set
            {
                _checkPointDatas = value;
            }
        }

        public List<DataType> CheckPointTypes
        {
            get => _checkPointTypes;
            set
            {
                _checkPointTypes = value;
            }
        }

        private TrackerTime FindWorldRecordClearTime(StreamReader dataFileReader)
        {
            return new TrackerTime(dataFileReader.ReadLine());
        }

        private int FindBossTotalHP(StreamReader dataFileReader)
        {
            return Int32.Parse(dataFileReader.ReadLine());
        }

        private int FindPhaseCount(StreamReader dataFileReader)
        {
            return Int32.Parse(dataFileReader.ReadLine());
        }

        private void SetCheckPointDatas(StreamReader dataFileReader) 
        {
            for (int i = 0; i < PhaseCount - 1; i++)
            {
                TrackerData checkPointData;
                String checkPointName = dataFileReader.ReadLine();
                String checkPointEventString = dataFileReader.ReadLine();

                DataType dataType = getDataType(dataFileReader.ReadLine());

                if(dataType == DataType.kDataTypeTime)
                {
                    checkPointData = new TrackerTime(dataFileReader.ReadLine(), false, checkPointEventString, checkPointName);
                    
                } else
                {
                    checkPointData = new TrackerHP(BossTotalHP, BossTotalHP - Int32.Parse(dataFileReader.ReadLine()), false, checkPointEventString, checkPointName);
                }

                CheckPointTypes.Add(dataType);
                CheckPointDatas.Add(checkPointData);
            }
        }

        private DataType getDataType(String typeString)
        {
            if(typeString.Equals("TimeCheckPoints"))
            {
                return DataType.kDataTypeTime;
            } else
            {
                return DataType.kDataTypeHP;
            }
        }
    }
}

