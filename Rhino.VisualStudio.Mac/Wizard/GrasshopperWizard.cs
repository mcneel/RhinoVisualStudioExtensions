using System;
using MonoDevelop.Ide.Projects;
using System.Collections.Generic;
using Eto.Forms;

namespace Rhino.VisualStudio.Mac.Wizard
{
  public class GrasshopperWizard : BaseTemplateWizard
  {
    public override string Id => "Rhino.VisualStudio.Mac.GrasshopperWizard";
    public override string PageTitle => "Grasshopper project options";
    protected override BaseWizardViewModel CreateModel() => new GrasshopperOptionsViewModel();
    protected override Control CreatePanel() => new GrasshopperOptionsPanel(true);
  }
}