using Duality;
using Duality.Drawing;
using Duality.Input;
using Duality.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls
{
    public abstract class Control
    {
		[Flags]
		public enum ControlStatus {
			None = 0x00,
			Disabled = 0x0001,
			Normal = 0x0010,
			Active = 0x0100,
			Hover = 0x1000
		}

		public enum ControlVisibility
		{
			Visible,
			Hidden,
			Collapsed
		}

        public delegate void UpdateDelegate(Control control, float msFrame);
        public UpdateDelegate UpdateHandler { get; set; }

        public delegate void FocusChangeDelegate(Control control, bool isFocused);
        public FocusChangeDelegate FocusChangeHandler { get; set; }

        protected static readonly float LAYOUT_ZOFFSET = -0.0001f;
        protected static readonly float INNER_ZOFFSET = -0.00001f;

		public Vector2 Position;
		public Size Size;
		public Vector2 ActualPosition;
		public Size ActualSize;

		public string Name { get; set; }
        public bool StretchToFill { get; set; }
        public DockPanel.Dock Docking { get; set; }
        public GridPanel.GridInfo Grid { get; set; }

		public ControlStatus Status { get; set; }
        public object Tag { get; set; }
        public ControlVisibility Visibility { get; set; }

        public ControlsContainer Parent { get; set; }
        public Rect ControlArea
        {
            get { return new Rect(this.ActualPosition.X, this.ActualPosition.Y, this.ActualSize.X, this.ActualSize.Y); }
        }

		protected Control()
		{
			this.StretchToFill = true;
			this.Visibility = ControlVisibility.Visible;
			this.Status = ControlStatus.Normal;
		}

        protected Vector2 AlignElement(Vector2 elementSize, Border margin, Alignment alignment)
        {
            Vector2 topLeft = Vector2.Zero;

            switch (alignment)
            {
                case Alignment.TopLeft:
                    topLeft.X = this.ActualPosition.X + margin.Left;
                    topLeft.Y = this.ActualPosition.Y + margin.Top;
                    break;

                case Alignment.Top:
                    topLeft.X = this.ActualPosition.X + (this.ActualSize.X - elementSize.X) / 2;
                    topLeft.Y = this.ActualPosition.Y + margin.Top;
                    break;

                case Alignment.TopRight:
                    topLeft.X = this.ActualPosition.X + this.ActualSize.X - margin.Right - elementSize.X;
                    topLeft.Y = this.ActualPosition.Y + margin.Top;
                    break;

                case Alignment.Left:
                    topLeft.X = this.ActualPosition.X + margin.Left;
                    topLeft.Y = this.ActualPosition.Y + (this.ActualSize.Y - elementSize.Y) / 2;
                    break;

                case Alignment.Center:
                    topLeft.X = this.ActualPosition.X + (this.ActualSize.X - elementSize.X) / 2;
                    topLeft.Y = this.ActualPosition.Y + (this.ActualSize.Y - elementSize.Y) / 2;
                    break;

                case Alignment.Right:
                    topLeft.X = this.ActualPosition.X + this.ActualSize.X - margin.Right - elementSize.X;
                    topLeft.Y = this.ActualPosition.Y + (this.ActualSize.Y - elementSize.Y) / 2;
                    break;

                case Alignment.BottomLeft:
                    topLeft.X = this.ActualPosition.X + margin.Left;
                    topLeft.Y = this.ActualPosition.Y + this.ActualSize.Y - margin.Bottom - elementSize.Y;
                    break;

                case Alignment.Bottom:
                    topLeft.X = this.ActualPosition.X + (this.ActualSize.X - elementSize.X) / 2;
                    topLeft.Y = this.ActualPosition.Y + this.ActualSize.Y - margin.Bottom - elementSize.Y;
                    break;

                case Alignment.BottomRight:
                    topLeft.X = this.ActualPosition.X + this.ActualSize.X - margin.Right - elementSize.X;
                    topLeft.Y = this.ActualPosition.Y + this.ActualSize.Y - margin.Bottom - elementSize.Y;
                    break;
            }

            return topLeft;
        }

		public virtual void OnUpdate(float msFrame) 
        {
            if (this.UpdateHandler != null)
			{ this.UpdateHandler(this, msFrame); }
        }

        public virtual void OnFocus() 
        {
            if (this.FocusChangeHandler != null)
			{ this.FocusChangeHandler(this, true); }
        }

        public virtual void OnBlur() 
        {
            if (this.FocusChangeHandler != null) 
			{ this.FocusChangeHandler(this, false); }
        }

		public abstract void Draw(Canvas canvas, float zOffset);

        public virtual void OnKeyboardKeyEvent(KeyboardKeyEventArgs args) { }
        public virtual void OnMouseButtonEvent(MouseButtonEventArgs args) { }

        public virtual void OnMouseEnterEvent()
        {
            this.Status |= Control.ControlStatus.Hover;
        }
        public virtual void OnMouseLeaveEvent()
        {
            this.Status &= ~Control.ControlStatus.Hover;
        }

        public override string ToString()
        {
            return String.IsNullOrWhiteSpace(this.Name) ? this.GetType().ToString() : this.Name;
        }
    }
}