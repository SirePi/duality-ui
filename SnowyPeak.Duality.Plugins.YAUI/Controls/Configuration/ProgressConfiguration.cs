// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration
{
	public struct ProgressConfiguration
	{
		public ContentRef<Appearance> BarAppearance { get; set; }

		public ProgressBar.BarStyle BarStyle { get; set; }
		public Direction Direction { get; set; }
		public Border Margin { get; set; }

		public ProgressConfiguration(ContentRef<Appearance>? barAppearance = null, Direction direction = Direction.LeftToRight, ProgressBar.BarStyle style = ProgressBar.BarStyle.Stretching, Border? margin = null)
		{
			this.BarAppearance = barAppearance ?? Appearance.DEFAULT;
			this.Direction = direction;
			this.BarStyle = style;
			this.Margin = margin ?? Border.Zero;
		}

		public void SetBarAppearance(ContentRef<Appearance> appearance) { this.BarAppearance = appearance; }
		public void SetBarStyle(ProgressBar.BarStyle style) { this.BarStyle = style; }
		public void SetDirection(Direction direction) { this.Direction = direction; }
		public void SetMargin(Border margin) { this.Margin = margin; }
	}
}
