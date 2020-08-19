using Godot;
using RunData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaisGodot.Scripts
{
    class ReactiveLabel : Label
    {
        internal void Assoc<T>(Reactive<T> data)
        {
            data.Bind((value) => Text = value);
        }
    }
}
