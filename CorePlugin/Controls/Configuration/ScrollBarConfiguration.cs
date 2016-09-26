using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls.Configuration
{
	public sealed class ScrollBarConfiguration
	{
		public static readonly ScrollBarConfiguration DEFAULT = new ScrollBarConfiguration();

		public Size ButtonsSize { get; set; }
		public Size CursorSize { get; set; }

		public ContentRef<Appearance> ButtonIncreaseAppearance { get; set; }
		public ContentRef<Appearance> ButtonDecreaseAppearance { get; set; }
		public ContentRef<Appearance> CursorAppearance { get; set; }

		public ScrollBarConfiguration()
		{
			this.ButtonsSize = new Size(20);
			this.CursorSize = new Size(20);

			this.ButtonIncreaseAppearance = new ContentRef<Appearance>(SnowyPeak.DualityUI.Appearance.DEFAULT);
			this.ButtonDecreaseAppearance = new ContentRef<Appearance>(SnowyPeak.DualityUI.Appearance.DEFAULT);
			this.CursorAppearance = new ContentRef<Appearance>(SnowyPeak.DualityUI.Appearance.DEFAULT);
		}
	}
}
