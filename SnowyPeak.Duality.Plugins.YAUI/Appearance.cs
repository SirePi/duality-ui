// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Controls;
using SnowyPeak.Duality.Plugins.YAUI.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI
{
	[EditorHintImage(ResNames.ImageAppearance)]
	[EditorHintCategory(ResNames.CategoryUI)]
	public class Appearance : Resource
	{
		public static readonly Appearance DEFAULT = new Appearance();

		public ContentRef<Material> Active;
		public ContentRef<Material> Disabled;
		public ContentRef<Material> Hover;
		public ContentRef<Material> Normal;
		public Border Border { get; set; }

		public Appearance()
		{
			Normal = ContentProvider.RequestContent<Material>(@"Default:Material:Checkerboard");
			Hover = ContentProvider.RequestContent<Material>(@"Default:Material:Checkerboard");
			Active = ContentProvider.RequestContent<Material>(@"Default:Material:Checkerboard");
			Disabled = ContentProvider.RequestContent<Material>(@"Default:Material:Checkerboard");
			Border = Border.Zero;
		}

		public Material this[Control.ControlStatus status]
		{
			get
			{
				if ((status & Control.ControlStatus.Disabled) != Control.ControlStatus.None)
				{ return this.Disabled.Res; }
				else if ((status & Control.ControlStatus.Active) != Control.ControlStatus.None)
				{ return this.Active.Res; }
				else if ((status & Control.ControlStatus.Hover) != Control.ControlStatus.None)
				{ return this.Hover.Res; }
				else
				{ return this.Normal.Res; }
			}
		}
	}
}