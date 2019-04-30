// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public sealed class RadioButton : CheckButton
	{
		private string radioGroup;

		public string RadioGroup
		{
			get => this.radioGroup;
			set
			{
				UIHelper.UnregisterRadioButton(this);
				this.radioGroup = value;
				UIHelper.RegisterRadioButton(this);
			}
		}

		public RadioButton(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{ }

		protected override void Init()
		{
			base.Init();
			this.OnMouseButton += this.RadioButton_OnMouseButton;
		}

		private void RadioButton_OnMouseButton(IInteractiveControl button, MouseButtonEventArgs args)
		{
			if (args.Button == MouseButton.Left && args.IsPressed)
			{
				foreach (RadioButton rb in UIHelper.GetRadioButtonsInGroup(this.RadioGroup))
				{ rb.Checked = false; }

				this.Checked = true;
			}
		}
	}
}
