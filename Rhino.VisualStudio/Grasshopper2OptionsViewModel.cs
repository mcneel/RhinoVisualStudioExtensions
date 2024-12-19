using Eto.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Rhino.VisualStudio
{
    public class Grasshopper2OptionsViewModel : BaseDesktopWizardViewModel
    {
        string _componentClassName;

        public override string ProjectTitle => "New Grasshopper2 Plug-In";

        public string ComponentClassName
        {
            get => _componentClassName ?? Utility.GetSafeName(ProjectName, "Component", "Component", "Components", "Plugin", "Addin");
            set
            {
                if (Set(ref _componentClassName, value))
                {
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        string _componentName;
        public string ComponentName
        {
            get => _componentName ?? ComponentClassName;
            set => Set(ref _componentName, value);
        }

        string _componentChapter;
        public string ComponentChapter
        {
            get => _componentChapter;
            set => Set(ref _componentChapter, value);
        }

        string _componentSection;
        public string ComponentSection
        {
            get => _componentSection;
            set => Set(ref _componentSection, value);
        }

        string _componentInfo;
        public string ComponentInfo
        {
            get => _componentInfo;
            set => Set(ref _componentInfo, value);
        }

        string _projectName;
        public override string ProjectName
        {
            get => _projectName ?? string.Empty;
            set
            {
                if (Set(ref _projectName, value))
                {
                    OnPropertyChanged(nameof(PlugInDisplayName));
                    OnPropertyChanged(nameof(ComponentClassName));
                    OnPropertyChanged(nameof(ComponentName));
                    OnPropertyChanged(nameof(IsValid));
                    OnPropertyChanged(nameof(IsProjectNameInvalid));
                }
            }
        }

        string _plugInDisplayName;
        public string PlugInDisplayName
        {
            get => _plugInDisplayName ?? ProjectName;
            set
            {
                if (Set(ref _plugInDisplayName, value))
                {
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        bool _includeSample;
        public bool IncludeSample
        {
            get => _includeSample;
            set
            {
                if (Set(ref _includeSample, value))
                {
                    if (value)
                    {
                        ComponentChapter = "Curve";
                        ComponentSection = "Primitive";
                        ComponentInfo = "Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.";
                    }
                }
            }
        }

        protected override string FindLocation(int version) => Global.Helpers.FindRhino(version);

        public Grasshopper2OptionsViewModel()
        {
            SetDefaults();
        }

        public override bool IsValid =>
          !IsProjectNameInvalid
          && Utility.IsValidIdentifier(ComponentClassName);

        void SetDefaults()
        {
            ComponentChapter = "Chapter";
            ComponentSection = "Section";

            ComponentInfo = "Description";
        }

        public override void Finish()
        {
            base.Finish();
            if (Host == null)
                return;

            Host.SetParameter("ComponentClassName", ComponentClassName);
            Host.SetParameter("AddonDisplayName", PlugInDisplayName);
            Host.SetParameter("IncludeSample", IncludeSample.ToString());
            Host.SetParameter("ComponentName", ComponentName);
            Host.SetParameter("ComponentChapter", ComponentChapter);
            Host.SetParameter("ComponentSsection", ComponentSection);
            Host.SetParameter("ComponentInfo", ComponentInfo);
        }

    }
}