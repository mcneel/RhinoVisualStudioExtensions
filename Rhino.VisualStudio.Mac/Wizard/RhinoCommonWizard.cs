using System;
using MonoDevelop.Ide.Projects;
using System.Collections.Generic;
using Eto.Forms;

namespace Rhino.VisualStudio.Mac.Wizard
{
  public class RhinoCommonWizard : BaseTemplateWizard
  {
    public override string Id => "Rhino.VisualStudio.Mac.RhinoCommonWizard";
    public override string PageTitle => "Rhino project options";

    protected override BaseWizardViewModel CreateModel() => new RhinoCommonOptionsViewModel();

    protected override Control CreatePanel() => new RhinoCommonOptionsPanel(true);
  }
}