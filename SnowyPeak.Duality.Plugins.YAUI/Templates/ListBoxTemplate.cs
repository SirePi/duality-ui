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
	public class ListBoxTemplate : ControlTemplate
	{
		public ListBoxConfiguration ListBoxConfiguration { get; set; }
		public Border ListBoxMargin { get; set; } = Border.Zero;
		public TextConfiguration TextConfiguration { get; set; }

		public ListBoxTemplate() : base(null) { }
		public ListBoxTemplate(ControlTemplate source)
			: base(source)
		{
			if (source is ListBoxTemplate lbt)
			{
				this.ListBoxMargin = lbt.ListBoxMargin;
				this.ListBoxConfiguration = lbt.ListBoxConfiguration;
				this.TextConfiguration = lbt.TextConfiguration;
			}
		}
	}
}
