using Eto.Forms;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Rhino.VisualStudio.Windows
{
    static class Helpers
    {
        static Helpers()
        {
            EtoInitializer.Initialize();
        }

        public static Window MainWindow
        {
            get
            {
                Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
                var uiShell = Services.GetService<IVsUIShell>();
                IntPtr hwnd;
                uiShell.GetDialogOwnerHwnd(out hwnd);
                return new Form(new Eto.Wpf.Forms.HwndFormHandler(hwnd));
            }
        }
    }
}
