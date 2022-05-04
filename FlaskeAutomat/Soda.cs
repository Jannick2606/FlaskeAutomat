using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomat
{
    class Soda : Drink
    {
        public Soda(string name, int nr)
        {
            Type = name;
            LabelNr = nr;
        }
        public override string Type { get; protected set; }
        public override int LabelNr { get; protected set; }
    }
}
