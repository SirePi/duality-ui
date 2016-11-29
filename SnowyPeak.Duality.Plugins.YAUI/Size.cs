using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI
{
    public struct Size
    {
        public static readonly Size Zero = new Size(0);

        public float X;
        public float Y;

        public Size(float value)
            : this(value, value)
        {
        }

        public Size(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public void AtLeast(Size minSize)
        {
            this.X = MathF.Max(minSize.X, this.X);
            this.Y = MathF.Max(minSize.Y, this.Y);
        }

        public override string ToString()
        {
            return String.Format("({0:0.00}, {1:0.00})", this.X, this.Y);
        }

        public static bool operator ==(Size a, Size b)
        {
            return (a.X == b.X && a.Y == b.Y);
        }

        public static bool operator !=(Size a, Size b)
        {
            return !(a == b);
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

        public static implicit operator Vector2(Size s)
        {
            return new Vector2(s.X, s.Y);
        }

        public static implicit operator Size(Vector2 v)
        {
            return new Size(v.X, v.Y);
        }
    }
}