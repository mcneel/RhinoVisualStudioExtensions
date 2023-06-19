using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhino.VisualStudio
{
    public class ZooPluginOptionsViewModel : BaseLocationWizardViewModel
    {
        public override string ProjectTitle => "New Zoo Plug-In";
        
        string _pluginClassName;
        public string PluginClassName
        {
            get => _pluginClassName ?? Utility.GetSuffixedName(ProjectName);
            set
            {
                if (Set(ref _pluginClassName, value))
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
                    OnPropertyChanged(nameof(ExecutableLocation));
                    OnPropertyChanged(nameof(PluginClassName));
                    OnPropertyChanged(nameof(IsValid));
                    OnPropertyChanged(nameof(IsProjectNameInvalid));
                }
            }
        }

        string _rhinoPluginId;
        public string RhinoPluginId
        {
            get => _rhinoPluginId;
            set
            {
                if (Set(ref _rhinoPluginId, value))
                {
                    OnPropertyChanged(nameof(IsRhinoPluginIdInvalid));
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public ZooPluginOptionsViewModel()
        {
            SetDefaults();
        }

        public override bool IsValid =>
            Utility.IsValidIdentifier(PluginClassName)
            && !IsProjectNameInvalid
            && !IsLocationInvalid
            && !IsRhinoPluginIdInvalid;

        public bool IsRhinoPluginIdInvalid => string.IsNullOrEmpty(RhinoPluginId) || !Guid.TryParse(RhinoPluginId, out _);

        void SetDefaults()
        {

        }

        public override void Finish()
        {
            base.Finish();
            if (Host == null)
                return;

            Host.SetParameter("ZooPluginPath", ExecutableLocation);
            Host.SetParameter("RhinoPluginId", RhinoPluginId);
            Host.SetParameter("PluginClassName", PluginClassName);
        }

        protected override string FindLocation(int version) => ZooFinder.FindZooDll(version);
    }
}