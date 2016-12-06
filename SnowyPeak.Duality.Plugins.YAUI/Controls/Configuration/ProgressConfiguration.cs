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
	public sealed class ProgressConfiguration
	{
		public static readonly ProgressConfiguration DEFAULT = new ProgressConfiguration();

		public ContentRef<Appearance> BarAppearance { get; set; }

		public ProgressBar.BarStyle BarStyle { get; set; }
		public Direction Direction { get; set; }
		public Border Margin { get; set; }

		public ProgressConfiguration()
		{
			this.BarAppearance = Appearance.DEFAULT;
			this.Direction = Direction.LeftToRight;
			this.BarStyle = ProgressBar.BarStyle.Stretching;
			this.Margin = Border.Zero;
		}

		public ProgressConfiguration Clone()
		{
			return new ProgressConfiguration()
			{
				BarAppearance = this.BarAppearance,
				Direction = this.Direction,
				BarStyle = this.BarStyle,
				Margin = this.Margin
			};
		}
	}
}