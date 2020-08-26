using System;
using DataVisit;

namespace Modder.UnitTest
{
    public class Demon
    {
        public static Demon inst;
        public static object Init()
        {
            inst = new Demon();
            return inst;
        }

        [DataVisitorProperty]
        public Item1 item1;

        public Demon()
        {
            item1 = new Item1();
        }
    }

    public class Item1
    {
        [DataVisitorProperty("item1.data1")]
        public int data1;

        [DataVisitorProperty("item1.data2")]
        public int data2;
    }
}
