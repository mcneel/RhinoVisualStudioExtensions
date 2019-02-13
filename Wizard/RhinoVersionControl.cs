using System;
using MonoDevelop.Ide.Templates;
using MonoDevelop.Ide.Projects;
using System.Collections.Generic;

namespace MonoDevelop.RhinoDebug.Wizard
{
  class RhinoVersionControl : ProjectConfigurationControl
  {
    readonly PluginWizard wizard;

    public RhinoVersionControl(PluginWizard wizard)
    {
      this.wizard = wizard;
    }

    public override string Label => "Rhino Version:";

    protected override object CreateNativeWidget<T> ()
    {
      var box = new Gtk.HBox();
      var rb5 = new Gtk.RadioButton("v5") { Active = wizard.RhinoVersion == 5 };
      var rb6 = new Gtk.RadioButton(rb5, "v6 (WIP)") { Active = wizard.RhinoVersion == 6 };
      rb5.Toggled += (sender, e) => { if (rb5.Active) wizard.RhinoVersion = 5; };
      rb6.Toggled += (sender, e) => { if (rb6.Active) wizard.RhinoVersion = 6; };

      box.Spacing = 2;
      box.PackStart(rb5, false, true, 0);
      box.PackStart(rb6, false, true, 0);


      return box;
    }
  }
}
