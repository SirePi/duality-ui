using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration
{
    public sealed class ListBoxConfiguration
    {
        public static readonly ListBoxConfiguration DEFAULT = new ListBoxConfiguration();

        public Size ItemsSize { get; set; }

        public ContentRef<Appearance> ItemAppearance { get; set; }

        public ListBoxConfiguration()
        {
            this.ItemsSize = new Size(20);
            this.ItemAppearance = Appearance.DEFAULT;
        }

        public ListBoxConfiguration Clone()
        {
            return new ListBoxConfiguration()
            {
                ItemsSize = this.ItemsSize,
                ItemAppearance = this.ItemAppearance
            };
        }
    }
}