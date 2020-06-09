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
	public struct TextConfiguration
	{
		public Alignment Alignment { get; private set; }
		public ColorRgba Color { get; private set; }
		public ContentRef<Font> Font { get; private set; }
		public Border Margin { get; private set; }

		public TextConfiguration(ContentRef<Font>? font = null, ColorRgba? color = null, Alignment alignment = Alignment.Center, Border? margin = null)
		{
			this.Font = font ?? ContentProvider.RequestContent<Font>(@"Default:Font:GenericMonospace10");
			this.Color = color ?? ColorRgba.Black;
			this.Alignment = alignment;
			this.Margin = margin ?? Border.Zero;
		}

		public void SetAlignment(Alignment alignment) { this.Alignment = alignment; }
		public void SetColor(ColorRgba color) { this.Color = color; }
		public void SetFont(ContentRef<Font> font) { this.Font = font; }
		public void SetMargin(Border margin) { this.Margin = margin; }
	}
}
