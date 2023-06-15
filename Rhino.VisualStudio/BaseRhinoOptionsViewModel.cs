using Eto.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Rhino.VisualStudio
{
    public abstract class BaseRhinoOptionsViewModel : BaseDesktopWizardViewModel
    {
        string _pluginType;
        List<ListItem> _types;
        public List<ListItem> Types => _types ?? (_types = GetTypes().ToList());

        private IEnumerable<ListItem> GetTypes()
        {
            yield return new ListItem { Key = "utility", Text = "General utility" };
            yield return new ListItem { Key = "digitize", Text = "Digitizer" };
            yield return new ListItem { Key = "render", Text = "Rendering" };
            yield return new ListItem { Key = "import", Text = "File Import" };
            yield return new ListItem { Key = "export", Text = "File Export" };
        }
        
        public string PluginType
        {
            get => _pluginType ?? Types.FirstOrDefault()?.Key;
            set
            {
                if (Set(ref _pluginType, value))
                {
                    OnPropertyChanged(nameof(ShowFileOptions));
                    OnPropertyChanged(nameof(CanProvideSample));
                    if (!CanProvideSample)
                        IncludeSample = false;
                }
            }
        }

        public bool ShowFileOptions => PluginType == "import" || PluginType == "export";

        protected override string FindLocation(int version) => Global.Helpers.FindRhino(version);
        
        string _fileExtension = ".txt";
        public string FileExtension
        {
            get => _fileExtension;
            set => Set(ref _fileExtension, value);
        }

        string _fileDescription = "Text file";
        public string FileDescription
        {
            get => _fileDescription;
            set => Set(ref _fileDescription, value);
        }
        
        public virtual bool CanProvideSample => PluginType == "utility";

        bool? _includeSample;
        public bool IncludeSample
        {
            get => _includeSample ?? CanProvideSample;
            set => Set(ref _includeSample, value);
        }

        public override void Finish()
        {
            base.Finish();
            if (Host == null)
                return;
            Host.SetParameter("PluginType", PluginType);
            Host.SetParameter("RhinoVersion", RhinoVersion.ToString());
            if (CanProvideSample)
            {
                Host.SetParameter("IncludeSample", IncludeSample.ToString());
            }
            if (ShowFileOptions)
            {
                Host.SetParameter("FileExtension", FileExtension.TrimStart('.'));
                Host.SetParameter("FileDescription", FileDescription);
            }
        }
    }
}