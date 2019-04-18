// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Input;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public abstract class ScrollBar : CompositeControl
	{
		public static readonly string CURSOR_TEMPLATE = ".Cursor";
		public static readonly string DECREASE_TEMPLATE = ".Decrease";
		public static readonly string INCREASE_TEMPLATE = ".Increase";

		protected Button _btnCursor;
		protected Button _btnDecrease;
		protected Button _btnIncrease;
		protected CanvasPanel _canvas;
		protected int _valueDelta;

		private Vector2? _cursorDragPosition;
		private bool _isDecreasing;
		private bool _isIncreasing;
		private int _maxValue;
		private int _minValue;
		private float _mseconds;
		private ScrollBarConfiguration _scrollBarConfiguration;
		private float _tempValue;
		private int _value;

		public int MaxValue
		{
			get { return _maxValue; }
			set
			{
				_maxValue = value;
				_valueDelta = _maxValue - _minValue;

				this.Value = MathF.Clamp(Value, MinValue, MaxValue);
			}
		}

		public int MinValue
		{
			get { return _minValue; }
			set
			{
				_minValue = value;
				_valueDelta = _maxValue - _minValue;

				this.Value = MathF.Clamp(Value, MinValue, MaxValue);
			}
		}

		public ScrollBarConfiguration ScrollBarConfiguration
		{
			get { return _scrollBarConfiguration; }
			set
			{
				_scrollBarConfiguration = value;

				_btnDecrease.Size = _scrollBarConfiguration.ButtonsSize;
				_btnIncrease.Size = _scrollBarConfiguration.ButtonsSize;
				_btnCursor.Size = _scrollBarConfiguration.CursorSize;

				_btnIncrease.Appearance = _scrollBarConfiguration.ButtonIncreaseAppearance;
				_btnDecrease.Appearance = _scrollBarConfiguration.ButtonDecreaseAppearance;
				_btnCursor.Appearance = _scrollBarConfiguration.CursorAppearance;
			}
		}

		public int Value
		{
			get { return _value; }
			set
			{
				if (_value != value)
				{ _onValueChange?.Invoke(this, _value, value); }

				_value = value;
			}
		}

		// Delegates
		public delegate void ValueChangeEventDelegate(ScrollBar scrollBar, int oldValue, int newValue);
		// Events
		[DontSerialize]
		private ValueChangeEventDelegate _onValueChange;
		public event ValueChangeEventDelegate OnValueChange
		{
			add { _onValueChange += value; }
			remove { _onValueChange -= value; }
		}

		public ScrollBar(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			this.MinValue = 0;
			this.Value = 0;
			this.MaxValue = 100;

			ApplySkin(_baseSkin);
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);

			ScrollBarTemplate template = _baseSkin.GetTemplate<ScrollBarTemplate>(this);
			this.ScrollBarConfiguration = template.ScrollBarConfiguration.Clone();
			this.Margin = template.ScrollBarMargin;
		}

		public override ControlsContainer BuildControl()
		{
			DockPanel scrollBar = new DockPanel();
			_btnDecrease = new Button(_baseSkin, this.TemplateName + DECREASE_TEMPLATE)
			{
				StretchToFill = false
			};
			_btnDecrease.OnMouseButton += _btnDecrease_OnMouseButton;
			_btnDecrease.OnFocusChange += _btnDecrease_OnFocusChange;

			_btnIncrease = new Button(_baseSkin, this.TemplateName + INCREASE_TEMPLATE)
			{
				StretchToFill = false
			};
			_btnIncrease.OnMouseButton += _btnIncrease_OnMouseButton;
			_btnIncrease.OnFocusChange += _btnIncrease_OnFocusChange;

			_btnCursor = new Button(_baseSkin, this.TemplateName + CURSOR_TEMPLATE)
			{
				StretchToFill = false
			};
			_btnCursor.OnMouseButton += _btnCursor_OnMouseButton;
			_btnCursor.OnFocusChange += _btnCursor_OnFocusChange;

			_canvas = new CanvasPanel(_baseSkin)
			{
				Docking = Dock.Center
			};
			_canvas.Add(_btnCursor);

			scrollBar
				.Add(_btnDecrease)
				.Add(_btnIncrease)
				.Add(_canvas);

			return scrollBar;
		}

		private void _btnCursor_OnFocusChange(Control control, bool isFocused)
		{
			if (!isFocused)
			{ _cursorDragPosition = null; }
		}

		private void _btnCursor_OnMouseButton(Button button, MouseButtonEventArgs args)
		{
			if (args.Button == MouseButton.Left)
			{
				_tempValue = this.Value;

				if (args.IsPressed)
				{ _cursorDragPosition = args.Pos; }
				else
				{ _cursorDragPosition = null; }
			}
		}

		private void _btnIncrease_OnFocusChange(Control control, bool isFocused)
		{
			if (!isFocused)
			{ _isIncreasing = false; }
		}

		private void _btnIncrease_OnMouseButton(Button button, MouseButtonEventArgs args)
		{
			if (args.Button == MouseButton.Left)
			{ _isIncreasing = args.IsPressed; }
		}

		private void _btnDecrease_OnFocusChange(Control control, bool isFocused)
		{
			if (!isFocused)
			{ _isDecreasing = false; }
		}

		private void _btnDecrease_OnMouseButton(Button button, MouseButtonEventArgs args)
		{
			if (args.Button == MouseButton.Left)
			{ _isDecreasing = args.IsPressed; }
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			if (_isIncreasing || _isDecreasing)
			{
				_mseconds += msFrame;

				if (_mseconds > 100)
				{
					if (_isIncreasing)
					{ this.Value += 1; }

					if (_isDecreasing)
					{ this.Value -= 1; }

					this.Value = MathF.Clamp(Value, MinValue, MaxValue);
					_mseconds -= 100;
				}
			}
			else if (_cursorDragPosition.HasValue)
			{
				Vector2 mouseDelta = DualityApp.Mouse.Pos - _cursorDragPosition.Value;

				if (mouseDelta.Length > 0)
				{
					_tempValue += ApplyMouseMovement(mouseDelta);

					this.Value = MathF.Clamp((int)MathF.Round(_tempValue, MidpointRounding.AwayFromZero), MinValue, MaxValue);
					_cursorDragPosition = DualityApp.Mouse.Pos;
				}
			}

			UpdateCursor();
		}

		protected abstract float ApplyMouseMovement(Vector2 mouseDelta);

		protected abstract void UpdateCursor();
	}
}