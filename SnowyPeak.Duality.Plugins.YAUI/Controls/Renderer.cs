// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public sealed class Renderer : Control
	{
		private Canvas mycanvas = new Canvas();
		private DrawDevice dDevice = new DrawDevice();
		private Texture tx;
		private BatchInfo bi;

		public Renderer(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		protected override void Init()
		{
			base.Init();
			this.tx = new Texture(200, 200);
			this.bi = new BatchInfo(DrawTechnique.Alpha, this.tx);

			this.dDevice = new DrawDevice
			{
				Projection = ProjectionMode.Orthographic,
				VisibilityMask = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay,
				Target = new RenderTarget(AAQuality.Off, false, this.tx),
				TargetSize = new Vector2(200, 200),
				ViewportRect = new Rect(200, 200)
			};
		}

		public override void Draw(Canvas canvas, float zOffset)
		{
			base.Draw(canvas, zOffset);

			this.mycanvas.Begin(this.dDevice);
			this.mycanvas.State.ColorTint = ColorRgba.Red;
			this.mycanvas.DrawRect(0, 0, 200, 200);
			this.mycanvas.End();

			this.dDevice.Render();

			canvas.State.Reset();
			canvas.State.SetMaterial(this.bi);
			canvas.State.ColorTint = ColorRgba.White;
			canvas.DrawRect(0, 0, this.tx.Size.X, this.tx.Size.Y);
		}
	}
}
