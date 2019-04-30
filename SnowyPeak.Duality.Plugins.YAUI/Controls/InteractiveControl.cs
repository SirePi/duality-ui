// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public abstract class InteractiveControl<T> : Control<T>, IInteractiveControl where T : ControlTemplate, new()
	{
		// Delegates
		public delegate void FocusChangeDelegate(Control control, bool isFocused);

		// Events
		[DontSerialize]
		private FocusChangeDelegate onFocusChange;
		public event FocusChangeDelegate OnFocusChange
		{
			add { this.onFocusChange += value; }
			remove { this.onFocusChange -= value; }
		}

		protected InteractiveControl(Skin skin, string templateName)
			: base(skin, templateName)
		{ }

		public virtual void OnBlur()
		{
			this.onFocusChange?.Invoke(this, false);
		}

		public virtual void OnFocus()
		{
			this.onFocusChange?.Invoke(this, true);
		}

		public virtual void OnKeyboardKeyEvent(KeyboardKeyEventArgs args)
		{ }

		public virtual void OnMouseButtonEvent(MouseButtonEventArgs args)
		{ }

		public virtual void OnMouseEnterEvent()
		{
			this.Status |= Control.ControlStatus.Hover;
		}

		public virtual void OnMouseLeaveEvent()
		{
			this.Status &= ~Control.ControlStatus.Hover;
		}
	}
}
