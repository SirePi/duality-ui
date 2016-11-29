using Duality;
using Duality.Drawing;
using Duality.Input;
using Duality.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
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

        public RadioButton(Skin skin = null, string templateName = null)
            : base(skin, templateName)
        {
            this.MouseButtonEventHandler = (button, args) =>
            {
                if (args.Button == MouseButton.Left && args.IsPressed)
                {
                    foreach (RadioButton rb in UIHelper.GetRadioButtonsInGroup(this.RadioGroup))
                    {
                        rb.Checked = false;
                    }

                    this.Checked = true;
                }
            };

            ApplySkin(_baseSkin);
        }
    }
}