using Godot;
using System;
namespace TaisGodot.Scripts
{
    public class Economy : PanelContainer
    {
        public override void _Ready()
        {
            GetNode<ReactiveLabel>("Button/Economy/Value").Assoc(RunData.Economy.inst.curr);
        }
    }
}
