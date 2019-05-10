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

		public static Skin DEFAULT { get; set; } = YAUI_DARK;

		private readonly Dictionary<string, ControlTemplate> customTemplates;
		private readonly Dictionary<Type, ControlTemplate> defaultTemplates;

		protected Skin()
		{
			this.customTemplates = new Dictionary<string, ControlTemplate>();
			this.defaultTemplates = new Dictionary<Type, ControlTemplate>();

			this.Initialize();
		}

		public void AddCustomTemplate(string templateName, ControlTemplate template)
		{
			this.customTemplates[templateName] = template;
		}

		public void AddDefaultTemplate(Type type, ControlTemplate template)
		{
			this.defaultTemplates[type] = template;
		}

		public T GetTemplate<T>(Control c) where T : ControlTemplate, new()
		{
			T template = null;

			if (this.customTemplates.ContainsKey(c.TemplateName))
				template = this.customTemplates[c.TemplateName] as T;

			if (template == null && this.defaultTemplates.ContainsKey(c.GetType()))
				template = this.defaultTemplates[c.GetType()] as T;

			if (template == null)
				template = new T();

			return template;
		}

		protected abstract void Initialize();
	}
}
