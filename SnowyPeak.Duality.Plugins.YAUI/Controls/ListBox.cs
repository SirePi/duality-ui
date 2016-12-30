// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public sealed class ListBox : CompositeControl
	{
		public static readonly string PANEL_TEMPLATE = ".Panel";
		public static readonly string SCROLLBAR_TEMPLATE = ".ScrollBar";

		private int _itemsInView;
		private VerticalScrollBar _scrollBar;
		private StackPanel _stackPanel;
		private TextConfiguration _textConfiguration;
		private List<ToggleButton> _toggleButtons;

		public ListBoxConfiguration ListBoxConfiguration { get; set; }
		public bool MultiSelection { get; set; }

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

		public IEnumerable<object> SelectedItems
		{
			get { return _toggleButtons.Where(tb => tb.Toggled).Select(tb => tb.Tag); }
		}

		public TextConfiguration TextConfiguration
		{
			get { return _textConfiguration; }
			set
			{
				_textConfiguration = value;
				foreach (ToggleButton tb in _toggleButtons)
				{
					tb.ApplySkin(_baseSkin);
					tb.TextConfiguration = _textConfiguration.Clone();
				}
			}
		}

		public ListBox(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			_toggleButtons = new List<ToggleButton>();
			ApplySkin(_baseSkin);
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);

			if (_scrollBar != null)
			{
				_scrollBar.ApplySkin(_baseSkin);

				ListBoxTemplate template = _baseSkin.GetTemplate<ListBoxTemplate>(this);

				this.ListBoxConfiguration = template.ListBoxConfiguration.Clone();
				this.TextConfiguration = template.TextConfiguration.Clone();
			}
		}

		public override ControlsContainer BuildControl()
		{
			return new DockPanel()
			.Add(_scrollBar = new VerticalScrollBar(_baseSkin, this.TemplateName + SCROLLBAR_TEMPLATE)
			{
				Docking = Dock.Right
			})
			.Add(_stackPanel = new StackPanel(_baseSkin, this.TemplateName + PANEL_TEMPLATE)
			{
				Docking = Dock.Center,
				Direction = Direction.UpToDown
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

				toggle.ApplySkin(_baseSkin);

				_toggleButtons.Add(toggle);
				_stackPanel.Add(toggle);
			}

			OnUpdate(0);
		}
	}
}