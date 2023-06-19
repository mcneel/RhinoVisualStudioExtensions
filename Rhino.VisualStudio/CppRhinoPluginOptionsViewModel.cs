namespace Rhino.VisualStudio
{

    public class CppRhinoPluginOptionsViewModel : BaseCppRhinoOptionsViewModel
    {
        public override string ProjectTitle => "New Rhino C++ Plug-In";

        string _commandName;
        public string CommandName
        {
            get => _commandName ?? Utility.GetSafeNamePrefixSuffix(ProjectName, null, null, "PlugIn", "Command");
            set
            {
                if (Set(ref _commandName, value))
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
                    OnPropertyChanged(nameof(CommandName));
                    OnPropertyChanged(nameof(IsValid));
                    OnPropertyChanged(nameof(IsProjectNameInvalid));
                }
            }
        }

        bool _useSDL = true;
        public bool UseSDL
        {
            get => _useSDL;
            set => Set(ref _useSDL, value);
        }

        bool _useAutomation;
        public bool UseAutomation
        {
            get => _useAutomation;
            set => Set(ref _useAutomation, value);
        }

        bool _useSockets;
        public bool UseSockets
        {
            get => _useSockets;
            set => Set(ref _useSockets, value);
        }

        public CppRhinoPluginOptionsViewModel()
        {
        }

        public override bool IsValid =>
          !IsProjectNameInvalid
          && Utility.IsValidIdentifier(CommandName)
          && !IsLocationInvalid
          && IsRhinoVersionValid;

        public override void Finish()
        {
            base.Finish();
            if (Host == null)
                return;

            Host.SetParameter("CommandName", CommandName);
            Host.SetParameter("Automation", UseAutomation.ToString());
            Host.SetParameter("Sockets", UseSockets.ToString());
            Host.SetParameter("SDL", UseSDL.ToString());
        }

    }
}