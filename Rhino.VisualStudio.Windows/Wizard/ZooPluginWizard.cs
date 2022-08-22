using System;
using Eto.Forms;

namespace Rhino.VisualStudio.Windows.Wizard
{
  public class ZooPluginWizard : EtoWizard
  {
    protected override Control CreatePanel() => new ZooPluginOptionsPanel(false);

    protected override BaseWizardViewModel CreateViewModel() => new ZooPluginOptionsViewModel();
  }
}