// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public sealed class HorizontalScrollBar : ScrollBar
	{
		public HorizontalScrollBar(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		public override ControlsContainer BuildControl()
		{
			ControlsContainer scrollBar = base.BuildControl();

			this.btnDecrease.Docking = Dock.Left;
			this.btnIncrease.Docking = Dock.Right;

			return scrollBar;
		}

		protected override float ApplyMouseMovement(Vector2 mouseDelta)
		{
			float delta = (this.canvas.ActualSize.X - this.btnCursor.ActualSize.X) / this.valueDelta;
			return mouseDelta.X / delta;
		}

		protected override void UpdateCursor()
		{
			float delta = (this.canvas.ActualSize.X - this.btnCursor.ActualSize.X) / this.valueDelta;
			this.btnCursor.Position.X = (delta * (this.Value - this.MinValue));
			this.btnCursor.Position.Y = (this.canvas.ActualSize.Y - this.btnCursor.ActualSize.Y) / 2;
		}
	}
}
