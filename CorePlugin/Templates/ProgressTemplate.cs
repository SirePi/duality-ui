using Duality;
using SnowyPeak.DualityUI.Controls.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Templates
{
	public class ProgressTemplate : TextTemplate
	{
        public ProgressConfiguration ProgressConfiguration { get; set; }

        public ProgressTemplate()
            : base()
        {
            this.ProgressConfiguration = ProgressConfiguration.DEFAULT;
        }
	}
}
