using System.Collections.Generic;
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

    public interface IPlatformHelpers
    {
        string FindRhino(int version);
    }

    public static class Global
    {
        public static IPlatformTheme Theme => Platform.Instance.CreateShared<IPlatformTheme>();

        public static IPlatformHelpers Helpers => Platform.Instance.CreateShared<IPlatformHelpers>();

        public static int LatestSdkRelease { get; internal set; } = 7;
        public static readonly int[] VersionsToCheck = { 9, 8, 7, 6 };
    }
}