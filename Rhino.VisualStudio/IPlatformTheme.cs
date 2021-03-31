using Eto;
using Eto.Drawing;

namespace Rhino.VisualStudio
{
  public interface IPlatformTheme
  {
    Color ProjectBackground { get; }
    Color ProjectForeground { get; }
    Color ProjectDialogBackground { get; }
    Color ErrorForeground { get; }
    Color SummaryBackground { get; }
    Color SummaryForeground { get; }
    Color DesignerBackground { get; }
    Color DesignerPanel { get; }
  }

  public static class Global
  {
    public static IPlatformTheme Theme => Platform.Instance.CreateShared<IPlatformTheme>();

  }
}