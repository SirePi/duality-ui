using Duality;
using Duality.Drawing;
using Duality.Resources;
using SnowyPeak.DualityUI.Controls;
using SnowyPeak.DualityUI.Controls.Configuration;
using SnowyPeak.DualityUI.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.DefaultSkins
{
    public sealed class Light : Skin
    {
        public static readonly ColorRgba COLOR_BASE = new ColorRgba(19, 26, 42);
        public static readonly ColorRgba COLOR_HIGHLIGHT = new ColorRgba(32, 40, 61);
        public static readonly ColorRgba COLOR_ACCENT = new ColorRgba(192, 67, 19);
        public static readonly ColorRgba COLOR_DULL = new ColorRgba(19, 19, 26);

        protected override void Initialize()
        {
            Assembly embeddingAssembly = typeof(Skin).GetTypeInfo().Assembly;

            ContentRef<Font> fntOpenSans = ResourceHelper.LoadFont(embeddingAssembly, "SnowyPeak.DualityUI.DefaultSkins.OpenSans.Font.res");
            ContentRef<Pixmap> pxRound = ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.DualityUI.DefaultSkins.round.png");
            ContentRef<Pixmap> pxSquare = ResourceHelper.LoadPixmap(embeddingAssembly, "SnowyPeak.DualityUI.DefaultSkins.square.png");

            ContentRef<Texture> txRound = new Texture(pxRound);
            ContentRef<Texture> txSquare = new Texture(pxSquare);

            ContentRef<Material> matRoundWhite = new Material(DrawTechnique.Mask, ColorRgba.White, txRound);
            ContentRef<Material> matRoundNormal = new Material(DrawTechnique.Mask, COLOR_BASE, txRound);
            ContentRef<Material> matRoundHover = new Material(DrawTechnique.Mask, COLOR_HIGHLIGHT, txRound);
            ContentRef<Material> matRoundActive = new Material(DrawTechnique.Mask, COLOR_ACCENT, txRound);
            ContentRef<Material> matRoundDisabled = new Material(DrawTechnique.Mask, COLOR_DULL, txRound);

            ContentRef<Material> matSquareWhite = new Material(DrawTechnique.Mask, ColorRgba.White, txSquare);
            ContentRef<Material> matSquareNormal = new Material(DrawTechnique.Mask, COLOR_BASE, txSquare);
            ContentRef<Material> matSquareHover = new Material(DrawTechnique.Mask, COLOR_HIGHLIGHT, txSquare);
            ContentRef<Material> matSquareActive = new Material(DrawTechnique.Mask, COLOR_ACCENT, txSquare);
            ContentRef<Material> matSquareDisabled = new Material(DrawTechnique.Mask, COLOR_DULL, txSquare);

            // Preparing Appearances
            Appearance scrollBarAppearance = new Appearance()
            {
                Border = new Border(6),
                Normal = matRoundNormal,
                Hover = matRoundHover,
                Active = matRoundActive,
                Disabled = matRoundDisabled
            };

            Appearance whiteSquareAppearance = new Appearance()
             {
                 Border = Border.Zero,
                 Normal = matSquareWhite,
                 Hover = matSquareWhite,
                 Active = matSquareWhite,
                 Disabled = matSquareWhite
             };

            Appearance whiteRoundAppearance = new Appearance()
            {
                Border = new Border(8),
                Normal = matRoundWhite,
                Hover = matRoundWhite,
                Active = matRoundWhite,
                Disabled = matRoundWhite
            };

            Appearance buttonAppearance = new Appearance()
            {
                Border = new Border(2),
                Active = matSquareActive,
                Disabled = matSquareDisabled,
                Hover = matSquareHover,
                Normal = matSquareNormal
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

            ControlTemplate panelTemplate = new ControlTemplate()
            {
                Appearance = whiteSquareAppearance
            };

            TextTemplate buttonTemplate = new TextTemplate()
            {
                Appearance = buttonAppearance,
                TextConfiguration = new TextConfiguration()
                {
                    Alignment = Alignment.Center,
                    Color = ColorRgba.White,
                    Font = fntOpenSans,
                    Margin = new Border(2)
                }
            };
            AddDefaultTemplate(typeof(Button), buttonTemplate);
            AddDefaultTemplate(typeof(ToggleButton), buttonTemplate);

            ScrollBarTemplate scrollBarTemplate = new ScrollBarTemplate()
            {
                Appearance = whiteRoundAppearance,
                MinSize = new Size(18),
                ScrollBarMargin = new Border(1),
                ScrollBarConfiguration = new ScrollBarConfiguration()
                {
                    ButtonDecreaseAppearance = scrollBarAppearance,
                    ButtonIncreaseAppearance = scrollBarAppearance,
                    CursorAppearance = scrollBarAppearance,
                    ButtonsSize = new Size(16),
                    CursorSize = new Size(12, 36)
                }
            };
            AddDefaultTemplate(typeof(ScrollBar), scrollBarTemplate);

            ListBoxTemplate listBoxTemplate = new ListBoxTemplate()
            {
                Appearance = whiteSquareAppearance,
                ListBoxConfiguration = new ListBoxConfiguration()
                {
                    ItemAppearance = buttonAppearance,
                    ItemsSize = new Size(20)
                },
                TextConfiguration = buttonTemplate.TextConfiguration,
                ListBoxMargin = new Border(2)
            };
            AddDefaultTemplate(typeof(ListBox), listBoxTemplate);
        }
    }
}