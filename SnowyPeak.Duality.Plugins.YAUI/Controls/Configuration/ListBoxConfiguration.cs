// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration
{
	public struct ListBoxConfiguration
	{
		public static readonly Size DEFAULT_ITEMS_SIZE = new Size(20);

		public ContentRef<Appearance> ItemAppearance { get; set; }
		public Size ItemsSize { get; set; }

		public ListBoxConfiguration(ContentRef<Appearance>? itemAppearance = null, Size? itemsSize = null)
		{
			this.ItemsSize = itemsSize ?? DEFAULT_ITEMS_SIZE;
			this.ItemAppearance = itemAppearance ?? Appearance.DEFAULT;
		}

		public void SetItemAppearance(ContentRef<Appearance> appearance) { this.ItemAppearance = appearance; }		
		public void SetitemSize(Size size) { this.ItemsSize = size; }
	}
}
