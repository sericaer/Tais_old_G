using System;
namespace RunData
{
    public class CollectTaxProcess
    {
        public CollectTaxProcess()
        {
        }

        public static void Start()
        {
            inst = new CollectTaxProcess();
        }

        public static CollectTaxProcess inst;
        public double expectTax;
    }
}
