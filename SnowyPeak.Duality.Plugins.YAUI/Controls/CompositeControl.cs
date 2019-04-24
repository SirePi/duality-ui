// This code is provided under the MIT license. Originally by Alessandro Pilati.
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
	public abstract class CompositeControl<T> : Control<T>, ILayout where T : ControlTemplate, new()
	{
		protected ControlsContainer container;
		public bool IsPassthrough => false;

		protected CompositeControl(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		protected override void Init()
		{
			base.Init();
			this.container = this.BuildControl();
		}

		public abstract ControlsContainer BuildControl();

		public override void Draw(Canvas canvas, float zOffset)
		{
			if (this.Visibility == ControlVisibility.Visible)
			{
				base.Draw(canvas, zOffset);
				this.container?.Draw(canvas, zOffset + Control.LAYOUT_ZOFFSET);
			}
		}

		public Control FindHoveredControl(Vector2 position)
		{
			return this.container.FindHoveredControl(position);
		}

		public void LayoutControls()
		{
			if (this.container != null)
			{
				this.container.ActualSize.X = this.ActualSize.X - this.Margin.Left - this.Margin.Right;
				this.container.ActualSize.Y = this.ActualSize.Y - this.Margin.Top - this.Margin.Bottom;

				this.container.ActualPosition.X = this.ActualPosition.X + this.Margin.Left;
				this.container.ActualPosition.Y = this.ActualPosition.Y + this.Margin.Top;

				this.container.LayoutControls();
			}
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);
			this.container.OnUpdate(msFrame);
		}
	}
}
