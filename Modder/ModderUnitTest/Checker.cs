using System;
using Modder;

namespace Modder.UnitTest
{
    public class Checker
    {
        public static void RecvEvent(GEvent gevent)
        {
            currEvent = gevent;
        }

        public static GEvent currEvent;
    }
}
