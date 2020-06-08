// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using SnowyPeak.Duality.Plugins.YAUI.Templates;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public abstract class CompositeControl<T> : Control<T>, ILayout where T : ControlTemplate, new()
	{
		protected ControlsContainer container;
		public bool IsPassthrough => false;

		protected CompositeControl(Skin skin, string templateName)
			: base(skin, templateName)
		{ }

		protected override void Init()
		{
			base.Init();
			this.container = this.BuildControl();
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);
			this.container.ApplySkin(skin);
		}

		public abstract ControlsContainer BuildControl();

		protected override void _Draw(Canvas canvas, float zOffset)
		{
			base._Draw(canvas, zOffset);
			this.container?.Draw(canvas, zOffset + Control.LAYOUT_ZOFFSET);
		}

		public Control FindHoveredControl(Vector2 position)
		{
			return this.container.FindHoveredControl(position);
		}

		public void LayoutControls()
		{
			if (this.container != null)
			{
				/*
				this.container.ActualSize.X = this.ActualSize.X - this.Margin.Left - this.Margin.Right;
				this.container.ActualSize.Y = this.ActualSize.Y - this.Margin.Top - this.Margin.Bottom;

				this.container.ActualPosition.X = this.ActualPosition.X + this.Margin.Left;
				this.container.ActualPosition.Y = this.ActualPosition.Y + this.Margin.Top;
				*/
				this.container.ActualSize.X = this.ActualSize.X;
				this.container.ActualSize.Y = this.ActualSize.Y;

				this.container.ActualPosition.X = this.ActualPosition.X;
				this.container.ActualPosition.Y = this.ActualPosition.Y;

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
