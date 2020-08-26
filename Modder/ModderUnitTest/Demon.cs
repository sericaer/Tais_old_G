using System;
using System.Collections.Generic;
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

        [DataVisitorPropertyArray("depart")]
        public List<Depart> departs;

        public Demon()
        {
            item1 = new Item1();

            departs = new List<Depart>() { new Depart(),
                                           new Depart()};
        }
    }

    public class Item1
    {
        [DataVisitorProperty("item1.data1")]
        public int data1;

        [DataVisitorProperty("item1.data2")]
        public int data2;
    }

    public class Depart
    {
        [DataVisitorProperty("depart.data1")]
        public int data1;

        [DataVisitorProperty("depart.data2")]
        public int data2;
    }
}
