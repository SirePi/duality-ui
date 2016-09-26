using Duality;
using Duality.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls
{
	public class GridPanel : ControlsContainer
	{
		public struct GridInfo
		{
			public int Row;
			public int Column;
			public int RowSpan;
			public int ColSpan;
		}

        public float[] Rows { get; set; }
        public float[] Columns { get; set; }

        private IEnumerable<int> rowsSize;
        private IEnumerable<int> columnsSize;

		internal override void _LayoutControls()
		{
			Size innerSize = this.ActualSize;
			innerSize.X -= (this.Margin.Left + this.Margin.Right);
			innerSize.Y -= (this.Margin.Top + this.Margin.Bottom);

			rowsSize = Rows.Select(y => MathF.RoundToInt(innerSize.Y * y, MidpointRounding.AwayFromZero));
			columnsSize = Columns.Select(x => MathF.RoundToInt(innerSize.X * x, MidpointRounding.AwayFromZero));

			foreach (Control c in this.Children)
            {
				int row = c.Grid.Row < this.Rows.Length ? c.Grid.Row : this.Rows.Length - 1;
				int col = c.Grid.Column < this.Columns.Length ? c.Grid.Column : this.Columns.Length - 1;
                int rspan = c.Grid.RowSpan != 0 ? c.Grid.RowSpan : 1;
                int cspan = c.Grid.ColSpan != 0 ? c.Grid.ColSpan : 1;

                int cellX = columnsSize.Take(col).Sum();
                int cellY = rowsSize.Take(row).Sum();
                int cellW = columnsSize.Skip(col).Take(cspan).Sum();
                int cellH = rowsSize.Skip(row).Take(rspan).Sum();

                if (c.StretchToFill)
                {
                    c.ActualPosition.X = cellX;
                    c.ActualPosition.Y = cellY;

                    c.ActualSize.X = cellW;
                    c.ActualSize.Y = cellH;
                }
                else
                {
                    c.ActualPosition.X = cellX + (cellW - c.ActualSize.X) / 2;
                    c.ActualPosition.Y = cellY + (cellH - c.ActualSize.Y) / 2;
                }

				c.ActualPosition += this.Margin.TopLeft;
            }
		}
	}
}
