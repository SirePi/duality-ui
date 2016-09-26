using Duality;
using Duality.Drawing;
using Duality.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls
{
	public abstract class SimpleControl : Control
	{
		public ContentRef<Appearance> Appearance { get; set; }

		protected VertexC1P3T2[] _vertices;

		protected SimpleControl()
		{
			_vertices = new VertexC1P3T2[36];

			if (!(this is ControlsContainer))
			{ this.Appearance = new ContentRef<Appearance>(SnowyPeak.DualityUI.Appearance.DEFAULT); }
		}

		public override void Draw(Canvas canvas, float zOffset)
		{
			if (this.Appearance.IsAvailable)
			{
				Appearance appearance = this.Appearance.Res;
				Material material = appearance[this.Status];

				Vector2 topLeft = this.ActualPosition;
				Vector2 bottomRight = this.ActualPosition + this.ActualSize;

				if (material != null && material.MainTexture.IsAvailable)
				{
					Vector2 innerTopLeft = new Vector2(
						this.ActualPosition.X + appearance.Border.Left,
						this.ActualPosition.Y + appearance.Border.Top
						);

					Vector2 innerBottomRight = new Vector2(
						this.ActualPosition.X + this.ActualSize.X - appearance.Border.Right,
						this.ActualPosition.Y + this.ActualSize.Y - appearance.Border.Bottom
						);

					Texture tx = material.MainTexture.Res;
					if (tx != null)
					{
						Vector2 uvSize = tx.UVRatio / tx.Size;
						Vector2 uvTopLeft = uvSize * new Vector2(appearance.Border.Left, appearance.Border.Top);
						Vector2 uvBottomRight = tx.UVRatio - (uvSize * new Vector2(appearance.Border.Right, appearance.Border.Bottom));

						SetupVertex(0, topLeft.X, topLeft.Y, zOffset, 0, 0, material.MainColor);
						SetupVertex(1, topLeft.X, innerTopLeft.Y, zOffset, 0, uvTopLeft.Y, material.MainColor);
						SetupVertex(2, innerTopLeft.X, innerTopLeft.Y, zOffset, uvTopLeft.X, uvTopLeft.Y, material.MainColor);
						SetupVertex(3, innerTopLeft.X, topLeft.Y, zOffset, uvTopLeft.X, 0, material.MainColor);

						CopyVertex(4, 3);
						CopyVertex(5, 2);
						SetupVertex(6, innerBottomRight.X, innerTopLeft.Y, zOffset, uvBottomRight.X, uvTopLeft.Y, material.MainColor);
						SetupVertex(7, innerBottomRight.X, topLeft.Y, zOffset, uvBottomRight.X, 0, material.MainColor);

						CopyVertex(8, 7);
						CopyVertex(9, 6);
						SetupVertex(10, bottomRight.X, innerTopLeft.Y, zOffset, tx.UVRatio.X, uvTopLeft.Y, material.MainColor);
						SetupVertex(11, bottomRight.X, topLeft.Y, zOffset, tx.UVRatio.X, 0, material.MainColor);

						CopyVertex(12, 1);
						SetupVertex(13, topLeft.X, innerBottomRight.Y, zOffset, 0, uvBottomRight.Y, material.MainColor);
						SetupVertex(14, innerTopLeft.X, innerBottomRight.Y, zOffset, uvTopLeft.X, uvBottomRight.Y, material.MainColor);
						CopyVertex(15, 2);

						CopyVertex(16, 2);
						CopyVertex(17, 14);
						SetupVertex(18, innerBottomRight.X, innerBottomRight.Y, zOffset, uvBottomRight.X, uvBottomRight.Y, material.MainColor);
						CopyVertex(19, 6);

						CopyVertex(20, 6);
						CopyVertex(21, 18);
						SetupVertex(22, bottomRight.X, innerBottomRight.Y, zOffset, tx.UVRatio.X, uvBottomRight.Y, material.MainColor);
						CopyVertex(23, 10);

						CopyVertex(24, 13);
						SetupVertex(25, topLeft.X, bottomRight.Y, zOffset, 0, tx.UVRatio.Y, material.MainColor);
						SetupVertex(26, innerTopLeft.X, bottomRight.Y, zOffset, uvTopLeft.X, tx.UVRatio.Y, material.MainColor);
						CopyVertex(27, 14);

						CopyVertex(28, 14);
						CopyVertex(29, 26);
						SetupVertex(30, innerBottomRight.X, bottomRight.Y, zOffset, uvBottomRight.X, tx.UVRatio.Y, material.MainColor);
						CopyVertex(31, 18);

						CopyVertex(32, 18);
						CopyVertex(33, 30);
						SetupVertex(34, bottomRight.X, bottomRight.Y, zOffset, tx.UVRatio.X, tx.UVRatio.Y, material.MainColor);
						CopyVertex(35, 22);

						canvas.State.Reset();
						canvas.State.SetMaterial(material);
						canvas.DrawVertices<VertexC1P3T2>(_vertices, VertexMode.Quads);
					}
				}
				else
				{
					SetupVertex(0, topLeft.X, topLeft.Y, zOffset, 0, 0, ColorRgba.Red);
					SetupVertex(1, topLeft.X, bottomRight.Y, zOffset, 0, 0, ColorRgba.Red);
					SetupVertex(2, bottomRight.X, bottomRight.Y, zOffset, 0, 0, ColorRgba.Red);
					SetupVertex(3, bottomRight.X, topLeft.Y, zOffset, 0, 0, ColorRgba.Red);

					canvas.State.Reset();
					canvas.State.SetMaterial(ContentProvider.RequestContent<Material>(@"Default:Material.White"));
					canvas.DrawVertices<VertexC1P3T2>(_vertices.Take(4).ToArray(), VertexMode.Quads);
				}
			}
		}

		protected void SetupVertex(int index, float x, float y, float z, float uvX, float uvY, ColorRgba color)
		{
			_vertices[index].Pos.X = x;
			_vertices[index].Pos.Y = y;
			_vertices[index].Pos.Z = z;
			_vertices[index].TexCoord.X = uvX;
			_vertices[index].TexCoord.Y = uvY;
			_vertices[index].Color = color;
		}

		protected void CopyVertex(int destinationIndex, int sourceIndex)
		{
			_vertices[destinationIndex].Pos = _vertices[sourceIndex].Pos;
			_vertices[destinationIndex].TexCoord = _vertices[sourceIndex].TexCoord;
			_vertices[destinationIndex].Color = _vertices[sourceIndex].Color;
		}
	}
}
