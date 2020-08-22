using System;
using System.Collections.Generic;

namespace Modder
{
    public class DataVisit
    {
        public static void Init(string key, object value)
        {
            throw new NotImplementedException();
        }

        public DataVisit(string raw)
        {
            this.raw = raw;
        }

        public dynamic Get()
        {
            int intRslt;
            if(int.TryParse(raw, out intRslt))
            {
                return intRslt;
            }

            double doubleRslt;
            if (double.TryParse(raw, out doubleRslt))
            {
                return doubleRslt;
            }

            if(!raw.Contains("."))
            {
                return raw;
            }

            var splits = raw.Split('.');
            if(splits.Length < 2)
            {
                throw new Exception("error value " + raw);
            }

            return VisitGet(splits);
        }

        private dynamic VisitGet(string[] splits)
        {
            object gmObj;
            if (!dictObj.TryGetValue(splits[0], out gmObj))
            {
                gmObj = dictObj["common"];
            }

            throw new NotImplementedException();

        }

        private string raw;
        private static Dictionary<string, object> dictObj;
    }
}
