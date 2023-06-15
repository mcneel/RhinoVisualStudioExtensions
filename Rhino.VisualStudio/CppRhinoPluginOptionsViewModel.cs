using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Win32;

namespace Rhino.VisualStudio
{
    public class CppRhinoPluginOptionsViewModel : BaseRhinoOptionsViewModel
    {
        public override string ProjectTitle => "New Rhino C++ Plug-In";

        string _commandClassName;
        public string CommandClassName
        {
            get => _commandClassName ?? Utility.GetSafeNamePrefixSuffix(ProjectName, "CCommand", null, "PlugIn", "Command");
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
            get => _pluginClassName ?? Utility.GetSafeNamePrefixSuffix(ProjectName, "C", "PlugIn", "Command");
            set
            {
                if (Set(ref _pluginClassName, value))
                {
                    OnPropertyChanged(nameof(IsValid));
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
          && Utility.IsValidIdentifier(CommandClassName)
          && Utility.IsValidIdentifier(PluginClassName)
          && !IsLocationInvalid
          && IsRhinoVersionValid;

        public override void Finish()
        {
            base.Finish();
            if (Host == null)
                return;

            Host.SetParameter("CommandClassName", CommandClassName);
            Host.SetParameter("PluginClassName", PluginClassName);
            Host.SetParameter("Automation", UseAutomation.ToString());
            Host.SetParameter("Sockets", UseSockets.ToString());
            Host.SetParameter("SDL", UseSDL.ToString());
        }

        public override bool IsRhinoVersionValid => base.IsRhinoVersionValid && IsSdkPathValid;
        public override string RhinoVersionValidationText
        {
            get
            {
                var version = RhinoVersion;
                var msg = version > 0
                    ? $"The C++ SDK for Rhino {RhinoVersion} is not installed."
                    : $"The Rhino C++ SDK is not installed.";
                var basemsg = base.RhinoVersionValidationText;
                if (basemsg != null) return string.Join("\n", basemsg, msg);
                return msg;
            }
        }

        public bool IsSdkPathValid => !string.IsNullOrEmpty(GetSdkPath(RhinoVersion));
        public string GetSdkPath(int version)
        {
            var path = Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\McNeel\Rhinoceros\SDK\{version}.0", "InstallPath", null) as string;
            if (path != null && Directory.Exists(path))
                return path;
            return null;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(RhinoVersion))
            {
                OnPropertyChanged(nameof(IsSdkPathValid));
            }
        }

    }
}