namespace Rhino.VisualStudio
{
  public interface IWizardHost
  {
    bool IsSupportedParameter(string name);
    string GetParameter(string name);
    void SetParameter(string name, string value);
    void Abort();
    void Continue();
  }
}