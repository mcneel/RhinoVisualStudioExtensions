using System;
using System.Diagnostics;
using Eto;
using Eto.Forms;

namespace Rhino.VisualStudio.Mac
{
    public static class EtoInitializer
    {
        static readonly object Cell_Key = new object();
        static bool initialized;
        public static void Initialize()
        {
            if (initialized)
                return;

            initialized = true;

#if VS2019
            // VS 2019 for Mac is dumb and GC's things even though they're still in use.
            Style.Add<Eto.Mac.Forms.Controls.TextBoxHandler>(null, h =>
            {
                h.Widget.Properties[Cell_Key] = h.Control.Cell;
            });
            Style.Add<Eto.Mac.Forms.Controls.GroupBoxHandler>(null, h =>
            {
                h.Widget.Properties[Cell_Key] = h.Control.ContentView;
            });
#endif

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
                Debug.WriteLine($"{ex}");
            }
        }
    }
}