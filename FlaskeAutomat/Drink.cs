using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomat
{
    abstract class Drink
    {
        public abstract string Type { get; protected set; }
        public abstract int LabelNr { get; protected set;}
    }
}
