using Duality;
using Duality.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls
{
	public class CanvasPanel : ControlsContainer
	{
		internal override void _LayoutControls() 
		{
			foreach (Control c in this.Children)
			{
				c.ActualPosition = c.Position + this.Margin.TopLeft;
			}
		}
	}
}
