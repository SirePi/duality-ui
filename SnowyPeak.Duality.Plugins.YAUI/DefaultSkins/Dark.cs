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
    public sealed class Dark : Skin
    {
        public static readonly ColorRgba COLOR_BACKGROUND = new ColorRgba(5, 5, 5);
        public static readonly ColorRgba COLOR_CONTROL = new ColorRgba(30, 30, 30);
        public static readonly ColorRgba COLOR_DULL = new ColorRgba(20, 20, 20);
        public static readonly ColorRgba COLOR_HIGHLIGHT = new ColorRgba(53, 60, 65);
        public static readonly ColorRgba COLOR_BRIGHT = new ColorRgba(45, 45, 45);
        public static readonly ColorRgba COLOR_ACCENT = new ColorRgba(133, 141, 142);

        protected override void Initialize()
        {
            Assembly embeddingAssembly = typeof(Skin).GetTypeInfo().Assembly;

            ContentRef<Font> fntOpenSans = ResourceHelper.LoadFont(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.OpenSans.Font.res");

            ContentRef<Texture> txSquare = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.square.png"));
            ContentRef<Texture> txGlyph = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.glyph.png"));

            ContentRef<Texture> txControlBase = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.control_base.png"));
            ContentRef<Texture> txControlDull = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.control_dull.png"));
            ContentRef<Texture> txControlHover = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.control_hover.png"));
            ContentRef<Texture> txControlActive = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.control_active.png"));

            ContentRef<Texture> txLeftBase = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.left_base.png"));
            ContentRef<Texture> txLeftDull = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.left_dull.png"));
            ContentRef<Texture> txLeftHover = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.left_hover.png"));
            ContentRef<Texture> txLeftActive = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.left_active.png"));

            ContentRef<Texture> txRightBase = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.right_base.png"));
            ContentRef<Texture> txRightDull = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.right_dull.png"));
            ContentRef<Texture> txRightHover = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.right_hover.png"));
            ContentRef<Texture> txRightActive = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.right_active.png"));

            ContentRef<Texture> txUpBase = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.up_base.png"));
            ContentRef<Texture> txUpDull = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.up_dull.png"));
            ContentRef<Texture> txUpHover = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.up_hover.png"));
            ContentRef<Texture> txUpActive = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.up_active.png"));

            ContentRef<Texture> txDownBase = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.down_base.png"));
            ContentRef<Texture> txDownDull = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.down_dull.png"));
            ContentRef<Texture> txDownHover = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.down_hover.png"));
            ContentRef<Texture> txDownActive = new Texture(ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.Dark.down_active.png"));

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

            ContentRef<Material> matLeftBase = new Material(DrawTechnique.Mask, ColorRgba.White, txLeftBase);
            ContentRef<Material> matLeftDull = new Material(DrawTechnique.Mask, ColorRgba.White, txLeftDull);
            ContentRef<Material> matLeftHover = new Material(DrawTechnique.Mask, ColorRgba.White, txLeftHover);
            ContentRef<Material> matLeftActive = new Material(DrawTechnique.Mask, ColorRgba.White, txLeftActive);

            ContentRef<Material> matRightBase = new Material(DrawTechnique.Mask, ColorRgba.White, txRightBase);
            ContentRef<Material> matRightDull = new Material(DrawTechnique.Mask, ColorRgba.White, txRightDull);
            ContentRef<Material> matRightHover = new Material(DrawTechnique.Mask, ColorRgba.White, txRightHover);
            ContentRef<Material> matRightActive = new Material(DrawTechnique.Mask, ColorRgba.White, txRightActive);

            ContentRef<Material> matUpBase = new Material(DrawTechnique.Mask, ColorRgba.White, txUpBase);
            ContentRef<Material> matUpDull = new Material(DrawTechnique.Mask, ColorRgba.White, txUpDull);
            ContentRef<Material> matUpHover = new Material(DrawTechnique.Mask, ColorRgba.White, txUpHover);
            ContentRef<Material> matUpActive = new Material(DrawTechnique.Mask, ColorRgba.White, txUpActive);

            ContentRef<Material> matDownBase = new Material(DrawTechnique.Mask, ColorRgba.White, txDownBase);
            ContentRef<Material> matDownDull = new Material(DrawTechnique.Mask, ColorRgba.White, txDownDull);
            ContentRef<Material> matDownHover = new Material(DrawTechnique.Mask, ColorRgba.White, txDownHover);
            ContentRef<Material> matDownActive = new Material(DrawTechnique.Mask, ColorRgba.White, txDownActive);

            // Preparing Appearances
            Appearance scrollBarLeftAppearance = new Appearance()
            {
                Border = new Border(3),
                Normal = matLeftBase,
                Hover = matLeftHover,
                Active = matLeftActive,
                Disabled = matLeftDull
            };

            Appearance scrollBarRightAppearance = new Appearance()
            {
                Border = new Border(3),
                Normal = matRightBase,
                Hover = matRightHover,
                Active = matRightActive,
                Disabled = matRightDull
            };

            Appearance scrollBarUpAppearance = new Appearance()
            {
                Border = new Border(3),
                Normal = matUpBase,
                Hover = matUpHover,
                Active = matUpActive,
                Disabled = matUpDull
            };

            Appearance scrollBarDownAppearance = new Appearance()
            {
                Border = new Border(3),
                Normal = matDownBase,
                Hover = matDownHover,
                Active = matDownActive,
                Disabled = matDownDull
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
                Border = new Border(3),
                Active = matControlBase,
                Disabled = matControlDull,
                Hover = matControlBase,
                Normal = matControlBase
            };

            Appearance buttonAppearance = new Appearance()
            {
                Border = new Border(3),
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
                    Alignment = Alignment.Left,
                    Color = COLOR_ACCENT,
                    Font = fntOpenSans,
                    Margin = new Border(5)
                }
            };
            AddDefaultTemplate(typeof(TextBlock), baseTemplate);

            TextTemplate buttonTemplate = new TextTemplate()
            {
                Appearance = buttonAppearance,
                MinSize = new Size(20),
                TextConfiguration = new TextConfiguration()
                {
                    Alignment = Alignment.Left,
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
                    ButtonDecreaseAppearance = scrollBarUpAppearance,
                    ButtonIncreaseAppearance = scrollBarDownAppearance,
                    CursorAppearance = buttonAppearance,
                    ButtonsSize = new Size(16),
                    CursorSize = new Size(12, 36)
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
                    ButtonDecreaseAppearance = scrollBarLeftAppearance,
                    ButtonIncreaseAppearance = scrollBarRightAppearance,
                    CursorAppearance = buttonAppearance,
                    ButtonsSize = new Size(16),
                    CursorSize = new Size(36, 12)
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
                    Direction = ProgressBar.Direction.LeftToRight,
                    Margin = new Border(5)
                },
                TextConfiguration = buttonTemplate.TextConfiguration
            };
            AddDefaultTemplate(typeof(ProgressBar), progressTemplate);
        }
    }
}