using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPClientFlightGear
{
    class Variable
    {
        private string name;
        private int index;
        private float min;
        private float max;
        private float fg_min;
        private float fg_max;


        public Variable(string name, float min, float max, float fg_min, float fg_max) : this(name, 0, min, max, fg_min, fg_max) { }

        public Variable(string name, int index, float min, float max, float fg_min, float fg_max)
        {
            this.name = name;
            this.index = index;
            this.min = min;
            this.max = max;
            this.fg_min = fg_min;
            this.fg_max = fg_max;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public float Min
        {
            get { return min; }
            set { min = value; }
        }
        public float Max
        {
            get { return max; }
            set { max = value; }
        }
        public float Fg_min
        {
            get { return fg_min; }
            set { fg_min = value; }
        }
        public float Fg_max
        {
            get { return fg_max; }
            set { fg_max = value; }
        }

        public override string ToString()
        {
            string text = "Name : " + this.name + ", Min : " + this.min + ", Max : " + this.max + ", Fg_min : " + this.fg_min + ", Fg_max : " + this.fg_max;
            return text;
        }
    }
}
