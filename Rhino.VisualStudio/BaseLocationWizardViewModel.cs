using System.IO;

namespace Rhino.VisualStudio
{
  public abstract class BaseLocationWizardViewModel : BaseWizardViewModel
  {

    int? _rhinoVersion;
    public int RhinoVersion
    {
      get => _rhinoVersion ?? Constants.DefaultRhinoVersion;
      set
      {
        if (Set(ref _rhinoVersion, value))
        {
          OnPropertyChanged(nameof(ExecutableLocation));
          OnPropertyChanged(nameof(IsLocationInvalid));
        }
      }
    }

    protected abstract string GetDefaultLocation();

    public bool ShowExecutableLocation => Host.IsSupportedParameter("ExecutableLocation");


    string _rhinoLocation;
    public string ExecutableLocation
    {
      get => _rhinoLocation ?? GetDefaultLocation();
      set
      {
        if (Set(ref _rhinoLocation, value))
        {
          OnPropertyChanged(nameof(IsLocationInvalid));
          OnPropertyChanged(nameof(IsValid));
        }
      }
    }

    public bool IsLocationInvalid => ShowExecutableLocation && (string.IsNullOrEmpty(ExecutableLocation) || !File.Exists(ExecutableLocation));
  }
}