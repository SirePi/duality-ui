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
	public abstract class ScrollBar : CompositeControl<ScrollBarTemplate>
	{
		public static readonly string CURSOR_TEMPLATE = ".Cursor";
		public static readonly string DECREASE_TEMPLATE = ".Decrease";
		public static readonly string INCREASE_TEMPLATE = ".Increase";

		protected Button btnCursor;
		protected Button btnDecrease;
		protected Button btnIncrease;
		protected CanvasPanel canvas;
		protected int valueDelta;

		private Vector2? cursorDragPosition;
		private bool isDecreasing;
		private bool isIncreasing;
		private int maxValue;
		private int minValue;
		private float mseconds;
		private ScrollBarConfiguration scrollBarConfiguration;
		private float tempValue;
		private int value;

		public int MaxValue
		{
			get => this.maxValue;
			set
			{
				this.maxValue = value;
				this.valueDelta = this.maxValue - this.minValue;

				this.Value = MathF.Clamp(this.Value, this.MinValue, this.MaxValue);
			}
		}

		public int MinValue
		{
			get => this.minValue;
			set
			{
				this.minValue = value;
				this.valueDelta = this.maxValue - this.minValue;

				this.Value = MathF.Clamp(this.Value, this.MinValue, this.MaxValue);
			}
		}

		public ScrollBarConfiguration ScrollBarConfiguration
		{
			get => this.scrollBarConfiguration;
			set
			{
				this.scrollBarConfiguration = value;

				this.btnDecrease.Size = this.scrollBarConfiguration.ButtonsSize;
				this.btnIncrease.Size = this.scrollBarConfiguration.ButtonsSize;
				this.btnCursor.Size = this.scrollBarConfiguration.CursorSize;

				this.btnIncrease.Appearance = this.scrollBarConfiguration.ButtonIncreaseAppearance;
				this.btnDecrease.Appearance = this.scrollBarConfiguration.ButtonDecreaseAppearance;
				this.btnCursor.Appearance = this.scrollBarConfiguration.CursorAppearance;
			}
		}

		public int Value
		{
			get => this.value;
			set
			{
				if (this.value != value)
				{ this.onValueChange?.Invoke(this, this.value, value); }

				this.value = value;
			}
		}

		public bool AllowDrag { get; set; }

		// Delegates
		public delegate void ValueChangeEventDelegate(ScrollBar scrollBar, int oldValue, int newValue);
		// Events
		[DontSerialize]
		private ValueChangeEventDelegate onValueChange;
		public event ValueChangeEventDelegate OnValueChange
		{
			add { this.onValueChange += value; }
			remove { this.onValueChange -= value; }
		}

		protected ScrollBar(Skin skin, string templateName)
			: base(skin, templateName)
		{ }

		protected override void Init()
		{
			base.Init();
		
			this.MinValue = 0;
			this.Value = 0;
			this.MaxValue = 100;

			this.AllowDrag = true;
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);

			this.ScrollBarConfiguration = this.Template.ScrollBarConfiguration.Clone();
			this.Margin = this.Template.ScrollBarMargin;
		}

		public override ControlsContainer BuildControl()
		{
			DockPanel scrollBar = new DockPanel(this.skin);
			this.btnDecrease = new Button(this.skin, this.TemplateName + DECREASE_TEMPLATE);
			this.btnDecrease.StretchToFill = false;
			this.btnDecrease.OnMouseButton += this.BtnDecrease_OnMouseButton;
			this.btnDecrease.OnFocusChange += this.BtnDecrease_OnFocusChange;

			this.btnIncrease = new Button(this.skin, this.TemplateName + INCREASE_TEMPLATE);
			this.btnIncrease.StretchToFill = false;
			this.btnIncrease.OnMouseButton += this.BtnIncrease_OnMouseButton;
			this.btnIncrease.OnFocusChange += this.BtnIncrease_OnFocusChange;

			this.btnCursor = new Button(this.skin, this.TemplateName + CURSOR_TEMPLATE);
			this.btnCursor.StretchToFill = false;
			this.btnCursor.OnMouseButton += this.BtnCursor_OnMouseButton;
			this.btnCursor.OnFocusChange += this.BtnCursor_OnFocusChange;

			this.canvas = new CanvasPanel(this.skin);
			this.canvas.Docking = Dock.Center;
			this.canvas.Add(this.btnCursor);

			scrollBar
				.Add(this.btnDecrease)
				.Add(this.btnIncrease)
				.Add(this.canvas);

			return scrollBar;
		}

		private void BtnCursor_OnFocusChange(Control control, bool isFocused)
		{
			if (!isFocused)
			{ this.cursorDragPosition = null; }
		}

		private void BtnCursor_OnMouseButton(IInteractiveControl button, MouseButtonEventArgs args)
		{
			if (args.Button == MouseButton.Left && this.AllowDrag)
			{
				this.tempValue = this.Value;

				if (args.IsPressed)
				{ this.cursorDragPosition = args.Pos; }
				else
				{ this.cursorDragPosition = null; }
			}
		}

		private void BtnIncrease_OnFocusChange(Control control, bool isFocused)
		{
			if (!isFocused)
			{ this.isIncreasing = false; }
		}

		private void BtnIncrease_OnMouseButton(IInteractiveControl button, MouseButtonEventArgs args)
		{
			if (args.Button == MouseButton.Left)
			{ this.isIncreasing = args.IsPressed; }
		}

		private void BtnDecrease_OnFocusChange(Control control, bool isFocused)
		{
			if (!isFocused)
			{ this.isDecreasing = false; }
		}

		private void BtnDecrease_OnMouseButton(IInteractiveControl button, MouseButtonEventArgs args)
		{
			if (args.Button == MouseButton.Left)
			{ this.isDecreasing = args.IsPressed; }
		}

		public override void OnUpdate(float msFrame)
		{
			base.OnUpdate(msFrame);

			if (this.isIncreasing || this.isDecreasing)
			{
				this.mseconds += msFrame;

				if (this.mseconds > 100)
				{
					if (this.isIncreasing)
					{ this.Value += 1; }

					if (this.isDecreasing)
					{ this.Value -= 1; }

					this.Value = MathF.Clamp(this.Value, this.MinValue, this.MaxValue);
					this.mseconds -= 100;
				}
			}
			else if (this.cursorDragPosition.HasValue)
			{
				Vector2 mouseDelta = DualityApp.Mouse.Pos - this.cursorDragPosition.Value;

				if (mouseDelta.Length > 0)
				{
					this.tempValue += this.ApplyMouseMovement(mouseDelta);

					this.Value = MathF.Clamp((int)MathF.Round(this.tempValue, MidpointRounding.AwayFromZero), this.MinValue, this.MaxValue);
					this.cursorDragPosition = DualityApp.Mouse.Pos;
				}
			}

			this.UpdateCursor();
		}

		protected abstract float ApplyMouseMovement(Vector2 mouseDelta);

		protected abstract void UpdateCursor();
	}
}
