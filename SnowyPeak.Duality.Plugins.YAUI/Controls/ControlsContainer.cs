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
	public abstract class ControlsContainer : Control, ILayout
	{
        private List<Control> _children;

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

		public Border Margin { get; set; }

		public bool IsPassthrough { get; set; }

		public IEnumerable<Control> Children { get { return _children; } }

		protected ControlsContainer(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			this.Margin = Border.Zero;
			this.IsPassthrough = true;
            _children = new List<Control>();

            ApplySkin(_baseSkin);
		}

		public ControlsContainer Add(Control child)
		{
			if (_children.Contains(child))
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
				_children.Add(child);

				return this;
			}
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);

			if (_children != null)
			{
				foreach (Control c in _children)
				{ c.ApplySkin(_baseSkin); }
			}
		}

		public void Clear()
		{
            _children.Clear();
		}

		public override void Draw(Canvas canvas, float zOffset)
		{
			base.Draw(canvas, zOffset);

			foreach (Control c in _children.Where(c => c.Visibility == Control.ControlVisibility.Visible))
			{ c.Draw(canvas, zOffset + Control.LAYOUT_ZOFFSET); }
		}

		public Control FindHoveredControl(Vector2 position)
		{
            if (this.Visibility != Control.ControlVisibility.Visible)
                return null;

			Control result = _children.FirstOrDefault(c =>
                (c is ILayout || c is InteractiveControl) &&
				(c.Status & Control.ControlStatus.Disabled) == Control.ControlStatus.None &&
				c.Visibility == Control.ControlVisibility.Visible &&
				c.ControlArea.Contains(position));

			if (result is ILayout)
			{ result = (result as ILayout).FindHoveredControl(position); }

			if(result == null && !this.IsPassthrough)
			{ result = this; }

			return result;
		}

		public void LayoutControls()
		{
			foreach (Control c in _children)
			{ c.ActualSize = c.Visibility == ControlVisibility.Collapsed ? Size.Zero : c.Size; }

			_LayoutControls();

			foreach (Control c in _children)
			{ c.ActualPosition += this.ActualPosition; }

			foreach (ILayout c in _children.Where(c => c is ILayout))
			{ c.LayoutControls(); }
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			foreach (Control c in _children)
			{ c.OnUpdate(msFrame); }
		}

		public ControlsContainer Remove(Control child)
		{
			_children.Remove(child);
			return this;
		}

		internal abstract void _LayoutControls();
	}
}