using Eto.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Rhino.VisualStudio
{
    public class RhinoTestingOptionsViewModel : BaseRhinoOptionsViewModel
    {

        public override string ProjectTitle => "New RhinoCommon .NET Tests";
        
        string _testClassName;
        public string TestClassName
        {
            get => _testClassName ?? Utility.GetSafeName(ProjectName, "Test", "Plugin", "Addin");
            set
            {
                if (Set(ref _testClassName, value))
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
                    OnPropertyChanged(nameof(ProjectClassName));
                    OnPropertyChanged(nameof(TestClassName));
                    OnPropertyChanged(nameof(IsValid));
                    OnPropertyChanged(nameof(IsProjectNameInvalid));
                }
            }
        }

        string _projectClassName;
        public string ProjectClassName
        {
            get => _projectClassName ?? Utility.GetSafeName(ProjectName, "Plugin", "Test", "Addin");
            set
            {
                if (Set(ref _projectClassName, value))
                {
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public override IEnumerable<int> RhinoVersionsAvailable
        {
            get
            {
                // Ensures Versionis are > 7
                foreach (int version in Global.VersionsToCheck.Where(v => v > 7))
                {
                    if (!string.IsNullOrEmpty(FindLocation(version)))
                    {
                        yield return version;
                    }
                }
            }
        }



        public RhinoTestingOptionsViewModel()
        {
        }


        public override bool IsValid =>
          !IsProjectNameInvalid
          && Utility.IsValidIdentifier(TestClassName)
          && Utility.IsValidIdentifier(ProjectClassName)
          && !IsLocationInvalid
          && IsRhinoVersionValid;

        public override void Finish()
        {
            base.Finish();
            if (Host == null)
                return;

            Host.SetParameter("TestClassName", TestClassName);
            Host.SetParameter("ProjectClassName", ProjectClassName);
        }

        protected override string FindLocation(int version) => Global.Helpers.FindRhino(version);
    }
}