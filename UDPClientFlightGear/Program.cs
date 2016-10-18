using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPClientFlightGear
{
    class Program
    {
        static void Main(string[] args)
        {
            UserProperties properties = new UserProperties("config.properties");
            Client client = new Client(properties);
            client.start();
        }
    }
}
