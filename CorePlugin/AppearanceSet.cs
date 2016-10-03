using Duality;
using DualityUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DualityUI
{
	public class AppearanceSet
	{
		public static readonly AppearanceSet DEFAULT = new AppearanceSet();

		public ContentRef<Appearance> Normal;
		public ContentRef<Appearance> Hover;
		public ContentRef<Appearance> Active;
		public ContentRef<Appearance> Disabled;

		public Border Border { get; set; }

		public AppearanceSet()
		{
			Normal = Appearance.DEFAULT;
			Hover = Appearance.DEFAULT;
			Active = Appearance.DEFAULT;
			Disabled = Appearance.DEFAULT;
			Border = Border.Zero;
		}

		public Appearance this[Control.ControlStatus status]
		{
			get
			{
				if ((status & Control.ControlStatus.Active) != Control.ControlStatus.None)
				{
					return this.Active.Res;
				}
				else if ((status & Control.ControlStatus.Hover) != Control.ControlStatus.None)
				{
					return this.Hover.Res;
				}
				else if ((status & Control.ControlStatus.Disabled) != Control.ControlStatus.None)
				{
					return this.Disabled.Res;
				}
				else
				{
					return this.Normal.Res;
				}
			}
		}
	}
}
