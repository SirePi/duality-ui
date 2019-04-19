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
		public const string PANEL_TEMPLATE = ".Panel";
		public const string SCROLLBAR_TEMPLATE = ".ScrollBar";

		private int itemsInView;
		private VerticalScrollBar scrollBar;
		private StackPanel stackPanel;
		private TextConfiguration textConfiguration;
		private readonly List<ToggleButton> toggleButtons;

		public ListBoxConfiguration ListBoxConfiguration { get; set; }
		public bool MultiSelection { get; set; }

		public ScrollBarConfiguration ScrollBarConfiguration
		{
			private get => this.scrollBar.ScrollBarConfiguration;
			set
			{
				this.scrollBar.ScrollBarConfiguration = value;
				this.scrollBar.Size = new Size(
					MathF.Max(value.ButtonsSize.X, value.CursorSize.X),
					MathF.Max(value.ButtonsSize.Y, value.CursorSize.Y));
			}
		}

		public IEnumerable<object> SelectedItems => this.toggleButtons.Where(tb => tb.Toggled).Select(tb => tb.Tag);

		public TextConfiguration TextConfiguration
		{
			get => this.textConfiguration;
			set
			{
				this.textConfiguration = value;
				foreach (ToggleButton tb in this.toggleButtons)
				{
					tb.ApplySkin(this.baseSkin);
					tb.TextConfiguration = this.textConfiguration.Clone();
				}
			}
		}

		// Delegates
		public delegate void SelectionChangeDelegate(ListBox listBox);

		// Events
		[DontSerialize]
		private SelectionChangeDelegate onSelectionChange;
		public event SelectionChangeDelegate OnSelectionChange
		{
			add { this.onSelectionChange += value; }
			remove { this.onSelectionChange -= value; }
		}

		public ListBox(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			this.toggleButtons = new List<ToggleButton>();
			this.ApplySkin(this.baseSkin);
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);

			if (this.scrollBar != null)
			{
				this.scrollBar.ApplySkin(this.baseSkin);

				ListBoxTemplate template = this.baseSkin.GetTemplate<ListBoxTemplate>(this);

				this.ListBoxConfiguration = template.ListBoxConfiguration.Clone();
				this.TextConfiguration = template.TextConfiguration.Clone();
			}
		}

		public override ControlsContainer BuildControl()
		{
			return new DockPanel()
			.Add(this.scrollBar = new VerticalScrollBar(this.baseSkin, this.TemplateName + SCROLLBAR_TEMPLATE)
			{
				Docking = Dock.Right
			})
			.Add(this.stackPanel = new StackPanel(this.baseSkin, this.TemplateName + PANEL_TEMPLATE)
			{
				Docking = Dock.Center,
				Direction = Direction.UpToDown
			});
		}

		public override void Draw(Canvas canvas, float zOffset)
		{
			this.itemsInView = (int)MathF.Floor(this.stackPanel.ControlArea.H / this.ListBoxConfiguration.ItemsSize.Y);

			this.scrollBar.MinValue = 0;
			this.scrollBar.MaxValue = Math.Max(0, this.toggleButtons.Count - this.itemsInView);

			this.scrollBar.Visibility = this.scrollBar.MaxValue == 0 ? ControlVisibility.Collapsed : ControlVisibility.Visible;

			int i = 0;

			foreach (ToggleButton tb in this.toggleButtons)
			{
				if (i < this.scrollBar.Value)
				{ tb.Visibility = ControlVisibility.Collapsed; }
				else if (i < this.scrollBar.Value + this.itemsInView)
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

			this.toggleButtons.Clear();
			this.stackPanel.Clear();

			foreach (object obj in items)
			{
				ToggleButton toggle = new ToggleButton()
				{
					Text = obj.ToString(),
					Tag = obj,
					Visibility = ControlVisibility.Collapsed,
					Toggled = selectedItems.Contains(obj),
					Size = this.ListBoxConfiguration.ItemsSize,
					TextConfiguration = this.TextConfiguration
				};

				toggle.OnToggleChange += (button, wasToggled, isToggled) =>
				{
					if (isToggled && !this.MultiSelection)
					{
						foreach (ToggleButton tb in this.toggleButtons.Where(tb => tb != button))
						{ tb.Toggled = false; }
					}

					if (button == toggle)
						this.onSelectionChange?.Invoke(this);
				};

				toggle.ApplySkin(this.baseSkin);

				this.toggleButtons.Add(toggle);
				this.stackPanel.Add(toggle);
			}

			this.OnUpdate(0);
		}
	}
}
