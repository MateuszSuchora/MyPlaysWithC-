using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Graph
{
    class Class2
    {
        public List<double> tableofweight(int size, double sigma)
        {
            List<double> list = new List<double>();
            double weight = 1;
            int i = 1;
            int s = size;
            while (weight > 0.001 && s > 0)
            {
                weight = normal(i, sigma);
                list.Add(weight);
                i++;
                s--;
            }

            return list;
        }

        public double normal(int x, double sigma)
        {
            double fx = (1 / (sigma * Math.Sqrt(2 * Math.PI))) * Math.Pow(Math.E, (-(x * x) / (2 * sigma * sigma)));

            return fx;
        }


    }
   
}
