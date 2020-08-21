using Godot;
using RunData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaisGodot.Scripts
{
    public class ReactiveLabel : Label
    {
        internal void Assoc<T>(Reactive<T> data, Func<string, string> adpt = null)
        {
            this.adpt = adpt;
            data.Bind(SetValue);
        }

        private Func<string, string> adpt;

        private void SetValue(string value)
        { 
            Text = adpt!=null ? adpt(value) : value;
        }
    }
}
