// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public sealed class GridPanel : ControlsContainer
	{
		private const string STAR_CHAR = "*";

		private class Dimension
		{
			public static readonly Dimension STAR_DIMENSION = new Dimension() { Value = 1, IsVariable = true };

			public int Value { get; set; }
			public bool IsVariable { get; set; }

			public override string ToString()
			{
				return string.Format("{0}{1}", this.Value, this.IsVariable ? STAR_CHAR : string.Empty);
			}
		}

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

		public GridPanel(Skin skin = null, string templateName = null, bool drawSelf = true)
			: base(skin, templateName, drawSelf)
		{ }

		internal override void _LayoutControls()
		{
			float preallocatedRows = 0;
			float preallocatedColumns = 0;

			float variableRows = 0;
			float variableColumns = 0;

			foreach (Dimension row in this.rows)
			{
				if (row.IsVariable)
					variableRows += row.Value;
				else
					preallocatedRows += row.Value;
			}

			foreach (Dimension col in this.columns)
			{
				if (col.IsVariable)
					variableColumns += col.Value;
				else
					preallocatedColumns += col.Value;
			}

			Size innerSize = this.ActualSize;
			innerSize.X -= (this.Margin.Left + this.Margin.Right + preallocatedColumns);
			innerSize.Y -= (this.Margin.Top + this.Margin.Bottom + preallocatedRows);

			int[] rowsSize = this.rows.Select(y =>
			{
				int result = y.Value;
				if (y.IsVariable)
				{ result = MathF.RoundToInt(innerSize.Y * y.Value / variableRows, MidpointRounding.AwayFromZero); }

				return result;
			}).ToArray();
			int[] columnsSize = this.columns.Select(x =>
			{
				int result = x.Value;
				if (x.IsVariable)
				{ result = MathF.RoundToInt(innerSize.X * x.Value / variableColumns, MidpointRounding.AwayFromZero); }

				return result;
			}).ToArray();

			foreach (Control c in this.children)
			{
				int row = c.Cell.Row < this.rows.Length ? c.Cell.Row : this.rows.Length - 1;
				int col = c.Cell.Column < this.columns.Length ? c.Cell.Column : this.columns.Length - 1;
				int rspan = c.Cell.RowSpan != 0 ? c.Cell.RowSpan : 1;
				int cspan = c.Cell.ColSpan != 0 ? c.Cell.ColSpan : 1;

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
