using Eto.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Rhino.VisualStudio
{
  public class RhinoCommonOptionsViewModel : BaseLocationWizardViewModel
  {
    List<ListItem> _types;
    string _pluginType;

    public List<ListItem> Types => _types ?? (_types = GetTypes().ToList());

    public override string ProjectTitle => "New RhinoCommon Plug-In";

    private IEnumerable<ListItem> GetTypes()
    {
      yield return new ListItem { Key = "utility", Text = "General utility" };
      yield return new ListItem { Key = "digitize", Text = "Digitizer" };
      yield return new ListItem { Key = "render", Text = "Rendering" };
      yield return new ListItem { Key = "import", Text = "File Import" };
      yield return new ListItem { Key = "export", Text = "File Export" };
    }

    string _commandClassName;
    public string CommandClassName
    {
      get => _commandClassName ?? Utility.GetSafeName(ProjectName, "Command", "Plugin");
      set
      {
        if (Set(ref _commandClassName, value))
        {
          OnPropertyChanged(nameof(IsValid));
        }
      }
    }

    string _projectName;
    public override string ProjectName
    {
      get => _projectName ?? string.Empty;
      set
      {
        if (Set(ref _projectName, value))
        {
          OnPropertyChanged(nameof(PluginClassName));
          OnPropertyChanged(nameof(CommandClassName));
          OnPropertyChanged(nameof(IsValid));
          OnPropertyChanged(nameof(IsProjectNameInvalid));
        }
      }
    }

    string _pluginClassName;
    public string PluginClassName
    {
      get => _pluginClassName ?? Utility.GetSafeName(ProjectName, "Plugin", "Command");
      set 
      {
        if (Set(ref _pluginClassName, value))
        {
          OnPropertyChanged(nameof(IsValid));
        }
      }
    }

    public string PluginType
    {
      get => _pluginType;
      set
      {
        if (Set(ref _pluginType, value))
        {
          OnPropertyChanged(nameof(ShowFileOptions));
          OnPropertyChanged(nameof(CanProvideSample));
          if (!CanProvideSample)
            IncludeSample = false;
        }
      }
    }

    bool _includeSample;
    public bool IncludeSample
    {
      get => _includeSample;
      set => Set(ref _includeSample, value);
    }


    public RhinoCommonOptionsViewModel()
    {
      SetDefaults();
    }

    string _fileExtension;
    public string FileExtension
    {
      get => _fileExtension;
      set => Set(ref _fileExtension, value);
    }

    string _fileDescription;
    public string FileDescription
    {
      get => _fileDescription;
      set => Set(ref _fileDescription, value);
    }
    public bool ShowFileOptions => PluginType == "import" || PluginType == "export";

    public bool CanProvideSample => PluginType == "utility";

    public override bool IsValid =>
      !IsProjectNameInvalid
      && Utility.IsValidIdentifier(CommandClassName)
      && Utility.IsValidIdentifier(PluginClassName)
      && !IsLocationInvalid;

    void SetDefaults()
    {
      PluginType = Types[0].Key;

      FileExtension = ".txt";
      FileDescription = "Text file";
      RhinoVersion = 7;
      IncludeSample = true;
    }

    public override void Finish()
    {
      if (Host == null)
        return;
        
      Host.SetParameter("PluginType", PluginType);
      Host.SetParameter("RhinoVersion", RhinoVersion.ToString());
      Host.SetParameter("CommandClassName", CommandClassName);
      Host.SetParameter("PluginClassName", PluginClassName);
      if (CanProvideSample)
      {
        Host.SetParameter("IncludeSample", IncludeSample.ToString());
      }
      if (ShowFileOptions)
      {
        Host.SetParameter("FileExtension", FileExtension.TrimStart('.'));
        Host.SetParameter("FileDescription", FileDescription);
      }
      if (ShowExecutableLocation)
      {
        Host.SetParameter("RhinoLocation", ExecutableLocation);
      }
    }

    protected override string GetDefaultLocation() => RhinoFinder.FindRhino(RhinoVersion);
  }
}