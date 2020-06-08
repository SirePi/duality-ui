// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Input;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using SnowyPeak.Duality.Plugins.YAUI.Templates;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public sealed class TextBox : InteractiveControl<TextTemplate>
	{
		private const string ELLIPSIS = "...";
		private const string WQ = "Wq";

		private int caretPosition;
		private Vector2 caretTopLeft;
		private bool caretVisible;
		private float seconds;
		private string text;

		public float CaretSpeed { get; set; }
		public bool IsPassword { get; set; }
		public int MaxLength { get; set; }
		public string Text
		{
			get => this.text;
			set
			{
				if (this.text != value)
				{ this.onTextChange?.Invoke(this, this.text, value); }

				this.text = value;
			}
		}
		public TextConfiguration TextConfiguration { get; set; }

		// Delegates
		public delegate void TextChangeEventDelegate(TextBox textBox, string oldText, string newText);
		// Events
		[DontSerialize]
		private TextChangeEventDelegate onTextChange;
		public event TextChangeEventDelegate OnTextChange
		{
			add { this.onTextChange += value; }
			remove { this.onTextChange -= value; }
		}

		public TextBox(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		protected override void Init()
		{
			base.Init();
		
			this.Text = string.Empty;
			this.CaretSpeed = .5f;
			this.MaxLength = int.MaxValue;
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);

			this.TextConfiguration = this.Template.TextConfiguration.Clone();
		}

		protected override void _Draw(Canvas canvas, float zOffset)
		{
			base._Draw(canvas, zOffset);

			Vector2 textPosition = this.AlignElement(Vector2.Zero, this.TextConfiguration.Margin, this.TextConfiguration.Alignment);

			canvas.State.Reset();
			canvas.State.ColorTint = this.TextConfiguration.Color;
			canvas.State.TextFont = this.TextConfiguration.Font;

			string textToDraw = this.IsPassword ? new string('*', this.Text.Length) : this.Text;
			Vector2 textSize = canvas.MeasureText(textToDraw);

			if (!string.IsNullOrWhiteSpace(this.Text))
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
					(int)textPosition.X,
					(int)textPosition.Y,
					zOffset + INNER_ZOFFSET,
					this.TextConfiguration.Alignment);
			}

			if (this.caretVisible)
			{
				Vector2 preCaretSize = canvas.MeasureText(textToDraw.Substring(0, MathF.Clamp(this.caretPosition, 0, textToDraw.Length)));

				if (textSize.Y == 0)
				{ textSize.Y = canvas.MeasureText(WQ).Y; }

				this.caretTopLeft = textPosition;

				switch (this.TextConfiguration.Alignment)
				{
					case Alignment.TopLeft:
						this.caretTopLeft.X += preCaretSize.X;
						break;

					case Alignment.Left:
						this.caretTopLeft.X += preCaretSize.X;
						this.caretTopLeft.Y -= (textSize.Y / 2);
						break;

					case Alignment.BottomLeft:
						this.caretTopLeft.X += preCaretSize.X;
						this.caretTopLeft.Y -= textSize.Y;
						break;

					case Alignment.Top:
						this.caretTopLeft.X += preCaretSize.X - (textSize.X / 2);
						break;

					case Alignment.Center:
						this.caretTopLeft.X += preCaretSize.X - (textSize.X / 2);
						this.caretTopLeft.Y -= (textSize.Y / 2);
						break;

					case Alignment.Bottom:
						this.caretTopLeft.X += preCaretSize.X - (textSize.X / 2);
						this.caretTopLeft.Y -= textSize.Y;
						break;

					case Alignment.TopRight:
						this.caretTopLeft.X += preCaretSize.X - textSize.X;
						break;

					case Alignment.Right:
						this.caretTopLeft.X += preCaretSize.X - textSize.X;
						this.caretTopLeft.Y -= (textSize.Y / 2);
						break;

					case Alignment.BottomRight:
						this.caretTopLeft.X += preCaretSize.X - textSize.X;
						this.caretTopLeft.Y -= textSize.Y;
						break;
				}

				canvas.DrawLine(this.caretTopLeft.X, this.caretTopLeft.Y, zOffset + INNER_ZOFFSET,
					this.caretTopLeft.X, this.caretTopLeft.Y + textSize.Y, zOffset + INNER_ZOFFSET);
			}
		}

		public override void OnBlur()
		{
			base.OnBlur();

			this.Status &= ~Control.ControlStatus.Active;
		}

		public override void OnKeyboardKeyEvent(KeyboardKeyEventArgs args)
		{
			base.OnKeyboardKeyEvent(args);

			if ((this.Status & ControlStatus.Active) != ControlStatus.None && args.IsPressed)
			{
				if (DualityApp.Keyboard.CharInput.Length > 0 && this.Text.Length < this.MaxLength)
				{
					this.Text = this.Text.Insert(this.caretPosition, DualityApp.Keyboard.CharInput);
					this.caretPosition += DualityApp.Keyboard.CharInput.Length;
				}
				if (args.Key == Key.BackSpace && this.caretPosition > 0)
				{
					this.Text = this.Text.Remove(this.caretPosition - 1, 1);
					this.caretPosition--;
				}
				if (args.Key == Key.Delete && this.caretPosition < this.Text.Length)
				{ this.Text = this.Text.Remove(this.caretPosition, 1); }

				if (args.Key == Key.Enter || args.Key == Key.KeypadEnter)
				{ this.OnBlur(); }

				if (args.Key == Key.Left)
				{ this.caretPosition--; }
				if (args.Key == Key.Right)
				{ this.caretPosition++; }
				if (args.Key == Key.Home)
				{ this.caretPosition = 0; }
				if (args.Key == Key.End)
				{ this.caretPosition = this.Text.Length; }

				this.caretPosition = MathF.Clamp(this.caretPosition, 0, this.Text.Length);
			}
		}

		public override void OnMouseButtonEvent(MouseButtonEventArgs args)
		{
			base.OnMouseButtonEvent(args);

			if (args.Button == MouseButton.Left && args.IsPressed)
			{
				if ((this.Status & Control.ControlStatus.Active) == Control.ControlStatus.None)
				{ this.caretPosition = this.Text.Length; }

				this.Status |= Control.ControlStatus.Active;
			}
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			if ((this.Status & ControlStatus.Active) != ControlStatus.None)
			{
				this.seconds += (msFrame / 1000);

				if (this.seconds > this.CaretSpeed)
				{
					this.caretVisible = !this.caretVisible;
					this.seconds -= this.CaretSpeed;
				}
			}
			else
			{ this.caretVisible = false; }
		}
	}
}
