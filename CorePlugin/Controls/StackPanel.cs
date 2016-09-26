﻿using Duality;
using Duality.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls
{
	public class StackPanel : ControlsContainer
	{
		public Orientation Orientation { get; set; }

		private float _start;

		internal override void _LayoutControls()
		{
			if (this.Orientation == Orientation.Horizontal)
			{ StackHorizontal(); }
			else // Vertical
			{ StackVertical(); }
		}

		private void StackHorizontal()
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

		private void StackVertical()
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
	}
}
