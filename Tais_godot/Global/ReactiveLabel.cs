using Godot;
using RunData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaisGodot.Scripts
{
	public class ReactiveProgressBar : ProgressBar
	{
		private IDisposable reactiveDispose;

		internal void Assoc(ObservableValue<double> data)
		{
			reactiveDispose = data.Subscribe(this.SetProgressValue);
		}

		internal void Assoc(SubjectValue<double> data)
		{
			reactiveDispose = data.Subscribe(this.SetProgressValue);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			reactiveDispose?.Dispose();
		}


		private void SetProgressValue(double value)
		{
			Value = value;
		}
	}
}
