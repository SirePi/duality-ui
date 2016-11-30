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
	public sealed class HorizontalScrollBar : ScrollBar
	{
		public HorizontalScrollBar(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		public override ControlsContainer BuildControl()
		{
			ControlsContainer scrollBar = base.BuildControl();

			_btnDecrease.Docking = DockPanel.Dock.Left;
			_btnIncrease.Docking = DockPanel.Dock.Right;

			return scrollBar;
		}

		protected override float ApplyMouseMovement(Vector2 mouseDelta)
		{
			float delta = (_canvas.ActualSize.X - _btnCursor.ActualSize.X) / _valueDelta;
			return mouseDelta.X / delta;
		}

		protected override void UpdateCursor()
		{
			float delta = (_canvas.ActualSize.X - _btnCursor.ActualSize.X) / _valueDelta;
			_btnCursor.Position.X = (delta * this.Value);
			_btnCursor.Position.Y = (_canvas.ActualSize.Y - _btnCursor.ActualSize.Y) / 2;
		}
	}
}