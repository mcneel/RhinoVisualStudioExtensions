using Eto.Forms;

namespace Rhino.VisualStudio.Windows.Wizard
{
  public class RhinoWizard : EtoWizard
  {
    protected override Control CreatePanel() => new RhinoCommonOptionsPanel(false);

    protected override BaseWizardViewModel CreateViewModel() => new RhinoCommonOptionsViewModel();
  }
}