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
		public Border ListBoxMargin { get; set; }
		public TextConfiguration TextConfiguration { get; set; }

		public ListBoxTemplate()
			: base()
		{
			this.ListBoxMargin = Border.Zero;
			this.ListBoxConfiguration = ListBoxConfiguration.DEFAULT;
			this.TextConfiguration = TextConfiguration.DEFAULT;
		}
	}
}