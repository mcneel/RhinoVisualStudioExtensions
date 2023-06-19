using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Win32;

namespace Rhino.VisualStudio
{
    public class CppRhinoSkinOptionsViewModel : BaseCppRhinoOptionsViewModel
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

    }
}