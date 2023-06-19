using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhino.VisualStudio
{
    public abstract class BaseLocationWizardViewModel : BaseWizardViewModel
    {

        int? _rhinoVersion;
        public int RhinoVersion
        {
            get => _rhinoVersion ?? RhinoVersionsAvailable?.FirstOrDefault() ?? Constants.DefaultRhinoVersion;
            set
            {
                if (Set(ref _rhinoVersion, value))
                {
                    OnPropertyChanged(nameof(ExecutableLocation));
                    OnPropertyChanged(nameof(IsLocationInvalid));
                    OnPropertyChanged(nameof(IsRhinoAvailable));
                    OnPropertyChanged(nameof(IsValid));
                    OnPropertyChanged(nameof(RhinoVersionValidationText));
                    OnPropertyChanged(nameof(IsRhinoVersionValid));
                }
            }
        }

        public virtual bool IsRhinoAvailable => RhinoVersion > 0;
        public virtual bool IsRhinoVersionValid => IsRhinoAvailable;

        public virtual string RhinoVersionValidationText
        {
            get
            {
                if (!IsRhinoAvailable)
                    return $"Rhino must be installed for this project type";
                return null;
            }
        }

        public virtual IEnumerable<int> RhinoVersionsAvailable
        {
            get
            {
                // yield break; // uncomment to test when there's no rhino installed
                
                foreach (int version in Global.VersionsToCheck)
                {
                    if (!string.IsNullOrEmpty(FindLocation(version)))
                    {
                        yield return version;
                    }
                }
            }
        }

        protected abstract string FindLocation(int version);

        protected virtual string GetDefaultLocation() => FindLocation(RhinoVersion);

        public bool ShowExecutableLocation => Host.IsSupportedParameter("ExecutableLocation");

        string _rhinoLocation;
        public string ExecutableLocation
        {
            get => _rhinoLocation ?? GetDefaultLocation();
            set
            {
                if (Set(ref _rhinoLocation, value))
                {
                    if (_rhinoLocation == GetDefaultLocation())
                        _rhinoLocation = null;
                    OnPropertyChanged(nameof(IsLocationInvalid));
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public bool IsLocationInvalid => ShowExecutableLocation && (string.IsNullOrEmpty(ExecutableLocation) || !File.Exists(ExecutableLocation));

        public override void Finish()
        {
            base.Finish();
            if (Host == null)
                return;

            Host.SetParameter("RhinoVersion", RhinoVersion.ToString());

            if (ShowExecutableLocation)
            {
                Host.SetParameter("RhinoLocation", ExecutableLocation);
            }
        }
    }
}