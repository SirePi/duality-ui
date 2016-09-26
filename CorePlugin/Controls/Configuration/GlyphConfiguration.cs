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
	public sealed class GlyphConfiguration
	{
		public static readonly GlyphConfiguration DEFAULT = new GlyphConfiguration();

		public ContentRef<Material> Glyph { get; set; }
		public Alignment Alignment { get; set; }
		public Border Margin { get; set; }

		public GlyphConfiguration()
		{
			this.Alignment = Alignment.Center;
			this.Margin = Border.Zero;
		}
	}
}
