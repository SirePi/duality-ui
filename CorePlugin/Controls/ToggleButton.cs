using Duality;
using Duality.Drawing;
using Duality.Resources;
using SnowyPeak.DualityUI.Controls.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls
{
	public class ToggleButton : Button
	{
		public delegate void ToggleChangeEventDelegate(ToggleButton toggleButton, bool isToggled);
		public ToggleChangeEventDelegate ToggleChangeEventHandler { get; set; }

		private bool _isToggled;
		public bool Toggled 
		{
			get { return _isToggled; }
			set
			{
				_isToggled = value;

				if (this.ToggleChangeEventHandler != null)
				{ this.ToggleChangeEventHandler(this, _isToggled); }
			}
		}

		public ToggleButton()
		{
			this.MouseButtonEventHandler = (button, args) =>
			{
				if (args.Button == Duality.Input.MouseButton.Left && args.IsPressed)
				{
					this.Toggled = !this.Toggled;
				}
			};
		}

		public override void OnUpdate(float msFrame)
		{
			if (this.Toggled)
			{ this.Status |= ControlStatus.Active; }
			else
			{ this.Status &= ~ControlStatus.Active; }
		}
	}
}
