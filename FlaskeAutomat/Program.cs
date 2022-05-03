using System;
using System.Threading;
using System.Collections.Generic;

namespace FlaskeAutomat
{
    class Program
    {
        //products is the queue with both sodas and beer in it and it splits into 2 specific queues
        static Queue<string> beer = new Queue<string>(20);
        static Queue<string> sodas = new Queue<string>(20);
        static Queue<string> products = new Queue<string>(20);
        static readonly object lock1 = new object();
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
                                    products.Enqueue("Øl ");
                                }
                                else
                                {
                                    products.Enqueue("Vand ");
                                }
                                Console.WriteLine("Producer added items");
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
            int sodaLabel = 0;
            int beerLabel = 0;
            string product;
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
                            notFull = products.TryDequeue(out product);
                            if (notFull == false)
                            {
                                break;
                            }
                            else if (product == "Øl ")
                            {
                                beerLabel++;
                                Console.WriteLine($"Added {product} {beerLabel}");
                                beer.Enqueue(product + beerLabel);
                            }
                            else
                            {
                                sodaLabel++;
                                Console.WriteLine($"Added {product} {sodaLabel}");
                                sodas.Enqueue(product + sodaLabel);
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
        static void BeerConsumer()
        {
            string drink;
            bool drinkInQueue;
            while (true)
            {
                //Checks if the queue is empty and if it is then it goes to sleep
                if (beer.Count > 0)
                {
                    while (true)
                    {
                        drinkInQueue = beer.TryDequeue(out drink);
                        if (drinkInQueue == true)
                        {
                            Console.WriteLine($"BeerConsumer drak {drink}");
                        }
                        else
                        {
                            //If the queue is empty it breaks out of the inner while loop and goes to sleep
                            break;
                        }
                    }
                }
                Thread.Sleep(100 / 15);
            }
        }
        static void SodaConsumer()
        {
            string drink;
            bool drinkInQueue;
            while (true)
            {
                if (sodas.Count > 0)
                {
                    while (true)
                    {
                        drinkInQueue = sodas.TryDequeue(out drink);
                        if (drinkInQueue == true)
                        {
                            Console.WriteLine($"SodaConsumer drak {drink}");
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                Thread.Sleep(100 / 15);
            }
        }
    }
}
