// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Resources;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public abstract class Renderer : Control
	{
		public ContentRef<DrawTechnique> MainDrawTechnique { get; set; }
		public AAQuality AAQuality { get; set; }

		private readonly Canvas innerCanvas = new Canvas();
		private DrawDevice drawDevice;
		private Texture texture;
		private BatchInfo batch;

		protected Renderer(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		protected override void Init()
		{
			base.Init();
			this.MainDrawTechnique = DrawTechnique.Mask;
			this.AAQuality = AAQuality.Off;
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			if(this.texture == null || this.Size != this.texture.Size)
			{
				this.texture = new Texture((int)this.Size.X, (int)this.Size.Y, sizeMode: TextureSizeMode.NonPowerOfTwo);
				this.batch = new BatchInfo(this.MainDrawTechnique, this.texture);
				RenderTarget rendertarget = new RenderTarget(this.AAQuality, true, this.texture);

				this.drawDevice = new DrawDevice
				{
					Projection = ProjectionMode.Screen,
					VisibilityMask = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay,
					Target = rendertarget,
					TargetSize = rendertarget.Size,
					ViewportRect = new Rect(rendertarget.Size)
				};
			}
		}

		protected override void _Draw(Canvas canvas, float zOffset)
		{
			base._Draw(canvas, zOffset);

			canvas.State.ColorTint = ColorRgba.Green;
			canvas.DrawRect(this.ActualPosition.X, this.ActualPosition.Y, 50, 50);

			this.drawDevice.PrepareForDrawcalls();
			this.innerCanvas.Begin(this.drawDevice);
			this.Render(this.innerCanvas);
			this.innerCanvas.End();
			this.drawDevice.Render();

			this.texture.ReloadData();

			canvas.State.Reset();
			canvas.State.SetMaterial(this.batch);
			canvas.State.ColorTint = ColorRgba.White;
			canvas.FillRect(this.ActualPosition.X, this.ActualPosition.Y, this.texture.Size.X, this.texture.Size.Y);
		}

		protected abstract void Render(Canvas canvas);
	}
}
