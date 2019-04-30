// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public sealed class ToggleButton : Button
	{
		private bool isMouseOver;
		private bool isMousePressed;
		private bool isToggled;

		public bool Toggled
		{
			get => this.isToggled;
			set
			{
				if (this.isToggled != value)
				{ this.onToggleChange?.Invoke(this, this.isToggled, value); }

				this.isToggled = value;
			}
		}

		// Delegates
		public delegate void ToggleChangeEventDelegate(ToggleButton toggleButton, bool previousValue, bool newValue);

		// Events
		[DontSerialize]
		private ToggleChangeEventDelegate onToggleChange;
		public event ToggleChangeEventDelegate OnToggleChange
		{
			add { this.onToggleChange += value; }
			remove { this.onToggleChange += value; }
		}

		public ToggleButton(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		protected override void Init()
		{
			base.Init();
			this.OnMouseButton += this.ToggleButton_OnMouseButton;
		}

		private void ToggleButton_OnMouseButton(IInteractiveControl button, MouseButtonEventArgs args)
		{
			if (args.Button == MouseButton.Left)
			{
				this.isMousePressed = args.IsPressed;
				if (args.IsPressed) this.Toggled = !this.Toggled;
			}
		}

		public override void OnMouseEnterEvent()
		{
			base.OnMouseEnterEvent();

			this.isMouseOver = true;
		}

		public override void OnMouseLeaveEvent()
		{
			base.OnMouseLeaveEvent();

			this.isMouseOver = false;
			this.isMousePressed = false;
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			if (!this.isMouseOver)
			{
				if (this.Toggled)
				{ this.Status |= ControlStatus.Active; }
				else
				{ this.Status &= ~ControlStatus.Active; }
			}
			else
			{
				if (this.isMousePressed)
				{
					this.Status &= ~ControlStatus.Hover;

					if (this.Toggled)
					{ this.Status |= ControlStatus.Active; }
					else
					{ this.Status &= ~ControlStatus.Active; }
				}
				else
				{
					this.Status &= ~ControlStatus.Active;
					this.Status |= ControlStatus.Hover;
				}
			}
		}
	}
}
