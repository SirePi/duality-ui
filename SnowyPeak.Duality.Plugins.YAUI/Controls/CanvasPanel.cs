// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public class CanvasPanel : ControlsContainer
	{
		//Required to allow direct modification of single fields
#pragma warning disable S1104 // Fields should not have public accessibility
		public Vector2 Offset;
#pragma warning restore S1104 // Fields should not have public accessibility

		public CanvasPanel(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		internal override void _LayoutControls()
		{
			foreach (Control c in this.Children)
			{ c.ActualPosition = c.Position + this.Offset + this.Margin.TopLeft; }
		}
	}
}
