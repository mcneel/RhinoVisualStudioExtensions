using Eto.Forms;

namespace Rhino.VisualStudio.Windows.Wizard
{
  public class RhinoTestWizard : EtoWizard
  {
    protected override Control CreatePanel() => new RhinoTestingOptionsPanel(false);

    protected override BaseWizardViewModel CreateViewModel() => new RhinoTestingOptionsViewModel();
  }
}