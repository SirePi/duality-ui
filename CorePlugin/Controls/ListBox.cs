using Duality;
using Duality.Drawing;
using SnowyPeak.DualityUI.Controls.Configuration;
using SnowyPeak.DualityUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls
{
	public sealed class ListBox : CompositeControl
	{
		public static readonly string SCROLLBAR_TEMPLATE = ".ScrollBar";
		public static readonly string PANEL_TEMPLATE = ".Panel";

		private StackPanel _stackPanel;
		private ScrollBar _scrollBar;

		private List<ToggleButton> _toggleButtons;
		private int _itemsInView;
        private Skin _skin;

		public IEnumerable<object> SelectedItems
		{
			get { return _toggleButtons.Where(tb => tb.Toggled).Select(tb => tb.Tag); }
		}

		public ScrollBarConfiguration ScrollBarConfiguration
		{
			private get { return _scrollBar.ScrollBarConfiguration; }
			set
			{
				_scrollBar.ScrollBarConfiguration = value;
				_scrollBar.Size = new Size(
					MathF.Max(value.ButtonsSize.X, value.CursorSize.X),
					MathF.Max(value.ButtonsSize.Y, value.CursorSize.Y));
			}
		}

		public ListBoxConfiguration ListBoxConfiguration { private get; set; }
		public TextConfiguration TextConfiguration { private get; set; }

		public bool MultiSelection { get; set; }

		public ListBox()
		{
			_toggleButtons = new List<ToggleButton>();

			this.ScrollBarConfiguration = ScrollBarConfiguration.DEFAULT;
			this.ListBoxConfiguration = ListBoxConfiguration.DEFAULT;
			this.TextConfiguration = TextConfiguration.DEFAULT;
		}

		public override ControlsContainer BuildControl()
		{
			return new DockPanel()
			.Add(_scrollBar = new ScrollBar()
			{
				Docking = DockPanel.Dock.Right,
				Orientation = Orientation.Vertical,
                TemplateName = this.TemplateName + SCROLLBAR_TEMPLATE
			})
			.Add(_stackPanel = new StackPanel()
			{
				Docking = DockPanel.Dock.Center,
				Orientation = Orientation.Vertical,
                TemplateName = this.TemplateName + PANEL_TEMPLATE
			});
		}

        public override void ApplySkin(Skin skin)
		{
            if (skin == null) return;
            base.ApplySkin(skin);

            _scrollBar.ApplySkin(skin);

            ListBoxTemplate template = skin.GetTemplate<ListBoxTemplate>(this);

			if (this.ListBoxConfiguration == ListBoxConfiguration.DEFAULT)
			{ this.ListBoxConfiguration = template.ListBoxConfiguration; }

			if (this.TextConfiguration == TextConfiguration.DEFAULT)
            this.TextConfiguration = template.TextConfiguration;

            foreach(ToggleButton tb in _toggleButtons)
            {
                tb.ApplySkin(skin);
                tb.TextConfiguration = this.TextConfiguration;
            }

            _skin = skin;
        }

		public override void Draw(Canvas canvas, float zOffset)
		{
			_itemsInView = (int)MathF.Floor(_stackPanel.ChildrenArea.H / this.ListBoxConfiguration.ItemsSize.Y);

			_scrollBar.MinValue = 0;
			_scrollBar.MaxValue = Math.Max(0, _toggleButtons.Count - _itemsInView);

			_scrollBar.Visibility = _scrollBar.MaxValue == 0 ? ControlVisibility.Collapsed : ControlVisibility.Visible;

			int i = 0;

			foreach (ToggleButton tb in _toggleButtons)
			{
				if (i < _scrollBar.Value)
				{ tb.Visibility = ControlVisibility.Collapsed; }
				else if (i < _scrollBar.Value + _itemsInView)
				{ tb.Visibility = ControlVisibility.Visible; }
				else
				{ tb.Visibility = ControlVisibility.Collapsed; }

				i++;
			}

			base.Draw(canvas, zOffset);
		}

		public void SetItems(IEnumerable<object> items)
		{
			object[] selectedItems = this.SelectedItems.ToArray();

			_toggleButtons.Clear();
			_stackPanel.Clear();

			foreach (object obj in items)
			{
				ToggleButton toggle = new ToggleButton()
				{
					Text = obj.ToString(),
					Tag = obj,
					Visibility = ControlVisibility.Collapsed,
					Toggled = selectedItems.Contains(obj),
					Size = this.ListBoxConfiguration.ItemsSize,
					TextConfiguration = this.TextConfiguration,
					ToggleChangeEventHandler = (button, isToggled) =>
					{
						if (isToggled && !MultiSelection)
						{
							foreach (ToggleButton tb in _toggleButtons.Where(tb => tb != button))
							{
								tb.Toggled = false;
							}
						}
					}
				};

                toggle.ApplySkin(_skin);

				_toggleButtons.Add(toggle);
				_stackPanel.Add(toggle);
			}

			OnUpdate(0);
		}
	}
}
