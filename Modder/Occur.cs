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

        internal Occur(bool defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        internal bool Rslt()
        {
            if(defaultValue != null)
            {
                return defaultValue.Value;
            }
            throw new NotImplementedException();
        }

        bool? defaultValue;
    }
}
