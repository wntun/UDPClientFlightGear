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
            //Console.WriteLine(variable.ToString());
            float normalized = 0;
            float newMin = variable.Fg_min;
            float newMax = variable.Fg_max;
            float min = variable.Min;
            float max = variable.Max;
            normalized = ((newMax - newMin) / (max - min)) * (value - min) + newMin;        // min-max normalization   [Normalizaiton: A Preprocessing Stage S. G. K. Patro, K. K. sahu)

            return normalized;
        }

        //static void Main(string[] args)
        //{
        //    Variable v = new Variable("t", -20, 12, -1, 1);
        //    float value = getNormalizedValue(0.037567797f, v);
        //    Console.WriteLine(value + "");
        //    Console.Read();
        //}
    }


}
