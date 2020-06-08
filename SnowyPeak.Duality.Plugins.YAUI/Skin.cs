// This code is provided under the MIT license. Originally by Alessandro Pilati.
using SnowyPeak.Duality.Plugins.YAUI.Controls;
using SnowyPeak.Duality.Plugins.YAUI.Templates;
using System;
using System.Collections.Generic;

namespace SnowyPeak.Duality.Plugins.YAUI
{
	public abstract class Skin
	{
		internal static string Path(string path)
		{
			return string.Format("SnowyPeak.Duality.Plugins.YAUI.DefaultSkins.{0}", path);
		}

		public static readonly Skin YAUI_DARK = new DefaultSkins.Dark();
		public static readonly Skin YAUI_FATHOMS = new DefaultSkins.Fathoms();
		public static readonly Skin YAUI_ROUNDED = new DefaultSkins.LightRounded();
		public static readonly Skin YAUI_FIGMA = new DefaultSkins.Figma();

		public static Skin DEFAULT { get; set; } = YAUI_FIGMA;

		private readonly Dictionary<string, ControlTemplate> customTemplates;
		private readonly Dictionary<Type, ControlTemplate> defaultTemplates;

		protected Skin()
		{
			this.customTemplates = new Dictionary<string, ControlTemplate>();
			this.defaultTemplates = new Dictionary<Type, ControlTemplate>();

#pragma warning disable S1699 // Constructors should only call non-overridable methods
			this.Initialize();
#pragma warning restore S1699 // Constructors should only call non-overridable methods
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
