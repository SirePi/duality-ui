// This code is provided under the MIT license. Originally by Alessandro Pilati.
using SnowyPeak.Duality.Plugins.YAUI.Controls;
using System.Collections.Generic;

namespace SnowyPeak.Duality.Plugins.YAUI
{
	internal static class UIHelper
	{
		private static readonly Dictionary<string, List<RadioButton>> radioGroups = new Dictionary<string, List<RadioButton>>();
		private static readonly RadioButton[] emptyGroup = new RadioButton[0];

		internal static IEnumerable<RadioButton> GetRadioButtonsInGroup(string group)
		{
			if (!string.IsNullOrWhiteSpace(group) && radioGroups.ContainsKey(group))
				return radioGroups[group];
			else
				return emptyGroup;
		}

		internal static void RegisterRadioButton(RadioButton radio)
		{
			if (!string.IsNullOrWhiteSpace(radio.RadioGroup))
			{
				if (!radioGroups.ContainsKey(radio.RadioGroup))
					radioGroups.Add(radio.RadioGroup, new List<RadioButton>());

				radioGroups[radio.RadioGroup].Add(radio);
			}
		}

		internal static void UnregisterRadioButton(RadioButton radio)
		{
			if (!string.IsNullOrWhiteSpace(radio.RadioGroup) && radioGroups.ContainsKey(radio.RadioGroup))
				radioGroups[radio.RadioGroup].Remove(radio);
		}
	}
}
