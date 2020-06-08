// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;

namespace SnowyPeak.Duality.Plugins.YAUI
{
	public struct Size
	{
		public static readonly Size Zero = new Size(0);

		public float X { get; set; }
		public float Y { get; set; }

		public bool IsVisible => this.X > 0 && this.Y > 0;

		public Size(float value)
			: this(value, value)
		{ }

		public Size(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		public static implicit operator Size(Vector2 v)
		{
			return new Size(v.X, v.Y);
		}

		public static implicit operator Vector2(Size s)
		{
			return new Vector2(s.X, s.Y);
		}

		public static bool operator !=(Size a, Size b)
		{
			return !(a == b);
		}

		public static bool operator ==(Size a, Size b)
		{
			return (a.X == b.X && a.Y == b.Y);
		}

		public static bool operator !=(Size a, Point2 b)
		{
			return !(a == b);
		}

		public static bool operator ==(Size a, Point2 b)
		{
			return (a.X == b.X && a.Y == b.Y);
		}

		public static Size operator +(Size a, Border b)
		{
			return new Size(a.X + b.Left + b.Right, a.Y + b.Top + b.Bottom);
		}

		public static Size operator -(Size a, Border b)
		{
			return new Size(a.X - b.Left - b.Right, a.Y - b.Top - b.Bottom);
		}

		public static Size operator *(Size a, float value)
		{
			return a.Scale(value, value);
		}

		public static Size operator /(Size a, float value)
		{
			value = 1 / value;
			return a.Scale(value, value);
		}

		public Size Scale(float x, float y)
		{
			return new Size(this.X * x, this.Y * y);
		}

		public void AtLeast(Size minSize)
		{
			this.X = MathF.Max(minSize.X, this.X);
			this.Y = MathF.Max(minSize.Y, this.Y);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Size))
			{ return false; }
			else
			{ return this == (Size)obj; }
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("({0:0.00}, {1:0.00})", this.X, this.Y);
		}
	}
}
