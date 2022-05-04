using System;
using System.Threading;
using System.Collections.Generic;

namespace FlaskeAutomat
{
    class Program
    {
        //products is the queue with both sodas and beer in it and it splits into 2 specific queues
        static Queue<Drink> beers = new Queue<Drink>(20);
        static Queue<Drink> sodas = new Queue<Drink>(20);
        static Queue<Drink> products = new Queue<Drink>(20);
        static readonly object lock1 = new object();
        static readonly object beerLock = new object();
        static readonly object sodaLock = new object();
        static void Main(string[] args)
        {
            Thread t1 = new Thread(Producer);
            Thread t2 = new Thread(Splitter);
            Thread t3 = new Thread(BeerConsumer);
            Thread t4 = new Thread(SodaConsumer);

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
        }
        static void Producer()
        {
            Random ran = new Random();
            int beerLabel = 0;
            int sodaLabel = 0;
            while (true)
            {
                //Tries to acquire the lock it shares with thread2(The one that splits the queues)
                if (Monitor.TryEnter(lock1))
                {
                    try
                    {
                        while (true)
                        {
                            if (products.Count < 20)
                            {
                                //Creates a random number that is either 1 or 2
                                //If the number is 1 it creates a beer and if it's 2 it creates a soda
                                if (ran.Next(1, 3) == 1)
                                {
                                    beerLabel++;
                                    Beer b = new Beer("Øl", beerLabel);
                                    products.Enqueue(b);
                                    Console.WriteLine($"Producer added {b.Type} {b.LabelNr}");
                                }
                                else
                                {
                                    sodaLabel++;
                                    Soda s = new Soda("Vand", sodaLabel);
                                    products.Enqueue(s);
                                    Console.WriteLine($"Producer added {s.Type} {s.LabelNr}");
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    finally
                    {
                        Monitor.Pulse(lock1);
                        Monitor.Wait(lock1);
                        Thread.Sleep(1000);
                    }
                }
            }
        }
        static void Splitter()
        {
            Drink product;
            bool notFull;
            while (true)
            {
                //The lock it shares with thread 1
                if (Monitor.TryEnter(lock1))
                {
                    try
                    {
                        while (true)
                        {
                            //Tries to dequeue from the products queue
                            //If the queue is empty it returns false and breaks out of the while loop
                            //If it dequeues successfully, the product from the queue is stored in the variable called product
                            //It then looks at the type of drink and determines if it should be put in the soda queue or beer queue
                            notFull = products.TryDequeue(out product);
                            if (notFull == false)
                            {
                                break;
                            }
                            else if (product.Type == "Beer")
                            {
                                Monitor.TryEnter(beerLock);
                                try
                                {
                                    Console.WriteLine($"Splitter moved {product.Type} {product.LabelNr}");
                                    beers.Enqueue(product);
                                }
                                finally
                                {
                                    Monitor.PulseAll(beerLock);
                                    Monitor.Wait(beerLock);
                                }
                            }
                            else
                            {
                                Monitor.TryEnter(sodaLock);
                                try
                                {
                                    Console.WriteLine($"Splitter moved {product.Type} {product.LabelNr}");
                                    sodas.Enqueue(product);
                                }
                                finally
                                {
                                    Monitor.PulseAll(sodaLock);
                                    Monitor.Wait(sodaLock);
                                }
                            }
                        }
                    }
                    //In the finally block it lets thread1 know that the lock will be available
                    //Then it releases the lock and waits for it to be available again
                    finally
                    {
                        Monitor.Pulse(lock1);
                        Monitor.Wait(lock1);
                        Thread.Sleep(1000);
                    }
                }
            }
        }
        static void BeerConsumer()
        {
            Drink beer;
            bool drinkInQueue;
            while (true)
            {
                //Checks if the queue is empty and if it is then it goes to sleep
                if (beers.Count > 0)
                {
                    Monitor.TryEnter(beerLock);
                    try
                    {
                        while (true)
                        {

                            drinkInQueue = beers.TryDequeue(out beer);
                            if (drinkInQueue == true)
                            {
                                Console.WriteLine($"BeerConsumer drak {beer.Type} {beer.LabelNr}");
                            }
                            else
                            {
                                //If the queue is empty it breaks out of the inner while loop and goes to sleep
                                break;
                            }
                        }

                    }
                    finally
                    {
                        Monitor.PulseAll(beerLock);
                        Monitor.Wait(beerLock);
                    }
                }
                Thread.Sleep(100);
            }
        }
        static void SodaConsumer()
        {
            Drink soda;
            bool drinkInQueue;
            while (true)
            {
                if (sodas.Count > 0)
                {
                    Monitor.TryEnter(sodaLock);
                    try
                    {
                        while (true)
                        {

                            drinkInQueue = sodas.TryDequeue(out soda);
                            if (drinkInQueue == true)
                            {
                                Console.WriteLine($"SodaConsumer drak {soda.Type} {soda.LabelNr}");
                            }
                            else
                            {
                                break;
                            }
                        }

                    }
                    finally
                    {
                        Monitor.PulseAll(sodaLock);
                        Monitor.Wait(sodaLock);
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}
