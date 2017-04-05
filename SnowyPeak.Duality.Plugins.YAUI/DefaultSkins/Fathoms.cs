// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Controls;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.DefaultSkins
{
	public sealed class Fathoms : Skin
	{
		public static readonly ColorRgba COLOR_ACCENT = new ColorRgba(200, 200, 200);
		public static readonly ColorRgba COLOR_BACKGROUND = new ColorRgba(0, 0, 13);
		public static readonly ColorRgba COLOR_BRIGHT = new ColorRgba(48, 59, 94);
		public static readonly ColorRgba COLOR_CONTROL = new ColorRgba(26, 26, 64);
		public static readonly ColorRgba COLOR_DULL = new ColorRgba(19, 12, 33);
		public static readonly ColorRgba COLOR_HIGHLIGHT = new ColorRgba(85, 111, 128);

		protected override void Initialize()
		{
			Assembly embeddingAssembly = typeof(Skin).GetTypeInfo().Assembly;

			ContentRef<Font> fntOpenSans = ResourceHelper.LoadFont(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.OpenSans.Font.res");

			ContentRef<Texture> txSquare = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.square.png"));
			ContentRef<Texture> txGlyph = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.glyph.png"));

			ContentRef<Texture> txControlBase = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Fathoms.control_base.png"));
			ContentRef<Texture> txControlDull = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Fathoms.control_dull.png"));
			ContentRef<Texture> txControlHover = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Fathoms.control_hover.png"));
			ContentRef<Texture> txControlActive = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Fathoms.control_active.png"));

			ContentRef<Texture> txScrollBase = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Fathoms.scroll_base.png"));
			ContentRef<Texture> txScrollDull = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Fathoms.scroll_dull.png"));
			ContentRef<Texture> txScrollHover = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Fathoms.scroll_hover.png"));
			ContentRef<Texture> txScrollActive = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Fathoms.scroll_active.png"));

			ContentRef<Material> matSquareWhite = new Material(DrawTechnique.Mask, ColorRgba.White, txSquare);
			ContentRef<Material> matSquareBackground = new Material(DrawTechnique.Mask, COLOR_BACKGROUND, txSquare);
			ContentRef<Material> matSquareNormal = new Material(DrawTechnique.Mask, COLOR_CONTROL, txSquare);
			ContentRef<Material> matSquareHover = new Material(DrawTechnique.Mask, COLOR_HIGHLIGHT, txSquare);
			ContentRef<Material> matSquareActive = new Material(DrawTechnique.Mask, COLOR_BRIGHT, txSquare);
			ContentRef<Material> matSquareDisabled = new Material(DrawTechnique.Mask, COLOR_DULL, txSquare);
			ContentRef<Material> matGlyph = new Material(DrawTechnique.Mask, COLOR_HIGHLIGHT, txGlyph);

			ContentRef<Material> matControlBase = new Material(DrawTechnique.Mask, ColorRgba.White, txControlBase);
			ContentRef<Material> matControlDull = new Material(DrawTechnique.Mask, ColorRgba.White, txControlDull);
			ContentRef<Material> matControlHover = new Material(DrawTechnique.Mask, ColorRgba.White, txControlHover);
			ContentRef<Material> matControlActive = new Material(DrawTechnique.Mask, ColorRgba.White, txControlActive);

			ContentRef<Material> matScrollBase = new Material(DrawTechnique.Mask, ColorRgba.White, txScrollBase);
			ContentRef<Material> matScrollDull = new Material(DrawTechnique.Mask, ColorRgba.White, txScrollDull);
			ContentRef<Material> matScrollHover = new Material(DrawTechnique.Mask, ColorRgba.White, txScrollHover);
			ContentRef<Material> matScrollActive = new Material(DrawTechnique.Mask, ColorRgba.White, txScrollActive);

			// Preparing Appearances
			Appearance scrollBarButtonAppearance = new Appearance()
			{
				Border = new Border(6),
				Normal = matScrollBase,
				Hover = matScrollHover,
				Active = matScrollActive,
				Disabled = matScrollDull
			};

			Appearance backgroundAppearance = new Appearance()
			{
				Border = Border.Zero,
				Normal = matSquareBackground,
				Hover = matSquareBackground,
				Active = matSquareBackground,
				Disabled = matSquareBackground
			};

			Appearance baseAppearance = new Appearance()
			{
				Border = new Border(6),
				Active = matControlBase,
				Disabled = matControlDull,
				Hover = matControlBase,
				Normal = matControlBase
			};

			Appearance buttonAppearance = new Appearance()
			{
				Border = new Border(6),
				Normal = matControlBase,
				Hover = matControlHover,
				Active = matControlActive,
				Disabled = matControlDull
			};

			ControlTemplate emptyTemplate = new ControlTemplate()
			{
				Appearance = null
			};
			AddDefaultTemplate(typeof(Separator), emptyTemplate);
			AddDefaultTemplate(typeof(ControlsContainer), emptyTemplate);
			AddDefaultTemplate(typeof(CanvasPanel), emptyTemplate);
			AddDefaultTemplate(typeof(DockPanel), emptyTemplate);
			AddDefaultTemplate(typeof(GridPanel), emptyTemplate);
			AddDefaultTemplate(typeof(StackPanel), emptyTemplate);

			TextTemplate baseTemplate = new TextTemplate()
			{
				Appearance = baseAppearance,
				MinSize = new Size(20),
				TextConfiguration = new TextConfiguration()
				{
					Alignment = Alignment.Center,
					Color = COLOR_ACCENT,
					Font = fntOpenSans,
					Margin = new Border(5)
				}
			};
			AddDefaultTemplate(typeof(TextBlock), baseTemplate);
            AddDefaultTemplate(typeof(SpriteBox), baseTemplate);

			TextTemplate buttonTemplate = new TextTemplate()
			{
				Appearance = buttonAppearance,
				MinSize = new Size(20),
				TextConfiguration = new TextConfiguration()
				{
					Alignment = Alignment.Center,
					Color = COLOR_ACCENT,
					Font = fntOpenSans,
					Margin = new Border(5)
				}
			};
			AddDefaultTemplate(typeof(Button), buttonTemplate);
			AddDefaultTemplate(typeof(ToggleButton), buttonTemplate);
			AddDefaultTemplate(typeof(TextBox), buttonTemplate);

			GlyphTemplate glyphTemplate = new GlyphTemplate()
			{
				Appearance = buttonAppearance,
				MinSize = new Size(20),
				GlyphConfiguration = new GlyphConfiguration()
				{
					Glyph = matGlyph,
					Margin = new Border(5),
					Alignment = Alignment.Right
				},
				TextConfiguration = buttonTemplate.TextConfiguration
			};
			AddDefaultTemplate(typeof(CheckButton), glyphTemplate);
			AddDefaultTemplate(typeof(RadioButton), glyphTemplate);

			ScrollBarTemplate vScrollBarTemplate = new ScrollBarTemplate()
			{
				Appearance = backgroundAppearance,
				MinSize = new Size(18),
				ScrollBarMargin = new Border(1),
				ScrollBarConfiguration = new ScrollBarConfiguration()
				{
					ButtonDecreaseAppearance = scrollBarButtonAppearance,
					ButtonIncreaseAppearance = scrollBarButtonAppearance,
					CursorAppearance = buttonAppearance,
					ButtonsSize = new Size(16),
					CursorSize = new Size(16, 24)
				}
			};
			AddDefaultTemplate(typeof(VerticalScrollBar), vScrollBarTemplate);

			ScrollBarTemplate hScrollBarTemplate = new ScrollBarTemplate()
			{
				Appearance = backgroundAppearance,
				MinSize = new Size(18),
				ScrollBarMargin = new Border(1),
				ScrollBarConfiguration = new ScrollBarConfiguration()
				{
					ButtonDecreaseAppearance = scrollBarButtonAppearance,
					ButtonIncreaseAppearance = scrollBarButtonAppearance,
					CursorAppearance = buttonAppearance,
					ButtonsSize = new Size(16),
					CursorSize = new Size(24, 16)
				}
			};
			AddDefaultTemplate(typeof(HorizontalScrollBar), hScrollBarTemplate);

			ListBoxTemplate listBoxTemplate = new ListBoxTemplate()
			{
				Appearance = backgroundAppearance,
				MinSize = new Size(4),
				ListBoxConfiguration = new ListBoxConfiguration()
				{
					ItemAppearance = buttonAppearance,
					ItemsSize = new Size(20)
				},
				TextConfiguration = buttonTemplate.TextConfiguration,
				ListBoxMargin = new Border(2)
			};
			AddDefaultTemplate(typeof(ListBox), listBoxTemplate);

			ProgressTemplate progressTemplate = new ProgressTemplate()
			{
				Appearance = backgroundAppearance,
				MinSize = new Size(15),
				ProgressConfiguration = new ProgressConfiguration()
				{
					BarAppearance = buttonAppearance,
					BarStyle = ProgressBar.BarStyle.Cutoff,
					Direction = Direction.LeftToRight,
					Margin = new Border(5)
				},
				TextConfiguration = buttonTemplate.TextConfiguration
			};
			AddDefaultTemplate(typeof(ProgressBar), progressTemplate);
		}
	}
}