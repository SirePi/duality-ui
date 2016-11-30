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
		private static readonly float GLOBAL_ZOFFSET = 0.01f;

		[DontSerialize]
		private Control _focusedControl;

		[DontSerialize]
		private Control _hoveredControl;

		[DontSerialize]
		private List<KeyboardKeyEventArgs> _lastFrameKeyboardKeyEventArgs;

		[DontSerialize]
		private List<MouseButtonEventArgs> _lastFrameMouseButtonEventArgs;

		[DontSerialize]
		private ControlsContainer _rootContainer;

		[EditorHintFlags(MemberFlags.Invisible)]
		public Control HoveredControl
		{
			get { return _hoveredControl; }
		}

		float ICmpRenderer.BoundRadius
		{
			get { return 0; }
		}

		public bool IsFullScreen { get; set; }
		public int Offset { get; set; }

		protected UI()
		{
			this.Offset = 1;
		}

		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate)
			{
				_rootContainer = CreateUI();

				OnUpdate();

				DualityApp.Mouse.ButtonDown += Mouse_ButtonDown;
				DualityApp.Mouse.ButtonUp += Mouse_ButtonUp;
				DualityApp.Keyboard.KeyDown += Keyboard_KeyDown;
				DualityApp.Keyboard.KeyUp += Keyboard_KeyUp;

				if (_lastFrameKeyboardKeyEventArgs == null)
					_lastFrameKeyboardKeyEventArgs = new List<KeyboardKeyEventArgs>();

				if (_lastFrameMouseButtonEventArgs == null)
					_lastFrameMouseButtonEventArgs = new List<MouseButtonEventArgs>();

				_lastFrameKeyboardKeyEventArgs.Clear();
				_lastFrameMouseButtonEventArgs.Clear();
			}
		}

		void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
			{
				_rootContainer = null;

				DualityApp.Mouse.ButtonDown -= Mouse_ButtonDown;
				DualityApp.Mouse.ButtonUp -= Mouse_ButtonUp;
				DualityApp.Keyboard.KeyDown -= Keyboard_KeyDown;
				DualityApp.Keyboard.KeyUp -= Keyboard_KeyUp;
			}
		}

		void ICmpRenderer.Draw(IDrawDevice device)
		{
			if (_rootContainer != null)
			{
				if (this.IsFullScreen)
				{
					_rootContainer.ActualSize.X = DualityApp.TargetResolution.X;
					_rootContainer.ActualSize.Y = DualityApp.TargetResolution.Y;
					_rootContainer.ActualPosition = Vector2.Zero;
				}
				else
				{
					_rootContainer.ActualSize = _rootContainer.Size;
					_rootContainer.ActualPosition = _rootContainer.Position;
				}

				_rootContainer.LayoutControls();

				Canvas c = new Canvas(device);
				_rootContainer.Draw(c, this.Offset * GLOBAL_ZOFFSET);
			}
		}

		bool ICmpRenderer.IsVisible(IDrawDevice device)
		{
			bool result = true;

			// this is visible only as Group30 / ScreenOverlay
			if ((device.VisibilityMask & VisibilityFlag.ScreenOverlay) == VisibilityFlag.None)
			{
				result = false;
			}
			// No match in any VisibilityGroup? Don't render!
			if ((VisibilityFlag.Group30 & device.VisibilityMask) == VisibilityFlag.None)
			{
				result = false;
			}

			return result;
		}

		void ICmpUpdatable.OnUpdate()
		{
			if (_rootContainer != null)
			{
				Control currentHoveredControl = _rootContainer.FindHoveredControl(DualityApp.Mouse.Pos);

				// Check if the hovered control changed
				if (_hoveredControl != currentHoveredControl)
				{
					if (currentHoveredControl != null)
					{ currentHoveredControl.OnMouseEnterEvent(); }

					if (_hoveredControl != null)
					{ _hoveredControl.OnMouseLeaveEvent(); }
				}

				// check if the focused control changed
				if (_lastFrameMouseButtonEventArgs.Count > 0)
				{
					if (currentHoveredControl != _focusedControl && _focusedControl != null)
					{
						_focusedControl.OnBlur();
						_focusedControl = null;
					}

					if (currentHoveredControl != null)
					{
						_focusedControl = currentHoveredControl;
						_focusedControl.OnFocus();
					}
				}

				// send events to the focused control
				if (_focusedControl != null)
				{
					foreach (MouseButtonEventArgs e in _lastFrameMouseButtonEventArgs)
					{
						_focusedControl.OnMouseButtonEvent(e);
					}

					foreach (KeyboardKeyEventArgs e in _lastFrameKeyboardKeyEventArgs)
					{
						_focusedControl.OnKeyboardKeyEvent(e);
					}
				}

				_lastFrameMouseButtonEventArgs.Clear();
				_lastFrameKeyboardKeyEventArgs.Clear();

				_hoveredControl = currentHoveredControl;
				_rootContainer.OnUpdate(Time.MsPFMult * Time.TimeMult);
			}

			OnUpdate();
		}

		#region Input Events

		private void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			_lastFrameKeyboardKeyEventArgs.Add(e);
		}

		private void Keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
		{
			_lastFrameKeyboardKeyEventArgs.Add(e);
		}

		private void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
		{
			_lastFrameMouseButtonEventArgs.Add(e);
		}

		private void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
		{
			_lastFrameMouseButtonEventArgs.Add(e);
		}

		#endregion Input Events

		protected abstract ControlsContainer CreateUI();

		protected virtual void OnUpdate()
		{
		}
	}
}