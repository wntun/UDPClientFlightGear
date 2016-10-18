using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private int timeStep;

        private Variable aileron;
        private Variable elevator;
        private Variable throttle;

        public Client(UserProperties properties)
        {
            this.properties = properties;
        }

        public void start()
        {
            Console.WriteLine("Client started..");

        }

        private void initClient()
        {
            setServerConfiguration();
            setInputDataFileConfiguration();
            setVariables();
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
            this.timeStep = properties.getInt("timeStep");
        }

        private void setVariables()
        {
            VariableKeyInConfig aileronKey = getAeileronKeyInConfig();
            VariableKeyInConfig elevatorKey = getElevatorKeyInConfig();
            VariableKeyInConfig throttleKey = getThrottleKeyInConfig();

            aileron = new Variable(name: aileronKey.Name, min: properties.getFloat(aileronKey.MinValueKey), max: properties.getFloat(aileronKey.MaxValueKey), fg_min: properties.getInt(aileronKey.FGMinValeKey), fg_max: properties.getInt(aileronKey.FGMaxValueKey));
            elevator = new Variable(name: elevatorKey.Name, min: properties.getFloat(elevatorKey.MinValueKey), max: properties.getFloat(elevatorKey.MaxValueKey), fg_min: properties.getInt(elevatorKey.FGMinValeKey), fg_max: properties.getInt(elevatorKey.FGMaxValueKey));
            throttle = new Variable(name: throttleKey.Name, min: properties.getFloat(throttleKey.MinValueKey), max: properties.getFloat(throttleKey.MaxValueKey), fg_min: properties.getInt(throttleKey.FGMinValeKey), fg_max: properties.getInt(throttleKey.FGMaxValueKey));
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
        
    }
}
