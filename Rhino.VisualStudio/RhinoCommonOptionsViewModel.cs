using Eto.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Rhino.VisualStudio
{
    public class RhinoCommonOptionsViewModel : BaseRhinoOptionsViewModel
    {

        public override string ProjectTitle => "New RhinoCommon .NET Plug-In";
        
        string _commandClassName;
        public string CommandClassName
        {
            get => _commandClassName ?? Utility.GetSafeName(ProjectName, "Command", "Plugin", "Addin");
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
            get => _pluginClassName ?? Utility.GetSafeName(ProjectName, "Plugin", "Command", "Addin");
            set
            {
                if (Set(ref _pluginClassName, value))
                {
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }



        public RhinoCommonOptionsViewModel()
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
        }

        protected override string FindLocation(int version) => Global.Helpers.FindRhino(version);
    }
}