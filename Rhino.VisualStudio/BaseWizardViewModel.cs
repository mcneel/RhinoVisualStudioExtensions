namespace Rhino.VisualStudio
{
  public abstract class BaseWizardViewModel : BaseViewModel
  {
    public IWizardHost Host { get; set; }

    public abstract bool IsValid { get; }

    public abstract string ProjectName { get; set; }

		public virtual bool IsProjectNameInvalid => !Utility.IsValidProjectName(ProjectName);

 		public virtual string ProjectNameValidationText => "Project name must only be a combination of letters, digits, or one of '_', '-', '.'";

    public virtual string ProjectTitle => string.Empty;

    public virtual void Continue()
    {
      if (IsValid)
        Host?.Continue();
    }
    
    public virtual void Abort()
    {
      Host?.Abort();
    }

    public virtual void Finish()
    {
    }
    
  }
}