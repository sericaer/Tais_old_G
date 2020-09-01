﻿using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DataVisit;

namespace RunData
{
    public class ObservableValue<T> : RValue<T>
    {
        internal readonly IObservable<T> obs;
        private T _value;

        public override T Value { get { return _value; } }

        public ObservableValue(IObservable<T> param)
        {
            obs = param;

            obs.Subscribe(x => _value = x);
        }

        public IDisposable Subscribe(Action<T> action)
        {
            return obs.Subscribe(action);
        }
    }

    public class SubjectValue<T> : RWValue<T>
    {
        internal readonly BehaviorSubject<T> obs;

        public override T Value
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
