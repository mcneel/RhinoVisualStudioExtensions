using System;
using Eto;
using Eto.Forms;

namespace Rhino.VisualStudio.Mac
{
	public static class EtoInitializer
	{
    static bool initialized;
		public static void Initialize()
		{
			if (initialized)
				return;

			initialized = true;

			try
			{
				var platform = Platform.Instance;
				if (platform == null)
				{
					platform = new Eto.Mac.Platform();
					Platform.Initialize(platform);
				}

				platform.LoadAssembly(typeof(EtoInitializer).Assembly);

				if (Application.Instance == null)
					new Application().Attach();

			}
			catch (Exception ex)
			{
				Console.WriteLine($"{ex}");
			}
		}
	}
}