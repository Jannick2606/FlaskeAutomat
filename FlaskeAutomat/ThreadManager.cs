using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FlaskeAutomat
{
    class ThreadManager
    {
        public static void StartThreads()
        {
            Producer p = new Producer();
            Consumer c = new Consumer();
            Splitter s = new Splitter();
            Thread t1 = new Thread(p.Produce);
            Thread t2 = new Thread(s.RemoveFromQueue);
            Thread t3 = new Thread(c.GetLock);
            Thread t4 = new Thread(c.GetLock);

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
        }
    }
}
