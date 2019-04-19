// This code is provided under the MIT license. Originally by Alessandro Pilati.
using Duality;
using Duality.Drawing;
using Duality.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugins.YAUI
{
	internal static class ResourceHelper
	{
		public static ContentRef<Font> LoadFont(Assembly embeddingAssembly, string resourceName)
		{
			ContentRef<Font> result;

			using (Stream stream = embeddingAssembly.GetManifestResourceStream(resourceName))
			{
				result = Resource.Load<Font>(stream);
			}

			return result;
		}

		public static ContentRef<Pixmap> LoadPixmap(Assembly embeddingAssembly, string resourceName)
		{
			IImageCodec codec = ImageCodec.GetRead(ImageCodec.FormatPng);
			ContentRef<Pixmap> result;

			using (Stream stream = embeddingAssembly.GetManifestResourceStream(resourceName))
			{
				result = new Pixmap(codec.Read(stream));
			}

			return result;
		}
	}
}
