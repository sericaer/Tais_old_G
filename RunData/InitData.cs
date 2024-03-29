﻿using System;
using DataVisit;

namespace RunData
{
    public class InitData
    {
        [DataVisitorProperty("init")]
        public Common common;

        public class Common
        {
            [DataVisitorProperty("name")]
            public string name;

            [DataVisitorProperty("age")]
            public int age;

            [DataVisitorProperty("party")]
            public string party;
        }
    }
}
