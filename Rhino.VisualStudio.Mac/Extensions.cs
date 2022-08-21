using System;
using Eto.Drawing;

namespace Rhino.VisualStudio.Mac
{
	public static class Extensions
	{
#if VS2022
		public static Color ToEto(this MonoDevelop.Ide.Gui.Styles.IdeColor color)
		{
			return new Color((AppKit.NSColor)color, (float)color.Red, (float)color.Green, (float)color.Blue, (float)color.Alpha);
		}
#endif
		public static Color ToEto(this Xwt.Drawing.Color color)
		{
			return new Color((float)color.Red, (float)color.Green, (float)color.Blue, (float)color.Alpha);
		}
	}
}
