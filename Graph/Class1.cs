using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using OxyPlot;
using System.Data;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Windows;
//using System.Web.DataVisualization.dll;
//using System.Web.UI.DataVisualization.Charting;
namespace WpfAppPlotNew
{//907
    public class MainViewModel
    {
        public MainViewModel()
        {
            int k = 40;
            double sigma = 10;
            var cs = "Host=localhost;port=5557; Username=postgres;Password=Test123;Database=baza_testowa";
            var con = new NpgsqlConnection(cs);
            con.Open();
            var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            string count = "SELECT device_ts, pa FROM iot.measurement_electricity WHERE measure_setup_id=907 ORDER BY device_ts LIMIT 1000";
            List<double> x = new List<double>();
            List<double> xx = new List<double>();
            List<DataPoint> y = new List<DataPoint>();
            List<DataPoint> z = new List<DataPoint>();
            List<DataPoint> zz = new List<DataPoint>();
            double i = 0;
            var cmdd = new NpgsqlCommand(count, con);
            NpgsqlDataReader rdr = cmdd.ExecuteReader();
            int k2;
            if (k % 2 == 1)
            {
                k2 = (k - 1) / 2;
            }
            else {
                k2 = k / 2;
            }
            
            while (rdr.Read())
            {
                x.Add(rdr.GetDouble(1));
                xx.Add(rdr.GetDouble(1));
                z.Add(new DataPoint(i, rdr.GetDouble(1)));
                i += 1;
                
            }
            con.Close();
            x = smoothing_graph(x, k);
            xx = better_smoothing_graph(xx, sigma);
            foreach (double u in x) {
                y.Add(new DataPoint(k2, u));
                k2 += 1;
            }
            List<double> weight = new List<double>();
            weight = table_of_weight(xx.Count, sigma);
            int k3 = weight.Count -1 ;
            foreach (double ua in xx){
                zz.Add(new DataPoint(k3, ua));
                k3 += 1;
            }


            this.Points = y;
            this.Points2 = z;
            this.Points3 = zz;
            this.Title = "PLOT";
        }

        private List<double> better_smoothing_graph(List<double> list, double sigma)
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
            int from = ((k - 1)/2);
            for (int i = from; i < size - from -1 ; i++)
            {
                smoth = smooth(weight, temp);
                list[i] = smoth;
                temp.RemoveAt(0);
                temp.Add(list[i + from + 1]);
                
            }
            for (int i = size - 1; i > size - from -2 ; i--)
            {
                list.RemoveAt(i);
            }
            for (int i = 0; i < from +1; i++)
            {
                list.RemoveAt(0);
            }
            return list;
        }

            private List<double> smoothing_graph(List<double> list, int k){

            int length = list.Count;
            Queue myQ = new Queue();
            double sum = 0;
            if (k % 2 == 1)
            {
                for (int j = 0; j < k; j++)
                {
                    myQ.Enqueue(list[j]);
                    sum = sum + list[j];
                }
                int from = ((k - 1) / 2);
                for (int i = from +2; i < length - from - 1; i++)
                {
                    list[i] = sum / k;
                    sum = sum - (double)myQ.Peek();
                    myQ.Dequeue();
                    myQ.Enqueue(list[i + from + 1]);
                    sum = sum + list[i + from + 1];
                }
                for (int i = length - 1; i > length - from -2; i--)
                {
                    list.RemoveAt(i);
                }
                for (int i = 0; i < from+ 2; i++)
                {
                    list.RemoveAt(0);
                }
            }
            else {
                k = k + 1;
                for (int j = 0; j < k; j++)
                {
                    myQ.Enqueue(list[j]);
                    sum = sum + list[j];
                }
                int from = ((k - 1) / 2);
                for (int i = from; i < length - from - 1; i++)
                {
                    list[i] = sum / k;
                    sum = sum - (double)myQ.Peek();
                    myQ.Dequeue();
                    myQ.Enqueue(list[i + from + 1]);
                    sum = sum + list[i + from + 1];
                }
                for (int i = length - 1; i > length - from - 2; i--)
                {
                    list.RemoveAt(i);
                }
                for (int i = 0; i < from +2; i++)
                {
                    list.RemoveAt(0);
                }
            }
            
            return list;

        }


        public string Title { get; private set; }
        public IList<DataPoint> Points { get; private set; }
        public IList<DataPoint> Points2 { get; private set; }
        public IList<DataPoint> Points3 { get; private set; }
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

        public static double smooth(List<double> weight, List<double> list) {
            int size = list.Count ;
            double smooth = weight[0] * list[(size-1)/2];

            for (int i = 1; i < (size-1)/2; i++) {
                 smooth += weight[i] * list[(size-1)/2-i];
                 smooth += weight[i] * list[(size - 1) / 2 +i];

            }
            return smooth;
        }

    }
}
