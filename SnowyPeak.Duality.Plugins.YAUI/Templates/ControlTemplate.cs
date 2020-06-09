// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;

namespace SnowyPeak.Duality.Plugins.YAUI.Templates
{
	public class ControlTemplate
	{
		public static readonly ControlTemplate Empty = new ControlTemplate
		{
			Appearance = null,
			Margin = Border.Zero,
			MinSize = Size.Zero
		};

		public ContentRef<Appearance> Appearance { get; set; }
		public Border Margin { get; set; }
		public Size MinSize { get; set; }

		public ControlTemplate() : this(null) { }
		public ControlTemplate(ControlTemplate source)
		{
			if (source == null)
			{
				this.Appearance = YAUI.Appearance.DEFAULT;
				this.Margin = Border.Zero;
				this.MinSize = Size.Zero;
			}
			else
			{
				this.Appearance = source.Appearance;
				this.Margin = source.Margin;
				this.MinSize = source.MinSize;
			}
		}
	}
}
