﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public sealed class DockPanel : ControlsContainer
	{
		public DockPanel(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		internal override void _LayoutControls()
		{
			Border centralArea = this.Margin;

			foreach (Control c in this.children.Where(c => c.Docking != Dock.Center))
			{
				switch (c.Docking)
				{
					case Dock.Left:
						c.ActualPosition.X = centralArea.Left;
						c.ActualPosition.Y = centralArea.Top;

						if (c.StretchToFill)
						{ c.ActualSize.Y = this.ActualSize.Y - centralArea.Top - centralArea.Bottom; }
						else
						{ c.ActualPosition.Y = centralArea.Top + ((this.ActualSize.Y - centralArea.Top - centralArea.Bottom - c.ActualSize.Y) / 2); }

						centralArea.Left += c.ActualSize.X;
						break;

					case Dock.Right:
						c.ActualPosition.X = this.ActualSize.X - centralArea.Right - c.ActualSize.X;
						c.ActualPosition.Y = centralArea.Top;

						if (c.StretchToFill)
						{ c.ActualSize.Y = this.ActualSize.Y - centralArea.Top - centralArea.Bottom; }
						else
						{ c.ActualPosition.Y = centralArea.Top + ((this.ActualSize.Y - centralArea.Top - centralArea.Bottom - c.ActualSize.Y) / 2); }

						centralArea.Right += c.ActualSize.X;
						break;

					case Dock.Top:
						c.ActualPosition.X = centralArea.Left;
						c.ActualPosition.Y = centralArea.Top;

						if (c.StretchToFill)
						{ c.ActualSize.X = this.ActualSize.X - centralArea.Left - centralArea.Right; }
						else
						{ c.ActualPosition.X = centralArea.Left + ((this.ActualSize.X - centralArea.Left - centralArea.Right - c.ActualSize.X) / 2); }

						centralArea.Top += c.ActualSize.Y;
						break;

					case Dock.Bottom:
						c.ActualPosition.X = centralArea.Left;
						c.ActualPosition.Y = this.ActualSize.Y - centralArea.Bottom - c.ActualSize.Y;

						if (c.StretchToFill)
						{ c.ActualSize.X = this.ActualSize.X - centralArea.Left - centralArea.Right; }
						else
						{ c.ActualPosition.X = centralArea.Left + ((this.ActualSize.X - centralArea.Left - centralArea.Right - c.ActualSize.X) / 2); }

						centralArea.Bottom += c.ActualSize.Y;
						break;
				}
			}

			foreach (Control c in this.children.Where(c => c.Docking == Dock.Center))
			{
				if (c.StretchToFill)
				{
					c.ActualSize.X = this.ActualSize.X - centralArea.Left - centralArea.Right;
					c.ActualSize.Y = this.ActualSize.Y - centralArea.Top - centralArea.Bottom;

					c.ActualPosition.X = centralArea.Left;
					c.ActualPosition.Y = centralArea.Top;
				}
				else
				{
					c.ActualPosition.X = centralArea.Left + ((this.ActualSize.X - centralArea.Left - centralArea.Right - c.ActualSize.X) / 2);
					c.ActualPosition.Y = centralArea.Top + ((this.ActualSize.Y - centralArea.Top - centralArea.Bottom - c.ActualSize.Y) / 2);
				}
			}
		}
	}
}
