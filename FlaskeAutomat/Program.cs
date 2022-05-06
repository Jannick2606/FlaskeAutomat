using System;
using System.Threading;
using System.Collections.Generic;

namespace FlaskeAutomat
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadManager.StartThreads();
        }
        public static void Print(string thread, string drink, int nr)
        {
            if (thread == "Producer")
            {
                Console.WriteLine($"{thread} har produceret {drink} {nr}");
            }
            else if (thread == "Splitter")
            {
                Console.WriteLine($"{thread} har flyttet {drink} {nr}");
            }
            else
            {
                Console.WriteLine($"{drink} {nr} er blevet drukket");
            }
            Thread.Sleep(300);
        }
    }
}
