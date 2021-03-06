﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.
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
	public sealed class GlyphConfiguration
	{
		public static readonly GlyphConfiguration DEFAULT = new GlyphConfiguration();

		public Alignment Alignment { get; set; }
		public ContentRef<Material> Glyph { get; set; }
		public Border Margin { get; set; }

		public GlyphConfiguration(ContentRef<Material>? glyph = null, Alignment alignment = Alignment.Center, Border? margin = null)
		{
			this.Glyph = glyph ?? null;
			this.Alignment = alignment;
			this.Margin = margin ?? Border.Zero;
		}

		public GlyphConfiguration Clone()
		{
			return new GlyphConfiguration()
			{
				Glyph = this.Glyph,
				Alignment = this.Alignment,
				Margin = this.Margin
			};
		}
	}
}
