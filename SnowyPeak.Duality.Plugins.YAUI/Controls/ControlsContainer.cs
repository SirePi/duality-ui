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
		public bool IsPassthrough { get; set; }

		protected List<Control> Children { get; private set; }

		protected ControlsContainer(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			this.Children = new List<Control>();
			this.IsPassthrough = true;
			this.ApplySkin(this.baseSkin);
		}

		public ControlsContainer Add(Control child)
		{
			if (this.Children.Contains(child))
			{ throw new Exception(string.Format("Duplicate control {0} in parent {1}", child, this)); }
			else
			{
				// check that I am not introducing a circular ancestry
				ControlsContainer cc = this.Parent;
				while (cc != null)
				{
					if (cc == child)
					{ throw new Exception(string.Format("Circular ancestry between {0} and {1}", child, this)); }

					cc = cc.Parent;
				}

				if (child.Parent != null)
				{
					Logs.Get<UILog>().WriteWarning("Control {0} moved from parent {1} to {2}. Might be an error.", child, child.Parent, this);
					child.Parent.Remove(child);
				}

				child.Parent = this;
				this.Children.Add(child);

				return this;
			}
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);

			if (this.Children != null)
			{
				foreach (Control c in this.Children)
				{ c.ApplySkin(this.baseSkin); }
			}
		}

		public void Clear()
		{
			this.Children.Clear();
		}

		public override void Draw(Canvas canvas, float zOffset)
		{
			base.Draw(canvas, zOffset);

			foreach (Control c in this.Children)
			{ c.Draw(canvas, zOffset + Control.LAYOUT_ZOFFSET); }
		}

		public Control FindHoveredControl(Vector2 position)
		{
			Control result = this.Children.FirstOrDefault(c =>
				(c is ILayout || c is InteractiveControl) &&
				(c.Status & Control.ControlStatus.Disabled) == Control.ControlStatus.None &&
				c.Visibility == Control.ControlVisibility.Visible &&
				c.ControlArea.Contains(position));

			if (result is ILayout il)
			{ result = il.FindHoveredControl(position); }

			if (result == null && !this.IsPassthrough)
			{ result = this; }

			return result;
		}

		public IEnumerable<T> GetChildren<T>() where T : Control
		{
			return this.Children.Where(c => c is T).Cast<T>();
		}

		public void LayoutControls()
		{
			foreach (Control c in this.Children)
			{ c.ActualSize = c.Visibility == ControlVisibility.Collapsed ? Size.Zero : c.Size; }

			this._LayoutControls();

			foreach (Control c in this.Children)
			{
				c.ActualPosition += this.ActualPosition + c.Margin.TopLeft;
				c.ActualSize -= (c.Margin.TopLeft + c.Margin.BottomRight);

				(c as ILayout)?.LayoutControls();
			}
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			foreach (Control c in this.Children)
			{ c.OnUpdate(msFrame); }
		}

		public ControlsContainer Remove(Control child)
		{
			this.Children.Remove(child);
			return this;
		}

		public ControlsContainer RemoveAll<T>() where T : Control
		{
			this.Children.RemoveAll(c => c is T);
			return this;
		}

		internal abstract void _LayoutControls();
	}
}
