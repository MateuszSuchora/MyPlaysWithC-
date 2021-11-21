using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Threads
{

    class Program
    {
        public static void ThreadRand(ArrayList alist )
        {
            alist.RemoveAt(0);
            alist.RemoveAt(alist.Count-1);
            alist.RemoveAt((alist.Count - alist.Count % 2) / 2);
            Random rnd = new Random();
            alist.Add(rnd.Next(100));
            alist.Add(rnd.Next(100));
            alist.Add(rnd.Next(100));
            alist.Reverse();
        }

        public static void ThreadSort(ArrayList alist)
        {
            BubbleSort(alist);
        }

        static void BubbleSort(ArrayList arr)
        {
            int n = arr.Count;
            for (int i = 0; i < n - 1; i++)
                for (int j = 0; j < n - i - 1; j++)
                    if ((int) arr[j] > (int)arr[j + 1])
                    {
                        // swap temp and arr[i] 
                        int temp = (int)arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
        }

        public static void ThreadWrite(ArrayList alist)
        {
            
            System.Console.WriteLine("LIST:");
            for (int i = 0; i < alist.Count; i++) {
                int x = (int)alist[i];
                System.Console.WriteLine(x);
            }
            System.Console.WriteLine(alist.IsSynchronized);
            int sleepFor = 1000;
            Thread.Sleep(sleepFor);
        }

        static void Main(string[] args)
        {
            ArrayList alist2 = new ArrayList();
            //ArrayList alist2 = ArrayList.Synchronized(alist);
            alist2.Add(1);
            alist2.Add(2);
            alist2.Add(3);
            alist2.Add(4);
            alist2.Add(5);
            alist2.Add(5);
            alist2.Add(5);
            alist2.Add(5);
            alist2.Add(5);
            alist2.Add(5);
            alist2.Add(5);
            alist2.Add(5);
            alist2.Add(5);
            alist2.Add(5);
            alist2.Add(5);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);
            alist2.Add(1);

            while (true) {

                var writetask = new Task(() => ThreadWrite(alist2));
                var randtask = new Task(() => ThreadRand(alist2));
                var sorttask = new Task(() => ThreadSort(alist2));
                randtask.RunSynchronously();
                sorttask.RunSynchronously();
                writetask.RunSynchronously();


                //Task.Run(() => ThreadRand(alist2));
                //Task.Run(() => ThreadSort(alist2));
                //Task.Run(() => ThreadWrite(alist2));

            }
            //DeathLock();
        }
        public static void DeathLock()
        {
            object lock1 = new object();
            object lock2 = new object();
            Console.WriteLine("Starting");
            var task1 = Task.Run(() =>
            {
                lock (lock1)
                {
                    Thread.Sleep(1000);
                    lock (lock2)
                    {
                        Console.WriteLine("Finished Thread 1");
                    }
                }
            });

            var task2 = Task.Run(() =>
            {
                lock (lock2)
                {
                    Thread.Sleep(1000);
                    lock (lock1)
                    {
                        Console.WriteLine("Finished Thread 2");
                    }
                }
            });

            Task.WaitAll(task1, task2);
            Console.WriteLine("Finished...");
        }
    }

}