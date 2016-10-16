using Duality;
using Duality.Drawing;
using Duality.Resources;
using SnowyPeak.DualityUI.Controls.Configuration;
using SnowyPeak.DualityUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls
{
	public class Button : Control
	{
		public delegate void MouseButtonEventDelegate(Button button, Duality.Input.MouseButtonEventArgs args);
		public MouseButtonEventDelegate MouseButtonEventHandler { get; set; }

		public string Text { get; set; }
		public TextConfiguration TextConfiguration { private get; set; }

		public Button()
		{
			this.Text = String.Empty;
			this.TextConfiguration = TextConfiguration.DEFAULT;
		}

        public override void ApplySkin(Skin skin)
        {
            if (skin == null) return;

            base.ApplySkin(skin);

			if (this.TextConfiguration == TextConfiguration.DEFAULT)
            { this.TextConfiguration = skin.GetTemplate<TextTemplate>(this).TextConfiguration; }
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
					zOffset + (INNER_ZOFFSET * 2),
					this.TextConfiguration.Alignment);
			}
		}

		public override void OnMouseButtonEvent(Duality.Input.MouseButtonEventArgs args)
		{
			base.OnMouseButtonEvent(args);

			if (args.Button == Duality.Input.MouseButton.Left)
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
