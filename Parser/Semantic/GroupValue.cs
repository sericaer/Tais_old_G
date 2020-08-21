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
        List<string> datas = new List<String>();

        public GroupValue(params string[] Params)
        {
            datas.AddRange(Params);
        }

        public IEnumerator<object> GetEnumerator()
        {
            for(int i=0; i< datas.Count(); i++)
            {
                yield return Visitor.GetValue(datas[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < datas.Count(); i++)
            {
                yield return Visitor.GetValue(datas[i]);
            }
        }
    }
}