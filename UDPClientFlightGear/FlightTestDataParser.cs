using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPClientFlightGear
{
    class FlightTestDataParser
    {
        private Dictionary<String, String[]> dataDic;

        public FlightTestDataParser(Dictionary<String, String[]> dataDic)
        {
            this.dataDic = dataDic;
        }


        public float[] getDataArrayAtLine(int line, string[] columnNames)
        {
            float[] dataArray = new float[columnNames.Length];
            for (int i = 0; i < columnNames.Length; i++)
            {
                string colName = columnNames[i];
                if (!String.IsNullOrEmpty(colName) && dataDic.ContainsKey(colName))
                    dataArray[i] = float.Parse(dataDic[colName][line]);

            }
            return dataArray;
        }
    }
}
