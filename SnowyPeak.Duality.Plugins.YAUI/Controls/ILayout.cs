// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public interface ILayout
	{
		bool IsPassthrough { get; }
		Control FindHoveredControl(Vector2 position);
		void LayoutControls();
	}
}
