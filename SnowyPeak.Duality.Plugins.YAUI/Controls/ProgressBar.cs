// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
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
	public class ProgressBar : Control
	{
		public enum BarStyle
		{
			Stretching,
			Cutoff
		}

		private RawList<VertexC1P3T2> _barVertices;
		private float _value;

		public ProgressConfiguration ProgressConfiguration { get; set; }
		public string Text { get; set; }
		public TextConfiguration TextConfiguration { get; set; }

		public float Value
		{
			get { return _value; }
			set { _value = MathF.Max(0, MathF.Min(value, 1)); }
		}

		public ProgressBar(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			_barVertices = new RawList<VertexC1P3T2>(36);

			ApplySkin(_baseSkin);
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);
			ProgressTemplate template = _baseSkin.GetTemplate<ProgressTemplate>(this);
			this.ProgressConfiguration = template.ProgressConfiguration.Clone();
			this.TextConfiguration = template.TextConfiguration.Clone();
		}

		public override void Draw(Canvas canvas, float zOffset)
		{
			base.Draw(canvas, zOffset);

			if (this.ProgressConfiguration.BarAppearance.IsAvailable)
			{
				Appearance appearance = this.ProgressConfiguration.BarAppearance.Res;
				Material material = appearance[this.Status];

				Texture tx = material.MainTexture.Res;
				if (tx != null)
				{
					Vector2 topLeft = this.ActualPosition + this.ProgressConfiguration.Margin.TopLeft;
					Vector2 bottomRight = this.ActualPosition + this.ActualSize - this.ProgressConfiguration.Margin.BottomRight;

					Vector2 barTopLeft = topLeft;
					Vector2 barBottomRight = bottomRight;

					Vector2 uvSize = tx.UVRatio / tx.Size;
					Vector2 uvTopLeft = uvSize * appearance.Border.TopLeft;
					Vector2 uvBottomRight = tx.UVRatio - (uvSize * appearance.Border.BottomRight);

					switch (this.ProgressConfiguration.Direction)
					{
						case Direction.LeftToRight:
							barBottomRight.X = barTopLeft.X + (bottomRight.X - topLeft.X) * this.Value;
							break;

						case Direction.RightToLeft:
							barTopLeft.X = barBottomRight.X + (topLeft.X - bottomRight.X) * this.Value;
							break;

						case Direction.UpToDown:
							barBottomRight.Y = barTopLeft.Y + (bottomRight.Y - topLeft.Y) * this.Value;
							break;

						case Direction.DownToUp:
							barTopLeft.Y = barBottomRight.Y + (topLeft.Y - bottomRight.Y) * this.Value;
							break;
					}

					Vector2 innerTopLeft = barTopLeft + appearance.Border.TopLeft;
					Vector2 innerBottomRight = barBottomRight - appearance.Border.BottomRight;

					Vector2 innerSize = innerBottomRight - innerTopLeft;

					switch (this.ProgressConfiguration.BarStyle)
					{
						case BarStyle.Stretching:
							if (innerSize.X < (appearance.Border.Left + appearance.Border.Right))
							{
								float halfSize = innerSize.X / 2;
								innerTopLeft.X += halfSize;
								innerBottomRight.X -= halfSize;
							}
							if (innerSize.Y < (appearance.Border.Top + appearance.Border.Bottom))
							{
								float halfSize = innerSize.Y / 2;
								innerTopLeft.Y += halfSize;
								innerBottomRight.Y -= halfSize;
							}

							SetupBarVertex(0, barTopLeft.X, barTopLeft.Y, zOffset, 0, 0, material.MainColor);
							SetupBarVertex(1, barTopLeft.X, innerTopLeft.Y, zOffset, 0, uvTopLeft.Y, material.MainColor);
							SetupBarVertex(2, innerTopLeft.X, innerTopLeft.Y, zOffset, uvTopLeft.X, uvTopLeft.Y, material.MainColor);
							SetupBarVertex(3, innerTopLeft.X, barTopLeft.Y, zOffset, uvTopLeft.X, 0, material.MainColor);

							CopyBarVertex(4, 3);
							CopyBarVertex(5, 2);
							SetupBarVertex(6, innerBottomRight.X, innerTopLeft.Y, zOffset, uvBottomRight.X, uvTopLeft.Y, material.MainColor);
							SetupBarVertex(7, innerBottomRight.X, barTopLeft.Y, zOffset, uvBottomRight.X, 0, material.MainColor);

							CopyBarVertex(8, 7);
							CopyBarVertex(9, 6);
							SetupBarVertex(10, barBottomRight.X, innerTopLeft.Y, zOffset, tx.UVRatio.X, uvTopLeft.Y, material.MainColor);
							SetupBarVertex(11, barBottomRight.X, barTopLeft.Y, zOffset, tx.UVRatio.X, 0, material.MainColor);

							CopyBarVertex(12, 1);
							SetupBarVertex(13, barTopLeft.X, innerBottomRight.Y, zOffset, 0, uvBottomRight.Y, material.MainColor);
							SetupBarVertex(14, innerTopLeft.X, innerBottomRight.Y, zOffset, uvTopLeft.X, uvBottomRight.Y, material.MainColor);
							CopyBarVertex(15, 2);

							CopyBarVertex(16, 2);
							CopyBarVertex(17, 14);
							SetupBarVertex(18, innerBottomRight.X, innerBottomRight.Y, zOffset, uvBottomRight.X, uvBottomRight.Y, material.MainColor);
							CopyBarVertex(19, 6);

							CopyBarVertex(20, 6);
							CopyBarVertex(21, 18);
							SetupBarVertex(22, barBottomRight.X, innerBottomRight.Y, zOffset, tx.UVRatio.X, uvBottomRight.Y, material.MainColor);
							CopyBarVertex(23, 10);

							CopyBarVertex(24, 13);
							SetupBarVertex(25, barTopLeft.X, barBottomRight.Y, zOffset, 0, tx.UVRatio.Y, material.MainColor);
							SetupBarVertex(26, innerTopLeft.X, barBottomRight.Y, zOffset, uvTopLeft.X, tx.UVRatio.Y, material.MainColor);
							CopyBarVertex(27, 14);

							CopyBarVertex(28, 14);
							CopyBarVertex(29, 26);
							SetupBarVertex(30, innerBottomRight.X, barBottomRight.Y, zOffset, uvBottomRight.X, tx.UVRatio.Y, material.MainColor);
							CopyBarVertex(31, 18);

							CopyBarVertex(32, 18);
							CopyBarVertex(33, 30);
							SetupBarVertex(34, barBottomRight.X, barBottomRight.Y, zOffset, tx.UVRatio.X, tx.UVRatio.Y, material.MainColor);
							CopyBarVertex(35, 22);
							break;

						case BarStyle.Cutoff:
							Vector2 fullSize = this.ActualSize - this.ProgressConfiguration.Margin.TopLeft - this.ProgressConfiguration.Margin.BottomRight;
							Vector2 barSize = barBottomRight - barTopLeft;

							Vector2 uvStart = Vector2.Zero;
							Vector2 uvEnd = tx.UVRatio;

							switch (this.ProgressConfiguration.Direction)
							{
								case Direction.LeftToRight:
									if (barSize.X < appearance.Border.Left)
									{
										innerTopLeft.X = barTopLeft.X + barSize.X;
										innerBottomRight.X = barTopLeft.X + barSize.X;

										uvTopLeft.X = uvTopLeft.X * barSize.X / appearance.Border.Left;
									}
									else if (barSize.X < fullSize.X - appearance.Border.Right)
									{
										innerBottomRight.X = barTopLeft.X + barSize.X;
									}
									else
									{
										innerBottomRight.X = barTopLeft.X + fullSize.X - appearance.Border.Right;

										uvEnd.X = uvBottomRight.X + (uvEnd.X - uvBottomRight.X) * (1 - ((fullSize.X - barSize.X) / appearance.Border.Right));
									}
									break;

								case Direction.RightToLeft:
									if (barSize.X < appearance.Border.Right)
									{
										innerTopLeft.X = barTopLeft.X;
										innerBottomRight.X = barTopLeft.X;

										uvBottomRight.X = uvEnd.X - ((uvEnd.X - uvBottomRight.X) * barSize.X / appearance.Border.Right);
									}
									else if (barSize.X < fullSize.X - appearance.Border.Left)
									{
										innerTopLeft.X = barTopLeft.X;
									}
									else
									{
										innerTopLeft.X = topLeft.X + appearance.Border.Left;

										uvStart.X = uvTopLeft.X * (fullSize.X - barSize.X) / appearance.Border.Left;
									}
									break;

								case Direction.UpToDown:
									if (barSize.Y < appearance.Border.Top)
									{
										innerTopLeft.Y = barTopLeft.Y + barSize.Y;
										innerBottomRight.Y = barTopLeft.Y + barSize.Y;

										uvTopLeft.Y = uvTopLeft.Y * barSize.Y / appearance.Border.Top;
									}
									else if (barSize.Y < fullSize.Y - appearance.Border.Bottom)
									{
										innerBottomRight.Y = barTopLeft.Y + barSize.Y;
									}
									else
									{
										innerBottomRight.Y = barTopLeft.Y + fullSize.Y - appearance.Border.Bottom;

										uvEnd.Y = uvBottomRight.Y + (uvEnd.Y - uvBottomRight.Y) * (1 - ((fullSize.Y - barSize.Y) / appearance.Border.Bottom));
									}
									break;

								case Direction.DownToUp:
									if (barSize.Y < appearance.Border.Bottom)
									{
										innerTopLeft.Y = barTopLeft.Y;
										innerBottomRight.Y = barTopLeft.Y;

										uvBottomRight.Y = uvEnd.Y - ((uvEnd.Y - uvBottomRight.Y) * barSize.Y / appearance.Border.Bottom);
									}
									else if (barSize.Y < fullSize.Y - appearance.Border.Top)
									{
										innerTopLeft.Y = barTopLeft.Y;
									}
									else
									{
										innerTopLeft.Y = topLeft.Y + appearance.Border.Top;

										uvStart.Y = uvTopLeft.Y * (fullSize.Y - barSize.Y) / appearance.Border.Top;
									}
									break;
							}

							SetupBarVertex(0, barTopLeft.X, barTopLeft.Y, zOffset, uvStart.X, uvStart.Y, material.MainColor);
							SetupBarVertex(1, barTopLeft.X, innerTopLeft.Y, zOffset, uvStart.X, uvTopLeft.Y, material.MainColor);
							SetupBarVertex(2, innerTopLeft.X, innerTopLeft.Y, zOffset, uvTopLeft.X, uvTopLeft.Y, material.MainColor);
							SetupBarVertex(3, innerTopLeft.X, barTopLeft.Y, zOffset, uvTopLeft.X, uvStart.Y, material.MainColor);

							CopyBarVertex(4, 3);
							CopyBarVertex(5, 2);
							SetupBarVertex(6, innerBottomRight.X, innerTopLeft.Y, zOffset, uvBottomRight.X, uvTopLeft.Y, material.MainColor);
							SetupBarVertex(7, innerBottomRight.X, barTopLeft.Y, zOffset, uvBottomRight.X, uvStart.Y, material.MainColor);

							CopyBarVertex(8, 7);
							CopyBarVertex(9, 6);
							SetupBarVertex(10, barBottomRight.X, innerTopLeft.Y, zOffset, uvEnd.X, uvTopLeft.Y, material.MainColor);
							SetupBarVertex(11, barBottomRight.X, barTopLeft.Y, zOffset, uvEnd.X, uvStart.Y, material.MainColor);

							CopyBarVertex(12, 1);
							SetupBarVertex(13, barTopLeft.X, innerBottomRight.Y, zOffset, uvStart.X, uvBottomRight.Y, material.MainColor);
							SetupBarVertex(14, innerTopLeft.X, innerBottomRight.Y, zOffset, uvTopLeft.X, uvBottomRight.Y, material.MainColor);
							CopyBarVertex(15, 2);

							CopyBarVertex(16, 2);
							CopyBarVertex(17, 14);
							SetupBarVertex(18, innerBottomRight.X, innerBottomRight.Y, zOffset, uvBottomRight.X, uvBottomRight.Y, material.MainColor);
							CopyBarVertex(19, 6);

							CopyBarVertex(20, 6);
							CopyBarVertex(21, 18);
							SetupBarVertex(22, barBottomRight.X, innerBottomRight.Y, zOffset, uvEnd.X, uvBottomRight.Y, material.MainColor);
							CopyBarVertex(23, 10);

							CopyBarVertex(24, 13);
							SetupBarVertex(25, barTopLeft.X, barBottomRight.Y, zOffset, uvStart.X, uvEnd.Y, material.MainColor);
							SetupBarVertex(26, innerTopLeft.X, barBottomRight.Y, zOffset, uvTopLeft.X, uvEnd.Y, material.MainColor);
							CopyBarVertex(27, 14);

							CopyBarVertex(28, 14);
							CopyBarVertex(29, 26);
							SetupBarVertex(30, innerBottomRight.X, barBottomRight.Y, zOffset, uvBottomRight.X, uvEnd.Y, material.MainColor);
							CopyBarVertex(31, 18);

							CopyBarVertex(32, 18);
							CopyBarVertex(33, 30);
							SetupBarVertex(34, barBottomRight.X, barBottomRight.Y, zOffset, uvEnd.X, uvEnd.Y, material.MainColor);
							CopyBarVertex(35, 22);
							break;
					}

					canvas.State.Reset();
					canvas.State.SetMaterial(material);
					canvas.DrawVertices<VertexC1P3T2>(_barVertices.Data, VertexMode.Quads, 36);
				}
			}

			Vector2 textPosition = AlignElement(Vector2.Zero, this.TextConfiguration.Margin, this.TextConfiguration.Alignment);

			if (!String.IsNullOrWhiteSpace(this.Text))
			{
				canvas.State.Reset();
				canvas.State.ColorTint = this.TextConfiguration.Color;
				canvas.State.TextFont = this.TextConfiguration.Font;

				canvas.DrawText(this.Text,
					(int)textPosition.X,
					(int)textPosition.Y,
					zOffset + (INNER_ZOFFSET * 2),
					this.TextConfiguration.Alignment);
			}
		}

		protected void CopyBarVertex(int destinationIndex, int sourceIndex)
		{
			_barVertices.Data[destinationIndex].Pos = _barVertices.Data[sourceIndex].Pos;
			_barVertices.Data[destinationIndex].TexCoord = _barVertices.Data[sourceIndex].TexCoord;
			_barVertices.Data[destinationIndex].Color = _barVertices.Data[sourceIndex].Color;
		}

		private void SetupBarVertex(int index, float x, float y, float z, float uvX, float uvY, ColorRgba color)
		{
			_barVertices.Data[index].Pos.X = x;
			_barVertices.Data[index].Pos.Y = y;
			_barVertices.Data[index].Pos.Z = z;
			_barVertices.Data[index].TexCoord.X = uvX;
			_barVertices.Data[index].TexCoord.Y = uvY;
			_barVertices.Data[index].Color = color;
		}
	}
}