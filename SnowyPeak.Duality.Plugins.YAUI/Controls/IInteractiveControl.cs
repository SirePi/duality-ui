// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality.Input;

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
