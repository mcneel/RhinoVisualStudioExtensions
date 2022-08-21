using MonoDevelop.Ide.Templates;
using Eto.Forms;

namespace Rhino.VisualStudio.Mac.Wizard
{
    class EtoWizardPage : WizardPage, IWizardHost
    {
        Control _control;
        string _title;
        BaseTemplateWizard _wizard;
        public EtoWizardPage(BaseTemplateWizard wizard, Control control, string title)
        {
            _control = control;
            _title = title;
            _wizard = wizard;
        }

        public override string Title => _title;

        public void Abort() => _control.ParentWindow?.Close();

        public void Continue() => OnNextPageRequested();

        public string GetParameter(string name) => _wizard.Parameters[name];

        public bool IsSupportedParameter(string name) => _wizard.IsSupportedParameter(name);

        public void SetParameter(string name, string value) => _wizard.Parameters[name] = value;

        protected override object CreateNativeWidget<T>()
        {
#if VS2022
            _control.Size = new Eto.Drawing.Size(900, 496);
#endif
            var native = _control.ToNative(true);
            Application.Instance.AsyncInvoke(() => native.Window?.RecalculateKeyViewLoop());
            return native;
        }
    }
}