﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.
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
        private Canvas _canvas;

		[DontSerialize]
		private Control _focusedControl;

		[DontSerialize]
		private Control _hoveredControl;

		[DontSerialize]
		private ControlsContainer _rootContainer;

		[EditorHintFlags(MemberFlags.Invisible)]
		public Control HoveredControl
		{
			get { return _hoveredControl; }
		}

		public bool IsFullScreen { get; set; }
		public int Offset { get; set; }

		protected UI()
		{
			this.Offset = 1;
		}

		void ICmpInitializable.OnActivate()
		{
			_rootContainer = CreateUI();
            OnUpdate();
		}

		void ICmpInitializable.OnDeactivate()
		{
			_rootContainer = null;
		}

		void ICmpRenderer.Draw(IDrawDevice device)
		{
			if (_rootContainer != null)
			{
				if (this.IsFullScreen)
				{
					_rootContainer.ActualSize.X = DualityApp.TargetViewSize.X;
					_rootContainer.ActualSize.Y = DualityApp.TargetViewSize.Y;
					_rootContainer.ActualPosition = Vector2.Zero;
				}
				else
				{
					_rootContainer.ActualSize = _rootContainer.Size;
					_rootContainer.ActualPosition = _rootContainer.Position;
				}

				_rootContainer.LayoutControls();

                if (_canvas == null)
                    _canvas = new Canvas();

                _canvas.Begin(device);
                try
                {
                    _rootContainer.Draw(_canvas, this.Offset * GLOBAL_ZOFFSET);
                }
                catch (Exception ex)
                {
                    Logs.Get<UILog>().WriteError("Exception {0} while drawing", ex.Message);
                    Logs.Get<UILog>().WriteError(ex.StackTrace);
                }
                _canvas.End();
			}
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
				if (YAUICorePlugin.LastFrameMouseButtonEventArgs.Count > 0)
				{
					if (currentHoveredControl != _focusedControl && _focusedControl != null)
					{
                        (_focusedControl as InteractiveControl)?.OnBlur();
						_focusedControl = null;
					}

					if (currentHoveredControl != null && currentHoveredControl != _focusedControl)
					{
						_focusedControl = currentHoveredControl;
                        (_focusedControl as InteractiveControl)?.OnFocus();
					}
				}

				// send events to the focused control
				if (_focusedControl != null)
				{
					foreach (MouseButtonEventArgs e in YAUICorePlugin.LastFrameMouseButtonEventArgs)
					{
						_focusedControl.OnMouseButtonEvent(e);
					}

					foreach (KeyboardKeyEventArgs e in YAUICorePlugin.LastFrameKeyboardKeyEventArgs)
					{
						_focusedControl.OnKeyboardKeyEvent(e);
					}
				}

				_hoveredControl = currentHoveredControl;

				// Added check because it might have been set to null due to a Deactivation event
				if (_rootContainer != null) _rootContainer.OnUpdate(Time.DeltaTime * 1000);
			}

			OnUpdate();
		}


		protected abstract ControlsContainer CreateUI();

		protected virtual void OnUpdate()
		{
		}

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