using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Win32;

namespace Rhino.VisualStudio
{
    public abstract class BaseCppRhinoOptionsViewModel : BaseRhinoOptionsViewModel
    {
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
            var localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine,  RegistryView.Registry64);
            using var key = localKey.OpenSubKey($@"SOFTWARE\McNeel\Rhinoceros\SDK\{version}.0");
            if (key != null)
            {
                var path = key.GetValue("InstallPath") as string;
                if (path != null && Directory.Exists(path))
                    return path;
            }
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