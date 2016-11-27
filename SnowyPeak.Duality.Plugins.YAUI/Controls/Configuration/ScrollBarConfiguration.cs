using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration
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
			this.ButtonsSize = new Size(10);
			this.CursorSize = new Size(10);

			this.ButtonIncreaseAppearance = Appearance.DEFAULT;
			this.ButtonDecreaseAppearance = Appearance.DEFAULT;
			this.CursorAppearance = Appearance.DEFAULT;
		}

        public ScrollBarConfiguration Clone()
        {
            return new ScrollBarConfiguration()
            {
                ButtonsSize = this.ButtonsSize,
                CursorSize = this.CursorSize,
                ButtonIncreaseAppearance = this.ButtonIncreaseAppearance,
                ButtonDecreaseAppearance = this.ButtonDecreaseAppearance,
                CursorAppearance = this.CursorAppearance
            };
        }
	}
}
