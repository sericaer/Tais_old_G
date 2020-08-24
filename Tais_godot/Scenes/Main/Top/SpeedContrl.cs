using Godot;
using System;

namespace TaisGodot.Scripts
{
	public class SpeedContrl : HBoxContainer
	{
		public static void UnPause()
		{
			isPause = false;
		}

		public static void Pause()
		{
			isPause = true;
		}

		public override void _Ready()
		{
			speed = 2;
		}

		private void _on_Button_Inc_pressed()
		{
			if (speed < MAX_SPEED)
			{
				speed++;
			}

			GetNode<Button>("Button_Dec").Disabled = false;
			if (speed == MAX_SPEED)
			{
				GetNode<Button>("Button_Inc").Disabled = true;
			}
		}
		private void _on_Button_Dec_pressed()
		{
			if (speed > MIN_SPEED)
			{
				speed--;
			}

			GetNode<Button>("Button_Inc").Disabled = false;
			if (speed == MIN_SPEED)
			{
				GetNode<Button>("Button_Dec").Disabled = true;
			}
		}

		private void _on_Timer_timeout()
		{
			if (isPause)
			{
				return;
			}

			RunData.Root.DaysInc();
			Modder.Mod.DaysInc();
		}

		private static int MAX_SPEED = 5;
		private static int MIN_SPEED = 1;

		private static bool isPause;
		private int speed
		{
			get
			{
				return _speed;
			}
			set
			{
				_speed = value;
				GetNode<Timer>("Timer").WaitTime = 2.0f / _speed;
			}
		}

		private static int _speed = 1;
	}
}
