using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPClientFlightGear
{
    class VariableKeyInConfig
    {
        public VariableKeyInConfig() { }
        public string Name { get; set; }
        public string MinValueKey { get; set; }
        public string MaxValueKey { get; set; }
        public string FGMinValeKey { get; set; }
        public string FGMaxValueKey { get; set; }
    }
}
