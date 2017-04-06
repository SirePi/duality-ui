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
        public ContentRef<Material> Sprite { get; set; }
        public ColorRgba SpriteTint { get; set; }
        public bool IsAnimated { get; set; }
        public int FirstFrame { get; set; }
        public int AnimationFrames { get; set; }
        public float FrameDuration { get; set; }

        private float _frameTime;
        private int _currentFrame;

        public SpriteBox(Skin skin = null, string templateName = null)
            : base(skin, templateName)
        {
            SpriteTint = ColorRgba.White;
            ApplySkin(_baseSkin);
        }

        public override void OnUpdate(float msFrame)
        {
            base.OnUpdate(msFrame);

            if (_currentFrame < FirstFrame) _currentFrame = FirstFrame;

            if (IsAnimated)
            {
                _frameTime += msFrame;
                if (_frameTime > FrameDuration)
                {
                    _currentFrame++;
                    if (_currentFrame >= FirstFrame + AnimationFrames) _currentFrame = FirstFrame;

                    _frameTime -= FrameDuration;
                }
            }
            else
            {
                _frameTime = 0;
            }
        }

        public override void Draw(Canvas canvas, float zOffset)
        {
            base.Draw(canvas, zOffset);

            if (!Sprite.IsExplicitNull && !Sprite.Res.MainTexture.IsExplicitNull && !Sprite.Res.MainTexture.Res.BasePixmap.IsExplicitNull)
            {
                Texture tx = Sprite.Res.MainTexture.Res;
                Vector2 uv = tx.UVRatio / tx.Size;

                Pixmap pm = tx.BasePixmap.Res;
                Rect pixelsRect = pm.LookupAtlas(_currentFrame);
                Vector2 uvTopLeft = pixelsRect.TopLeft * uv;
                Vector2 uvBottomRight = pixelsRect.BottomRight * uv;

                VertexC1P3T2[] vertices = canvas.RequestVertexArray(4);
                Vector2 pixelsTopLeft = Vector2.Zero;
                Vector2 pixelsBottomRight = Vector2.Zero;

                switch (SpriteFill)
                {
                    case ImageFill.Stretch:
                        pixelsTopLeft = AlignElement((this.ActualSize - this.Appearance.Res.Border), this.Appearance.Res.Border, Alignment.Center);
                        pixelsBottomRight = pixelsTopLeft + this.ActualSize;
                        break;

                    case ImageFill.FitControl:
                        Vector2 ratios = (this.ActualSize - this.Appearance.Res.Border) / pixelsRect.Size;
                        float ratio = Math.Min(ratios.X, ratios.Y);
                        Vector2 realSize = pixelsRect.Size * ratio;

                        pixelsTopLeft = AlignElement(realSize, this.Appearance.Res.Border, this.SpriteAlignment);
                        pixelsBottomRight = pixelsTopLeft + realSize;
                        break;

                    case ImageFill.KeepSize:
                        pixelsTopLeft = AlignElement(pixelsRect.Size, this.Appearance.Res.Border, this.SpriteAlignment);
                        pixelsBottomRight = pixelsTopLeft + pixelsRect.Size;

                        float delta;

                        delta = this.ActualPosition.X + this.Appearance.Res.Border.Left - pixelsTopLeft.X;
                        if(delta > 0)
                        {
                            uvTopLeft.X += (delta * uv.X);
                            pixelsTopLeft.X = this.ActualPosition.X + this.Appearance.Res.Border.Left;
                        }

                        delta = this.ActualPosition.Y + this.Appearance.Res.Border.Top - pixelsTopLeft.Y;
                        if(delta > 0)
                        {
                            uvTopLeft.Y += (delta * uv.Y);
                            pixelsTopLeft.Y = this.ActualPosition.Y + this.Appearance.Res.Border.Top;
                        }

                        delta = this.ActualPosition.X + this.ActualSize.X - this.Appearance.Res.Border.Right - pixelsBottomRight.X;
                        if(delta < 0)
                        {
                            uvBottomRight.X += (delta * uv.X);
                            pixelsBottomRight.X = this.ActualPosition.X + this.ActualSize.X - this.Appearance.Res.Border.Right;
                        }

                        delta = this.ActualPosition.Y + this.ActualSize.Y - this.Appearance.Res.Border.Bottom - pixelsBottomRight.Y;
                        if (delta < 0)
                        {
                            uvBottomRight.Y += (delta * uv.Y);
                            pixelsBottomRight.Y = this.ActualPosition.Y + this.ActualSize.Y - this.Appearance.Res.Border.Bottom;
                        }
                        break;
                }

                vertices[0].Pos.X = pixelsTopLeft.X;
                vertices[0].Pos.Y = pixelsTopLeft.Y;
                vertices[0].Pos.Z = zOffset;
                vertices[0].TexCoord.X = uvTopLeft.X;
                vertices[0].TexCoord.Y = uvTopLeft.Y;
                vertices[0].Color = SpriteTint;

                vertices[1].Pos.X = pixelsTopLeft.X;
                vertices[1].Pos.Y = pixelsBottomRight.Y;
                vertices[1].Pos.Z = zOffset;
                vertices[1].TexCoord.X = uvTopLeft.X;
                vertices[1].TexCoord.Y = uvBottomRight.Y;
                vertices[1].Color = SpriteTint;

                vertices[2].Pos.X = pixelsBottomRight.X;
                vertices[2].Pos.Y = pixelsBottomRight.Y;
                vertices[2].Pos.Z = zOffset;
                vertices[2].TexCoord.X = uvBottomRight.X;
                vertices[2].TexCoord.Y = uvBottomRight.Y;
                vertices[2].Color = SpriteTint;

                vertices[3].Pos.X = pixelsBottomRight.X;
                vertices[3].Pos.Y = pixelsTopLeft.Y;
                vertices[3].Pos.Z = zOffset;
                vertices[3].TexCoord.X = uvBottomRight.X;
                vertices[3].TexCoord.Y = uvTopLeft.Y;
                vertices[3].Color = SpriteTint;

                canvas.State.Reset();
                canvas.State.SetMaterial(Sprite);
                canvas.DrawVertices<VertexC1P3T2>(vertices, VertexMode.Quads, 4);
            }
        }
    }
}