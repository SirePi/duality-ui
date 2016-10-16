using Duality;
using Duality.Drawing;
using SnowyPeak.DualityUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls
{
	public abstract class ControlsContainer : Control, ILayout
	{
		public Border Margin { get; set; }
		protected List<Control> Children { get; private set; }

        protected Skin _skin;

		public Rect ChildrenArea
		{
			get
			{
				return new Rect(
					this.ActualPosition.X + this.Margin.Left,
					this.ActualPosition.Y + this.Margin.Top,
					this.ActualSize.X - this.Margin.Left - this.Margin.Right,
					this.ActualSize.Y - this.Margin.Top - this.Margin.Bottom);
			}
		}

		protected ControlsContainer()
		{
			this.Margin = Border.Zero;
			this.Children = new List<Control>();
		}

        public ControlsContainer Add(Control child)
		{
			if (this.Children.Contains(child))
			{ throw new Exception(String.Format("Duplicate control {0} in parent {1}", child, this)); }
			else
			{
				// check that I am not introducing a circular ancestry
                ControlsContainer cc = this.Parent;
				while (cc != null)
				{
					if (cc == child)
					{ throw new Exception(String.Format("Circular ancestry between {0} and {1}", child, this)); }

					cc = cc.Parent;
				}

				if (child.Parent != null)
				{
					Log.Editor.WriteWarning("Control {0} moved from parent {1} to {2}. Might be an error.", child, child.Parent, this);
					child.Parent.Remove(child);
				}

				child.Parent = this;
                child.ApplySkin(_skin);

                this.Children.Add(child);

				return this;
			}
		}

		public ControlsContainer Remove(Control child)
		{
			this.Children.Remove(child);
			return this;
		}

		public void Clear()
		{
			this.Children.Clear();
		}

		public void LayoutControls()
		{
			foreach (Control c in this.Children)
			{
				c.ActualSize = c.Visibility == ControlVisibility.Collapsed ? Size.Zero : c.Size;
			}

			_LayoutControls();

			foreach (Control c in this.Children)
			{
				c.ActualPosition += this.ActualPosition;
			}

			foreach (ILayout c in this.Children.Where(c => c is ILayout))
			{
				c.LayoutControls();
			}
		}

        public override void ApplySkin(Skin skin)
        {
            if (skin == null) return;

            base.ApplySkin(skin);
            foreach(Control c in this.Children)
            {
                c.ApplySkin(skin);
            }

            _skin = skin;
        }

		public Control FindHoveredControl(Vector2 position)
		{
			Control result = this.Children.FirstOrDefault(c =>
				(c.Status & Control.ControlStatus.Disabled) == Control.ControlStatus.None &&
				c.Visibility == Control.ControlVisibility.Visible &&
				c.ControlArea.Contains(position));

			while (result is ILayout)
			{ result = (result as ILayout).FindHoveredControl(position); }

			return result;
		}

		public override void Draw(Canvas canvas, float zOffset)
		{
			base.Draw(canvas, zOffset);

			foreach (Control c in this.Children.Where(c => c.Visibility == Control.ControlVisibility.Visible))
			{
				c.Draw(canvas, zOffset + Control.LAYOUT_ZOFFSET);
			}
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			foreach (Control c in this.Children)
			{
				c.OnUpdate(msFrame);
			}
		}

		internal abstract void _LayoutControls();
	}
}
