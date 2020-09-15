using Godot;
using System;

namespace TaisGodot.Scripts
{
    internal class ReportPopNumDialog : SpecialEventDialog
    {
        public ReportPopNumDialog()
        {
        }

        public override bool IsVaild()
        {
            return RunData.Date.inst == (null, 8, 1);
        }
    }
}
