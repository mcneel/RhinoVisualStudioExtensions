using Eto.Forms;

namespace Rhino.VisualStudio.Windows.Wizard
{
  public class Grasshopper2Wizard : EtoWizard
  {
    protected override Control CreatePanel() => new Grasshopper2OptionsPanel(false);

    protected override BaseWizardViewModel CreateViewModel() => new Grasshopper2OptionsViewModel();
  }
}