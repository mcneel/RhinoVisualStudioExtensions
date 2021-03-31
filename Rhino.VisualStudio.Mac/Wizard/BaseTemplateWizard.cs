using MonoDevelop.Ide.Templates;
using Eto.Forms;

namespace Rhino.VisualStudio.Mac.Wizard
{
  public abstract class BaseTemplateWizard : TemplateWizard
  {
    static BaseTemplateWizard()
    {
      EtoInitializer.Initialize();
    }

    BaseWizardViewModel _model;
    DualBinding<bool> _isValidBinding;

    protected BaseWizardViewModel Model => _model;

    protected abstract BaseWizardViewModel CreateModel();

    protected abstract Control CreatePanel();

    public abstract string PageTitle { get; }

    public override void OnFinish()
    {
      base.OnFinish();
      var model = Model;
      if (model != null)
      {
        model.Finish();

        if (!string.IsNullOrWhiteSpace(model.ProjectName))
        {
          Parameters["ProjectName"] = model.ProjectName;
        }
      }
    }

    public override int TotalPages => 1;

    public override WizardPage GetPage(int pageNumber)
    {
      var panel = CreatePanel();
      
      var page = new EtoWizardPage(this, panel, PageTitle);

      _model = CreateModel();
      _model.Host = page;

      panel.DataContext = _model;

      // bind IsValid to CanMoveToNextPage      
      _isValidBinding = new DualBinding<bool>(
        Binding.Property(_model, (BaseWizardViewModel m) => m.IsValid),
        Binding.Property(page, (EtoWizardPage p) => p.CanMoveToNextPage)
      );
      return page;
    }
  }
}