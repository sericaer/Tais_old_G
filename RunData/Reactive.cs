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
            this.weakEvent = new WeakEvent<T>();
            this.value = value;
        }

        public void Bind(Action<T> act)
        {
            weakEvent.Add(act);
            act.Invoke(_value);
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
                    weakEvent.Invoke(_value);
                }

            }
        }

        internal T _value;
        internal WeakEvent<T> weakEvent;

        internal static Reactive<T> From(Reactive<T> src, Func<T, T> convert)
        {
            var dst = new Reactive<T>(convert(src.value));

            src.Bind((src_value) =>
            {
                dst.value = convert(src_value);
            });

            return dst;
        }
    }
}
