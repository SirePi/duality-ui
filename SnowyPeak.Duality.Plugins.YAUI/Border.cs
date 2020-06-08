// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;

namespace SnowyPeak.Duality.Plugins.YAUI
{
	public struct Border
	{
		public static readonly Border Zero = new Border(0);

		public float Bottom { get; set; }
		public float Left { get; set; }
		public float Right { get; set; }
		public float Top { get; set; }

		public Vector2 BottomRight => new Vector2(this.Right, this.Bottom);
		public Vector2 TopLeft => new Vector2(this.Left, this.Top);
		public float Horizontal => this.Left + this.Right;
		public float Vertical => this.Top + this.Bottom;

		public Border(float value)
			: this(value, value, value, value)
		{ }

		public Border(float horizontal, float vertical)
			: this(horizontal, vertical, horizontal, vertical)
		{ }

		public Border(float left, float top, float right, float bottom)
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}

		public override string ToString()
		{
			return string.Format("({0:0.00}, {1:0.00}, {2:0.00}, {3:0.00})", this.Left, this.Top, this.Right, this.Bottom);
		}
	}
}
