// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public sealed class VerticalScrollBar : ScrollBar
	{
		public VerticalScrollBar(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		public override ControlsContainer BuildControl()
		{
			ControlsContainer scrollBar = base.BuildControl();

			this.btnDecrease.Docking = Dock.Top;
			this.btnIncrease.Docking = Dock.Bottom;

			return scrollBar;
		}

		protected override float ApplyMouseMovement(Vector2 mouseDelta)
		{
			float delta = (this.canvas.ActualSize.Y - this.btnCursor.ActualSize.Y) / this.valueDelta;
			return mouseDelta.Y / delta;
		}

		protected override void UpdateCursor()
		{
			float delta = (this.canvas.ActualSize.Y - this.btnCursor.ActualSize.Y) / this.valueDelta;
			this.btnCursor.Position.Y = (delta * (this.Value - this.MinValue));
			this.btnCursor.Position.X = (this.canvas.ActualSize.X - this.btnCursor.ActualSize.X) / 2;
		}
	}
}