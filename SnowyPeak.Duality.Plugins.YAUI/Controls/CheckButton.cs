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
	public class CheckButton : Button
	{
		private bool _isChecked;

		public bool Checked
		{
			get { return _isChecked; }
			set
			{
                if (_isChecked != value)
                { this.OnCheckedChange.Invoke(this, _isChecked, value); }

				_isChecked = value;
			}
		}

		public GlyphConfiguration GlyphConfiguration { get; set; }

		// Delegates
		public delegate void CheckedChangeEventDelegate(CheckButton checkButton, bool previousValue, bool newValue);

		// Events
		public event CheckedChangeEventDelegate OnCheckedChange = delegate { };

		public CheckButton(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			this.OnMouseButton += CheckButton_OnMouseButton;
			ApplySkin(_baseSkin);
		}

		private void CheckButton_OnMouseButton(Button button, MouseButtonEventArgs args)
		{
			if (args.Button == MouseButton.Left)
			{
				if (args.IsPressed)
				{ this.Checked = !this.Checked; }
			}
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);
			this.GlyphConfiguration = _baseSkin.GetTemplate<GlyphTemplate>(this).GlyphConfiguration.Clone();
		}

		public override void Draw(Canvas canvas, float zOffset)
		{
			base.Draw(canvas, zOffset);

			if (this.GlyphConfiguration.Glyph.IsAvailable &&
				this.GlyphConfiguration.Glyph.Res.MainTexture.IsAvailable &&
				this.Checked)
			{
				Material material = this.GlyphConfiguration.Glyph.Res;
				Texture tx = material.MainTexture.Res;

				Vector2 glyphTopLeft = AlignElement(tx.Size, this.GlyphConfiguration.Margin, this.GlyphConfiguration.Alignment);
				Vector2 glyphBottomRight = glyphTopLeft + tx.Size;

				VertexC1P3T2[] glyphVertices = canvas.RequestVertexArray(4);

				glyphVertices[0].Pos.X = glyphTopLeft.X;
				glyphVertices[0].Pos.Y = glyphTopLeft.Y;
				glyphVertices[0].Pos.Z = zOffset + INNER_ZOFFSET;
				glyphVertices[0].TexCoord.X = 0;
				glyphVertices[0].TexCoord.Y = 0;
				glyphVertices[0].Color = material.MainColor;

				glyphVertices[1].Pos.X = glyphTopLeft.X;
				glyphVertices[1].Pos.Y = glyphBottomRight.Y;
				glyphVertices[1].Pos.Z = zOffset + INNER_ZOFFSET;
				glyphVertices[1].TexCoord.X = 0;
				glyphVertices[1].TexCoord.Y = tx.UVRatio.Y;
				glyphVertices[1].Color = material.MainColor;

				glyphVertices[2].Pos.X = glyphBottomRight.X;
				glyphVertices[2].Pos.Y = glyphBottomRight.Y;
				glyphVertices[2].Pos.Z = zOffset + INNER_ZOFFSET;
				glyphVertices[2].TexCoord.X = tx.UVRatio.X;
				glyphVertices[2].TexCoord.Y = tx.UVRatio.Y;
				glyphVertices[2].Color = material.MainColor;

				glyphVertices[3].Pos.X = glyphBottomRight.X;
				glyphVertices[3].Pos.Y = glyphTopLeft.Y;
				glyphVertices[3].Pos.Z = zOffset + INNER_ZOFFSET;
				glyphVertices[3].TexCoord.X = tx.UVRatio.X;
				glyphVertices[3].TexCoord.Y = 0;
				glyphVertices[3].Color = material.MainColor;

				canvas.State.Reset();
				canvas.State.SetMaterial(material);
				canvas.DrawVertices<VertexC1P3T2>(glyphVertices, VertexMode.Quads, 4);
			}
		}
	}
}