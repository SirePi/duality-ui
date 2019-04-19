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
	public class SpriteBox : Control
	{
		public ImageFill SpriteFill { get; set; }
		public Alignment SpriteAlignment { get; set; }
		public BatchInfo Sprite { get; set; }
		public ColorRgba SpriteTint { get; set; }
		public bool IsAnimated { get; set; }
		public int FirstFrame { get; set; }
		public int AnimationFrames { get; set; }
		public float FrameDuration { get; set; }

		public ShaderParameterCollection SpriteVariables { get; private set; }

		private float frameTime;
		private int currentFrame;
		private RawList<VertexC1P3T2> spriteVertices;

		public SpriteBox(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			this.spriteVertices = new RawList<VertexC1P3T2>(36);
			this.SpriteTint = ColorRgba.White;
			this.SpriteVariables = new ShaderParameterCollection();
			this.SpriteAlignment = Alignment.Center;
			this.SpriteFill = ImageFill.FitControl;

			this.ApplySkin(this.baseSkin);
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			if (this.currentFrame < this.FirstFrame)
				this.currentFrame = this.FirstFrame;

			if (this.IsAnimated)
			{
				this.frameTime += msFrame;
				if (this.frameTime > this.FrameDuration)
				{
					this.currentFrame++;
					if (this.currentFrame >= this.FirstFrame + this.AnimationFrames)
						this.currentFrame = this.FirstFrame;

					this.frameTime -= this.FrameDuration;
				}
			}
			else
			{
				this.frameTime = 0;
			}
		}

		public override void Draw(Canvas canvas, float zOffset)
		{
			base.Draw(canvas, zOffset);

			if (this.Sprite != null)
			{
				Border border = (this.Appearance.Res?.Border).GetValueOrDefault();
				ContentRef<Texture> mainTex = (this.Sprite?.MainTexture).GetValueOrDefault();

				Vector2 pixelsTopLeft = this.AlignElement(Vector2.Zero, border, Alignment.TopLeft);
				Vector2 pixelsBottomRight = this.AlignElement(Vector2.Zero, border, Alignment.BottomRight);
				Vector2 uvTopLeft = Vector2.Zero;
				Vector2 uvBottomRight = Vector2.One;

				if (mainTex.IsAvailable)
				{
					Texture tx = mainTex.Res;
					Vector2 uv = tx.UVRatio / tx.Size;

					Pixmap pm = tx.BasePixmap.Res;
					Rect pixelsRect = pm.LookupAtlas(this.currentFrame);
					uvTopLeft = pixelsRect.TopLeft * uv;
					uvBottomRight = pixelsRect.BottomRight * uv;

					switch (this.SpriteFill)
					{
						case ImageFill.Stretch:
							pixelsTopLeft = this.AlignElement((this.ActualSize - border), border, Alignment.Center);
							pixelsBottomRight = pixelsTopLeft + this.ActualSize;
							break;

						case ImageFill.FitControl:
							Vector2 ratios = (this.ActualSize - border) / pixelsRect.Size;
							float ratio = Math.Min(ratios.X, ratios.Y);
							Vector2 realSize = pixelsRect.Size * ratio;

							pixelsTopLeft = this.AlignElement(realSize, border, this.SpriteAlignment);
							pixelsBottomRight = pixelsTopLeft + realSize;
							break;

						case ImageFill.KeepSize:
							pixelsTopLeft = this.AlignElement(pixelsRect.Size, border, this.SpriteAlignment);
							pixelsBottomRight = pixelsTopLeft + pixelsRect.Size;

							float delta;

							delta = this.ActualPosition.X + border.Left - pixelsTopLeft.X;
							if (delta > 0)
							{
								uvTopLeft.X += (delta * uv.X);
								pixelsTopLeft.X = this.ActualPosition.X + border.Left;
							}

							delta = this.ActualPosition.Y + border.Top - pixelsTopLeft.Y;
							if (delta > 0)
							{
								uvTopLeft.Y += (delta * uv.Y);
								pixelsTopLeft.Y = this.ActualPosition.Y + border.Top;
							}

							delta = this.ActualPosition.X + this.ActualSize.X - border.Right - pixelsBottomRight.X;
							if (delta < 0)
							{
								uvBottomRight.X += (delta * uv.X);
								pixelsBottomRight.X = this.ActualPosition.X + this.ActualSize.X - border.Right;
							}

							delta = this.ActualPosition.Y + this.ActualSize.Y - border.Bottom - pixelsBottomRight.Y;
							if (delta < 0)
							{
								uvBottomRight.Y += (delta * uv.Y);
								pixelsBottomRight.Y = this.ActualPosition.Y + this.ActualSize.Y - border.Bottom;
							}
							break;
					}
				}

				this.Sprite.SetVariables(this.SpriteVariables);

				this.spriteVertices.Data[0].Pos.X = pixelsTopLeft.X;
				this.spriteVertices.Data[0].Pos.Y = pixelsTopLeft.Y;
				this.spriteVertices.Data[0].Pos.Z = zOffset;
				this.spriteVertices.Data[0].TexCoord.X = uvTopLeft.X;
				this.spriteVertices.Data[0].TexCoord.Y = uvTopLeft.Y;
				this.spriteVertices.Data[0].Color = this.SpriteTint;

				this.spriteVertices.Data[1].Pos.X = pixelsTopLeft.X;
				this.spriteVertices.Data[1].Pos.Y = pixelsBottomRight.Y;
				this.spriteVertices.Data[1].Pos.Z = zOffset;
				this.spriteVertices.Data[1].TexCoord.X = uvTopLeft.X;
				this.spriteVertices.Data[1].TexCoord.Y = uvBottomRight.Y;
				this.spriteVertices.Data[1].Color = this.SpriteTint;

				this.spriteVertices.Data[2].Pos.X = pixelsBottomRight.X;
				this.spriteVertices.Data[2].Pos.Y = pixelsBottomRight.Y;
				this.spriteVertices.Data[2].Pos.Z = zOffset;
				this.spriteVertices.Data[2].TexCoord.X = uvBottomRight.X;
				this.spriteVertices.Data[2].TexCoord.Y = uvBottomRight.Y;
				this.spriteVertices.Data[2].Color = this.SpriteTint;

				this.spriteVertices.Data[3].Pos.X = pixelsBottomRight.X;
				this.spriteVertices.Data[3].Pos.Y = pixelsTopLeft.Y;
				this.spriteVertices.Data[3].Pos.Z = zOffset;
				this.spriteVertices.Data[3].TexCoord.X = uvBottomRight.X;
				this.spriteVertices.Data[3].TexCoord.Y = uvTopLeft.Y;
				this.spriteVertices.Data[3].Color = this.SpriteTint;

				canvas.State.Reset();
				canvas.State.SetMaterial(this.Sprite);
				canvas.DrawVertices<VertexC1P3T2>(this.spriteVertices.Data, VertexMode.Quads, 4);
			}
		}
	}
}
