using Godot;
using System;

namespace TaisGodot.Scripts
{
    public class Date : Label
    {
        public Date()
        {
            RunData.Date.inst.desc.Bind((value) => Text = TranslateServerEx.Translate("STATIC_DATE_VALUE", value.Split('-')));
        }

        public override void _EnterTree()
        {
            GD.Print("_EnterTree");
        }

        public override void _ExitTree()
        {
            GD.Print("_ExitTree");
        }

        private void _on_Timer_timeout()
        {
            RunData.Date.Inc();
            Modder.Mod.DaysInc();
        }
    }

}