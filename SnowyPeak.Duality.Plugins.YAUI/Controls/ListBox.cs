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
	public sealed class ListBox : CompositeControl<ListBoxTemplate>
	{
		public const string PANEL_TEMPLATE = ".Panel";
		public const string SCROLLBAR_TEMPLATE = ".ScrollBar";

		private int itemsInView;
		private VerticalScrollBar scrollBar;
		private StackPanel stackPanel;
		private TextConfiguration textConfiguration;
		private List<ToggleButton> toggleButtons;

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
					tb.ApplySkin(this.skin);
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
		{ }

		protected override void Init()
		{
			base.Init();
		
			this.toggleButtons = new List<ToggleButton>();
		}

		public override void ApplySkin(Skin skin)
		{
			base.ApplySkin(skin);

			if (this.scrollBar != null)
			{
				this.scrollBar.ApplySkin(this.skin);

				this.ListBoxConfiguration = this.Template.ListBoxConfiguration.Clone();
				this.TextConfiguration = this.Template.TextConfiguration.Clone();
			}
		}

		public override ControlsContainer BuildControl()
		{
			this.scrollBar = new VerticalScrollBar(this.skin, this.TemplateName + SCROLLBAR_TEMPLATE);
			this.scrollBar.Docking = Dock.Right;
			this.stackPanel = new StackPanel(this.skin, this.TemplateName + PANEL_TEMPLATE);
			this.stackPanel.Docking = Dock.Center;
			this.stackPanel.Direction = Direction.UpToDown;

			return new DockPanel(this.skin)
				.Add(this.scrollBar)
				.Add(this.stackPanel);
		}

		protected override void _Draw(Canvas canvas, float zOffset)
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

			base._Draw(canvas, zOffset);
		}

		public void SetItems(IEnumerable<object> items)
		{
			object[] selectedItems = this.SelectedItems.ToArray();

			this.toggleButtons.Clear();
			this.stackPanel.Clear();

			foreach (object obj in items)
			{
				ToggleButton toggle = new ToggleButton(this.skin);
				toggle.Text = obj.ToString();
				toggle.Tag = obj;
				toggle.Visibility = ControlVisibility.Collapsed;
				toggle.Toggled = selectedItems.Contains(obj);
				toggle.Size = this.ListBoxConfiguration.ItemsSize;
				toggle.TextConfiguration = this.TextConfiguration;

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

				this.toggleButtons.Add(toggle);
				this.stackPanel.Add(toggle);
			}

			this.OnUpdate(0);
		}
	}
}
