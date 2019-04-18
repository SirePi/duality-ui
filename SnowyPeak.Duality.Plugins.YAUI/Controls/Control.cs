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
		protected RawList<VertexC1P3T2> _vertices;

		public ContentRef<Appearance> Appearance { get; set; }
		public CellInfo Cell { get; set; }
		public Rect ControlArea
		{
			get { return new Rect(this.ActualPosition.X, this.ActualPosition.Y, this.ActualSize.X, this.ActualSize.Y); }
		}
		public Dock Docking { get; set; }
		public Border Margin { get; set; }
		public string Name { get; set; }
		public ControlsContainer Parent { get; set; }
		public ControlStatus Status
		{
			get { return _status; }
			set
			{
				if (_status != value)
				{ _onStatusChange?.Invoke(this, _status, value); }

				_status = value;
			}
		}
		public bool StretchToFill { get; set; }
		public object Tag { get; set; }
		public ShaderParameterCollection ControlVariables { get; private set; }
		public ControlVisibility Visibility
		{
			get { return _visibility; }
			set
			{
				if (_visibility != value)
				{ _onVisibilityChange?.Invoke(this, _visibility, value); }

				_visibility = value;
			}
		}

		internal string TemplateName { get; private set; }

		private Dictionary<ControlStatus, BatchInfo> _customAppearance;
		private ControlVisibility _visibility;
		private ControlStatus _status;
		private ContentRef<Texture> _mainTex;
		private ColorRgba _mainColor;

		// Delegates
		public delegate void UpdateDelegate(Control control, float msFrame);
		public delegate void StatusChangeDelegate(Control control, ControlStatus previousValue, ControlStatus newValue);
		public delegate void VisibilityChangeDelegate(Control control, ControlVisibility previousValue, ControlVisibility newValue);

		// Events
		[DontSerialize]
		private UpdateDelegate _onGameUpdate;
		public event UpdateDelegate OnGameUpdate
		{
			add { _onGameUpdate += value; }
			remove { _onGameUpdate -= value; }
		}
		[DontSerialize]
		private StatusChangeDelegate _onStatusChange;
		public event StatusChangeDelegate OnStatusChange
		{
			add { _onStatusChange += value; }
			remove { _onStatusChange -= value; }
		}
		[DontSerialize]
		private VisibilityChangeDelegate _onVisibilityChange;
		public event VisibilityChangeDelegate OnVisibilityChange
		{
			add { _onVisibilityChange += value; }
			remove { _onVisibilityChange -= value; }
		}

		protected Control(Skin skin, string templateName)
		{
			_vertices = new RawList<VertexC1P3T2>(36);

			this.StretchToFill = true;
			this.Visibility = ControlVisibility.Visible;
			this.Status = ControlStatus.Normal;

			this.ControlVariables = new ShaderParameterCollection();

			this.TemplateName = templateName ?? this.GetType().Name;
			_baseSkin = skin ?? Skin.DEFAULT;
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
			if (!this.Appearance.IsExplicitNull)
			{
				Border border = (this.Appearance.Res?.Border).GetValueOrDefault();
				Material material = this.Appearance.Res?[this.Status];

				Vector2 topLeft = this.ActualPosition;
				Vector2 bottomRight = this.ActualPosition + this.ActualSize;

				if (material != null)
				{
					// if some shader variables are set, create a new BatchInfo to detach from the common 
					if (this.ControlVariables.Count > 0)
					{
						if (_customAppearance == null)
							_customAppearance = new Dictionary<ControlStatus, BatchInfo>();

						if (!_customAppearance.ContainsKey(this.Status))
							_customAppearance[this.Status] = new BatchInfo(material);

						_customAppearance[this.Status].SetVariables(this.ControlVariables);
					}

					canvas.State.Reset();
					if (_customAppearance != null)
					{
						_mainTex = _customAppearance[this.Status].MainTexture;
						_mainColor = _customAppearance[this.Status].MainColor;
						canvas.State.SetMaterial(_customAppearance[this.Status]);
					}
					else
					{
						_mainTex = material.MainTexture;
						_mainColor = material.MainColor;
						canvas.State.SetMaterial(material);
					}

					if (_mainTex.IsAvailable)
					{
						Vector2 innerTopLeft = topLeft + border.TopLeft;
						Vector2 innerBottomRight = bottomRight - border.BottomRight;

						Texture tx = _mainTex.Res;
						if (tx != null)
						{
							Vector2 uvSize = tx.UVRatio / tx.Size;
							Vector2 uvTopLeft = uvSize * border.TopLeft;
							Vector2 uvBottomRight = tx.UVRatio - (uvSize * border.BottomRight);

							SetupVertex(0, topLeft.X, topLeft.Y, zOffset, 0, 0, _mainColor);
							SetupVertex(1, topLeft.X, innerTopLeft.Y, zOffset, 0, uvTopLeft.Y, _mainColor);
							SetupVertex(2, innerTopLeft.X, innerTopLeft.Y, zOffset, uvTopLeft.X, uvTopLeft.Y, _mainColor);
							SetupVertex(3, innerTopLeft.X, topLeft.Y, zOffset, uvTopLeft.X, 0, _mainColor);

							CopyVertex(4, 3);
							CopyVertex(5, 2);
							SetupVertex(6, innerBottomRight.X, innerTopLeft.Y, zOffset, uvBottomRight.X, uvTopLeft.Y, _mainColor);
							SetupVertex(7, innerBottomRight.X, topLeft.Y, zOffset, uvBottomRight.X, 0, _mainColor);

							CopyVertex(8, 7);
							CopyVertex(9, 6);
							SetupVertex(10, bottomRight.X, innerTopLeft.Y, zOffset, tx.UVRatio.X, uvTopLeft.Y, _mainColor);
							SetupVertex(11, bottomRight.X, topLeft.Y, zOffset, tx.UVRatio.X, 0, _mainColor);

							CopyVertex(12, 1);
							SetupVertex(13, topLeft.X, innerBottomRight.Y, zOffset, 0, uvBottomRight.Y, _mainColor);
							SetupVertex(14, innerTopLeft.X, innerBottomRight.Y, zOffset, uvTopLeft.X, uvBottomRight.Y, _mainColor);
							CopyVertex(15, 2);

							CopyVertex(16, 2);
							CopyVertex(17, 14);
							SetupVertex(18, innerBottomRight.X, innerBottomRight.Y, zOffset, uvBottomRight.X, uvBottomRight.Y, _mainColor);
							CopyVertex(19, 6);

							CopyVertex(20, 6);
							CopyVertex(21, 18);
							SetupVertex(22, bottomRight.X, innerBottomRight.Y, zOffset, tx.UVRatio.X, uvBottomRight.Y, _mainColor);
							CopyVertex(23, 10);

							CopyVertex(24, 13);
							SetupVertex(25, topLeft.X, bottomRight.Y, zOffset, 0, tx.UVRatio.Y, material.MainColor);
							SetupVertex(26, innerTopLeft.X, bottomRight.Y, zOffset, uvTopLeft.X, tx.UVRatio.Y, _mainColor);
							CopyVertex(27, 14);

							CopyVertex(28, 14);
							CopyVertex(29, 26);
							SetupVertex(30, innerBottomRight.X, bottomRight.Y, zOffset, uvBottomRight.X, tx.UVRatio.Y, _mainColor);
							CopyVertex(31, 18);

							CopyVertex(32, 18);
							CopyVertex(33, 30);
							SetupVertex(34, bottomRight.X, bottomRight.Y, zOffset, tx.UVRatio.X, tx.UVRatio.Y, _mainColor);
							CopyVertex(35, 22);

							canvas.DrawVertices<VertexC1P3T2>(_vertices.Data, VertexMode.Quads, 36);
						}
					}
					else
					{
						SetupVertex(0, topLeft.X, topLeft.Y, zOffset, 0, 0, _mainColor);
						SetupVertex(1, topLeft.X, bottomRight.Y, zOffset, 0, 1, _mainColor);
						SetupVertex(2, bottomRight.X, bottomRight.Y, zOffset, 1, 1, _mainColor);
						SetupVertex(3, bottomRight.X, topLeft.Y, zOffset, 1, 0, _mainColor);

						canvas.DrawVertices<VertexC1P3T2>(_vertices.Data, VertexMode.Quads, 4);
					}
				}
				else
				{
					SetupVertex(0, topLeft.X, topLeft.Y, zOffset, 0, 0, ColorRgba.White);
					SetupVertex(1, topLeft.X, bottomRight.Y, zOffset, 0, 1, ColorRgba.Red);
					SetupVertex(2, bottomRight.X, bottomRight.Y, zOffset, 1, 1, ColorRgba.White);
					SetupVertex(3, bottomRight.X, topLeft.Y, zOffset, 1, 0, ColorRgba.Red);

					canvas.State.Reset();
					canvas.State.SetMaterial(ContentProvider.RequestContent<Material>(@"Default:Material:Checkerboard"));
					canvas.DrawVertices<VertexC1P3T2>(_vertices.Data, VertexMode.Quads, 4);
				}
			}
		}

		public virtual void OnKeyboardKeyEvent(KeyboardKeyEventArgs args)
		{ }

		public virtual void OnMouseButtonEvent(MouseButtonEventArgs args)
		{ }

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
			_onGameUpdate?.Invoke(this, msFrame);
		}

		public override string ToString()
		{
			return String.IsNullOrWhiteSpace(this.Name) ? this.GetType().ToString() : this.Name;
		}

		protected Vector2 AlignElement(Vector2 elementSize, Border margin, Alignment alignment)
		{
			Vector2 topLeft = this.ActualPosition;

			switch (alignment)
			{
				case Alignment.TopLeft:
					topLeft.X += margin.Left;
					topLeft.Y += margin.Top;
					break;

				case Alignment.Top:
					topLeft.X += (this.ActualSize.X - elementSize.X) / 2;
					topLeft.Y += margin.Top;
					break;

				case Alignment.TopRight:
					topLeft.X += this.ActualSize.X - margin.Right - elementSize.X;
					topLeft.Y += margin.Top;
					break;

				case Alignment.Left:
					topLeft.X += margin.Left;
					topLeft.Y += (this.ActualSize.Y - elementSize.Y) / 2;
					break;

				case Alignment.Center:
					topLeft.X += (this.ActualSize.X - elementSize.X) / 2;
					topLeft.Y += (this.ActualSize.Y - elementSize.Y) / 2;
					break;

				case Alignment.Right:
					topLeft.X += this.ActualSize.X - margin.Right - elementSize.X;
					topLeft.Y += (this.ActualSize.Y - elementSize.Y) / 2;
					break;

				case Alignment.BottomLeft:
					topLeft.X += margin.Left;
					topLeft.Y += this.ActualSize.Y - margin.Bottom - elementSize.Y;
					break;

				case Alignment.Bottom:
					topLeft.X += (this.ActualSize.X - elementSize.X) / 2;
					topLeft.Y += this.ActualSize.Y - margin.Bottom - elementSize.Y;
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
			_vertices.Data[destinationIndex].Pos = _vertices.Data[sourceIndex].Pos;
			_vertices.Data[destinationIndex].TexCoord = _vertices.Data[sourceIndex].TexCoord;
			_vertices.Data[destinationIndex].Color = _vertices.Data[sourceIndex].Color;
		}

		protected void SetupVertex(int index, float x, float y, float z, float uvX, float uvY, ColorRgba color)
		{
			_vertices.Data[index].Pos.X = x;
			_vertices.Data[index].Pos.Y = y;
			_vertices.Data[index].Pos.Z = z;
			_vertices.Data[index].TexCoord.X = uvX;
			_vertices.Data[index].TexCoord.Y = uvY;
			_vertices.Data[index].Color = color;
		}
	}
}