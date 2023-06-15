using Eto;
using Eto.Drawing;
using Eto.Wpf;
using Rhino.VisualStudio;
using Rhino.VisualStudio.Windows;
using Microsoft.VisualStudio.PlatformUI;

[assembly: ExportHandler(typeof(IPlatformTheme), typeof(PlatformThemeHandler))]

namespace Rhino.VisualStudio.Windows
{
    class PlatformThemeHandler : IPlatformTheme
    {
        public Color ProjectBackground => VSColorTheme.GetThemedColor(ThemedDialogColors.WindowPanelColorKey).ToEto();

        public Color ProjectForeground => VSColorTheme.GetThemedColor(ThemedDialogColors.WindowPanelTextColorKey).ToEto();
        public Color ProjectDialogBackground => VSColorTheme.GetThemedColor(EnvironmentColors.NewProjectBackgroundColorKey).ToEto();

        public Color ErrorForeground => VSColorTheme.GetThemedColor(ThemedDialogColors.ValidationErrorTextBrushKey).ToEto();

        public Color SummaryBackground => VSColorTheme.GetThemedColor(ThemedDialogColors.WizardFooterColorKey).ToEto();

        public Color SummaryForeground => VSColorTheme.GetThemedColor(ThemedDialogColors.WindowPanelTextColorKey).ToEto();

        public Color DesignerBackground => VSColorTheme.GetThemedColor(EnvironmentColors.DesignerBackgroundColorKey).ToEto();

        public Color DesignerPanel => Color.FromRgb(0xF0F0F0); // hmm.
    }
}
