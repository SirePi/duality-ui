// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using SnowyPeak.Duality.Plugins.YAUI.Templates;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public class CheckButton : Button<GlyphTemplate>
	{
		private readonly RawList<VertexC1P3T2> glyphVertices = new RawList<VertexC1P3T2>();
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

		public bool UseToggleStyle { get; set; }

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
			this.GlyphConfiguration = this.Template.GlyphConfiguration;
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			if (this.UseToggleStyle)
			{
				if (this.Checked) this.Status |= ControlStatus.Active;
				else this.Status &= ~ControlStatus.Active;
			}
		}

		protected override void _Draw(Canvas canvas, float zOffset)
		{
			base._Draw(canvas, zOffset);

			if (!this.UseToggleStyle && 
				this.GlyphConfiguration.Glyph.IsAvailable &&
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
