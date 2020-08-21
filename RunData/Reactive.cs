using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunData
{
    public class Reactive<T>
    {
        public Reactive(T value)
        {
            this.value = value;
            this.weakEvent = new WeakEvent<string>();
        }

        public void Bind(Action<string> act)
        {
            weakEvent.Add(act);
            act.Invoke(_value.ToString());
        }

        public T value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == null || !_value.Equals(value))
                {
                    _value = value;
                    weakEvent.Invoke(_value.ToString());
                }

            }
        }

        internal T _value;
        internal WeakEvent<string> weakEvent;
    }
}
