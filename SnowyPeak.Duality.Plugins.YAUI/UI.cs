// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Input;
using SnowyPeak.Duality.Plugins.YAUI.Controls;
using SnowyPeak.Duality.Plugins.YAUI.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI
{
	[EditorHintImage(ResNames.ImageUI)]
	[EditorHintCategory(ResNames.CategoryUI)]
	public abstract class UI : Component, ICmpUpdatable, ICmpRenderer, ICmpInitializable
	{
		private const float GLOBAL_ZOFFSET = 0.01f;

		[DontSerialize]
		private Canvas canvas;

		[DontSerialize]
		private Control focusedControl;

		[DontSerialize]
		private Control hoveredControl;

		[DontSerialize]
		private ControlsContainer rootContainer;

		[DontSerialize]
		private Size lastSize;

		[EditorHintFlags(MemberFlags.Invisible)]
		public Control HoveredControl => this.hoveredControl;

		public bool IsFullScreen { get; set; }
		public int Offset { get; set; }

		// Events
		[DontSerialize]
		private Action onResize;
		public event Action OnResize
		{
			add { this.onResize += value; }
			remove { this.onResize += value; }
		}

		protected UI()
		{
			this.Offset = 1;
		}

		void ICmpInitializable.OnActivate()
		{
			this.rootContainer = this.CreateUI();
			this.canvas = new Canvas();
			this.OnUpdate();
		}

		void ICmpInitializable.OnDeactivate()
		{
			this.rootContainer = null;
		}

		void ICmpRenderer.Draw(IDrawDevice device)
		{
			if (this.rootContainer != null)
			{
				if (this.IsFullScreen)
				{
					this.rootContainer.ActualSize.X = DualityApp.TargetViewSize.X;
					this.rootContainer.ActualSize.Y = DualityApp.TargetViewSize.Y;
					this.rootContainer.ActualPosition = Vector2.Zero;
				}
				else
				{
					this.rootContainer.ActualSize = this.rootContainer.Size;
					this.rootContainer.ActualPosition = this.rootContainer.Position;
				}

				this.rootContainer.LayoutControls();

				this.canvas.Begin(device);
				try
				{
					this.rootContainer.Draw(this.canvas, this.Offset * GLOBAL_ZOFFSET);
				}
				catch (Exception ex)
				{
					Logs.Get<UILog>().WriteError("Exception {0} while drawing", ex.Message);
					Logs.Get<UILog>().WriteError(ex.StackTrace);
				}
				this.canvas.End();
			}
		}

		void ICmpUpdatable.OnUpdate()
		{
			if (this.rootContainer != null)
			{
				Control currentHoveredControl = this.rootContainer.FindHoveredControl(DualityApp.Mouse.Pos);

				// Check if the hovered control changed
				if (this.hoveredControl != currentHoveredControl)
				{
					if (currentHoveredControl is IInteractiveControl ic1)
					{ ic1.OnMouseEnterEvent(); }

					if (this.hoveredControl is IInteractiveControl ic2)
					{ ic2.OnMouseLeaveEvent(); }
				}

				// check if the focused control changed
				if (YAUICorePlugin.LastFrameMouseButtonEventArgs.Count > 0)
				{
					if (currentHoveredControl != this.focusedControl && this.focusedControl != null)
					{
						(this.focusedControl as IInteractiveControl)?.OnBlur();
						this.focusedControl = null;
					}

					if (currentHoveredControl != null && currentHoveredControl != this.focusedControl)
					{
						this.focusedControl = currentHoveredControl;
						(this.focusedControl as IInteractiveControl)?.OnFocus();
					}
				}

				// send events to the focused control
				if (this.focusedControl is IInteractiveControl ic3)
				{
					foreach (MouseButtonEventArgs e in YAUICorePlugin.LastFrameMouseButtonEventArgs)
					{
						ic3.OnMouseButtonEvent(e);
					}

					foreach (KeyboardKeyEventArgs e in YAUICorePlugin.LastFrameKeyboardKeyEventArgs)
					{
						ic3.OnKeyboardKeyEvent(e);
					}
				}

				this.hoveredControl = currentHoveredControl;

				// Added check because it might have been set to null due to a Deactivation event
				if (this.rootContainer != null)
				{
					this.rootContainer.OnUpdate(Time.DeltaTime * 1000);
					if (this.lastSize != this.rootContainer.ActualSize)
					{ this.onResize?.Invoke(); }

					this.lastSize = this.rootContainer.ActualSize;
				}
			}

			this.OnUpdate();
		}

		protected abstract ControlsContainer CreateUI();

		protected virtual void OnUpdate()
		{ }

		void ICmpRenderer.GetCullingInfo(out CullingInfo info)
		{
			info = new CullingInfo()
			{
				Radius = 0,
				Visibility = VisibilityFlag.Group30 | VisibilityFlag.ScreenOverlay
			};
		}
	}
}
