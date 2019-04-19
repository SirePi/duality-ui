// This code is provided under the MIT license. Originally by Alessandro Pilati.
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
		public static readonly Size DEFAULT_BUTTON_SIZE = new Size(10);
		public static readonly Size DEFAULT_CURSOR_SIZE = new Size(10);

		public ContentRef<Appearance> ButtonDecreaseAppearance { get; set; }
		public ContentRef<Appearance> ButtonIncreaseAppearance { get; set; }
		public Size ButtonsSize { get; set; }

		public ContentRef<Appearance> CursorAppearance { get; set; }
		public Size CursorSize { get; set; }

		public ScrollBarConfiguration(ContentRef<Appearance>? buttonIncreaseAppearance = null, ContentRef<Appearance>? buttonDecreaseAppearance = null, ContentRef<Appearance>? cursorAppearance = null, Size? buttonsSize = null, Size? cursorSize = null)
		{
			this.ButtonsSize = buttonsSize ?? DEFAULT_BUTTON_SIZE;
			this.CursorSize = cursorSize ?? DEFAULT_CURSOR_SIZE;

			this.ButtonIncreaseAppearance = buttonIncreaseAppearance ?? Appearance.DEFAULT;
			this.ButtonDecreaseAppearance = buttonDecreaseAppearance ?? Appearance.DEFAULT;
			this.CursorAppearance = cursorAppearance ?? Appearance.DEFAULT;
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
