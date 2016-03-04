using System;
using MonoDevelop.Ide.Templates;
using MonoDevelop.Ide.Projects;
using System.Collections.Generic;

namespace MonoDevelop.RhinoDebug.Wizard
{
  public class PluginWizard : TemplateWizard
  {
    public override string Id { get { return "MonoDevelop.RhinoDebug.PluginWizard"; } }

    public bool ProvideCodeSample { get; set; } = true;

    public override void ConfigureWizard()
    {
      base.ConfigureWizard();

      Parameters["ProvideCodeSample"] = ProvideCodeSample.ToString();

      // provide some guid's for our templates
      for (int i = 0; i < 10; i++)
      {
        Parameters["Guid" + i] = Guid.NewGuid().ToString();
      }
    }

    public override IEnumerable<ProjectConfigurationControl> GetFinalPageControls()
    {
      yield return new ProvideSampleControl(this);
    }

    public override int TotalPages
    {
      get { return 0; }
    }

    public override WizardPage GetPage(int pageNumber)
    {
      return null;
    }
  }
}