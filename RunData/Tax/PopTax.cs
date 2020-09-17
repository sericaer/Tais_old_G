using System;
namespace RunData
{
    public class PopTax
    {
        public static int maxTaxLevel = 5;

        public static double CalcTax(int level)
        {
            switch(level)
            {
                case 1:
                    return 0.001;
                case 2:
                    return 0.002;
                case 3:
                    return 0.003;
                case 4:
                    return 0.0035;
                case 5:
                    return 0.004;
                default:
                    throw new Exception($"not supoort level {level}");
            }
        }
    }
}
