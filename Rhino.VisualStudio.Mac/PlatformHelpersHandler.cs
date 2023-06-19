using System;
using System.Collections.Generic;
using Eto;
using Eto.Drawing;
using MonoDevelop.Ide.Gui;
using Rhino.VisualStudio;
using Rhino.VisualStudio.Mac;

[assembly: ExportHandler(typeof(IPlatformHelpers), typeof(PlatformHelpersHandler))]

namespace Rhino.VisualStudio.Mac
{
    public class PlatformHelpersHandler : IPlatformHelpers
    {
        public string FindRhino(int version)
        {
            return Helpers.FindRhinoWithVersion(version);
        }
    }
}