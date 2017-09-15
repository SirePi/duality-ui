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
		private static string STAR = "*";

		private class Dimension
		{
			public int Value { get; set; }
			public bool IsVariable { get; set; }

			public override string ToString()
			{
				return String.Format("{0}{1}", Value, IsVariable ? STAR : String.Empty);
			}
		}

		private IEnumerable<int> columnsSize;
		private IEnumerable<int> rowsSize;

		private Dimension[] _columns;
		private Dimension[] _rows;

		public IEnumerable<string> Columns
		{
			get { return _columns.Select(x => x.ToString()); }
			set
			{
				_columns = value.Select(x =>
				{
					if (x.Equals(STAR))
						return new Dimension() { Value = 1, IsVariable = true };
					else
					{
						bool isVariable = x.EndsWith(STAR);
						int val = Convert.ToInt32(x.Substring(0, x.Length - (isVariable ? 1 : 0)));

						return new Dimension() { Value = val, IsVariable = isVariable };
					}
				}).ToArray();
			}
		}

		public IEnumerable<string> Rows
		{
			get { return _rows.Select(x => x.ToString()); }
			set
			{
				_rows = value.Select(x =>
				{
					if (x.Equals(STAR))
						return new Dimension() { Value = 1, IsVariable = true };
					else
					{
						bool isVariable = x.EndsWith(STAR);
						int val = Convert.ToInt32(x.Substring(0, x.Length - (isVariable ? 1 : 0)));

						return new Dimension() { Value = val, IsVariable = isVariable };
					}
				}).ToArray();
			}
		}

		public GridPanel(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			ApplySkin(_baseSkin);
		}

		internal override void _LayoutControls()
		{
			float preallocatedRows = _rows.Where(y => !y.IsVariable).Sum(y => y.Value);
			float preallocatedColumns = _columns.Where(x => !x.IsVariable).Sum(x => x.Value);

			float variableRows = _rows.Where(y => y.IsVariable).Sum(y => y.Value);
			float variableColumns = _columns.Where(x => x.IsVariable).Sum(x => x.Value);

			Size innerSize = this.ActualSize;
			innerSize.X -= (this.Margin.Left + this.Margin.Right + preallocatedColumns);
			innerSize.Y -= (this.Margin.Top + this.Margin.Bottom + preallocatedRows);

			rowsSize = _rows.Select(y =>
			{
				int result = y.Value;
				if (y.IsVariable)
				{ result = MathF.RoundToInt(innerSize.Y * y.Value / variableRows, MidpointRounding.AwayFromZero); }

				return result;
			});
			columnsSize = _columns.Select(x =>
			{
				int result = x.Value;
				if (x.IsVariable)
				{ result = MathF.RoundToInt(innerSize.X * x.Value / variableColumns, MidpointRounding.AwayFromZero); }

				return result;
			});

			foreach (Control c in this.Children)
			{
				int row = c.Cell.Row < _rows.Length ? c.Cell.Row : _rows.Length - 1;
				int col = c.Cell.Column < _columns.Length ? c.Cell.Column : _columns.Length - 1;
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