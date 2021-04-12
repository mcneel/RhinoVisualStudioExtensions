using System;
using MonoDevelop.Ide.Templates;
using MonoDevelop.Ide.Projects;
using System.Collections.Generic;
using Eto.Forms;

namespace Rhino.VisualStudio.Mac.Wizard
{
  class ProvideSampleControl : ProjectConfigurationControl
  {
    readonly RhinoCommonWizard wizard;

    public ProvideSampleControl(RhinoCommonWizard wizard)
    {
      this.wizard = wizard;
    }

    public override string Label => string.Empty;

    Eto.Forms.CheckBox cb;

    protected override object CreateNativeWidget<T> ()
    {
      cb = new Eto.Forms.CheckBox{ Text = "Provide Code Sample"};
      cb.ToolTip = "Check to provide a sample implementation for the command/component";
      //cb.Checked = wizard.ProvideCodeSample;
      //cb.CheckedChanged += (sender, e) => wizard.ProvideCodeSample = cb.Checked == true;
      return cb.ToNative(true);
    }
  }
}
