// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration
{
	public struct ScrollBarConfiguration
	{
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

		public void SetButtonDecreaseAppearance(ContentRef<Appearance> appearance) { this.ButtonDecreaseAppearance = appearance; }
		public void SetButtonIncreaseAppearance(ContentRef<Appearance> appearance) { this.ButtonIncreaseAppearance = appearance; }
		public void SetCursorAppearance(ContentRef<Appearance> appearance) { this.CursorAppearance = appearance; }
		public void SetButtonsSize(Size size) { this.ButtonsSize = size; }
		public void SetCursorSize(Size size) { this.CursorSize = size; }
	}
}
