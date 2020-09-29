using System;
using DataVisit;

namespace RunData
{
    public class ProcessCOM
    {
        [DataVisitorProperty("start")]
        public string start
        {
            set
            {
                Process.Start(value);
            }
        }

        [DataVisitorProperty("cancel")]
        public string cancel
        {
            set
            {
                Process.Cancel(value);
            }
        }
    }
}
