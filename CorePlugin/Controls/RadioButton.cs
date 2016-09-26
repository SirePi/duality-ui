using Duality;
using Duality.Drawing;
using Duality.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.DualityUI.Controls
{
	public class RadioButton : CheckButton
	{
		private string _radioGroup;

        public string RadioGroup 
		{
			get { return _radioGroup; }
			set
			{
				UIHelper.UnregisterRadioButton(this);
				_radioGroup = value;
				UIHelper.RegisterRadioButton(this);
			}
		}

		public RadioButton()
        {
			this.MouseButtonEventHandler = (button, args) =>
			{
				if (args.Button == Duality.Input.MouseButton.Left && args.IsPressed)
				{
					foreach(RadioButton rb in UIHelper.GetRadioButtonsInGroup(this.RadioGroup))
					{
						rb.Checked = false;
					}

					this.Checked = true;
				}
			};
        }
	}
}
