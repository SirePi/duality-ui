// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public interface IInteractiveControl
	{
		void OnBlur();
		void OnFocus();
		void OnKeyboardKeyEvent(KeyboardKeyEventArgs args);
		void OnMouseButtonEvent(MouseButtonEventArgs args);
		void OnMouseEnterEvent();
		void OnMouseLeaveEvent();
	}
}
