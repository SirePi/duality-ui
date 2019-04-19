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
	public sealed class TextConfiguration
	{
		public static readonly TextConfiguration DEFAULT = new TextConfiguration();

		public Alignment Alignment { get; set; }
		public ColorRgba Color { get; set; }
		public ContentRef<Font> Font { get; set; }
		public Border Margin { get; set; }

		public TextConfiguration(ContentRef<Font>? font = null, ColorRgba? color = null, Alignment alignment = Alignment.Center, Border? margin = null)
		{
			this.Font = font ?? ContentProvider.RequestContent<Font>(@"Default:Font:GenericMonospace10");
			this.Color = color ?? ColorRgba.Black;
			this.Alignment = alignment;
			this.Margin = margin ?? Border.Zero;
		}

		public TextConfiguration Clone()
		{
			return new TextConfiguration()
			{
				Font = this.Font,
				Color = this.Color,
				Alignment = this.Alignment,
				Margin = this.Margin
			};
		}
	}
}
