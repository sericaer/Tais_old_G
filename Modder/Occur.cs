using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modder
{
    class Occur
    {
        internal Occur()
        {

        }

        internal Occur(int? raw)
        {
            this.raw = raw;
        }

        internal int? raw;

        internal bool isTrue()
        {
            return Tools.GRandom.isOccur(100 / +raw.Value);
        }
    }
}
