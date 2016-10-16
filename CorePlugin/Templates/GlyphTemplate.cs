using Duality;
using SnowyPeak.DualityUI.Controls.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Templates
{
	public class GlyphTemplate : TextTemplate
	{
		public GlyphConfiguration GlyphConfiguration { get; set; }

		public GlyphTemplate()
            : base()
        {
			this.GlyphConfiguration = GlyphConfiguration.DEFAULT;
        }
	}
}
