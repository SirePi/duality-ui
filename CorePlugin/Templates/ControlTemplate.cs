using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Templates
{
	public class ControlTemplate
	{
		public Size MinSize { get; set; }
		public ContentRef<Appearance> Appearance { get; set; }

        public ControlTemplate()
        {
            this.Appearance = DualityUI.Appearance.DEFAULT;
        }
	}
}
