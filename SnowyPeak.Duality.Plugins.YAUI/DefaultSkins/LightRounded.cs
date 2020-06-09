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
	public sealed class LightRounded : Skin
	{
		public static readonly ColorRgba COLOR_ACCENT = new ColorRgba(126, 144, 191);
		public static readonly ColorRgba COLOR_BACKGROUND = new ColorRgba(130, 130, 130);
		public static readonly ColorRgba COLOR_BRIGHT = new ColorRgba(214, 214, 214);
		public static readonly ColorRgba COLOR_CONTROL = new ColorRgba(163, 163, 163);
		public static readonly ColorRgba COLOR_DULL = new ColorRgba(140, 140, 140);
		public static readonly ColorRgba COLOR_HIGHLIGHT = new ColorRgba(220, 220, 220);

		protected override void Initialize()
		{
			Assembly embeddingAssembly = typeof(Skin).GetTypeInfo().Assembly;

			ContentRef<Font> fntFont = ResourceHelper.LoadFont(embeddingAssembly, Fonts.NotoSansRegular);
			ContentRef<Pixmap> pxRound = ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.round.png");

			ContentRef<Texture> txRound = new Texture(pxRound);

			ContentRef<Material> matRoundWhite = new Material(DrawTechnique.Mask, ColorRgba.White, txRound);
			ContentRef<Material> matRoundBackground = new Material(DrawTechnique.Mask, COLOR_BACKGROUND, txRound);
			ContentRef<Material> matRoundNormal = new Material(DrawTechnique.Mask, COLOR_CONTROL, txRound);
			ContentRef<Material> matRoundHover = new Material(DrawTechnique.Mask, COLOR_HIGHLIGHT, txRound);
			ContentRef<Material> matRoundActive = new Material(DrawTechnique.Mask, COLOR_ACCENT, txRound);
			ContentRef<Material> matRoundDisabled = new Material(DrawTechnique.Mask, COLOR_DULL, txRound);

			// Preparing Appearances
			Appearance scrollBarAppearance = new Appearance()
			{
				Border = new Border(6),
				Normal = matRoundNormal,
				Hover = matRoundHover,
				Active = matRoundActive,
				Disabled = matRoundDisabled
			};

			Appearance backgroundAppearance = new Appearance()
			{
				Border = new Border(6),
				Normal = matRoundBackground,
				Hover = matRoundBackground,
				Active = matRoundBackground,
				Disabled = matRoundBackground
			};

			Appearance baseAppearance = new Appearance()
			{
				Border = new Border(6),
				Active = matRoundNormal,
				Disabled = matRoundDisabled,
				Hover = matRoundNormal,
				Normal = matRoundNormal
			};

			Appearance buttonAppearance = new Appearance()
			{
				Border = new Border(6),
				Active = matRoundActive,
				Disabled = matRoundDisabled,
				Hover = matRoundHover,
				Normal = matRoundNormal
			};

			ControlTemplate emptyTemplate = new ControlTemplate()
			{
				Appearance = null
			};
			this.AddDefaultTemplate(typeof(Separator), emptyTemplate);
			this.AddDefaultTemplate(typeof(ControlsContainer), emptyTemplate);
			this.AddDefaultTemplate(typeof(CanvasPanel), emptyTemplate);
			this.AddDefaultTemplate(typeof(DockPanel), emptyTemplate);
			this.AddDefaultTemplate(typeof(GridPanel), emptyTemplate);
			this.AddDefaultTemplate(typeof(StackPanel), emptyTemplate);

			TextTemplate baseTemplate = new TextTemplate()
			{
				Appearance = baseAppearance,
				MinSize = new Size(20),
				TextConfiguration = new TextConfiguration(
					font: fntFont,
					color: ColorRgba.White,
					alignment: Alignment.Left,
					margin: new Border(5)
				)
			};
			this.AddDefaultTemplate(typeof(TextBlock), baseTemplate);
			this.AddDefaultTemplate(typeof(SpriteBox), baseTemplate);

			TextTemplate buttonTemplate = new TextTemplate()
			{
				Appearance = buttonAppearance,
				MinSize = new Size(20),
				TextConfiguration = new TextConfiguration(
					font: fntFont,
					color: ColorRgba.White,
					alignment: Alignment.Right,
					margin: new Border(5)
				)
			};
			this.AddDefaultTemplate(typeof(Button), buttonTemplate);
			this.AddDefaultTemplate(typeof(ToggleButton), buttonTemplate);
			this.AddDefaultTemplate(typeof(TextBox), buttonTemplate);

			GlyphTemplate glyphTemplate = new GlyphTemplate()
			{
				Appearance = buttonAppearance,
				MinSize = new Size(20),
				GlyphConfiguration = new GlyphConfiguration(
					glyph: matRoundWhite,
					alignment: Alignment.Right,
					margin: new Border(5)
				),
				TextConfiguration = buttonTemplate.TextConfiguration
			};
			this.AddDefaultTemplate(typeof(CheckButton), glyphTemplate);
			this.AddDefaultTemplate(typeof(RadioButton), glyphTemplate);

			ScrollBarTemplate vScrollBarTemplate = new ScrollBarTemplate()
			{
				Appearance = backgroundAppearance,
				MinSize = new Size(20),
				ScrollBarMargin = new Border(2),
				ScrollBarConfiguration = new ScrollBarConfiguration(
					buttonIncreaseAppearance: scrollBarAppearance,
					buttonDecreaseAppearance: scrollBarAppearance,
					cursorAppearance: scrollBarAppearance,
					buttonsSize: new Size(16),
					cursorSize: new Size(16, 32)
				)
			};
			this.AddDefaultTemplate(typeof(VerticalScrollBar), vScrollBarTemplate);

			ScrollBarTemplate hScrollBarTemplate = new ScrollBarTemplate()
			{
				Appearance = backgroundAppearance,
				MinSize = new Size(20),
				ScrollBarMargin = new Border(2),
				ScrollBarConfiguration = new ScrollBarConfiguration(
					buttonIncreaseAppearance: scrollBarAppearance,
					buttonDecreaseAppearance: scrollBarAppearance,
					cursorAppearance: scrollBarAppearance,
					buttonsSize: new Size(16),
					cursorSize: new Size(32, 16)
				)
			};
			this.AddDefaultTemplate(typeof(HorizontalScrollBar), hScrollBarTemplate);

			ListBoxTemplate listBoxTemplate = new ListBoxTemplate()
			{
				Appearance = backgroundAppearance,
				MinSize = new Size(4),
				ListBoxConfiguration = new ListBoxConfiguration(
					itemAppearance: buttonAppearance,
					itemsSize: new Size(20)
				),
				TextConfiguration = buttonTemplate.TextConfiguration,
				ListBoxMargin = new Border(2)
			};
			this.AddDefaultTemplate(typeof(ListBox), listBoxTemplate);

			ProgressTemplate progressTemplate = new ProgressTemplate()
			{
				Appearance = backgroundAppearance,
				MinSize = new Size(15),
				ProgressConfiguration = new ProgressConfiguration(
					barAppearance: buttonAppearance,
					direction: Direction.LeftToRight,
					style: ProgressBar.BarStyle.Cutoff,
					margin: new Border(5)
				),
				TextConfiguration = new TextConfiguration(
					font: fntFont, 
					color: ColorRgba.DarkGrey, 
					alignment: Alignment.Center
				)
			};
			this.AddDefaultTemplate(typeof(ProgressBar), progressTemplate);
		}
	}
}
