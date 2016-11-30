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
	public class TextTemplate : ControlTemplate
	{
		public TextConfiguration TextConfiguration { get; set; }

		public TextTemplate()
			: base()
		{
			this.TextConfiguration = TextConfiguration.DEFAULT;
		}
	}
}