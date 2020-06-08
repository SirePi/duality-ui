// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SnowyPeak.Duality.Plugins.YAUI
{
	internal static class ResourceHelper
	{
		private static IImageCodec pngCodec = ImageCodec.GetRead(ImageCodec.FormatPng);
		private static Dictionary<string, IContentRef> resources = new Dictionary<string, IContentRef>();

		private static IContentRef GetCachedRef(string key, Func<IContentRef> loader)
		{	
			if (!resources.ContainsKey(key))
			{
				IContentRef newRef = loader();
				resources.Add(key, newRef);
			}

			return resources[key];
		}

		public static ContentRef<Font> LoadFont(Assembly embeddingAssembly, string resourceName)
		{
			string key = string.Format("fnt-{0}", resourceName);
			return (ContentRef<Font>)GetCachedRef(key, () =>
			{
				using (Stream stream = embeddingAssembly.GetManifestResourceStream(resourceName))
					return new ContentRef<Font>(Resource.Load<Font>(stream));
			});
		}

		public static ContentRef<Pixmap> LoadPixmap(Assembly embeddingAssembly, string resourceName)
		{
			string key = string.Format("pix-{0}", resourceName);
			return (ContentRef<Pixmap>)GetCachedRef(key, () =>
			{
				Logs.Game.Write("Loading {0}", resourceName);
				using (Stream stream = embeddingAssembly.GetManifestResourceStream(resourceName))
					return new ContentRef<Pixmap>(new Pixmap(pngCodec.Read(stream)));
			});
		}

		public static ContentRef<Texture> GetTexture(ContentRef<Pixmap> pixmap)
		{
			string key = string.Format("tex-{0}", pixmap.FullName);
			return (ContentRef<Texture>)GetCachedRef(key, () => new ContentRef<Texture>(new Texture(pixmap)));
		}

		public static ContentRef<Material> GetMaterial(ContentRef<Texture> texture, ContentRef<DrawTechnique> technique)
		{
			string key = string.Format("mat-{0}", texture.FullName);
			return (ContentRef<Material>)GetCachedRef(key, () => new ContentRef<Material>(new Material(technique, texture)));
		}

		internal static ContentRef<Material> GetMaterial(Assembly embeddingAssembly, string pixmapName)
		{
			return GetMaterial(embeddingAssembly, pixmapName, DrawTechnique.Mask);
		}

		internal static ContentRef<Material> GetMaterial(Assembly embeddingAssembly, string pixmapName, ContentRef<DrawTechnique> technique)
		{
			ContentRef<Pixmap> pixmap = ResourceHelper.LoadPixmap(embeddingAssembly, pixmapName);
			ContentRef<Texture> texture = ResourceHelper.GetTexture(pixmap);
			return GetMaterial(texture, technique);
		}
	}
}
