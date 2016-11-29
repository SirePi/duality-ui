using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI
{
    public struct Border
    {
        public static readonly Border Zero = new Border(0);

        public float Left;
        public float Right;
        public float Top;
        public float Bottom;

        public Vector2 TopLeft
        {
            get { return new Vector2(this.Left, this.Top); }
        }

        public Vector2 BottomRight
        {
            get { return new Vector2(this.Right, this.Bottom); }
        }

        public Border(float value)
            : this(value, value, value, value)
        {
        }

        public Border(float horizontal, float vertical)
            : this(horizontal, vertical, horizontal, vertical)
        {
        }

        public Border(float left, float top, float right, float bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public override string ToString()
        {
            return String.Format("({0:0.00}, {1:0.00}, {2:0.00}, {3:0.00})", this.Left, this.Top, this.Right, this.Bottom);
        }
    }
}