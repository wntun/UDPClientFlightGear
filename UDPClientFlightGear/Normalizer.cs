using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPClientFlightGear
{
    class Normalizer
    {
        public static float getNormalizedValue(float value, Variable variable)
        {
            float normalized = 0;
            float newMin = variable.Fg_min;
            float newMax = variable.Fg_max;
            float min = variable.Min;
            float max = variable.Max;
            normalized = (newMax - newMin) / (max - min) * (value - max) + newMax;           

            return normalized;
        }
    }
}
