using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomat
{
    class Beer : Drink
    {
        public Beer(string type, int nr)
        {
            Type = type;
            Nr = nr;
        }
        public override string Type { get; protected set; }
        public override int Nr { get; protected set; }
    }
}
