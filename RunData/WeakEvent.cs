using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RunData
{
    internal class WeakEvent<TEventArgs>
    {
        private class Unit
        {
            private WeakReference reference;
            private MethodInfo method;
            private bool isStatic;
            public string Name;

            public bool IsDead
            {
                get
                {
                    return !this.isStatic && !this.reference.IsAlive;
                }
            }

            public Unit(Action<TEventArgs> callback)
            {
                this.isStatic = callback.Target == null;
                this.reference = new WeakReference(callback.Target);
                this.method = callback.Method;
                this.Name = callback.Target.ToString(); 
            }

            public bool Equals(Action<TEventArgs> callback)
            {
                return this.reference.Target == callback.Target && this.method == callback.Method;
            }

            public void Invoke(object[] args)
            {
                this.method.Invoke(this.reference.Target, args);
            }
        }

        private List<Unit> list = new List<Unit>();

        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        public void Add(Action<TEventArgs> callback)
        {
            this.list.Add(new Unit(callback));
        }

        public void Remove(Action<TEventArgs> callback)
        {
            for (int i = this.list.Count - 1; i > -1; i--)
            {
                if (this.list[i].Equals(callback))
                {
                    this.list.RemoveAt(i);
                }
            }
        }

        public void Invoke(TEventArgs args)
        {
            object[] ARGS = { args };

            for (int i = this.list.Count - 1; i > -1; i--)
            {
                try
                {
                    if (this.list[i].IsDead)
                    {
                        Root.logger($"Remove {this.list[i].Name}");
                        this.list.RemoveAt(i);
                    }
                    else
                    {
                        this.list[i].Invoke(ARGS);
                    }
                }
                catch(TargetInvocationException)
                {
                    Root.logger($"Remove {this.list[i].Name}");
                    this.list.RemoveAt(i);
                }

            }
        }

        public void Clear()
        {
            this.list.Clear();
        }
    }
}
