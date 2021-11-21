using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizedAsynchronized
{

    class Program
    {
        public static void RandomizeFunction(ArrayList alist)
        {
            lock (alist.SyncRoot)
            {
                alist.RemoveAt(0);
                alist.RemoveAt(alist.Count - 1);
                alist.RemoveAt((alist.Count - alist.Count % 2) / 2);
                Random rnd = new Random();
                alist.Add(rnd.Next(100));
                alist.Add(rnd.Next(100));
                alist.Add(rnd.Next(100));
                alist.Reverse();// more complication 
            }
        }

        public static void SortingFunction(ArrayList alist)
        {
            lock (alist.SyncRoot)
            {
                alist.Sort();
            }
        }

        static void BubbleSort(ArrayList arr)
        {
                lock (arr.SyncRoot)
                {
                    int n = arr.Count;
                    for (int i = 0; i < n - 1; i++)
                        for (int j = 0; j < n - i - 1; j++)
                            if ((int)arr[j] > (int)arr[j + 1])
                            {
                                // swap temp and arr[i] 
                                int temp = (int)arr[j];
                                arr[j] = arr[j + 1];
                                arr[j + 1] = temp;
                            }
                }
        }

        public static void PrintingFunction(ArrayList alist)
        {
            lock (alist.SyncRoot)
            {
                System.Console.WriteLine("LIST:");
                for (int i = 0; i < alist.Count; i++)
                {
                    int x = (int)alist[i];
                    System.Console.WriteLine(x);
                }
                //System.Console.WriteLine(alist.IsSynchronized);
                //int sleepFor = 1000;
                Thread.Sleep(1000);
            }
        }

        static void Main(string[] args)
        {
            ArrayList alist = new ArrayList();
            ArrayList alist2 = ArrayList.Synchronized(alist);

            for (int i = 0; i < 23; i++) {
                alist2.Add(1);
            }


            while (true)
            {

                var writetask =  new Task(() => PrintingFunction(alist2));
                var randtask = new Task(() => RandomizeFunction(alist2));
                var sorttask = new Task(() => SortingFunction(alist2));
                //randtask.RunSynchronously();
                //sorttask.RunSynchronously();
                //writetask.RunSynchronously();

                //Console.WriteLine(writetask.GetType());
                randtask.Start();
                //randtask.Wait();
                sorttask.Start();
               // sorttask.Wait();
                writetask.Start();
                //writetask.Wait();
               
                
             }
            //DeathLockTest();
        }
        public static void DeathLockTest()
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