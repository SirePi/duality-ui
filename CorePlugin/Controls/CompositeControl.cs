using Duality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls
{
	public abstract class CompositeControl : Control
	{
		protected ControlsContainer _container;

		public CompositeControl()
		{
			_container = BuildControl();
		}

		public abstract ControlsContainer BuildControl();

		public override void Draw(Duality.Drawing.Canvas canvas, float zOffset)
		{
			if (_container != null) 
			{ _container.Draw(canvas, zOffset + Control.LAYOUT_ZOFFSET); }
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			_container.OnUpdate(msFrame);
		}

		internal virtual void LayoutControls()
		{
			if(_container != null)
			{
				_container.ActualSize = this.ActualSize;
				_container.ActualPosition = this.ActualPosition;

				_container.LayoutControls();
			}
		}

        internal Control FindHoveredControl(Vector2 position)
        {
            return _container.FindHoveredControl(position);
        }
	}
}
