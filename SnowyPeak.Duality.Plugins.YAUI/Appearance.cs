// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Editor;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Controls;
using SnowyPeak.Duality.Plugins.YAUI.Properties;

namespace SnowyPeak.Duality.Plugins.YAUI
{
	[EditorHintImage(ResNames.ImageAppearance)]
	[EditorHintCategory(ResNames.CategoryUI)]
	public class Appearance : Resource
	{
		public static readonly Appearance DEFAULT = new Appearance();

		public ContentRef<Material> Active { get; set; }
		public ContentRef<Material> Disabled { get; set; }
		public ContentRef<Material> Hover { get; set; }
		public ContentRef<Material> Normal { get; set; }
		public Border Border { get; set; }

		public Appearance()
		{
			this.Normal = ContentProvider.RequestContent<Material>(@"Default:Material:Checkerboard");
			this.Hover = ContentProvider.RequestContent<Material>(@"Default:Material:Checkerboard");
			this.Active = ContentProvider.RequestContent<Material>(@"Default:Material:Checkerboard");
			this.Disabled = ContentProvider.RequestContent<Material>(@"Default:Material:Checkerboard");
			this.Border = Border.Zero;
		}

		public Material this[Control.ControlStatus status]
		{
			get
			{
				if (status.HasFlag(Control.ControlStatus.Disabled))
				{ return this.Disabled.Res; }
				else if (status.HasFlag(Control.ControlStatus.Active))
				{ return this.Active.Res; }
				else if (status.HasFlag(Control.ControlStatus.Hover))
				{ return this.Hover.Res; }
				else
				{ return this.Normal.Res; }
			}
		}
	}
}
