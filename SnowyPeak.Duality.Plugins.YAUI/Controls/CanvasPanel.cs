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
		public Vector2 Offset;

		public CanvasPanel(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			this.ApplySkin(this.baseSkin);
		}

		internal override void _LayoutControls()
		{
			foreach (Control c in this.Children)
			{ c.ActualPosition = c.Position + this.Offset + this.Margin.TopLeft; }
		}
	}
}
