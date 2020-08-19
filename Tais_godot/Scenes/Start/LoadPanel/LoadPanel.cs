using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaisGodot.Scripts
{
    class LoadPanel : Panel
    {
        private void _on_Button_Confirm_button_up()
        {
            GetNode<SavePanel>("LoadPanel").Visible = true;
        }
    }
}
