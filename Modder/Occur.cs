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
            if (raw == null)
            {
                throw new Exception("event must have occur");
            }
        }

        int? raw;

        internal bool isTrue()
        {
            return Tools.GRandom.isOccur(100 / +raw.Value);
        }
    }
}
