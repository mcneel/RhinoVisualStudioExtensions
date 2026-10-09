using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Rhino.VisualStudio.Windows
{
    /// <summary>
    /// Colours a window's OS-drawn title bar to match the VS theme, following theme changes while it's open.
    /// </summary>
    static class WindowTitleBar
    {
        const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        const int DWMWA_BORDER_COLOR = 34;
        const int DWMWA_CAPTION_COLOR = 35;
        const int DWMWA_TEXT_COLOR = 36;

        [DllImport("dwmapi.dll")]
        static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

        /// <summary>Themes the title bar of the window now, or once it has a handle.</summary>
        public static void Attach(Window window)
        {
            void Apply() => ApplyTheme(window);
            // The window's dispatcher is VS's UI thread, so there's nothing to deadlock on.
#pragma warning disable VSTHRD001
            void ThemeChanged(ThemeChangedEventArgs e) => _ = window.Dispatcher.BeginInvoke((Action)Apply);
#pragma warning restore VSTHRD001

            if (new WindowInteropHelper(window).Handle != IntPtr.Zero)
                Apply();
            else
                window.SourceInitialized += (sender, e) => Apply();

            VSColorTheme.ThemeChanged += ThemeChanged;
            window.Closed += (sender, e) => VSColorTheme.ThemeChanged -= ThemeChanged;
        }

        static void ApplyTheme(Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero)
                return;

            var caption = VSColorTheme.GetThemedColor(EnvironmentColors.MainWindowActiveCaptionColorKey);
            var text = VSColorTheme.GetThemedColor(EnvironmentColors.MainWindowActiveCaptionTextColorKey);
            var border = VSColorTheme.GetThemedColor(EnvironmentColors.MainWindowActiveDefaultBorderColorKey);

            // dark mode alone covers Windows 10, where the caption colours below are ignored
            var dark = caption.GetBrightness() < 0.5f ? 1 : 0;
            DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref dark, sizeof(int));
            Set(hwnd, DWMWA_CAPTION_COLOR, caption);
            Set(hwnd, DWMWA_TEXT_COLOR, text);
            Set(hwnd, DWMWA_BORDER_COLOR, border);
        }

        static void Set(IntPtr hwnd, int attribute, System.Drawing.Color color)
        {
            // COLORREF is 0x00BBGGRR
            var value = color.R | (color.G << 8) | (color.B << 16);
            DwmSetWindowAttribute(hwnd, attribute, ref value, sizeof(int));
        }
    }
}
