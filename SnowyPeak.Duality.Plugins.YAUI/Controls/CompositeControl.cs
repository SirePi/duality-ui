using Duality;
using Duality.Drawing;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public abstract class CompositeControl : Control, ILayout 
	{
		public Border Margin { get; set; }

		protected ControlsContainer _container;

		public CompositeControl(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			_container = BuildControl();
		}

		public abstract ControlsContainer BuildControl();

        public override void Draw(Canvas canvas, float zOffset)
		{
			base.Draw(canvas, zOffset);

			if (_container != null) 
			{ _container.Draw(canvas, zOffset + Control.LAYOUT_ZOFFSET); }
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			_container.OnUpdate(msFrame);
		}

		public void LayoutControls()
		{
			if(_container != null)
			{
				_container.ActualSize.X = this.ActualSize.X - this.Margin.Left - this.Margin.Right;
				_container.ActualSize.Y = this.ActualSize.Y - this.Margin.Top - this.Margin.Bottom;

				_container.ActualPosition.X = this.ActualPosition.X + this.Margin.Left;
				_container.ActualPosition.Y = this.ActualPosition.Y + this.Margin.Top;

				_container.LayoutControls();
			}
		}

        public Control FindHoveredControl(Vector2 position)
        {
            return _container.FindHoveredControl(position);
        }
	}
}
