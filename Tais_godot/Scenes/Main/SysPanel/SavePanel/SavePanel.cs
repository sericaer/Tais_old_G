using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunData;

namespace TaisGodot.Scripts
{
    class SavePanel : Panel
    {
        private void _on_ButtonSave_button_up(string name)
        {
            //Root.Save(GlobalPath.save + name);
            QueueFree();
        }
    }
}
