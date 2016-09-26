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
	public sealed class ListBox : CompositeControl
	{
		private StackPanel _stackPanel;
		private ScrollBar _scrollBar;

		private List<ToggleButton> _toggleButtons;
		private int _itemsInView;

		public IEnumerable<object> SelectedItems
		{
			get { return _toggleButtons.Where(tb => tb.Toggled).Select(tb => tb.Tag); }
		}

		public ScrollBarConfiguration ScrollBarConfiguration
		{
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
				Orientation = Orientation.Vertical
			})
			.Add(_stackPanel = new StackPanel()
			{
				Docking = DockPanel.Dock.Center,
				Orientation = Orientation.Vertical
			});
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
					Appearance = this.ListBoxConfiguration.ItemAppearance,
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

				_toggleButtons.Add(toggle);
				_stackPanel.Add(toggle);
			}

			OnUpdate(0);
		}
	}
}
