// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Templates
{
	public class ControlTemplate
	{
		public ContentRef<Appearance> Appearance { get; set; }
		public Size MinSize { get; set; }

		public ControlTemplate()
		{
			this.Appearance = SnowyPeak.Duality.Plugins.YAUI.Appearance.DEFAULT;
		}
	}
}