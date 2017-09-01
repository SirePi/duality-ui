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
	public class StackPanel : ControlsContainer
	{
		public enum Stacking
		{
			LeftToRight,
			RightToLeft
		}

		private float _start;
		public Direction Direction { get; set; }

		public StackPanel(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			ApplySkin(_baseSkin);
		}

		internal override void _LayoutControls()
		{
			switch (this.Direction)
			{
				case YAUI.Direction.LeftToRight:
					StackFromLeft();
					break;

				case YAUI.Direction.RightToLeft:
					StackFromRight();
					break;

				case YAUI.Direction.UpToDown:
					StackFromTop();
					break;

				case YAUI.Direction.DownToUp:
					StackFromBottom();
					break;
			}
		}

		private void StackFromLeft()
		{
			_start = this.Margin.Left;

			foreach (Control c in this.Children)
			{
				c.ActualPosition.X = _start;
				c.ActualPosition.Y = this.Margin.Top;

				if (c.StretchToFill)
				{ c.ActualSize.Y = this.ActualSize.Y - this.Margin.Top - this.Margin.Bottom; }
				else
				{ c.ActualPosition.Y = (this.ActualSize.Y - c.ActualSize.Y) / 2; }

				_start += c.ActualSize.X;
			}
		}

		private void StackFromRight()
		{
			_start = this.ActualSize.X - this.Margin.Right;

			foreach (Control c in this.Children)
			{
				c.ActualPosition.X = _start - c.ActualSize.X;
				c.ActualPosition.Y = this.Margin.Top;

				if (c.StretchToFill)
				{ c.ActualSize.Y = this.ActualSize.Y - this.Margin.Top - this.Margin.Bottom; }
				else
				{ c.ActualPosition.Y = (this.ActualSize.Y - c.ActualSize.Y) / 2; }

				_start -= c.ActualSize.X;
			}
		}

		private void StackFromTop()
		{
			_start = this.Margin.Top;
			foreach (Control c in this.Children)
			{
				c.ActualPosition.X = this.Margin.Left;
				c.ActualPosition.Y = _start;

				if (c.StretchToFill)
				{ c.ActualSize.X = this.ActualSize.X - this.Margin.Left - this.Margin.Right; }
				else
				{ c.ActualPosition.X = (this.ActualSize.X - c.ActualSize.X) / 2; }

				_start += c.ActualSize.Y;
			}
		}

		private void StackFromBottom()
		{
			_start = this.ActualSize.Y - this.Margin.Bottom;
			foreach (Control c in this.Children)
			{
				c.ActualPosition.X = this.Margin.Left;
				c.ActualPosition.Y = _start - c.ActualSize.Y;

				if (c.StretchToFill)
				{ c.ActualSize.X = this.ActualSize.X - this.Margin.Left - this.Margin.Right; }
				else
				{ c.ActualPosition.X = (this.ActualSize.X - c.ActualSize.X) / 2; }

				_start -= c.ActualSize.Y;
			}
		}
	}
}