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
		private VertexC1P3T2[] _glyphVertices;
		private bool _isChecked;

		public CheckChangeEventDelegate CheckChangeEventHandler { get; set; }

		public bool Checked
		{
			get { return _isChecked; }
			set
			{
				_isChecked = value;
				if (this.CheckChangeEventHandler != null) { this.CheckChangeEventHandler(this, _isChecked); }
			}
		}

		public GlyphConfiguration GlyphConfiguration { get; set; }
		public delegate void CheckChangeEventDelegate(CheckButton checkButton, bool isChecked);

		public CheckButton(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			_glyphVertices = new VertexC1P3T2[4];

			this.MouseButtonEventHandler = (button, args) =>
			{
				if (args.Button == MouseButton.Left)
				{
					if (args.IsPressed) this.Checked = !this.Checked;
				}
			};

			ApplySkin(_baseSkin);
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

				_glyphVertices[0].Pos.X = glyphTopLeft.X;
				_glyphVertices[0].Pos.Y = glyphTopLeft.Y;
				_glyphVertices[0].Pos.Z = zOffset + INNER_ZOFFSET;
				_glyphVertices[0].TexCoord.X = 0;
				_glyphVertices[0].TexCoord.Y = 0;
				_glyphVertices[0].Color = material.MainColor;

				_glyphVertices[1].Pos.X = glyphTopLeft.X;
				_glyphVertices[1].Pos.Y = glyphBottomRight.Y;
				_glyphVertices[1].Pos.Z = zOffset + INNER_ZOFFSET;
				_glyphVertices[1].TexCoord.X = 0;
				_glyphVertices[1].TexCoord.Y = tx.UVRatio.Y;
				_glyphVertices[1].Color = material.MainColor;

				_glyphVertices[2].Pos.X = glyphBottomRight.X;
				_glyphVertices[2].Pos.Y = glyphBottomRight.Y;
				_glyphVertices[2].Pos.Z = zOffset + INNER_ZOFFSET;
				_glyphVertices[2].TexCoord.X = tx.UVRatio.X;
				_glyphVertices[2].TexCoord.Y = tx.UVRatio.Y;
				_glyphVertices[2].Color = material.MainColor;

				_glyphVertices[3].Pos.X = glyphBottomRight.X;
				_glyphVertices[3].Pos.Y = glyphTopLeft.Y;
				_glyphVertices[3].Pos.Z = zOffset + INNER_ZOFFSET;
				_glyphVertices[3].TexCoord.X = tx.UVRatio.X;
				_glyphVertices[3].TexCoord.Y = 0;
				_glyphVertices[3].Color = material.MainColor;

				canvas.State.Reset();
				canvas.State.SetMaterial(material);
				canvas.DrawVertices<VertexC1P3T2>(_glyphVertices, VertexMode.Quads);
			}
		}
	}
}