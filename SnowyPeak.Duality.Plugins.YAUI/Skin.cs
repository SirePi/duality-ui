// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Resources;
using SnowyPeak.Duality.Plugins.YAUI.Controls;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI
{
	public abstract class Skin
	{
		public static readonly Skin YAUI_DARK = new DefaultSkins.Dark();
		public static readonly Skin YAUI_FATHOMS = new DefaultSkins.Fathoms();
		public static readonly Skin YAUI_ROUNDED = new DefaultSkins.LightRounded();

		public static Skin DEFAULT = YAUI_DARK;

		private Dictionary<string, ControlTemplate> _customTemplates;
		private Dictionary<Type, ControlTemplate> _defaultTemplates;

		protected Skin()
		{
			_customTemplates = new Dictionary<string, ControlTemplate>();
			_defaultTemplates = new Dictionary<Type, ControlTemplate>();

			Initialize();
		}

		public void AddCustomTemplate(string templateName, ControlTemplate template)
		{
			_customTemplates[templateName] = template;
		}

		public void AddDefaultTemplate(Type type, ControlTemplate template)
		{
			_defaultTemplates[type] = template;
		}

		public T GetTemplate<T>(Control c) where T : ControlTemplate, new()
		{
			T template = null;

			if (_customTemplates.ContainsKey(c.TemplateName)) template = _customTemplates[c.TemplateName] as T;
			if (template == null && _defaultTemplates.ContainsKey(c.GetType())) template = _defaultTemplates[c.GetType()] as T;
			if (template == null) template = new T();

			return template;
		}

		protected abstract void Initialize();
	}
}