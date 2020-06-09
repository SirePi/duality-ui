// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Templates
{
	public class ScrollBarTemplate : ControlTemplate
	{
		public ScrollBarConfiguration ScrollBarConfiguration { get; set; }
		public Border ScrollBarMargin { get; set; } = Border.Zero;

		public ScrollBarTemplate() : this(null) { }
		public ScrollBarTemplate(ControlTemplate source)
			: base(source)
		{
			if (source is ScrollBarTemplate sbt)
			{
				this.ScrollBarMargin = sbt.ScrollBarMargin;
				this.ScrollBarConfiguration = sbt.ScrollBarConfiguration;
			}
		}
	}
}
