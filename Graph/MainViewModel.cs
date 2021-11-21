using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
//using System.Web.DataVisualization.dll;
//using System.Web.UI.DataVisualization.Charting;
namespace WpfAppPlotNew
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            //this.Title = "Plot \n";
            var rand = new Random();
            List<double> list1 = new List<double>();
            for (int i = 0; i < 50; i++)
            {
                list1.Add(rand.NextDouble() * 10);
            }
            List<DataPoint> list2 = new List<DataPoint>();
            for (int i = 0; i < 50; i++)
            {
                list2.Add(new DataPoint(i, list1[i]));
            }
            this.Points = list2;
            this.arr = list1;
            this.Title = "Plot \n" + "average=" + average() + ", min=" + min() + ", max=" + max() + "\n" + ", variance=" + variance() + ", standardDeviation=" + standardDeviation();
        }
        public string Title { get; private set; }
        public IList<DataPoint> Points { get; private set; }
        public IList<double> arr { get; private set; }
        public double average()
        {
            double sum = 0;
            for (int i = 0; i < arr.Count(); i++)
            {
                sum += arr[i];
            }
            return sum / arr.Count();
        }
        public double min()
        {
            double min = arr[0];
            for (int i = 1; i < arr.Count(); i++)
            {
                if (arr[i] < min)
                    min = arr[i];
            }
            return min;
        }
        public double max()
        {
            double max = arr[0];
            for (int i = 1; i < arr.Count(); i++)
            {
                if (arr[i] > max)
                    max = arr[i];
            }
            return max;
        }
        public double variance()
        {
            double sum = 0;
            for (int i = 0; i < arr.Count(); i++)
            {
                sum += Math.Pow((arr[i] - average()), 2);
            }
            return sum / (arr.Count() - 1);
        }
        public double standardDeviation()
        {
            return Math.Sqrt(variance());
        }
    }
}