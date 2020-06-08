// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality.Input;

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
					rb.Checked = false;

				this.Checked = true;
			}
		}
	}
}
