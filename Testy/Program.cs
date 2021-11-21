using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Testy
{
    class Program
    {
        static void Main(string[] args)
        {
            List<double> test = new List<double>();
            test.Add(5600);
            test.Add(9000);
            test.Add(3100);
            test.Add(5300);
            test.Add(1200);
            test.Add(600);
            test.Add(9200);
            test.Add(5100);
            test.Add(3000);
            test.Add(8300);
            test = better_smoothing_graph(test, 1);
            foreach (double x in test) {
                System.Console.WriteLine(x);
            }
            //test = table_of_weight(2000, 31);

            //foreach (double x in test) {
            //    System.Console.WriteLine(x);
            //}
            //System.Console.WriteLine(2*test.Sum()-test[0]);
            //System.Console.WriteLine(test.Count);

        }

        private static List<double> better_smoothing_graph(List<double> list, double sigma)
        {
            int size = list.Count;
            List<double> weight = new List<double>();
            weight = table_of_weight(size, sigma);
            int k = (weight.Count * 2) - 1;
            double smoth = 0;
            List<double> temp = new List<double>();
            for (int j = 0; j < k; j++)
            {
                temp.Add(list[j]);
            }
            int from = ((k - 1) / 2);
            for (int i = from; i < size - from - 1; i++)
            {
                smoth = smooth(weight, temp);
                list[i] = smoth;
                temp.RemoveAt(0);
                temp.Add(list[i + from + 1]);

            }
            for (int i = size - 1; i > size - from -2; i--)
            {
                list.RemoveAt(i);
            }
            for (int i = 0; i < from ; i++)
            {
                list.RemoveAt(0);
            }
            return list;
        }


 
        public static List<double> table_of_weight(int size, double sigma)
        {
            List<double> list = new List<double>();
            double weight = 1;
            int i = 1;
            int s = size;
            weight = normal(0, sigma);
            while (weight > 0.001 && s > 0)
            {
                list.Add(weight);
                weight = normal(i, sigma);
                i++;
                s--;
            }
            double normalizer = 1 / (2 * list.Sum() - list[0]);
            for (int j = 0; j < list.Count; j++)
            {
                list[j] *= normalizer;
            }
            return list;
        }

        public static double normal(int x, double sigma)
        {
            double fx = (1 / (sigma * Math.Sqrt(2 * Math.PI))) * Math.Pow(Math.E, (-(x * x) / (2 * sigma * sigma)));

            return fx;
        }

        public static double smooth(List<double> weight, List<double> list)
        {
            int size = list.Count;
            double smooth = weight[0] * list[(size - 1) / 2];

            for (int i = 1; i < (size - 1) / 2; i++)
            {
                smooth += weight[i] * list[(size - 1) / 2 - i];
                smooth += weight[i] * list[(size - 1) / 2 + i];

            }
            return smooth;
        }
    }
}
