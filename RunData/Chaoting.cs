using System;
namespace RunData
{
    public class Chaoting
    {
        public SubjectValue<double> requireTax;

        public Chaoting()
        {
            requireTax = new SubjectValue<double>(60);
        }

        internal static Chaoting Init()
        {
            return new Chaoting();
        }
    }
}
