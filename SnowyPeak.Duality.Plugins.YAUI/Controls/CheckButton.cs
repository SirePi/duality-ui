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
	public class CheckButton : Button<GlyphTemplate>
	{
		private RawList<VertexC1P3T2> glyphVertices;
		private bool isChecked;

		public bool Checked
		{
			get => this.isChecked;
			set
			{
				this.onCheckedChange?.Invoke(this, this.isChecked, value);
				this.isChecked = value;
			}
		}

		public GlyphConfiguration GlyphConfiguration { get; set; }

		// Delegates
		public delegate void CheckedChangeEventDelegate(CheckButton checkButton, bool previousValue, bool newValue);

		// Events
		[DontSerialize]
		private CheckedChangeEventDelegate onCheckedChange;
		public event CheckedChangeEventDelegate OnCheckedChange
		{
			add { this.onCheckedChange += value; }
			remove { this.onCheckedChange -= value; }
		}

		public CheckButton(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		protected override void Init()
		{
			base.Init();
			this.glyphVertices = new RawList<VertexC1P3T2>(4);
			this.OnMouseButton += this.CheckButton_OnMouseButton;
		}

		private void CheckButton_OnMouseButton(IInteractiveControl button, MouseButtonEventArgs args)
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
			this.GlyphConfiguration = this.Template.GlyphConfiguration.Clone();
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

				Vector2 glyphTopLeft = this.AlignElement(tx.Size, this.GlyphConfiguration.Margin, this.GlyphConfiguration.Alignment);
				Vector2 glyphBottomRight = glyphTopLeft + tx.Size;

				this.glyphVertices.Data[0].Pos.X = glyphTopLeft.X;
				this.glyphVertices.Data[0].Pos.Y = glyphTopLeft.Y;
				this.glyphVertices.Data[0].Pos.Z = zOffset + INNER_ZOFFSET;
				this.glyphVertices.Data[0].TexCoord.X = 0;
				this.glyphVertices.Data[0].TexCoord.Y = 0;
				this.glyphVertices.Data[0].Color = material.MainColor;

				this.glyphVertices.Data[1].Pos.X = glyphTopLeft.X;
				this.glyphVertices.Data[1].Pos.Y = glyphBottomRight.Y;
				this.glyphVertices.Data[1].Pos.Z = zOffset + INNER_ZOFFSET;
				this.glyphVertices.Data[1].TexCoord.X = 0;
				this.glyphVertices.Data[1].TexCoord.Y = tx.UVRatio.Y;
				this.glyphVertices.Data[1].Color = material.MainColor;

				this.glyphVertices.Data[2].Pos.X = glyphBottomRight.X;
				this.glyphVertices.Data[2].Pos.Y = glyphBottomRight.Y;
				this.glyphVertices.Data[2].Pos.Z = zOffset + INNER_ZOFFSET;
				this.glyphVertices.Data[2].TexCoord.X = tx.UVRatio.X;
				this.glyphVertices.Data[2].TexCoord.Y = tx.UVRatio.Y;
				this.glyphVertices.Data[2].Color = material.MainColor;

				this.glyphVertices.Data[3].Pos.X = glyphBottomRight.X;
				this.glyphVertices.Data[3].Pos.Y = glyphTopLeft.Y;
				this.glyphVertices.Data[3].Pos.Z = zOffset + INNER_ZOFFSET;
				this.glyphVertices.Data[3].TexCoord.X = tx.UVRatio.X;
				this.glyphVertices.Data[3].TexCoord.Y = 0;
				this.glyphVertices.Data[3].Color = material.MainColor;

				canvas.State.Reset();
				canvas.State.SetMaterial(material);
				canvas.DrawVertices<VertexC1P3T2>(this.glyphVertices.Data, VertexMode.Quads, 4);
			}
		}
	}
}
