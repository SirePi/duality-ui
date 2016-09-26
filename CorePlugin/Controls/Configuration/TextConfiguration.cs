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
	public sealed class TextConfiguration
	{
		public static readonly TextConfiguration DEFAULT = new TextConfiguration();

		public ContentRef<Font> Font { get; set; }
		public ColorRgba Color { get; set; }
		public Alignment Alignment { get; set; }
		public Border Margin { get; set; }

		public TextConfiguration()
		{
			this.Font = ContentProvider.RequestContent<Font>(@"Default:Font:GenericMonospace10");
			this.Color = ColorRgba.Black;
			this.Alignment = Alignment.Center;
			this.Margin = Border.Zero;
		}
	}
}
