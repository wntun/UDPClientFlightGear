using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace UDPClientFlightGear
{
    //input to Flight Gear
    //need to specify and open server and socket
    class Client
    {
        private UserProperties properties;
        private const string defaultServerIP = "127.0.0.1";
        private const int defaultServerInputPort = 49000;
        private const string configFile = "config.properties";
        private const string defaultDataFile = "test1.csv";

        private string serverIP;
        private int serverInputPort;
        private string dataFile;
        private int startLine;
        private int numberOfLines;
        private int timesASecond;

        private Variable aileron;
        private Variable elevator;
        private Variable throttle;

        private StreamWriter outputWriter; 
        private FlightTestDataParser flightTestDataParser;


        public Client(UserProperties properties)
        {
            this.properties = properties;

            FileStream fileStream = new FileStream("output" + DateTime.Now.ToString("yyyMMddHHmmss")+".txt", FileMode.CreateNew, FileAccess.Write);
            this.outputWriter = new StreamWriter(fileStream);
            this.outputWriter.AutoFlush = true;
            
        }

        public void start()
        {
            Console.WriteLine("Client started..");
            initClient();
            Console.WriteLine("Loading Flight Test Data Parser..");
            loadFlightTestDataParser();
            Console.WriteLine("Sending streams to FlightGear");
            sendToFlightGear();
            Console.Read();
            this.outputWriter.Close();

        }

        #region initialize client
        private void initClient()
        {
            Console.WriteLine("setting server configuration");
            setServerConfiguration();
            Console.WriteLine("setting input data file configuration");
            setInputDataFileConfiguration();
            Console.WriteLine("setting variables");
            setVariables();
            Console.WriteLine("finished setting variables");
        }

        private void setServerConfiguration()
        {
            this.serverIP = properties.get("server", defaultServerIP);
            this.serverInputPort = properties.getInt("port", defaultServerInputPort);
        }

        private void setInputDataFileConfiguration()
        {
            this.dataFile = properties.get("file", defaultDataFile);
            this.startLine = properties.getInt("startLine");
            this.numberOfLines = properties.getInt("numberOfLines");
            this.timesASecond = properties.getInt("timesASecond");
        }

        private void setVariables()
        {
            VariableKeyInConfig aileronKey = getAeileronKeyInConfig();
            VariableKeyInConfig elevatorKey = getElevatorKeyInConfig();
            VariableKeyInConfig throttleKey = getThrottleKeyInConfig();

            aileron = new Variable(properties.get(aileronKey.Name), properties.getFloat(aileronKey.MinValueKey), properties.getFloat(aileronKey.MaxValueKey), properties.getFloat(aileronKey.FGMinValeKey), properties.getFloat(aileronKey.FGMaxValueKey));
            elevator = new Variable(properties.get(elevatorKey.Name), properties.getFloat(elevatorKey.MinValueKey), properties.getFloat(elevatorKey.MaxValueKey), properties.getFloat(elevatorKey.FGMinValeKey), properties.getFloat(elevatorKey.FGMaxValueKey));
            throttle = new Variable(properties.get(throttleKey.Name), properties.getFloat(throttleKey.MinValueKey), properties.getFloat(throttleKey.MaxValueKey), properties.getFloat(throttleKey.FGMinValeKey), properties.getFloat(throttleKey.FGMaxValueKey));
        }

        private VariableKeyInConfig getElevatorKeyInConfig()
        {
            VariableKeyInConfig key = new VariableKeyInConfig();
            key.Name = "elevator";
            key.MaxValueKey = "max_elevator";
            key.MinValueKey = "min_elevator";
            key.FGMinValeKey = "fg_max_elevator";
            key.FGMaxValueKey = "fg_min_elevator";
            return key;
        }

        private VariableKeyInConfig getAeileronKeyInConfig()
        {
            VariableKeyInConfig key = new VariableKeyInConfig();
            key.Name = "aileron";
            key.MaxValueKey = "max_aileron";
            key.MinValueKey = "min_aileron";
            key.FGMaxValueKey = "fg_max_aileron";
            key.FGMinValeKey = "fg_min_aileron";
            return key;
        }

        private VariableKeyInConfig getThrottleKeyInConfig()
        {
            VariableKeyInConfig key = new VariableKeyInConfig();
            key.Name = "throttle";
            key.MaxValueKey = "max_throttle";
            key.MinValueKey = "min_throttle";
            key.FGMaxValueKey = "fg_max_throttle";
            key.FGMinValeKey = "fg_min_throttle";
            return key;
        }
        #endregion


        #region load flight Test data
        private void loadFlightTestDataParser()
        {
            flightTestDataParser = new FlightTestDataParser(loadFlightTestData());
        }
        private Dictionary<string, string[]> loadFlightTestData()
        {
            Dictionary<string, string[]> flightTestData = new Dictionary<string, string[]>();
            FlightTestCSVFileReader csvReader = new FlightTestCSVFileReader(dataFile);
            csvReader.Load(getCSVFileColumnNames(), this.startLine, this.numberOfLines);
            flightTestData = csvReader.getFlightDataInDictionary();

            return flightTestData;
        }

        private string[] getCSVFileColumnNames()
        {
            string[] columns = new string[3];
            columns[0] = aileron.Name;
            columns[1] = elevator.Name;
            columns[2] = throttle.Name;
          //  Console.WriteLine(columns[0] + columns[1] + columns[2]);
            return columns;
        }
        #endregion

        #region send to FlightGear
        private void sendToFlightGear()
        {
            try
            {
                UdpClient udpClient = new UdpClient(this.serverIP, this.serverInputPort);
                int line = 0;
                Variable[] variableLists = new Variable[] { aileron, elevator, throttle };
                string headerString = "";
                foreach(Variable tempVariable in variableLists)
                {
                    headerString += tempVariable.Name + ",";
                }
                outputWriter.WriteLine(headerString);
                while (line < this.numberOfLines)
                {
                    float[] controlValues = getNormalizedDataArray(line, getCSVFileColumnNames(),variableLists);
                    string send_data = null;
                    foreach(float value in controlValues)
                    {
                        send_data += value.ToString() + ",";
                    }
                    send_data = send_data.Substring(0, send_data.Length - 1);
                    send_data = send_data + "\n";

                    outputWriter.Write(send_data);
                    outputWriter.WriteLine();
                    Console.WriteLine(send_data);

                    Byte[] sendByes = Encoding.ASCII.GetBytes(send_data);
                    udpClient.Send(sendByes, sendByes.Length);
                    int timeStep = getTimeStep();
                    Thread.Sleep(timeStep);
                    line++;
                }
                udpClient.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Client: sendToFlightGear(): " +e.Message);
            }
        }

        private float[] getNormalizedDataArray(int line, string[] columnNames, params Variable[] variableList)
        {
           // Console.WriteLine("getNormalizedDataArray");
            float[] dataArray = flightTestDataParser.getDataArrayAtLine(line, columnNames);
           // Console.WriteLine("getNormalizedDataArray2");
          //  Console.WriteLine(dataArray[0]);
            foreach (Variable var in variableList)
            {
                for (int i = 0; i < columnNames.Length; i++)
                {
                    string outputTemp = "";
                    if (columnNames[i].Equals(var.Name) && !(String.IsNullOrEmpty(columnNames[i])))
                    {
                        outputTemp = var.Name + " :=> Before Normalized " + var.Name + " : " + dataArray[i];
                        dataArray[i] = Normalizer.getNormalizedValue(dataArray[i], var);
                        outputTemp = " After Normalized : " + var.Name + " : " + dataArray[i];
                        //if(arrayFormat[i].Equals("throttle"))
                        System.Diagnostics.Debug.WriteLine(outputTemp);
                        break;
                    }
                }
            }

            return dataArray;
        }

        private int getTimeStep()
        {
            return (1000 / this.timesASecond);
        }
        #endregion
    }
}
