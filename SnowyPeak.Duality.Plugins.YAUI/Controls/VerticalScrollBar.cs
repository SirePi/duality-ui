// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

			_btnDecrease.Docking = Dock.Top;
			_btnIncrease.Docking = Dock.Bottom;

			return scrollBar;
		}

		protected override float ApplyMouseMovement(Vector2 mouseDelta)
		{
			float delta = (_canvas.ActualSize.Y - _btnCursor.ActualSize.Y) / _valueDelta;
			return mouseDelta.Y / delta;
		}

		protected override void UpdateCursor()
		{
			float delta = (_canvas.ActualSize.Y - _btnCursor.ActualSize.Y) / _valueDelta;
			_btnCursor.Position.Y = (delta * (this.Value - this.MinValue));
			_btnCursor.Position.X = (_canvas.ActualSize.X - _btnCursor.ActualSize.X) / 2;
		}
	}
}