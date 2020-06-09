// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Input;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using SnowyPeak.Duality.Plugins.YAUI.Templates;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public abstract class Button<T> : InteractiveControl<T> where T : TextTemplate, new()
	{
		private readonly FormattedText fText = new FormattedText();

		public string Text { get; set; }
		public TextConfiguration TextConfiguration { get; set; }

		// Delegates
		public delegate void MouseButtonEventDelegate(IInteractiveControl button, MouseButtonEventArgs args);

		// Events
		[DontSerialize]
		private MouseButtonEventDelegate onMouseButton;
		public event MouseButtonEventDelegate OnMouseButton
		{
			add { this.onMouseButton += value; }
			remove { this.onMouseButton -= value; }
		}

		protected Button(Skin skin, string templateName)
			: base(skin, templateName)
		{ }

		protected override void Init()
		{
			base.Init();

			this.Text = string.Empty;
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);
			this.TextConfiguration = this.Template.TextConfiguration;
		}

		protected override void _Draw(Canvas canvas, float zOffset)
		{
			base._Draw(canvas, zOffset);

			Vector2 textPosition = this.AlignElement(Vector2.Zero, this.TextConfiguration.Margin, this.TextConfiguration.Alignment);

			if (!string.IsNullOrWhiteSpace(this.Text))
			{
				canvas.State.Reset();
				canvas.State.ColorTint = this.TextConfiguration.Color;

				this.fText.SourceText = this.Text;
				if (this.fText.Fonts[0] != this.TextConfiguration.Font)
				{
					this.fText.Fonts[0] = this.TextConfiguration.Font;
					this.fText.UpdateVertexCache();
				}

				canvas.DrawText(this.fText,
					(int)textPosition.X,
					(int)textPosition.Y,
					zOffset + (INNER_ZOFFSET * 2),
					null,
					this.TextConfiguration.Alignment);
			}
		}

		public override void OnMouseButtonEvent(MouseButtonEventArgs args)
		{
			base.OnMouseButtonEvent(args);

			if (args.Button == MouseButton.Left)
			{
				if (args.IsPressed)
				{ this.Status |= Control.ControlStatus.Active; }
				else
				{ this.Status &= ~Control.ControlStatus.Active; }
			}

			this.onMouseButton?.Invoke(this, args);
		}

		public override void OnMouseLeaveEvent()
		{
			base.OnMouseLeaveEvent();

			this.Status &= ~Control.ControlStatus.Active;
		}
	}

	public class Button : Button<TextTemplate>
	{
		public Button(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }
	}
}
