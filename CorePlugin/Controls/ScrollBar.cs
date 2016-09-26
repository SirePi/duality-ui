using Duality;
using Duality.Drawing;
using SnowyPeak.DualityUI.Controls.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls
{
	public sealed class ScrollBar : CompositeControl
	{
		public delegate void ValueChangedEventDelegate(ScrollBar scrollBar, int value);
		public ValueChangedEventDelegate ValueChangedEventHandler { get; set; }

		private Button _btnDecrease;
		private Button _btnIncrease;
		private Button _btnCursor;

        private CanvasPanel _canvas;

        private bool _isIncreasing;
        private bool _isDecreasing;
        private Vector2? _cursorDragPosition;
        private float _mseconds;
        private float _tempValue;

        private int _minValue;
        private int _maxValue;
		private int _value;
        private int _valueDelta;

        private Orientation _orientation;
		public Orientation Orientation 
        {
            set
            {
                _orientation = value;
                if (_orientation == DualityUI.Orientation.Horizontal)
                {
                    _btnDecrease.Docking = DockPanel.Dock.Left;
                    _btnIncrease.Docking = DockPanel.Dock.Right;
                }
                else
                {
                    _btnDecrease.Docking = DockPanel.Dock.Top;
                    _btnIncrease.Docking = DockPanel.Dock.Bottom;
                }
            }
        }

        public int MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                _valueDelta = _maxValue - _minValue;

				this.Value = Math.Max(Math.Min(MaxValue, Value), MinValue);
            }
        }
        public int Value 
		{
			get { return _value; }
			set
			{
				bool valueChanged = _value != value;
				_value = value;

				if (valueChanged && this.ValueChangedEventHandler != null)
				{ this.ValueChangedEventHandler(this, value); }
			}
		}
        public int MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                _valueDelta = _maxValue - _minValue;

				this.Value = Math.Max(Math.Min(MaxValue, Value), MinValue);
            }
        }

		private ScrollBarConfiguration _scrollBarConfiguration;
		public ScrollBarConfiguration ScrollBarConfiguration
		{
			private get { return _scrollBarConfiguration; }
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


        public ScrollBar()
        {
            this.MinValue = 0;
            this.Value = 0;
            this.MaxValue = 100;
        }

		public override ControlsContainer BuildControl()
		{
			DockPanel scrollBar = new DockPanel();
            scrollBar.Add(_btnDecrease = new Button()
            {
                MouseButtonEventHandler = (button, args) => 
                {
					if (args.Button == Duality.Input.MouseButton.Left)
					{ _isDecreasing = args.IsPressed; }
                }
            });
            scrollBar.Add(_btnIncrease = new Button()
            {
                MouseButtonEventHandler = (button, args) => 
                {
					if (args.Button == Duality.Input.MouseButton.Left)
					{ _isIncreasing = args.IsPressed; }
                }
            });
            scrollBar.Add(_canvas = new CanvasPanel()
            {
                Docking = DockPanel.Dock.Center
            });

            _canvas.Add(_btnCursor = new Button()
            {
                StretchToFill = false,
                Grid = new GridPanel.GridInfo() { Row = 1, Column = 1 },
                MouseButtonEventHandler = (button, args) =>
                {
                    if(args.Button == Duality.Input.MouseButton.Left)
                    {
                        _tempValue = this.Value;

                        if (args.IsPressed) 
						{ _cursorDragPosition = args.Position; }
                        else 
						{ _cursorDragPosition = null; }
                    }
                },
                FocusChangeHandler = (button, isFocused) =>
                {
                    if (!isFocused) 
					{ _cursorDragPosition = null; }
                }
            });

			return scrollBar;
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

                    this.Value = Math.Max(Math.Min(MaxValue, Value), MinValue);
                    _mseconds -= 100;
                }
            }
            else if (_cursorDragPosition.HasValue)
            {
                Vector2 mouseDelta = DualityApp.Mouse.Pos - _cursorDragPosition.Value;

                if (mouseDelta.Length > 0)
                {
                    if (_orientation == DualityUI.Orientation.Horizontal)
                    {
                        float delta = (_canvas.ActualSize.X - _btnCursor.ActualSize.X) / _valueDelta;
                        _tempValue += (mouseDelta.X / delta);
                    }
                    else
                    {
                        float delta = (_canvas.ActualSize.Y - _btnCursor.ActualSize.Y) / _valueDelta;
                        _tempValue += (mouseDelta.Y / delta);
                    }

                    this.Value = Math.Max(Math.Min(MaxValue, (int)_tempValue), MinValue);
                    _cursorDragPosition = DualityApp.Mouse.Pos;
                }
            }

            UpdateCursor();
        }

        private void UpdateCursor()
        {
            if (_orientation == DualityUI.Orientation.Horizontal)
            {
                float delta = (_canvas.ActualSize.X - _btnCursor.ActualSize.X) / _valueDelta;
                _btnCursor.Position.X = (delta * this.Value);
            }
            else
            {
                float delta = (_canvas.ActualSize.Y - _btnCursor.ActualSize.Y) / _valueDelta;
                _btnCursor.Position.Y = (delta * this.Value);
            }
        }
	}
}
