// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using SnowyPeak.Duality.Plugins.YAUI.Templates;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public sealed class TextBlock : Control<TextTemplate>
	{
		private readonly FormattedText fText = new FormattedText();

		private string text;
		public string Text {
			get => this.text;
			set
			{
				if (this.text != value)
					this.text = value;
			}
		}
		public TextConfiguration TextConfiguration { get; set; }

		public TextBlock(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

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

			if (!string.IsNullOrWhiteSpace(this.Text))
			{
				this.fText.SourceText = this.Text;
				this.fText.Fonts[0] = this.TextConfiguration.Font;
				this.fText.UpdateVertexCache();
				this.fText.MaxHeight = (int)(this.ActualSize.Y - this.TextConfiguration.Margin.Vertical);
				this.fText.MaxWidth = (int)(this.ActualSize.X - this.TextConfiguration.Margin.Horizontal);

				canvas.DrawText(this.fText,
					(int)textPosition.X,
					(int)textPosition.Y,
					zOffset + (INNER_ZOFFSET * 2),
					null,
					this.TextConfiguration.Alignment);
			}
		}
	}
}
