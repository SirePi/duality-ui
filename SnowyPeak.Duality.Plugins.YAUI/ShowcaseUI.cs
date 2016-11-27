using Duality;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Controls;
using SnowyPeak.Duality.Plugins.YAUI.Controls.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI
{
    public class ShowcaseUI : UI
    {
        protected override Controls.ControlsContainer CreateUI()
        {
            DockPanel root = new DockPanel();

            // Grid
            GridPanel grid = new GridPanel()
            {
                Docking = DockPanel.Dock.Left,
                Rows = new float[] { .333f, .333f, .333f },
                Columns = new float[] { .5f, .5f },
                Margin = new Border(5),
                Size = new Size(200)
            };

            for (int r = 0; r < grid.Rows.Length; r++)
            {
                for (int c = 0; c < grid.Columns.Length; c++)
                {
                    RadioButton rb = new RadioButton()
                    {
                        RadioGroup = "RadioGroup",
                        Text = String.Format("Cell {0},{1}", r, c),
                        Cell = new GridPanel.Cell()
                        {
                            Row = r,
                            Column = c
                        }
                    };

                    rb.TextConfiguration = rb.TextConfiguration.Clone();
                    rb.TextConfiguration.Alignment = Alignment.Center;
                    rb.GlyphConfiguration = rb.GlyphConfiguration.Clone();
                    rb.GlyphConfiguration.Alignment = Alignment.Top;

                    grid.Add(rb);
                }
            }

            // Vertical Stack
            StackPanel stackV = new StackPanel()
            {
                Docking = DockPanel.Dock.Right,
                Orientation = Orientation.Vertical,
                Margin = new Border(5),
                Size = new Size(200)
            };

            stackV.Add(new Button()
            {
                Text = "Apply Light Skin",
                Size = new Size(30),
                MouseButtonEventHandler = (sender, e) =>
                {
                    root.ApplySkin(Skin.YAUI_ROUNDED);
                }
            });
            stackV.Add(new Separator() { Size = new Size(5) });
            stackV.Add(new Button()
            {
                Text = "Apply Dark Skin",
                Size = new Size(30),
                MouseButtonEventHandler = (sender, e) =>
                {
                    root.ApplySkin(Skin.YAUI_DARK);
                }
            });
            stackV.Add(new Separator() { Size = new Size(5) });
            stackV.Add(new Button()
            {
                Text = "Apply Fathoms Skin",
                Size = new Size(30),
                MouseButtonEventHandler = (sender, e) =>
                {
                    root.ApplySkin(Skin.YAUI_FATHOMS);
                }
            });
            
            stackV.Add(new Separator() { Size = new Size(15) });
            TextBox textBoxDisabled = new TextBox() { 
                Text = "This is disabled.. :(",
                Size = new Size(30) ,
                Status = Control.ControlStatus.Disabled,
            };
            textBoxDisabled.TextConfiguration = textBoxDisabled.TextConfiguration.Clone();
            textBoxDisabled.TextConfiguration.Alignment = Alignment.Right;
            stackV.Add(textBoxDisabled);
            stackV.Add(new Separator() { Size = new Size(5) });
            TextBox textBox = new TextBox() { 
                Text = "This is not :)",
                Size = new Size(30)
            };
            stackV.Add(textBox);
            stackV.Add(new Separator() { Size = new Size(5) });
            stackV.Add(new TextBox()
            {
                Text = "Password!",
                IsPassword = true,
                MaxLength = 15,
                Size = new Size(30),
            });

            stackV.Add(new Separator() { Size = new Size(15) });

            ProgressBar pBar = new ProgressBar()
            {
                Size = new Size(30),
                UpdateHandler = (sender, ms) =>
                {
                    ProgressBar p = sender as ProgressBar;
                    p.Value += (ms / 1000 / 10);
                    if (p.Value == 1) p.Value = 0;

                    p.Text = String.Format("Restart in {0:0}...", (1 - p.Value) * 10);
                }
            };
            pBar.TextConfiguration = pBar.TextConfiguration.Clone();
            pBar.TextConfiguration.Alignment = Alignment.Center;
            stackV.Add(pBar);
            stackV.Add(new Separator() { Size = new Size(15) });
            stackV.Add(new TextBlock()
                {
                    Size = new Size(80),
                    Text = "Text/nNewLine/nAnd Another"
                });
            // Horizontal Stack
            StackPanel stackH = new StackPanel()
            {
                Docking = DockPanel.Dock.Bottom,
                Orientation = Orientation.Horizontal,
                Margin = new Border(5),
                Size = new Size(80)
            };

            for (int i = 0; i < 5; i++)
            {
                ToggleButton b = new ToggleButton()
                {
                    Text = "Toggle/nButton",
                    Size = new Size(120)
                };

                b.TextConfiguration = b.TextConfiguration.Clone();
                b.TextConfiguration.Alignment = Alignment.Right;

                stackH.Add(b);
                stackH.Add(new Separator() { Size = new Size(5) });
            }

            ToggleButton stopAndGo = new ToggleButton()
            {
                Text = "Stop&Go",
                Size = new Size(80)
            };
            stackH.Add(stopAndGo);

            ProgressBar vBar = new ProgressBar()
            {
                Size = new Size(40),
                UpdateHandler = (sender, ms) =>
                {
                    if (stopAndGo.Toggled)
                    {
                        ProgressBar p = sender as ProgressBar;
                        p.Value += (ms / 1000 / 8); // takes 8 seconds to fill
                        if (p.Value == 1) p.Value = 0;
                    }
                }
            };
            vBar.ProgressConfiguration = vBar.ProgressConfiguration.Clone();
            vBar.ProgressConfiguration.Direction = ProgressBar.Direction.DownToUp;

            stackH.Add(vBar);

            // Central panel: Canvas w/ scrollbars!
            DockPanel central = new DockPanel();
            CanvasPanel canvas = new CanvasPanel();

            HorizontalScrollBar hScrollBar = new HorizontalScrollBar()
            {
                ValueChangedEventHandler = (sender, e) => {
                    canvas.Offset.X = -(sender.Value * 2);
                }
            };

            VerticalScrollBar vScrollBar = new VerticalScrollBar()
            {
                Docking = DockPanel.Dock.Right,
                ValueChangedEventHandler = (sender, e) => {
                    canvas.Offset.Y = -(sender.Value * 2);
                }
            };

            CheckButton funButton = new CheckButton()
            {
                Size = new Size(150, 50),
                Position = new Vector2(100, 200),
                Text = "Checked = fun!",
                UpdateHandler = (sender, ms) => {
                    if((sender as CheckButton).Checked) 
                    {
                        bool growing = Convert.ToBoolean(sender.Tag);

                        stackV.Size.X += (ms * .1f * (growing ? 1 : -1));
                        if (stackV.Size.X > 300) sender.Tag = false;
                        if (stackV.Size.X < 150) sender.Tag = true;
                    }
                }
            };

            canvas.Add(funButton);

            DockPanel cBottom = new DockPanel() { Docking = DockPanel.Dock.Bottom, Size = new Size(20) };
            cBottom.Add(new Separator() { Docking = DockPanel.Dock.Right, Size = new Size(20) });
            cBottom.Add(hScrollBar);

            central
                .Add(cBottom)
                .Add(vScrollBar)
                .Add(canvas);

            return root.Add(stackH)
                .Add(grid)
                .Add(stackV)
                .Add(central);
        }
    }
}
