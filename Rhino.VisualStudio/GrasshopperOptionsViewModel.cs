using Eto.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Rhino.VisualStudio
{
    public class GrasshopperOptionsViewModel : BaseDesktopWizardViewModel
    {
        string _componentClassName;

        public override string ProjectTitle => "New Grasshopper Add-On";

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

        string _componentNickname;
        public string ComponentNickname
        {
            get => _componentNickname;
            set => Set(ref _componentNickname, value);
        }

        string _componentCategory;
        public string ComponentCategory
        {
            get => _componentCategory;
            set => Set(ref _componentCategory, value);
        }

        string _componentSubcategory;
        public string ComponentSubcategory
        {
            get => _componentSubcategory;
            set => Set(ref _componentSubcategory, value);
        }

        string _componentDescription;
        public string ComponentDescription
        {
            get => _componentDescription;
            set => Set(ref _componentDescription, value);
        }

        string _projectName;
        public override string ProjectName
        {
            get => _projectName ?? string.Empty;
            set
            {
                if (Set(ref _projectName, value))
                {
                    OnPropertyChanged(nameof(AddonDisplayName));
                    OnPropertyChanged(nameof(ComponentClassName));
                    OnPropertyChanged(nameof(ComponentName));
                    OnPropertyChanged(nameof(IsValid));
                    OnPropertyChanged(nameof(IsProjectNameInvalid));
                }
            }
        }

        string _addonDisplayName;
        public string AddonDisplayName
        {
            get => _addonDisplayName ?? ProjectName;
            set
            {
                if (Set(ref _addonDisplayName, value))
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
                        ComponentNickname = "ASpi";
                        ComponentCategory = "Curve";
                        ComponentSubcategory = "Primitive";
                        ComponentDescription = "Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.";
                    }
                }
            }
        }

        protected override string FindLocation(int version) => Global.Helpers.FindRhino(version);

        public GrasshopperOptionsViewModel()
        {
            SetDefaults();
        }

        public override bool IsValid =>
          !IsProjectNameInvalid
          && Utility.IsValidIdentifier(ComponentClassName);

        void SetDefaults()
        {
            ComponentNickname = "Nickname";
            ComponentCategory = "Category";
            ComponentSubcategory = "Subcategory";
            ComponentDescription = "Description";
            UseWinForms = true;
        }

        public override void Finish()
        {
            base.Finish();
            if (Host == null)
                return;

            Host.SetParameter("ComponentClassName", ComponentClassName);
            Host.SetParameter("AddonDisplayName", AddonDisplayName);
            Host.SetParameter("IncludeSample", IncludeSample.ToString());
            Host.SetParameter("ComponentName", ComponentName);
            Host.SetParameter("ComponentNickname", ComponentNickname);
            Host.SetParameter("ComponentCategory", ComponentCategory);
            Host.SetParameter("ComponentSubcategory", ComponentSubcategory);
            Host.SetParameter("ComponentDescription", ComponentDescription);
        }

    }
}