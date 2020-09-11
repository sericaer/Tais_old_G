using System;
using DataVisit;

namespace RunData
{
    public class InitData
    {
        [DataVisitorProperty("name")]
        public string name;

        [DataVisitorProperty("age")]
        public int age;

        [DataVisitorProperty("background")]
        public string background;
    }
}
