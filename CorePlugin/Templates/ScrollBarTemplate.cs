using Duality;
using SnowyPeak.DualityUI.Controls.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Templates
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
