using System;
using MonoDevelop.Ide.Templates;
using MonoDevelop.Ide.Projects;

namespace MonoDevelop.RhinoDebug
{
  public class PluginWizard : TemplateWizard
  {
    public override string Id { get { return "MonoDevelop.RhinoDebug.PluginWizard"; } }

    bool provideCommandSample;

    public bool ProvideCommandSample
    {
      get { return provideCommandSample; }
      set
      {
        provideCommandSample = value;
        Parameters["ProvideCommandSample"] = value.ToString();
      }
    }

    public override void ConfigureWizard()
    {
      base.ConfigureWizard();

      ProvideCommandSample = true; // no ui for this yet

      // provide some guid's for our templates
      for (int i = 0; i < 10; i++)
      {
        Parameters["Guid" + i] = Guid.NewGuid().ToString();
      }
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

