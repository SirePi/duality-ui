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
	public class GridPanel : ControlsContainer
	{
		private const string STAR_CHAR = "*";

		private class Dimension
		{
			public static Dimension STAR_DIMENSION = new Dimension() { Value = 1, IsVariable = true };

			public int Value { get; set; }
			public bool IsVariable { get; set; }

			public override string ToString()
			{
				return string.Format("{0}{1}", this.Value, this.IsVariable ? STAR_CHAR : string.Empty);
			}
		}

		private IEnumerable<int> columnsSize;
		private IEnumerable<int> rowsSize;

		// these are arrays to avoid multiple iterations over the set values
		private Dimension[] columns = new[] { Dimension.STAR_DIMENSION };
		private Dimension[] rows = new[] { Dimension.STAR_DIMENSION };

		public IEnumerable<string> Columns
		{
			get => this.columns.Select(x => x.ToString());
			set => this.columns = this.ParseDimensions(value).ToArray();
		}

		public IEnumerable<string> Rows
		{
			get => this.rows.Select(x => x.ToString());
			set => this.rows = this.ParseDimensions(value).ToArray();
		}

		private IEnumerable<Dimension> ParseDimensions(IEnumerable<string> value)
		{
			return value.Select(x =>
			{
				if (x.Equals(STAR_CHAR))
					return Dimension.STAR_DIMENSION;
				else
				{
					bool isVariable = x.EndsWith(STAR_CHAR);
					int val = Convert.ToInt32(x.Substring(0, x.Length - (isVariable ? 1 : 0)));

					return new Dimension() { Value = val, IsVariable = isVariable };
				}
			});
		}

		public GridPanel(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		internal override void _LayoutControls()
		{
			float preallocatedRows = this.rows.Where(y => !y.IsVariable).Sum(y => y.Value);
			float preallocatedColumns = this.columns.Where(x => !x.IsVariable).Sum(x => x.Value);

			float variableRows = this.rows.Where(y => y.IsVariable).Sum(y => y.Value);
			float variableColumns = this.columns.Where(x => x.IsVariable).Sum(x => x.Value);

			Size innerSize = this.ActualSize;
			innerSize.X -= (this.Margin.Left + this.Margin.Right + preallocatedColumns);
			innerSize.Y -= (this.Margin.Top + this.Margin.Bottom + preallocatedRows);

			this.rowsSize = this.rows.Select(y =>
			{
				int result = y.Value;
				if (y.IsVariable)
				{ result = MathF.RoundToInt(innerSize.Y * y.Value / variableRows, MidpointRounding.AwayFromZero); }

				return result;
			});
			this.columnsSize = this.columns.Select(x =>
			{
				int result = x.Value;
				if (x.IsVariable)
				{ result = MathF.RoundToInt(innerSize.X * x.Value / variableColumns, MidpointRounding.AwayFromZero); }

				return result;
			});

			foreach (Control c in this.Children)
			{
				int row = c.Cell.Row < this.rows.Length ? c.Cell.Row : this.rows.Length - 1;
				int col = c.Cell.Column < this.columns.Length ? c.Cell.Column : this.columns.Length - 1;
				int rspan = c.Cell.RowSpan != 0 ? c.Cell.RowSpan : 1;
				int cspan = c.Cell.ColSpan != 0 ? c.Cell.ColSpan : 1;

				int cellX = this.columnsSize.Take(col).Sum();
				int cellY = this.rowsSize.Take(row).Sum();
				int cellW = this.columnsSize.Skip(col).Take(cspan).Sum();
				int cellH = this.rowsSize.Skip(row).Take(rspan).Sum();

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
