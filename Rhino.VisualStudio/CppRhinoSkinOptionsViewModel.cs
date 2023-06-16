using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Win32;

namespace Rhino.VisualStudio
{
    public class CppRhinoSkinOptionsViewModel : BaseRhinoOptionsViewModel
    {
        public override string ProjectTitle => "New Rhino C++ Skin";

        string _projectName;
        public override string ProjectName
        {
            get => _projectName ?? string.Empty;
            set
            {
                if (Set(ref _projectName, value))
                {
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

        bool _useCustomMenus = true;
        public bool UseCustomMenus
        {
            get => _useCustomMenus;
            set => Set(ref _useCustomMenus, value);
        }

        public override bool IsValid =>
          !IsProjectNameInvalid
          && !IsLocationInvalid
          && IsRhinoVersionValid;

        public override void Finish()
        {
            base.Finish();
            if (Host == null)
                return;

            Host.SetParameter("CustomMenus", UseCustomMenus.ToString());
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