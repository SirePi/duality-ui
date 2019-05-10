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

		protected readonly List<Control> children = new List<Control>();

		protected ControlsContainer(Skin skin, string templateName)
			: base(skin, templateName)
		{ }

		protected override void Init()
		{
			base.Init();
			this.IsPassthrough = true;
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);

			foreach (Control c in this.children)
				c.ApplySkin(skin);
		}

		public void ApplySkinSingle(Skin skin)
		{
			base.ApplySkin(skin);
		}

		public ControlsContainer Add(Control child)
		{
			if (this.children.Contains(child))
			{ throw new InvalidOperationException(string.Format("Duplicate control {0} in parent {1}", child, this)); }
			else
			{
				// check that I am not introducing a circular ancestry
				ControlsContainer cc = this.Parent;
				while (cc != null)
				{
					if (cc == child)
					{ throw new InvalidOperationException(string.Format("Circular ancestry between {0} and {1}", child, this)); }

					cc = cc.Parent;
				}

				if (child.Parent != null)
				{
					Logs.Get<UILog>().WriteWarning("Control {0} moved from parent {1} to {2}. Might be an error.", child, child.Parent, this);
					child.Parent.Remove(child);
				}

				child.Parent = this;
				this.children.Add(child);

				return this;
			}
		}

		public void Clear()
		{
			this.children.Clear();
		}

		protected override void _Draw(Canvas canvas, float zOffset)
		{
			base._Draw(canvas, zOffset);
			foreach (Control c in this.children)
				c.Draw(canvas, zOffset + Control.LAYOUT_ZOFFSET);
		}

		public Control FindHoveredControl(Vector2 position)
		{
			Control result = this.children.FirstOrDefault(c =>
				(c is ILayout || c is IInteractiveControl) &&
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
			return this.children.Where(c => c is T).Cast<T>();
		}

		public void LayoutControls()
		{
			foreach (Control c in this.children)
			{ c.ActualSize = c.Visibility == ControlVisibility.Collapsed ? Size.Zero : c.Size; }

			this._LayoutControls();

			foreach (Control c in this.children)
			{
				c.ActualPosition += this.ActualPosition + c.Margin.TopLeft;
				c.ActualSize -= (c.Margin.TopLeft + c.Margin.BottomRight);

				(c as ILayout)?.LayoutControls();
			}
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			foreach (Control c in this.children)
			{ c.OnUpdate(msFrame); }
		}

		public ControlsContainer Remove(Control child)
		{
			this.children.Remove(child);
			return this;
		}

		public ControlsContainer RemoveAll<T>() where T : Control
		{
			this.children.RemoveAll(c => c is T);
			return this;
		}

		internal abstract void _LayoutControls();
	}
}
