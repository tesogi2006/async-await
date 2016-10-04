using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    public delegate int BinaryOp(int x, int y);
    class Program
    {
        static void Main(string[] args)
        {
            Printer p = new Printer();
            // 10 threads
            Thread [] threads = new Thread[10];
            for (int i = 0; i < 10; i++)
            {
                threads[i] = new Thread(new ThreadStart(p.PrintNumbers));
                threads[i].Name = string.Format("Worker thread #{0}", i);
            }

            // start each thread
            foreach (var thread in threads)
            {
                thread.Start();
            }
            
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }

    class Printer
    {
        // lock token
        private readonly object _lockObj = new object();
        public void PrintNumbers()
        {
            lock (_lockObj)
            {
                // display thread info
                Console.WriteLine("-> {0} is executing PrintNumbers()", Thread.CurrentThread.Name);

                for (int i = 0; i < 10; i++)
                {
                    // put thread to sleep for random amount of time
                    Random r = new Random();
                    Thread.Sleep(100 * r.Next(5));
                    Console.Write("{0}, ", i);
                }
                Console.WriteLine();
            }
        }
    }
}
