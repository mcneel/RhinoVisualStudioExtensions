using Eto.Forms;

namespace Rhino.VisualStudio.Windows.Wizard
{
  public class CppSkinWizard : EtoWizard
  {
    protected override Control CreatePanel() => new CppRhinoSkinOptionsPanel(false);

    protected override BaseWizardViewModel CreateViewModel() => new CppRhinoSkinOptionsViewModel();
  }
}