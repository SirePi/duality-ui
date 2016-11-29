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
        public Border ListBoxMargin { get; set; }

        public ListBoxConfiguration ListBoxConfiguration { get; set; }

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