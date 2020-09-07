using DataVisit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunData
{
    public class Taishou
    {
        [DataVisitorProperty("is_revoke")]
        public bool isRevoke;

        public Taishou()
        {
            isRevoke = false;
        }
    }
}
