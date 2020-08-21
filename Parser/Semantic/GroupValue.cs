using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Semantic
{
    public class GroupValue : IEnumerable<object>
    {
        List<DataVisit> dataVisitors = new List<DataVisit>();

        public GroupValue(params string[] Params)
        {
            dataVisitors.AddRange(Params.Select(x => new DataVisit(x)));
        }

        public IEnumerator<object> GetEnumerator()
        {
            for(int i=0; i< dataVisitors.Count(); i++)
            {
                yield return dataVisitors[i].Get();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < dataVisitors.Count(); i++)
            {
                yield return dataVisitors[i].Get();
            }
        }
    }
}