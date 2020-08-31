using System;
namespace RunData
{
    public class Chaoting
    {
        public double requireTax;

        public Chaoting()
        {
            requireTax = 60;
        }

        internal static Chaoting Init()
        {
            return new Chaoting();
        }
    }
}
