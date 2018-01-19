// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public class ToggleButton : Button
	{
		private bool _isMouseOver;
		private bool _isMousePressed;
		private bool _isToggled;

		public ToggleChangeEventDelegate ToggleChangeEventHandler { get; set; }

		public bool Toggled
		{
			get { return _isToggled; }
			set
			{
                if (_isToggled != value && this.ToggleChangeEventHandler != null)
                { this.ToggleChangeEventHandler(this, _isToggled, value); }

                _isToggled = value;
			}
		}

		public delegate void ToggleChangeEventDelegate(ToggleButton toggleButton, bool previousValue, bool newValue);

		public ToggleButton(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			this.MouseButtonEventHandler = (button, args) =>
			{
				if (args.Button == MouseButton.Left)
				{
					_isMousePressed = args.IsPressed;
					if (args.IsPressed) this.Toggled = !this.Toggled;
				}
			};

			ApplySkin(_baseSkin);
		}

		public override void OnMouseEnterEvent()
		{
			base.OnMouseEnterEvent();

			_isMouseOver = true;
		}

		public override void OnMouseLeaveEvent()
		{
			base.OnMouseLeaveEvent();

			_isMouseOver = false;
			_isMousePressed = false;
		}

		public override void OnUpdate(float msFrame)
		{
            base.OnUpdate(msFrame);

			if (!_isMouseOver)
			{
				if (this.Toggled)
				{ this.Status |= ControlStatus.Active; }
				else
				{ this.Status &= ~ControlStatus.Active; }
			}
			else
			{
				if (_isMousePressed)
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