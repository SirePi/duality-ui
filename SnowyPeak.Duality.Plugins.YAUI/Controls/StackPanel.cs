// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public sealed class StackPanel : ControlsContainer
	{
		public enum Stacking
		{
			LeftToRight,
			RightToLeft
		}

		private float start;
		public Direction Direction { get; set; }

		public StackPanel(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		internal override void _LayoutControls()
		{
			switch (this.Direction)
			{
				case YAUI.Direction.LeftToRight:
					this.StackFromLeft();
					break;

				case YAUI.Direction.RightToLeft:
					this.StackFromRight();
					break;

				case YAUI.Direction.UpToDown:
					this.StackFromTop();
					break;

				case YAUI.Direction.DownToUp:
					this.StackFromBottom();
					break;
			}
		}

		private void StackFromLeft()
		{
			this.start = this.Margin.Left;

			foreach (Control c in this.children)
			{
				c.ActualPosition.X = this.start;
				c.ActualPosition.Y = this.Margin.Top;

				if (c.StretchToFill)
				{ c.ActualSize.Y = this.ActualSize.Y - this.Margin.Top - this.Margin.Bottom; }
				else
				{ c.ActualPosition.Y = (this.ActualSize.Y - c.ActualSize.Y) / 2; }

				this.start += c.ActualSize.X;
			}
		}

		private void StackFromRight()
		{
			this.start = this.ActualSize.X - this.Margin.Right;

			foreach (Control c in this.children)
			{
				c.ActualPosition.X = this.start - c.ActualSize.X;
				c.ActualPosition.Y = this.Margin.Top;

				if (c.StretchToFill)
				{ c.ActualSize.Y = this.ActualSize.Y - this.Margin.Top - this.Margin.Bottom; }
				else
				{ c.ActualPosition.Y = (this.ActualSize.Y - c.ActualSize.Y) / 2; }

				this.start -= c.ActualSize.X;
			}
		}

		private void StackFromTop()
		{
			this.start = this.Margin.Top;
			foreach (Control c in this.children)
			{
				c.ActualPosition.X = this.Margin.Left;
				c.ActualPosition.Y = this.start;

				if (c.StretchToFill)
				{ c.ActualSize.X = this.ActualSize.X - this.Margin.Left - this.Margin.Right; }
				else
				{ c.ActualPosition.X = (this.ActualSize.X - c.ActualSize.X) / 2; }

				this.start += c.ActualSize.Y;
			}
		}

		private void StackFromBottom()
		{
			this.start = this.ActualSize.Y - this.Margin.Bottom;
			foreach (Control c in this.children)
			{
				c.ActualPosition.X = this.Margin.Left;
				c.ActualPosition.Y = this.start - c.ActualSize.Y;

				if (c.StretchToFill)
				{ c.ActualSize.X = this.ActualSize.X - this.Margin.Left - this.Margin.Right; }
				else
				{ c.ActualPosition.X = (this.ActualSize.X - c.ActualSize.X) / 2; }

				this.start -= c.ActualSize.Y;
			}
		}
	}
}
