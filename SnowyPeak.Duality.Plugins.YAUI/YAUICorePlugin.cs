// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnowyPeak.Duality.Plugins.YAUI
{
	/// <summary>
	/// Defines a Duality core plugin.
	/// </summary>
	public class YAUICorePlugin : CorePlugin
	{
        internal static List<KeyboardKeyEventArgs> LastFrameKeyboardKeyEventArgs { get; private set; } = new List<KeyboardKeyEventArgs>();
        internal static List<MouseButtonEventArgs> LastFrameMouseButtonEventArgs { get; private set; } = new List<MouseButtonEventArgs>();

        // Override methods here for global logic
        protected override void InitPlugin()
        {
            DualityApp.Mouse.ButtonDown += Mouse_ButtonDown;
            DualityApp.Mouse.ButtonUp += Mouse_ButtonUp;
            DualityApp.Keyboard.KeyDown += Keyboard_KeyDown;
            DualityApp.Keyboard.KeyUp += Keyboard_KeyUp;

            LastFrameKeyboardKeyEventArgs.Clear();
            LastFrameMouseButtonEventArgs.Clear();
        }

        protected override void OnDisposePlugin()
        {
            DualityApp.Mouse.ButtonDown -= Mouse_ButtonDown;
            DualityApp.Mouse.ButtonUp -= Mouse_ButtonUp;
            DualityApp.Keyboard.KeyDown -= Keyboard_KeyDown;
            DualityApp.Keyboard.KeyUp -= Keyboard_KeyUp;

            base.OnDisposePlugin();
        }

        protected override void OnAfterUpdate()
        {
            base.OnAfterUpdate();
            LastFrameKeyboardKeyEventArgs.Clear();
            LastFrameMouseButtonEventArgs.Clear();
        }

        #region Input Events
        private void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            LastFrameKeyboardKeyEventArgs.Add(e);
        }

        private void Keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            LastFrameKeyboardKeyEventArgs.Add(e);
        }

        private void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            LastFrameMouseButtonEventArgs.Add(e);
        }

        private void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            LastFrameMouseButtonEventArgs.Add(e);
        }
        #endregion Input Events
    }
}