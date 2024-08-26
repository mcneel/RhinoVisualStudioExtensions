namespace Rhino.VisualStudio
{
    public abstract class BaseDesktopWizardViewModel : BaseLocationWizardViewModel
    {
        public bool ShowWindowsDesktop => Host.IsSupportedParameter("WindowsDesktop");
        public bool SupportsYak => Host.IsSupportedParameter("Yak");
        public bool SupportsVSCode => Host.IsSupportedParameter("VSCode");

        bool _useWpf;
        public bool UseWpf
        {
            get => _useWpf;
            set => Set(ref _useWpf, value);
        }

        bool _useWinForms;
        public bool UseWinForms
        {
            get => _useWinForms;
            set => Set(ref _useWinForms, value);
        }

        bool _buildYak;
        public bool BuildYak
        {
            get => _buildYak;
            set => Set(ref _buildYak, value);
        }

        bool _includeVSCode = true;
        public bool IncludeVSCode
        {
            get => _includeVSCode;
            set => Set(ref _includeVSCode, value);
        }

        public override void Finish()
        {
            base.Finish();
            if (Host == null)
                return;
                
            if (ShowWindowsDesktop)
            {
                Host.SetParameter("UseWpf", UseWpf.ToString());
                Host.SetParameter("UseWinForms", UseWinForms.ToString());
            }

            if (SupportsYak)
            {
                Host.SetParameter("BuildYak", BuildYak.ToString());
            }
            if (SupportsVSCode)
            {
                Host.SetParameter("IncludeVSCode", IncludeVSCode.ToString());
            }
        }
    }
}