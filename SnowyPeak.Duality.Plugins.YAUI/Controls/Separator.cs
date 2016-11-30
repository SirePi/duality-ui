// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI.Controls
{
	public sealed class Separator : Control
	{
		public Separator(Skin skin = null, string templateName = null)
			: base(skin, templateName)
		{
			ApplySkin(_baseSkin);
		}
	}
}