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
	public class ProgressBar : Control<ProgressTemplate>
	{
		public enum BarStyle
		{
			Stretching,
			Cutoff
		}

		private RawList<VertexC1P3T2> barVertices;
		private float value;

		public ProgressConfiguration ProgressConfiguration { get; set; }
		public string Text { get; set; }
		public TextConfiguration TextConfiguration { get; set; }

		public float Value
		{
			get => this.value;
			set => this.value = MathF.Max(0, MathF.Min(value, 1));
		}

		public ProgressBar(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		protected override void Init()
		{
			base.Init();
		
			this.barVertices = new RawList<VertexC1P3T2>(36);
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);
			
			this.ProgressConfiguration = this.Template.ProgressConfiguration.Clone();
			this.TextConfiguration = this.Template.TextConfiguration.Clone();
		}

		protected override void _Draw(Canvas canvas, float zOffset)
		{
			base._Draw(canvas, zOffset);

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

							this.SetupBarVertex(0, barTopLeft.X, barTopLeft.Y, zOffset, 0, 0, material.MainColor);
							this.SetupBarVertex(1, barTopLeft.X, innerTopLeft.Y, zOffset, 0, uvTopLeft.Y, material.MainColor);
							this.SetupBarVertex(2, innerTopLeft.X, innerTopLeft.Y, zOffset, uvTopLeft.X, uvTopLeft.Y, material.MainColor);
							this.SetupBarVertex(3, innerTopLeft.X, barTopLeft.Y, zOffset, uvTopLeft.X, 0, material.MainColor);

							this.CopyBarVertex(4, 3);
							this.CopyBarVertex(5, 2);
							this.SetupBarVertex(6, innerBottomRight.X, innerTopLeft.Y, zOffset, uvBottomRight.X, uvTopLeft.Y, material.MainColor);
							this.SetupBarVertex(7, innerBottomRight.X, barTopLeft.Y, zOffset, uvBottomRight.X, 0, material.MainColor);

							this.CopyBarVertex(8, 7);
							this.CopyBarVertex(9, 6);
							this.SetupBarVertex(10, barBottomRight.X, innerTopLeft.Y, zOffset, tx.UVRatio.X, uvTopLeft.Y, material.MainColor);
							this.SetupBarVertex(11, barBottomRight.X, barTopLeft.Y, zOffset, tx.UVRatio.X, 0, material.MainColor);

							this.CopyBarVertex(12, 1);
							this.SetupBarVertex(13, barTopLeft.X, innerBottomRight.Y, zOffset, 0, uvBottomRight.Y, material.MainColor);
							this.SetupBarVertex(14, innerTopLeft.X, innerBottomRight.Y, zOffset, uvTopLeft.X, uvBottomRight.Y, material.MainColor);
							this.CopyBarVertex(15, 2);

							this.CopyBarVertex(16, 2);
							this.CopyBarVertex(17, 14);
							this.SetupBarVertex(18, innerBottomRight.X, innerBottomRight.Y, zOffset, uvBottomRight.X, uvBottomRight.Y, material.MainColor);
							this.CopyBarVertex(19, 6);

							this.CopyBarVertex(20, 6);
							this.CopyBarVertex(21, 18);
							this.SetupBarVertex(22, barBottomRight.X, innerBottomRight.Y, zOffset, tx.UVRatio.X, uvBottomRight.Y, material.MainColor);
							this.CopyBarVertex(23, 10);

							this.CopyBarVertex(24, 13);
							this.SetupBarVertex(25, barTopLeft.X, barBottomRight.Y, zOffset, 0, tx.UVRatio.Y, material.MainColor);
							this.SetupBarVertex(26, innerTopLeft.X, barBottomRight.Y, zOffset, uvTopLeft.X, tx.UVRatio.Y, material.MainColor);
							this.CopyBarVertex(27, 14);

							this.CopyBarVertex(28, 14);
							this.CopyBarVertex(29, 26);
							this.SetupBarVertex(30, innerBottomRight.X, barBottomRight.Y, zOffset, uvBottomRight.X, tx.UVRatio.Y, material.MainColor);
							this.CopyBarVertex(31, 18);

							this.CopyBarVertex(32, 18);
							this.CopyBarVertex(33, 30);
							this.SetupBarVertex(34, barBottomRight.X, barBottomRight.Y, zOffset, tx.UVRatio.X, tx.UVRatio.Y, material.MainColor);
							this.CopyBarVertex(35, 22);
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

							this.SetupBarVertex(0, barTopLeft.X, barTopLeft.Y, zOffset, uvStart.X, uvStart.Y, material.MainColor);
							this.SetupBarVertex(1, barTopLeft.X, innerTopLeft.Y, zOffset, uvStart.X, uvTopLeft.Y, material.MainColor);
							this.SetupBarVertex(2, innerTopLeft.X, innerTopLeft.Y, zOffset, uvTopLeft.X, uvTopLeft.Y, material.MainColor);
							this.SetupBarVertex(3, innerTopLeft.X, barTopLeft.Y, zOffset, uvTopLeft.X, uvStart.Y, material.MainColor);

							this.CopyBarVertex(4, 3);
							this.CopyBarVertex(5, 2);
							this.SetupBarVertex(6, innerBottomRight.X, innerTopLeft.Y, zOffset, uvBottomRight.X, uvTopLeft.Y, material.MainColor);
							this.SetupBarVertex(7, innerBottomRight.X, barTopLeft.Y, zOffset, uvBottomRight.X, uvStart.Y, material.MainColor);

							this.CopyBarVertex(8, 7);
							this.CopyBarVertex(9, 6);
							this.SetupBarVertex(10, barBottomRight.X, innerTopLeft.Y, zOffset, uvEnd.X, uvTopLeft.Y, material.MainColor);
							this.SetupBarVertex(11, barBottomRight.X, barTopLeft.Y, zOffset, uvEnd.X, uvStart.Y, material.MainColor);

							this.CopyBarVertex(12, 1);
							this.SetupBarVertex(13, barTopLeft.X, innerBottomRight.Y, zOffset, uvStart.X, uvBottomRight.Y, material.MainColor);
							this.SetupBarVertex(14, innerTopLeft.X, innerBottomRight.Y, zOffset, uvTopLeft.X, uvBottomRight.Y, material.MainColor);
							this.CopyBarVertex(15, 2);

							this.CopyBarVertex(16, 2);
							this.CopyBarVertex(17, 14);
							this.SetupBarVertex(18, innerBottomRight.X, innerBottomRight.Y, zOffset, uvBottomRight.X, uvBottomRight.Y, material.MainColor);
							this.CopyBarVertex(19, 6);

							this.CopyBarVertex(20, 6);
							this.CopyBarVertex(21, 18);
							this.SetupBarVertex(22, barBottomRight.X, innerBottomRight.Y, zOffset, uvEnd.X, uvBottomRight.Y, material.MainColor);
							this.CopyBarVertex(23, 10);

							this.CopyBarVertex(24, 13);
							this.SetupBarVertex(25, barTopLeft.X, barBottomRight.Y, zOffset, uvStart.X, uvEnd.Y, material.MainColor);
							this.SetupBarVertex(26, innerTopLeft.X, barBottomRight.Y, zOffset, uvTopLeft.X, uvEnd.Y, material.MainColor);
							this.CopyBarVertex(27, 14);

							this.CopyBarVertex(28, 14);
							this.CopyBarVertex(29, 26);
							this.SetupBarVertex(30, innerBottomRight.X, barBottomRight.Y, zOffset, uvBottomRight.X, uvEnd.Y, material.MainColor);
							this.CopyBarVertex(31, 18);

							this.CopyBarVertex(32, 18);
							this.CopyBarVertex(33, 30);
							this.SetupBarVertex(34, barBottomRight.X, barBottomRight.Y, zOffset, uvEnd.X, uvEnd.Y, material.MainColor);
							this.CopyBarVertex(35, 22);
							break;
					}

					canvas.State.Reset();
					canvas.State.SetMaterial(material);
					canvas.DrawVertices<VertexC1P3T2>(this.barVertices.Data, VertexMode.Quads, 36);
				}
			}

			Vector2 textPosition = this.AlignElement(Vector2.Zero, this.TextConfiguration.Margin, this.TextConfiguration.Alignment);

			if (!string.IsNullOrWhiteSpace(this.Text))
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
			this.barVertices.Data[destinationIndex].Pos = this.barVertices.Data[sourceIndex].Pos;
			this.barVertices.Data[destinationIndex].TexCoord = this.barVertices.Data[sourceIndex].TexCoord;
			this.barVertices.Data[destinationIndex].Color = this.barVertices.Data[sourceIndex].Color;
		}

		private void SetupBarVertex(int index, float x, float y, float z, float uvX, float uvY, ColorRgba color)
		{
			this.barVertices.Data[index].Pos.X = x;
			this.barVertices.Data[index].Pos.Y = y;
			this.barVertices.Data[index].Pos.Z = z;
			this.barVertices.Data[index].TexCoord.X = uvX;
			this.barVertices.Data[index].TexCoord.Y = uvY;
			this.barVertices.Data[index].Color = color;
		}
	}
}
