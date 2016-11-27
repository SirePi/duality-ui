using Duality;
using Duality.Drawing;
using Duality.Input;
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
	public class TestUI : UI
	{
		[DontSerialize]
		private DockPanel _root;
		[DontSerialize]
		private StackPanel _sp;
		[DontSerialize]
		private CheckButton _check;
		[DontSerialize]
		private Button _btn;
		[DontSerialize]
		private ListBox _list;

		protected override ControlsContainer CreateUI()
		{
			_root = new DockPanel()
			{
				Size = new Size(500, 500),
				Position = new Vector2(200, 100)
			};

			_sp = new StackPanel()
			{
				Size = new Size(100, 100),
				Docking = DockPanel.Dock.Left,
				Orientation = Orientation.Vertical,
				Margin = new Border(5)
			};
			_sp.Add(new RadioButton()
			{
				Size = new Size(50, 50),
				Docking = DockPanel.Dock.Bottom,
				RadioGroup = "Group"
			});
			_sp.Add(new Separator()
			{
				Size = new Size(5)
			});
			_sp.Add(new RadioButton()
			{
				Size = new Size(50, 50),
				Docking = DockPanel.Dock.Bottom,
				RadioGroup = "Group"
			});
			_sp.Add(new Separator()
			{
				Size = new Size(5)
			});
			_sp.Add(new RadioButton()
			{
				Size = new Size(50, 50),
				Docking = DockPanel.Dock.Bottom,
				RadioGroup = "Group"
			});
			_sp.Add(new Button()
			{
				Size = new Size(20, 20),
				Docking = DockPanel.Dock.Bottom,
				StretchToFill = false
			});
			_sp.Add(new Separator()
			{
				Size = new Size(5)
			});
			_sp.Add(new Button()
			{
				Size = new Size(50, 50),
				Docking = DockPanel.Dock.Bottom,
				Status = Control.ControlStatus.Disabled
			});

			DockPanel dp = new DockPanel()
			{
				Size = new Size(200, 100),
				Docking = DockPanel.Dock.Left
			};
			dp.Add(new VerticalScrollBar()
			{
				Docking = DockPanel.Dock.Left
			});
			dp.Add(_sp);
			dp.Add(new Button()
			{
				Size = new Size(50, 50),
				Docking = DockPanel.Dock.Right
			});

			GridPanel gp = new GridPanel()
			{
				Docking = DockPanel.Dock.Center,
				Margin = new Border(4),
				Rows = new float[] { .5f, .5f },
				Columns = new float[] { .2f, .6f, .2f }
			};
			gp.Add(new ToggleButton());
			gp.Add(_check = new CheckButton()
			{
				Size = new Size(100, 100),
				Cell = new GridPanel.Cell() { Row = 1, ColSpan = 2 },
				StretchToFill = false,
				Text = "ClickMe!"
			});
			gp.Add(new Button()
			{
				Cell = new GridPanel.Cell() { Row = 1, Column = 2 }
			});

			_root.Add(_btn = new Button()
			{
				Size = new Size(20, 100),
				Docking = DockPanel.Dock.Top,
				StretchToFill = false,
				MouseButtonEventHandler = (button, args) =>
					{
						if (args.Button == MouseButton.Left && args.IsPressed)
						{
							Log.Game.Write("CLICK!");
						}
					}
			});
			_btn.MouseButtonEventHandler += (button, args) =>
				{
					if(args.IsPressed)
					{
						if (args.Button == MouseButton.Right)
						{
							List<object> l = new List<object>();
							for(int i = 0; i < MathF.Rnd.Next(3, 10); i++)
							{
								l.Add(MathF.Rnd.NextVector2(100));
							}
							_list.SetItems(l);
						}
						else
						{
							Log.Game.Write("MultiCastDelegates! {0}", args.Button);
						}
					}
				};


            ProgressBar pbL2R = new ProgressBar()
            {
                Docking = DockPanel.Dock.Bottom,
                Size = new Size(30),
                UpdateHandler = (cnt, msFrame) =>
                {
                    ProgressBar pb = cnt as ProgressBar;

                    pb.Value += (msFrame / 10000);
                    if (pb.Value == 1) pb.Value = 0;
                    pb.Text = String.Format("{0:0}/100", pb.Value * 100);
                }
            };

            ProgressBar pbR2L = new ProgressBar()
            {
                Docking = DockPanel.Dock.Bottom,
                Size = new Size(30),
                UpdateHandler = (cnt, msFrame) =>
                {
                    ProgressBar pb = cnt as ProgressBar;

                    pb.Value += (msFrame / 2000);
                    if (pb.Value == 1) pb.Value = 0;
                    pb.Text = String.Format("{0:0}/100", pb.Value * 100);
                }
            };

            pbR2L.ProgressConfiguration.Direction = ProgressBar.Direction.RightToLeft;

            ProgressBar pbU2D = new ProgressBar()
            {
                Docking = DockPanel.Dock.Bottom,
                Size = new Size(30),
                UpdateHandler = (cnt, msFrame) =>
                {
                    ProgressBar pb = cnt as ProgressBar;

                    pb.Value += (msFrame / 5000);
                    if (pb.Value == 1) pb.Value = 0;
                    pb.Text = String.Format("{0:0}/100", pb.Value * 100);
                }
            };

            pbU2D.ProgressConfiguration.Direction = ProgressBar.Direction.UpToDown;

            ProgressBar pbD2U = new ProgressBar()
            {
                Docking = DockPanel.Dock.Bottom,
                Size = new Size(30),
                UpdateHandler = (cnt, msFrame) =>
                {
                    ProgressBar pb = cnt as ProgressBar;

                    pb.Value += (msFrame / 15000);
                    if (pb.Value == 1) pb.Value = 0;
                    pb.Text = String.Format("{0:0}/100", pb.Value * 100);
                }
            };

            pbD2U.ProgressConfiguration.Direction = ProgressBar.Direction.DownToUp;

            GridPanel progressGrid = new GridPanel()
            {
                Rows = new float[] { .5f, .5f },
                Columns = new float[] { .5f, .5f },
                Size = new Size(60),
                Docking = DockPanel.Dock.Bottom
            };

            pbL2R.Cell = new GridPanel.Cell() { Column = 0, Row = 0 };
            pbR2L.Cell = new GridPanel.Cell() { Column = 1, Row = 1 };
            pbU2D.Cell = new GridPanel.Cell() { Column = 1, Row = 0 };
            pbD2U.Cell = new GridPanel.Cell() { Column = 0, Row = 1 };

            progressGrid
                .Add(pbL2R)
                .Add(pbR2L)
                .Add(pbU2D)
                .Add(pbD2U);

			_root
			.Add(dp)
			.Add(new TextBox()
			{
				Size = new Size(200, 50),
				Docking = DockPanel.Dock.Bottom
			})
			.Add(new HorizontalScrollBar()
			{
				Docking = DockPanel.Dock.Bottom
			})
            .Add(progressGrid)
			.Add(_list = new ListBox()
			{
				Size = new Size(200, 100),
				Docking = DockPanel.Dock.Right,
				StretchToFill = false,
				MultiSelection = true
			})
			.Add(gp);

			_list.SetItems(new object[] { "1", "2", "3", "4", "5", "6", "7", "8" });

			return _root;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			_sp.Visibility = _check.Checked ? Control.ControlVisibility.Visible : Control.ControlVisibility.Collapsed;
		}
	}
}
