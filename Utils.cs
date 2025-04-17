using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Utils
    {
        public static double NextDouble(double min, double max)
        {
            Random rand = new Random();
            double value = rand.NextDouble() * (max - min) + min;

            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
            {
                return max;
            }
            else if (value < min)
            {
                return min;
            }
            else
            {
                return value;
            }
        }
    }
}
