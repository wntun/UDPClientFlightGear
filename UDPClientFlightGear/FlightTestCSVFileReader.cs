using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UDPClientFlightGear
{
    class FlightTestCSVFileReader
    {
        private string fileName;
        private string[,] dataArray;
        private int[] colIndexArray;

        public FlightTestCSVFileReader(string fileName)
        {
            this.fileName = fileName;
        }

        public void Load(string[] colNames, int startLine, int numberOfLines)
        {


            if (System.IO.File.Exists(fileName))
                loadFromFile(colNames, startLine, numberOfLines);
            else
                Console.WriteLine("Flight Test CSV file not found.");
        }

        private void loadFromFile(string[] colNames, int startLine, int numberOfLines)
        {
            this.dataArray = new string[colNames.Length, numberOfLines + 1];
            string line;
            int index = 0;
            int lineCount = 0;
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            System.IO.StreamReader file = new System.IO.StreamReader(stream);
            while ((line = file.ReadLine()) != null && index <= numberOfLines)
            {
                List<string> temp = line.Split(',').ToList();
                if (temp.Count == 0) continue;
                if (lineCount == 0)
                {
                    setColumnIndexArray(temp, colNames);
                    lineCount++;
                }
                else if (lineCount > 0 && lineCount < startLine)
                {
                    lineCount++;
                    continue;
                }
                setDataValues(temp, index);
                index++;
            }
            file.Close();
            stream.Close();
        }

        private void setColumnIndexArray(List<string> titleList, string[] colNames)
        {
            this.colIndexArray = new int[colNames.Length];
            for (int i = 0; i < this.colIndexArray.Length; i++)
                this.colIndexArray[i] = titleList.IndexOf(colNames[i]);

        }
        private void setDataValues(List<string> dataList, int index)
        {
            for (int i = 0; i < this.colIndexArray.Length; i++)
                this.dataArray[i, index] = dataList.ElementAt(colIndexArray[i]);
        }

        public Dictionary<string, string[]> getFlightDataInDictionary()
        {

            Dictionary<string, string[]> dataList = new Dictionary<string, string[]>();
            for (int row = 0; row < this.dataArray.GetLength(0); row++)
            {
                dataList.Add(this.dataArray[row, 0], getDataArrayForTitleAtRow(row));
            }

            return dataList;
        }

        private string[] getDataArrayForTitleAtRow(int row)
        {
            string[] data = new string[this.dataArray.GetLength(1) - 1]; //-1 for title
            for (int col = 1; col < this.dataArray.GetLength(1); col++)
                data[col - 1] = this.dataArray[row, col];
            return data;
        }
    }
}
