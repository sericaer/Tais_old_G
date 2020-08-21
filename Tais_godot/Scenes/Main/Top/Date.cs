using Godot;
using System;

namespace TaisGodot.Scripts
{
    public class Date : Label
    {
        public static void Pause()
        {
            isPause = true;
        }

        public static void UnPause()
        {
            isPause = false;
        }

        public Date()
        {
            RunData.Date.inst.desc.Bind((value) => Text = TranslateServerEx.Translate("STATIC_DATE_VALUE", value.Split('-')));
        }

        private void _on_Timer_timeout()
        {
            if(isPause)
            {
                return;
            }

            RunData.Date.Inc();
            Modder.Mod.DaysInc();
        }

        private static bool isPause;
    }

}