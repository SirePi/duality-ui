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

		protected const float INNER_ZOFFSET = -0.00001f;
		protected const float LAYOUT_ZOFFSET = -0.0001f;
		protected Skin baseSkin;
		protected RawList<VertexC1P3T2> vertices;

		public ContentRef<Appearance> Appearance { get; set; }
		public CellInfo Cell { get; set; }
		public Rect ControlArea => new Rect(this.ActualPosition.X, this.ActualPosition.Y, this.ActualSize.X, this.ActualSize.Y);
		public Dock Docking { get; set; }
		public Border Margin { get; set; }
		public string Name { get; set; }
		public ControlsContainer Parent { get; set; }
		public ControlStatus Status
		{
			get => this.status;
			set
			{
				if (this.status != value)
				{ this.onStatusChange?.Invoke(this, this.status, value); }

				this.status = value;
			}
		}
		public bool StretchToFill { get; set; }
		public object Tag { get; set; }
		public ShaderParameterCollection ControlVariables { get; private set; }
		public ControlVisibility Visibility
		{
			get { return this.visibility; }
			set
			{
				if (this.visibility != value)
				{ this.onVisibilityChange?.Invoke(this, this.visibility, value); }

				this.visibility = value;
			}
		}

		internal string TemplateName { get; private set; }

		private Dictionary<ControlStatus, BatchInfo> customAppearance;
		private ControlVisibility visibility;
		private ControlStatus status;
		private ContentRef<Texture> mainTex;
		private ColorRgba mainColor;

		// Delegates
		public delegate void UpdateDelegate(Control control, float msFrame);
		public delegate void StatusChangeDelegate(Control control, ControlStatus previousValue, ControlStatus newValue);
		public delegate void VisibilityChangeDelegate(Control control, ControlVisibility previousValue, ControlVisibility newValue);

		// Events
		[DontSerialize]
		private UpdateDelegate onGameUpdate;
		public event UpdateDelegate OnGameUpdate
		{
			add { this.onGameUpdate += value; }
			remove { this.onGameUpdate -= value; }
		}
		[DontSerialize]
		private StatusChangeDelegate onStatusChange;
		public event StatusChangeDelegate OnStatusChange
		{
			add { this.onStatusChange += value; }
			remove { this.onStatusChange -= value; }
		}
		[DontSerialize]
		private VisibilityChangeDelegate onVisibilityChange;
		public event VisibilityChangeDelegate OnVisibilityChange
		{
			add { this.onVisibilityChange += value; }
			remove { this.onVisibilityChange -= value; }
		}

		protected Control(Skin skin, string templateName)
		{
			this.TemplateName = templateName ?? this.GetType().Name;

			this.Init();
			this.ApplySkin(skin ?? Skin.DEFAULT);
		}

		protected virtual void Init()
		{
			this.vertices = new RawList<VertexC1P3T2>(36);

			this.StretchToFill = true;
			this.Visibility = ControlVisibility.Visible;
			this.Status = ControlStatus.Normal;

			this.ControlVariables = new ShaderParameterCollection();
		}

		public virtual void ApplySkin(Skin skin)
		{
			if (skin == null)
				return;

			this.baseSkin = skin;
		}

		public virtual void Draw(Canvas canvas, float zOffset)
		{
			if (this.Visibility == ControlVisibility.Visible && !this.Appearance.IsExplicitNull)
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
						if (this.customAppearance == null)
							this.customAppearance = new Dictionary<ControlStatus, BatchInfo>();

						if (!this.customAppearance.ContainsKey(this.Status))
							this.customAppearance[this.Status] = new BatchInfo(material);

						this.customAppearance[this.Status].SetVariables(this.ControlVariables);
					}

					canvas.State.Reset();
					if (this.customAppearance != null)
					{
						this.mainTex = this.customAppearance[this.Status].MainTexture;
						this.mainColor = this.customAppearance[this.Status].MainColor;
						canvas.State.SetMaterial(this.customAppearance[this.Status]);
					}
					else
					{
						this.mainTex = material.MainTexture;
						this.mainColor = material.MainColor;
						canvas.State.SetMaterial(material);
					}

					if (this.mainTex.IsAvailable)
					{
						Vector2 innerTopLeft = topLeft + border.TopLeft;
						Vector2 innerBottomRight = bottomRight - border.BottomRight;

						Texture tx = this.mainTex.Res;
						if (tx != null)
						{
							Vector2 uvSize = tx.UVRatio / tx.Size;
							Vector2 uvTopLeft = uvSize * border.TopLeft;
							Vector2 uvBottomRight = tx.UVRatio - (uvSize * border.BottomRight);

							this.SetupVertex(0, topLeft.X, topLeft.Y, zOffset, 0, 0, this.mainColor);
							this.SetupVertex(1, topLeft.X, innerTopLeft.Y, zOffset, 0, uvTopLeft.Y, this.mainColor);
							this.SetupVertex(2, innerTopLeft.X, innerTopLeft.Y, zOffset, uvTopLeft.X, uvTopLeft.Y, this.mainColor);
							this.SetupVertex(3, innerTopLeft.X, topLeft.Y, zOffset, uvTopLeft.X, 0, this.mainColor);

							this.CopyVertex(4, 3);
							this.CopyVertex(5, 2);
							this.SetupVertex(6, innerBottomRight.X, innerTopLeft.Y, zOffset, uvBottomRight.X, uvTopLeft.Y, this.mainColor);
							this.SetupVertex(7, innerBottomRight.X, topLeft.Y, zOffset, uvBottomRight.X, 0, this.mainColor);

							this.CopyVertex(8, 7);
							this.CopyVertex(9, 6);
							this.SetupVertex(10, bottomRight.X, innerTopLeft.Y, zOffset, tx.UVRatio.X, uvTopLeft.Y, this.mainColor);
							this.SetupVertex(11, bottomRight.X, topLeft.Y, zOffset, tx.UVRatio.X, 0, this.mainColor);

							this.CopyVertex(12, 1);
							this.SetupVertex(13, topLeft.X, innerBottomRight.Y, zOffset, 0, uvBottomRight.Y, this.mainColor);
							this.SetupVertex(14, innerTopLeft.X, innerBottomRight.Y, zOffset, uvTopLeft.X, uvBottomRight.Y, this.mainColor);
							this.CopyVertex(15, 2);

							this.CopyVertex(16, 2);
							this.CopyVertex(17, 14);
							this.SetupVertex(18, innerBottomRight.X, innerBottomRight.Y, zOffset, uvBottomRight.X, uvBottomRight.Y, this.mainColor);
							this.CopyVertex(19, 6);

							this.CopyVertex(20, 6);
							this.CopyVertex(21, 18);
							this.SetupVertex(22, bottomRight.X, innerBottomRight.Y, zOffset, tx.UVRatio.X, uvBottomRight.Y, this.mainColor);
							this.CopyVertex(23, 10);

							this.CopyVertex(24, 13);
							this.SetupVertex(25, topLeft.X, bottomRight.Y, zOffset, 0, tx.UVRatio.Y, this.mainColor);
							this.SetupVertex(26, innerTopLeft.X, bottomRight.Y, zOffset, uvTopLeft.X, tx.UVRatio.Y, this.mainColor);
							this.CopyVertex(27, 14);

							this.CopyVertex(28, 14);
							this.CopyVertex(29, 26);
							this.SetupVertex(30, innerBottomRight.X, bottomRight.Y, zOffset, uvBottomRight.X, tx.UVRatio.Y, this.mainColor);
							this.CopyVertex(31, 18);

							this.CopyVertex(32, 18);
							this.CopyVertex(33, 30);
							this.SetupVertex(34, bottomRight.X, bottomRight.Y, zOffset, tx.UVRatio.X, tx.UVRatio.Y, this.mainColor);
							this.CopyVertex(35, 22);

							canvas.DrawVertices<VertexC1P3T2>(this.vertices.Data, VertexMode.Quads, 36);
						}
					}
					else
					{
						this.SetupVertex(0, topLeft.X, topLeft.Y, zOffset, 0, 0, this.mainColor);
						this.SetupVertex(1, topLeft.X, bottomRight.Y, zOffset, 0, 1, this.mainColor);
						this.SetupVertex(2, bottomRight.X, bottomRight.Y, zOffset, 1, 1, this.mainColor);
						this.SetupVertex(3, bottomRight.X, topLeft.Y, zOffset, 1, 0, this.mainColor);

						canvas.DrawVertices<VertexC1P3T2>(this.vertices.Data, VertexMode.Quads, 4);
					}
				}
				else
				{
					this.SetupVertex(0, topLeft.X, topLeft.Y, zOffset, 0, 0, ColorRgba.White);
					this.SetupVertex(1, topLeft.X, bottomRight.Y, zOffset, 0, 1, ColorRgba.Red);
					this.SetupVertex(2, bottomRight.X, bottomRight.Y, zOffset, 1, 1, ColorRgba.White);
					this.SetupVertex(3, bottomRight.X, topLeft.Y, zOffset, 1, 0, ColorRgba.Red);

					canvas.State.Reset();
					canvas.State.SetMaterial(ContentProvider.RequestContent<Material>(@"Default:Material:Checkerboard"));
					canvas.DrawVertices<VertexC1P3T2>(this.vertices.Data, VertexMode.Quads, 4);
				}
			}
		}

		public virtual void OnUpdate(float msFrame)
		{
			this.onGameUpdate?.Invoke(this, msFrame);
		}

		public override string ToString()
		{
			return string.IsNullOrWhiteSpace(this.Name) ? this.GetType().ToString() : this.Name;
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
			this.vertices.Data[destinationIndex].Pos = this.vertices.Data[sourceIndex].Pos;
			this.vertices.Data[destinationIndex].TexCoord = this.vertices.Data[sourceIndex].TexCoord;
			this.vertices.Data[destinationIndex].Color = this.vertices.Data[sourceIndex].Color;
		}

		protected void SetupVertex(int index, float x, float y, float z, float uvX, float uvY, ColorRgba color)
		{
			this.vertices.Data[index].Pos.X = x;
			this.vertices.Data[index].Pos.Y = y;
			this.vertices.Data[index].Pos.Z = z;
			this.vertices.Data[index].TexCoord.X = uvX;
			this.vertices.Data[index].TexCoord.Y = uvY;
			this.vertices.Data[index].Color = color;
		}
	}

	public abstract class Control<T> : Control where T : ControlTemplate, new()
	{
		protected T Template { get; private set; }

		public Control(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);

			this.Template = this.baseSkin.GetTemplate<T>(this);

			this.Appearance = this.Template.Appearance;
			this.Margin = this.Template.Margin;
			this.Size.AtLeast(this.Template.MinSize);
		}
	}
}