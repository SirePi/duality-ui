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
		public Border ScrollBarMargin { get; set; }
        public ScrollBarConfiguration ScrollBarConfiguration { get; set; }

        public ScrollBarTemplate()
            : base()
        {
			this.ScrollBarMargin = Border.Zero;
            this.ScrollBarConfiguration = ScrollBarConfiguration.DEFAULT;
        }
    }
}
