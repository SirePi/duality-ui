// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public abstract class Control
	{
		[Flags]
		public enum ControlStatus
		{
			None = 0x00,
			Disabled = 0x0001,
			Normal = 0x0010,
			Active = 0x0100,
			Hover = 0x1000
		}

		public enum ControlVisibility
		{
			Visible,
			Hidden,
			Collapsed
		}

		public Vector2 ActualPosition;
		public Size ActualSize;
		public Vector2 Position;
		public Size Size;

		protected static readonly float INNER_ZOFFSET = -0.00001f;
		protected static readonly float LAYOUT_ZOFFSET = -0.0001f;
		protected Skin _baseSkin;
		protected VertexC1P3T2[] _vertices;

		public ContentRef<Appearance> Appearance { get; set; }
		public CellInfo Cell { get; set; }
		public Rect ControlArea
		{
			get { return new Rect(this.ActualPosition.X, this.ActualPosition.Y, this.ActualSize.X, this.ActualSize.Y); }
		}

		public Dock Docking { get; set; }
		public FocusChangeDelegate FocusChangeHandler { get; set; }
		public string Name { get; set; }
		public ControlsContainer Parent { get; set; }
		public ControlStatus Status { get; set; }
		public bool StretchToFill { get; set; }
		public object Tag { get; set; }
		public UpdateDelegate UpdateHandler { get; set; }
		public ControlVisibility Visibility { get; set; }
		internal string TemplateName { get; private set; }

		public delegate void FocusChangeDelegate(Control control, bool isFocused);

		public delegate void UpdateDelegate(Control control, float msFrame);

		protected Control(Skin skin, string templateName)
		{
			this.StretchToFill = true;
			this.Visibility = ControlVisibility.Visible;
			this.Status = ControlStatus.Normal;

			this.TemplateName = templateName == null ? this.GetType().Name : templateName;
			_baseSkin = (skin == null ? Skin.DEFAULT : skin);
		}

		public virtual void ApplySkin(Skin skin)
		{
			if (skin == null) return;

			_baseSkin = skin;
			ControlTemplate template = _baseSkin.GetTemplate<ControlTemplate>(this);

			this.Appearance = template.Appearance;
			this.Size.AtLeast(template.MinSize);
		}

		public virtual void Draw(Canvas canvas, float zOffset)
		{
			if (this.Appearance.IsAvailable)
			{
				Appearance appearance = this.Appearance.Res;
				Material material = appearance[this.Status];

				Vector2 topLeft = this.ActualPosition;
				Vector2 bottomRight = this.ActualPosition + this.ActualSize;

                _vertices = canvas.RequestVertexArray(36);

				if (material != null && material.MainTexture.IsAvailable)
				{
					Vector2 innerTopLeft = this.ActualPosition + appearance.Border.TopLeft;
					Vector2 innerBottomRight = this.ActualPosition + this.ActualSize - appearance.Border.BottomRight;

					Texture tx = material.MainTexture.Res;
					if (tx != null)
					{
						Vector2 uvSize = tx.UVRatio / tx.Size;
						Vector2 uvTopLeft = uvSize * appearance.Border.TopLeft;
						Vector2 uvBottomRight = tx.UVRatio - (uvSize * appearance.Border.BottomRight);

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
						canvas.DrawVertices<VertexC1P3T2>(_vertices, VertexMode.Quads, 36);
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
					canvas.DrawVertices<VertexC1P3T2>(_vertices, VertexMode.Quads, 4);
				}
			}
		}

		public virtual void OnBlur()
		{
			if (this.FocusChangeHandler != null)
			{ this.FocusChangeHandler(this, false); }
		}

		public virtual void OnFocus()
		{
			if (this.FocusChangeHandler != null)
			{ this.FocusChangeHandler(this, true); }
		}

		public virtual void OnKeyboardKeyEvent(KeyboardKeyEventArgs args)
		{
		}

		public virtual void OnMouseButtonEvent(MouseButtonEventArgs args)
		{
		}

		public virtual void OnMouseEnterEvent()
		{
			this.Status |= Control.ControlStatus.Hover;
		}

		public virtual void OnMouseLeaveEvent()
		{
			this.Status &= ~Control.ControlStatus.Hover;
		}

		public virtual void OnUpdate(float msFrame)
		{
			if (this.UpdateHandler != null)
			{ this.UpdateHandler(this, msFrame); }
		}

		public override string ToString()
		{
			return String.IsNullOrWhiteSpace(this.Name) ? this.GetType().ToString() : this.Name;
		}

		protected Vector2 AlignElement(Vector2 elementSize, Border margin, Alignment alignment)
		{
			Vector2 topLeft = Vector2.Zero;

			switch (alignment)
			{
				case Alignment.TopLeft:
					topLeft.X = this.ActualPosition.X + margin.Left;
					topLeft.Y = this.ActualPosition.Y + margin.Top;
					break;

				case Alignment.Top:
					topLeft.X = this.ActualPosition.X + (this.ActualSize.X - elementSize.X) / 2;
					topLeft.Y = this.ActualPosition.Y + margin.Top;
					break;

				case Alignment.TopRight:
					topLeft.X = this.ActualPosition.X + this.ActualSize.X - margin.Right - elementSize.X;
					topLeft.Y = this.ActualPosition.Y + margin.Top;
					break;

				case Alignment.Left:
					topLeft.X = this.ActualPosition.X + margin.Left;
					topLeft.Y = this.ActualPosition.Y + (this.ActualSize.Y - elementSize.Y) / 2;
					break;

				case Alignment.Center:
					topLeft.X = this.ActualPosition.X + (this.ActualSize.X - elementSize.X) / 2;
					topLeft.Y = this.ActualPosition.Y + (this.ActualSize.Y - elementSize.Y) / 2;
					break;

				case Alignment.Right:
					topLeft.X = this.ActualPosition.X + this.ActualSize.X - margin.Right - elementSize.X;
					topLeft.Y = this.ActualPosition.Y + (this.ActualSize.Y - elementSize.Y) / 2;
					break;

				case Alignment.BottomLeft:
					topLeft.X = this.ActualPosition.X + margin.Left;
					topLeft.Y = this.ActualPosition.Y + this.ActualSize.Y - margin.Bottom - elementSize.Y;
					break;

				case Alignment.Bottom:
					topLeft.X = this.ActualPosition.X + (this.ActualSize.X - elementSize.X) / 2;
					topLeft.Y = this.ActualPosition.Y + this.ActualSize.Y - margin.Bottom - elementSize.Y;
					break;

				case Alignment.BottomRight:
					topLeft.X = this.ActualPosition.X + this.ActualSize.X - margin.Right - elementSize.X;
					topLeft.Y = this.ActualPosition.Y + this.ActualSize.Y - margin.Bottom - elementSize.Y;
					break;
			}

			return topLeft;
		}

		protected void CopyVertex(int destinationIndex, int sourceIndex)
		{
			_vertices[destinationIndex].Pos = _vertices[sourceIndex].Pos;
			_vertices[destinationIndex].TexCoord = _vertices[sourceIndex].TexCoord;
			_vertices[destinationIndex].Color = _vertices[sourceIndex].Color;
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
	}
}