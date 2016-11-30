// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public class TextBox : Control
	{
		private static readonly string ELLIPSIS = "...";
		private static readonly string WQ = "Wq";

		private int _caretPosition;
		private Vector2 _caretTopLeft;
		private bool _caretVisible;
		private float _seconds;

		public float CaretSpeed { get; set; }
		public bool IsPassword { get; set; }
		public int MaxLength { get; set; }
		public string Text { get; set; }
		public TextConfiguration TextConfiguration { get; set; }

		public TextBox(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			this.Text = String.Empty;
			this.CaretSpeed = .5f;
			this.MaxLength = int.MaxValue;

			ApplySkin(_baseSkin);
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);
			this.TextConfiguration = _baseSkin.GetTemplate<TextTemplate>(this).TextConfiguration.Clone();
		}

		public override void Draw(Canvas canvas, float zOffset)
		{
			base.Draw(canvas, zOffset);

			Vector2 textPosition = AlignElement(Vector2.Zero, this.TextConfiguration.Margin, this.TextConfiguration.Alignment);

			canvas.State.Reset();
			canvas.State.ColorTint = this.TextConfiguration.Color;
			canvas.State.TextFont = this.TextConfiguration.Font;

			string textToDraw = this.IsPassword ? new String('*', this.Text.Length) : this.Text;
			Vector2 textSize = canvas.MeasureText(textToDraw);

			if (!String.IsNullOrWhiteSpace(this.Text))
			{
				float availableWidth = this.ActualSize.X - this.TextConfiguration.Margin.Left - this.TextConfiguration.Margin.Right;
				if (textSize.X > availableWidth)
				{
					Vector2 ellipsisSize = canvas.MeasureText(ELLIPSIS);
					int subLength = textToDraw.Length - 1;

					while (canvas.MeasureText(textToDraw.Substring(0, subLength)).X + ellipsisSize.X > availableWidth)
						subLength--;

					textToDraw = textToDraw.Substring(0, subLength) + ELLIPSIS;
					textSize = canvas.MeasureText(textToDraw);
				}

				canvas.DrawText(textToDraw,
					textPosition.X,
					textPosition.Y,
					zOffset + INNER_ZOFFSET,
					this.TextConfiguration.Alignment);
			}

			if (_caretVisible)
			{
				Vector2 preCaretSize = canvas.MeasureText(textToDraw.Substring(0, _caretPosition));

				if (textSize.Y == 0)
				{ textSize.Y = canvas.MeasureText(WQ).Y; }

				_caretTopLeft = textPosition;

				switch (this.TextConfiguration.Alignment)
				{
					case Alignment.TopLeft:
						_caretTopLeft.X += preCaretSize.X;
						break;

					case Alignment.Left:
						_caretTopLeft.X += preCaretSize.X;
						_caretTopLeft.Y -= (textSize.Y / 2);
						break;

					case Alignment.BottomLeft:
						_caretTopLeft.X += preCaretSize.X;
						_caretTopLeft.Y -= textSize.Y;
						break;

					case Alignment.Top:
						_caretTopLeft.X += preCaretSize.X - (textSize.X / 2);
						break;

					case Alignment.Center:
						_caretTopLeft.X += preCaretSize.X - (textSize.X / 2);
						_caretTopLeft.Y -= (textSize.Y / 2);
						break;

					case Alignment.Bottom:
						_caretTopLeft.X += preCaretSize.X - (textSize.X / 2);
						_caretTopLeft.Y -= textSize.Y;
						break;

					case Alignment.TopRight:
						_caretTopLeft.X += preCaretSize.X - textSize.X;
						break;

					case Alignment.Right:
						_caretTopLeft.X += preCaretSize.X - textSize.X;
						_caretTopLeft.Y -= (textSize.Y / 2);
						break;

					case Alignment.BottomRight:
						_caretTopLeft.X += preCaretSize.X - textSize.X;
						_caretTopLeft.Y -= textSize.Y;
						break;
				}

				canvas.DrawLine(_caretTopLeft.X, _caretTopLeft.Y, zOffset + INNER_ZOFFSET,
					_caretTopLeft.X, _caretTopLeft.Y + textSize.Y, zOffset + INNER_ZOFFSET);
			}
		}

		public override void OnBlur()
		{
			this.Status &= ~Control.ControlStatus.Active;
		}

		public override void OnKeyboardKeyEvent(KeyboardKeyEventArgs args)
		{
			base.OnKeyboardKeyEvent(args);

			if ((this.Status & ControlStatus.Active) != ControlStatus.None && args.IsPressed)
			{
				if (this.Text.Length < MaxLength)
				{
					if (DualityApp.Keyboard.CharInput.Length > 0)
					{
						this.Text = this.Text.Insert(_caretPosition, DualityApp.Keyboard.CharInput);
						_caretPosition += DualityApp.Keyboard.CharInput.Length;
					}
					if (args.Key == Key.BackSpace && _caretPosition > 0)
					{
						this.Text = this.Text.Remove(_caretPosition - 1, 1);
						_caretPosition--;
					}
					if (args.Key == Key.Delete && _caretPosition < this.Text.Length)
					{ this.Text = this.Text.Remove(_caretPosition, 1); }

					if (args.Key == Key.Enter || args.Key == Key.KeypadEnter)
					{ this.OnBlur(); }

					if (args.Key == Key.Left)
					{ _caretPosition--; }
					if (args.Key == Key.Right)
					{ _caretPosition++; }
					if (args.Key == Key.Home)
					{ _caretPosition = 0; }
					if (args.Key == Key.End)
					{ _caretPosition = this.Text.Length; }
				}

				_caretPosition = Math.Min(Math.Max(0, _caretPosition), this.Text.Length);
			}
		}

		public override void OnMouseButtonEvent(MouseButtonEventArgs args)
		{
			base.OnMouseButtonEvent(args);

			if (args.Button == MouseButton.Left && args.IsPressed)
			{
				if ((this.Status & Control.ControlStatus.Active) == Control.ControlStatus.None)
				{ _caretPosition = this.Text.Length; }

				this.Status |= Control.ControlStatus.Active;
			}
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			if ((this.Status & ControlStatus.Active) != ControlStatus.None)
			{
				_seconds += (msFrame / 1000);

				if (_seconds > CaretSpeed)
				{
					_caretVisible = !_caretVisible;
					_seconds -= CaretSpeed;
				}
			}
			else
			{ _caretVisible = false; }
		}
	}
}