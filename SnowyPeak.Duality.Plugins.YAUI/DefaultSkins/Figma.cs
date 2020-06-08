// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Controls;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using SnowyPeak.Duality.Plugins.YAUI.Templates;

namespace SnowyPeak.Duality.Plugins.YAUI.DefaultSkins
{
	public sealed class Figma : Skin
	{
		public static readonly ColorRgba COLOR_ACCENT = new ColorRgba(133, 141, 142);
		public static readonly ColorRgba COLOR_BACKGROUND = new ColorRgba(5, 5, 5);
		public static readonly ColorRgba COLOR_BRIGHT = new ColorRgba(45, 45, 45);
		public static readonly ColorRgba COLOR_CONTROL = new ColorRgba(30, 30, 30);
		public static readonly ColorRgba COLOR_DULL = new ColorRgba(20, 20, 20);
		public static readonly ColorRgba COLOR_HIGHLIGHT = new ColorRgba(53, 60, 65);

		protected override void Initialize()
		{
			ContentRef<Font> fntFont = ResourceHelper.LoadFont(YAUICorePlugin.YAUIAssembly, Fonts.ManropeRegular);

			this.AddDefaultTemplate(typeof(ControlsContainer), ControlTemplate.Empty);
			this.AddDefaultTemplate(typeof(CanvasPanel), ControlTemplate.Empty);
			this.AddDefaultTemplate(typeof(DockPanel), ControlTemplate.Empty);
			this.AddDefaultTemplate(typeof(GridPanel), ControlTemplate.Empty);
			this.AddDefaultTemplate(typeof(StackPanel), ControlTemplate.Empty);

			// layout components
			ContentRef<Material> matForm = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.form.png"));
			ControlTemplate formTemplate = new ControlTemplate
			{
				Appearance = new Appearance
				{
					Active = matForm,
					Disabled = matForm,
					Hover = matForm,
					Normal = matForm,
					Border = new Border(20)
				},
				MinSize = new Size(40)
			};
			this.AddCustomTemplate("form", formTemplate);

			TextTemplate buttonTemplate = new TextTemplate
			{
				Appearance = new Appearance
				{
					Active = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.button-active.png")),
					Disabled = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.button-disabled.png")),
					Hover = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.button-hover.png")),
					Normal = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.button-normal.png")),
					Border = new Border(5)
				},
				MinSize = new Size(32, 16),
				TextConfiguration = new TextConfiguration
				{
					Font = fntFont,
					Alignment = Alignment.Center,
					Color = ColorRgba.White,
					Margin = new Border(5)
				}
			};
			this.AddDefaultTemplate(typeof(Button), buttonTemplate);
			this.AddDefaultTemplate(typeof(ToggleButton), buttonTemplate);

			GlyphTemplate checkTemplate = new GlyphTemplate(buttonTemplate)
			{
				GlyphConfiguration = new GlyphConfiguration
				{
					Alignment = Alignment.Left,
					Margin = new Border(5),
					Glyph = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.glyph-check-active.png"))
				}
			};
			this.AddDefaultTemplate(typeof(CheckButton), checkTemplate);

			GlyphTemplate radioTemplate = new GlyphTemplate(buttonTemplate)
			{
				GlyphConfiguration = new GlyphConfiguration
				{
					Alignment = Alignment.Left,
					Margin = new Border(5),
					Glyph = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.glyph-radio-active.png"))
				}
			};
			this.AddDefaultTemplate(typeof(RadioButton), radioTemplate);

			TextTemplate textTemplate = new TextTemplate(buttonTemplate);
			this.AddDefaultTemplate(typeof(TextBox), textTemplate);
			this.AddDefaultTemplate(typeof(TextBlock), textTemplate);

			ContentRef<Material> scrollbarHBaseMaterial = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-horizontal-normal.png"));
			scrollbarHBaseMaterial.Res.Technique = DrawTechnique.Alpha;
			ContentRef<Material> scrollbarVBaseMaterial = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-vertical-normal.png"));
			scrollbarVBaseMaterial.Res.Technique = DrawTechnique.Alpha;

			ScrollBarTemplate scrollHTemplate = new ScrollBarTemplate
			{
				Appearance = new Appearance
				{
					Normal = scrollbarHBaseMaterial,
					Hover = scrollbarHBaseMaterial,
					Active = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-horizontal-active.png")),
					Disabled = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-horizontal-disabled.png")),
					Border = Border.Zero
				},
				MinSize = new Size(16),
				ScrollBarConfiguration = new ScrollBarConfiguration
				{
					CursorAppearance = new Appearance
					{
						Normal = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-cursor-normal.png")),
						Hover = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-cursor-hover.png")),
						Active = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-cursor-active.png")),
						Disabled = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-cursor-disabled.png")),
						Border = Border.Zero
					},
					ButtonDecreaseAppearance = null,
					ButtonIncreaseAppearance = null,
					ButtonsSize = Size.Zero,
					CursorSize = new Size(16)
				}
			};
			this.AddDefaultTemplate(typeof(HorizontalScrollBar), scrollHTemplate);

			ScrollBarTemplate scrollVTemplate = new ScrollBarTemplate
			{
				Appearance = new Appearance
				{
					Normal = scrollbarVBaseMaterial,
					Hover = scrollbarVBaseMaterial,
					Active = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-vertical-active.png")),
					Disabled = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-vertical-disabled.png")),
					Border = Border.Zero
				},
				MinSize = new Size(16),
				ScrollBarConfiguration = new ScrollBarConfiguration
				{
					CursorAppearance = new Appearance
					{
						Normal = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-cursor-normal.png")),
						Hover = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-cursor-hover.png")),
						Active = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-cursor-active.png")),
						Disabled = ResourceHelper.GetMaterial(YAUICorePlugin.YAUIAssembly, Skin.Path("Figma.scrollbar-cursor-disabled.png")),
						Border = Border.Zero
					},
					ButtonDecreaseAppearance = null,
					ButtonIncreaseAppearance = null,
					ButtonsSize = Size.Zero,
					CursorSize = new Size(16)
				}
			};
			this.AddDefaultTemplate(typeof(VerticalScrollBar), scrollVTemplate);

			ListBoxTemplate listTemplate = new ListBoxTemplate
			{
				Appearance = null,
				ListBoxConfiguration = new ListBoxConfiguration
				{
					ItemAppearance = textTemplate.Appearance,
					ItemsSize = new Size(20)
				},
				ListBoxMargin = new Border(5),
				MinSize = new Size(20),
				TextConfiguration = textTemplate.TextConfiguration
			};
			this.AddDefaultTemplate(typeof(ListBox), listTemplate);
		}
	}
}