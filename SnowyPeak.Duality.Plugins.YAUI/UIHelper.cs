﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.
using SnowyPeak.Duality.Plugins.YAUI.Controls;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI
{
	internal static class UIHelper
	{
		private static Dictionary<string, List<RadioButton>> _radioGroups = new Dictionary<string, List<RadioButton>>();

		internal static IEnumerable<RadioButton> GetRadioButtonsInGroup(string group)
		{
			if (!String.IsNullOrWhiteSpace(group))
			{
				foreach (RadioButton rb in _radioGroups[group])
				{ yield return rb; }
			}
		}

		internal static void RegisterRadioButton(RadioButton radio)
		{
			if (!String.IsNullOrWhiteSpace(radio.RadioGroup))
			{
				if (!_radioGroups.ContainsKey(radio.RadioGroup))
				{ _radioGroups.Add(radio.RadioGroup, new List<RadioButton>()); }

				_radioGroups[radio.RadioGroup].Add(radio);
			}
		}

		internal static void UnregisterRadioButton(RadioButton radio)
		{
			if (!String.IsNullOrWhiteSpace(radio.RadioGroup) && _radioGroups.ContainsKey(radio.RadioGroup))
			{ _radioGroups[radio.RadioGroup].Remove(radio); }
		}
	}
}