using Eto.Forms;

namespace Rhino.VisualStudio.Windows.Wizard
{
  public class CppRhinoWizard : EtoWizard
  {
    protected override Control CreatePanel() => new CppRhinoPluginOptionsPanel(false);

    protected override BaseWizardViewModel CreateViewModel() => new CppRhinoPluginOptionsViewModel();
  }
}