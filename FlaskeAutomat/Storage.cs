using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomat
{
    class Storage
    {
        public static Queue<Drink> beer = new Queue<Drink>(20);
        public static Queue<Drink> sodas = new Queue<Drink>(20);
        public static Queue<Drink> products = new Queue<Drink>(20);
    }
}
