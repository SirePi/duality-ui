using Duality;
using Duality.Drawing;
using Duality.Resources;
using SnowyPeak.DualityUI.Controls.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls
{
	public class TextBox : SimpleControl
	{
        private float _seconds;
        private bool _caretVisible;
		private Vector2 _caretTopLeft;

		public string Text { get; set; }
		public float CaretSpeed { get; set; }
		public TextConfiguration TextConfiguration { private get; set; }

        public TextBox()
        {
			this.Text = String.Empty;
			this.CaretSpeed = .5f;
			this.TextConfiguration = TextConfiguration.DEFAULT;
        }

		public override void OnKeyboardKeyEvent(Duality.Input.KeyboardKeyEventArgs args)
		{
			base.OnKeyboardKeyEvent(args);

			if(args.IsPressed)
			{
				this.Text += DualityApp.Keyboard.CharInput;

				if (args.Key == Duality.Input.Key.BackSpace && Text.Length > 0)
				{ this.Text = this.Text.Remove(Text.Length - 1); }
			}
		}

        public override void OnMouseButtonEvent(Duality.Input.MouseButtonEventArgs args)
        {
            base.OnMouseButtonEvent(args);

			if (args.Button == Duality.Input.MouseButton.Left && args.IsPressed)
			{ this.Status |= Control.ControlStatus.Active; }
        }

        public override void OnBlur()
        {
            this.Status &= ~Control.ControlStatus.Active;
        }

		public override void OnUpdate(float msFrame)
		{
            base.OnUpdate(msFrame);

			if((this.Status & ControlStatus.Active) != ControlStatus.None)
			{
                _seconds += (msFrame / 1000);

				if (_seconds > CaretSpeed)
                {
                    _caretVisible = !_caretVisible;
					_seconds -= CaretSpeed;
                }
			}
		}

		public override void Draw(Canvas canvas, float zOffset)
        {
            base.Draw(canvas, zOffset);

            Vector2 textPosition = AlignElement(Vector2.Zero, this.TextConfiguration.Margin, this.TextConfiguration.Alignment);

            if (!String.IsNullOrWhiteSpace(this.Text))
            {
                canvas.State.Reset();
                canvas.State.ColorTint = this.TextConfiguration.Color;
                canvas.State.TextFont = this.TextConfiguration.Font;

                canvas.DrawText(this.Text,
                    textPosition.X,
                    textPosition.Y,
                    zOffset + INNER_ZOFFSET,
                    this.TextConfiguration.Alignment);
            }

            if(_caretVisible)
            {
				Vector2 textSize = canvas.MeasureText(this.Text);

				if (textSize.Y == 0)
				{ textSize.Y = canvas.MeasureText("Wq").Y; }

				_caretTopLeft = textPosition;

                switch(this.TextConfiguration.Alignment)
				{
					case Alignment.TopLeft:
						_caretTopLeft.X += textSize.X;
						break;

					case Alignment.Left:
						_caretTopLeft.X += textSize.X;
						_caretTopLeft.Y -= (textSize.Y / 2);
						break;

					case Alignment.BottomLeft:
						_caretTopLeft.X += textSize.X;
						_caretTopLeft.Y -= textSize.Y;
						break;

					case Alignment.Top:
						_caretTopLeft.X += (textSize.X / 2);
						break;

					case Alignment.Center:
						_caretTopLeft.X += (textSize.X / 2);
						_caretTopLeft.Y -= (textSize.Y / 2);
						break;

					case Alignment.Bottom:
						_caretTopLeft.X += (textSize.X / 2);
						_caretTopLeft.Y -= textSize.Y;
						break;

					case Alignment.TopRight:
						break;

					case Alignment.Right:
						_caretTopLeft.Y -= (textSize.Y / 2);
						break;

					case Alignment.BottomRight:
						_caretTopLeft.Y -= textSize.Y;
						break;
				}

				canvas.DrawLine(_caretTopLeft.X, _caretTopLeft.Y, zOffset + INNER_ZOFFSET,
					_caretTopLeft.X, _caretTopLeft.Y + textSize.Y, zOffset + INNER_ZOFFSET);
            }
        }
	}
}
