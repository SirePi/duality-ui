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
		private DrawDevice dDevice;
		private Texture tx;
		private RenderTarget rt;
		private BatchInfo bi;

		public Renderer(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ 
			this.tx = new Texture(100, 100, sizeMode: TextureSizeMode.NonPowerOfTwo);
			this.bi = new BatchInfo(DrawTechnique.Alpha, this.tx);
			this.rt = new RenderTarget(AAQuality.Off, true, this.tx);

			this.dDevice = new DrawDevice
			{
				Projection = ProjectionMode.Screen,
				VisibilityMask = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay,
				Target = this.rt,
				TargetSize = this.rt.Size,
				ViewportRect = new Rect(this.rt.Size)
			};
		}

		protected override void _Draw(Canvas canvas, float zOffset)
		{
			base._Draw(canvas, zOffset);

			canvas.State.ColorTint = ColorRgba.Green;
			canvas.DrawRect(this.ActualPosition.X, this.ActualPosition.Y, 50, 50);

			this.dDevice.PrepareForDrawcalls();
			this.mycanvas.Begin(this.dDevice);
			this.mycanvas.State.ColorTint = ColorRgba.Red;
			this.mycanvas.FillRect(0, 0, 200, 50);
			this.mycanvas.State.ColorTint = ColorRgba.Blue;
			this.mycanvas.FillRect(0, 50, 200, 50);
			this.mycanvas.End();
			this.dDevice.Render();

			this.tx.ReloadData();

			canvas.State.Reset();
			canvas.State.SetMaterial(this.bi);
			canvas.State.ColorTint = ColorRgba.White;
			canvas.FillRect(this.ActualPosition.X, this.ActualPosition.Y, this.tx.Size.X, this.tx.Size.Y);
		}
	}
}
