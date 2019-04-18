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
		private RawList<VertexC1P3T2> _glyphVertices;
		private bool _isChecked;

		public bool Checked
		{
			get { return _isChecked; }
			set
			{
				_onCheckedChange?.Invoke(this, _isChecked, value);
				_isChecked = value;
			}
		}

		public GlyphConfiguration GlyphConfiguration { get; set; }

		// Delegates
		public delegate void CheckedChangeEventDelegate(CheckButton checkButton, bool previousValue, bool newValue);

		// Events
		[DontSerialize]
		private CheckedChangeEventDelegate _onCheckedChange;
		public event CheckedChangeEventDelegate OnCheckedChange
		{
			add { _onCheckedChange += value; }
			remove { _onCheckedChange -= value; }
		}

		public CheckButton(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			_glyphVertices = new RawList<VertexC1P3T2>(4);

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

				_glyphVertices.Data[0].Pos.X = glyphTopLeft.X;
				_glyphVertices.Data[0].Pos.Y = glyphTopLeft.Y;
				_glyphVertices.Data[0].Pos.Z = zOffset + INNER_ZOFFSET;
				_glyphVertices.Data[0].TexCoord.X = 0;
				_glyphVertices.Data[0].TexCoord.Y = 0;
				_glyphVertices.Data[0].Color = material.MainColor;

				_glyphVertices.Data[1].Pos.X = glyphTopLeft.X;
				_glyphVertices.Data[1].Pos.Y = glyphBottomRight.Y;
				_glyphVertices.Data[1].Pos.Z = zOffset + INNER_ZOFFSET;
				_glyphVertices.Data[1].TexCoord.X = 0;
				_glyphVertices.Data[1].TexCoord.Y = tx.UVRatio.Y;
				_glyphVertices.Data[1].Color = material.MainColor;

				_glyphVertices.Data[2].Pos.X = glyphBottomRight.X;
				_glyphVertices.Data[2].Pos.Y = glyphBottomRight.Y;
				_glyphVertices.Data[2].Pos.Z = zOffset + INNER_ZOFFSET;
				_glyphVertices.Data[2].TexCoord.X = tx.UVRatio.X;
				_glyphVertices.Data[2].TexCoord.Y = tx.UVRatio.Y;
				_glyphVertices.Data[2].Color = material.MainColor;

				_glyphVertices.Data[3].Pos.X = glyphBottomRight.X;
				_glyphVertices.Data[3].Pos.Y = glyphTopLeft.Y;
				_glyphVertices.Data[3].Pos.Z = zOffset + INNER_ZOFFSET;
				_glyphVertices.Data[3].TexCoord.X = tx.UVRatio.X;
				_glyphVertices.Data[3].TexCoord.Y = 0;
				_glyphVertices.Data[3].Color = material.MainColor;

				canvas.State.Reset();
				canvas.State.SetMaterial(material);
				canvas.DrawVertices<VertexC1P3T2>(_glyphVertices.Data, VertexMode.Quads, 4);
			}
		}
	}
}