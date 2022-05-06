using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FlaskeAutomat
{
    class Splitter
    {
        public void RemoveFromQueue()
        {
            Drink product;
            bool drinksInQueue;
            while (true)
            {
                Queue<Drink> temp = new Queue<Drink>();
                if (Monitor.TryEnter(Storage.products))
                {
                    try
                    {
                        if (Storage.products.Count > 0)
                        {
                            while (true)
                            {
                                drinksInQueue = Storage.products.TryDequeue(out product);
                                if (drinksInQueue == false)
                                {
                                    break;
                                }
                                else
                                {
                                    temp.Enqueue(product);
                                }
                            }
                            MoveToQueue(temp);
                        }
                    }
                    finally
                    {
                        Monitor.Exit(Storage.products);
                    }
                }
            }
        }
        public void MoveToQueue(Queue<Drink> drinks)
        {
            if (Monitor.TryEnter(Storage.beer))
            {
                try
                {
                    foreach (Drink drink in drinks)
                    {
                        Storage.beer.Enqueue(drink);
                        Program.Print("Splitter", drink.Type, drink.Nr);
                    }
                }
                finally
                {
                    Monitor.Pulse(Storage.beer);
                    Monitor.Exit(Storage.beer);
                }
            }
            else if (Monitor.TryEnter(Storage.sodas))
            {
                try
                {
                    foreach (Drink drink in drinks)
                    {
                        Storage.sodas.Enqueue(drink);
                        Program.Print("Splitter", drink.Type, drink.Nr);
                    }
                }
                finally
                {
                    Monitor.Pulse(Storage.sodas);
                    Monitor.Exit(Storage.sodas);
                }
            }
        }
    }
}
