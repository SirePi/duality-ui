using Duality;
using Duality.Drawing;
using Duality.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls.Configuration
{
	public sealed class ProgressConfiguration
	{
        public static readonly ProgressConfiguration DEFAULT = new ProgressConfiguration();

		public ContentRef<Appearance> Bar { get; set; }
		public ProgressBar.Direction Direction { get; set; }
		public ProgressBar.BarStyle BarStyle { get; set; }
		public Border Margin { get; set; }

        public ProgressConfiguration()
		{
            this.Bar = Appearance.DEFAULT;
			this.Direction = ProgressBar.Direction.LeftToRight;
			this.BarStyle = ProgressBar.BarStyle.Stretching;
			this.Margin = Border.Zero;
		}
	}
}
