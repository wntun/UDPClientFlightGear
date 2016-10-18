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
        private const string defaultDatFile = "test1.csv";

        private string serverIP;
        private int serverInputPort;
        private int startLine;
        private int numberOfLines;
        private int timeStep;

        
    }
}
