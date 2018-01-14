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
	public class Button : Control
	{
		private FormattedText _fText;

		public MouseButtonEventDelegate MouseButtonEventHandler { get; set; }
		public string Text { get; set; }
		public TextConfiguration TextConfiguration { get; set; }

		public delegate void MouseButtonEventDelegate(Button button, MouseButtonEventArgs args);

		public Button(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			this.Text = String.Empty;
			_fText = new FormattedText();

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

			if (!String.IsNullOrWhiteSpace(this.Text))
			{
				canvas.State.Reset();
				canvas.State.ColorTint = this.TextConfiguration.Color;

				_fText.SourceText = this.Text;
				if (_fText.Fonts[0] != this.TextConfiguration.Font)
				{
					_fText.Fonts[0] = this.TextConfiguration.Font;
					_fText.UpdateVertexCache();
				}

				canvas.DrawText(_fText,
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

			if (this.MouseButtonEventHandler != null) { this.MouseButtonEventHandler(this, args); }
		}

		public override void OnMouseLeaveEvent()
		{
			base.OnMouseLeaveEvent();

			this.Status &= ~Control.ControlStatus.Active;
		}
	}
}