using System;
using MonoDevelop.Ide.Templates;
using MonoDevelop.Ide.Projects;
using System.Collections.Generic;

namespace MonoDevelop.RhinoDebug.Wizard
{
  class ProvideSampleControl : ProjectConfigurationControl
  {
    readonly PluginWizard wizard;

    public ProvideSampleControl(PluginWizard wizard)
    {
      this.wizard = wizard;
    }

    public override string Label
    {
      get { return string.Empty; }
    }

    protected override object CreateNativeWidget<T> ()
    {
      var cb = new Gtk.CheckButton("Provide Code Sample");
      cb.TooltipText = "Check to provide a sample implementation for the command/component";
      cb.Active = wizard.ProvideCodeSample;
      cb.Toggled += (sender, e) => wizard.ProvideCodeSample = cb.Active;
      return cb;
    }
  }
}
