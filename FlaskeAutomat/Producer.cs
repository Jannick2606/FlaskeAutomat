using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FlaskeAutomat
{
    class Producer
    {
        public void Produce()
        {
            Random ran = new Random();
            int beerLabel = 0;
            int sodaLabel = 0;
            while (true)
            {
                if (Monitor.TryEnter(Storage.products))
                {
                    try
                    {
                        while (true)
                        {
                            if (Storage.products.Count < 20)
                            {
                                //Creates a random number that is either 1 or 2
                                //If the number is 1 it creates a beer and if it's 2 it creates a soda
                                if (ran.Next(1, 3) == 1)
                                {
                                    beerLabel++;
                                    Beer b = new Beer("Øl", beerLabel);
                                    Storage.products.Enqueue(b);
                                    Program.Print("Producer", b.Type, b.Nr);
                                }
                                else
                                {
                                    sodaLabel++;
                                    Soda s = new Soda("Vand", sodaLabel);
                                    Storage.products.Enqueue(s);
                                    Program.Print("Producer", s.Type, s.Nr);
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
                        Monitor.Exit(Storage.products);
                    }
                }
            }
        }
    }
}
