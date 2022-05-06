using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FlaskeAutomat
{
    class Consumer
    {
        public void GetLock()
        {
            while (true)
            {
                if (Monitor.TryEnter(Storage.beer, 100))
                {
                    try
                    {
                        EmptyQueue(Storage.beer);
                    }
                    finally
                    {
                        Monitor.Wait(Storage.beer);
                    }
                }
                else if (Monitor.TryEnter(Storage.sodas, 100))
                {
                    try
                    {
                        EmptyQueue(Storage.sodas);
                    }
                    finally
                    {
                        Monitor.Wait(Storage.sodas);
                    }
                }
            }
        }
        void EmptyQueue(Queue<Drink> drinks)
        {
            Drink drink;
            bool drinkInQueue;
            while (true)
            {
                if (drinks.Count > 0)
                {
                    drinkInQueue = drinks.TryDequeue(out drink);
                    if (drinkInQueue == true)
                    {
                        Program.Print("Consumer", drink.Type, drink.Nr);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}
