using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunData;

namespace TaisGodot.Scripts
{
    class MsgboxPanel : Panel
    {
        public string desc;
        public Action action;

        public override void _Ready()
        {
            GetNode<Label>("Desc").Text = desc;
        }

        private void _on_Button_Confirm_pressed()
        {
            action?.Invoke();
            QueueFree();
        }
    }
}
