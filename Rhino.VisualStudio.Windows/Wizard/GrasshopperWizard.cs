using Eto.Forms;

namespace Rhino.VisualStudio.Windows.Wizard
{
  public class GrasshopperWizard : EtoWizard
  {
    protected override Control CreatePanel() => new GrasshopperOptionsPanel(false);

    protected override BaseWizardViewModel CreateViewModel() => new GrasshopperOptionsViewModel();
  }
}