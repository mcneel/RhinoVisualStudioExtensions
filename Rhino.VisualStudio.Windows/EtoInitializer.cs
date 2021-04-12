using Eto;
using Eto.Forms;
using Eto.Wpf.Forms;
using System;

namespace Rhino.VisualStudio.Windows
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
        Style.Add<FormHandler>("rhino.themed", h => ThemeWindow(h.Control));
        Style.Add<DialogHandler>("rhino.themed", h => ThemeWindow(h.Control));

        var platform = Platform.Instance;
        if (platform == null)
        {
          platform = new Eto.Wpf.Platform();
          Platform.Initialize(platform);
        }

        platform.LoadAssembly(typeof(EtoInitializer).Assembly);

        if (Application.Instance == null)
          new Eto.Forms.Application().Attach();

      }
      catch (Exception ex)
      {
        Console.WriteLine($"{ex}");
      }
    }
    private static void ThemeWindow(System.Windows.Window w)
    {
      w.Resources.MergedDictionaries.Add(new System.Windows.ResourceDictionary { Source = new Uri("pack://application:,,,/Rhino.VisualStudio.Windows;component/theme/WindowStyles.xaml", UriKind.RelativeOrAbsolute) });
    }

  }
}
