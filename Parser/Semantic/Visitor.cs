﻿using System;
namespace Parser.Semantic
{
    public static class Visitor
    {
        public static Func<string, dynamic> GetValue = (raw) =>
         {
             return raw;
         };

        public static Action<string, object> SetValue = (p1, p2) => { };
    }
}
