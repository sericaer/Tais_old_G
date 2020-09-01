using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RunData
{
    public class ObservableValue<T>
    {
        internal readonly IObservable<T> obs;

        public T value { get; private set; }

        public ObservableValue(IObservable<T> param)
        {
            obs = param;

            obs.Subscribe(x => value = x);
        }

        public IDisposable Subscribe(Action<T> action)
        {
            return obs.Subscribe(action);
        }
    }

    public class SubjectValue<T>
    {
        internal readonly BehaviorSubject<T> obs;

        public T Value
        {
            get
            {
                return obs.Value;
            }
            set
            {
                obs.OnNext(value);
            }
        }

        public SubjectValue(T param)
        {
            obs = new BehaviorSubject<T>(param);
        }

        public IDisposable Subscribe(Action<T> action)
        {
            return obs.Subscribe(action);
        }
    }
}
