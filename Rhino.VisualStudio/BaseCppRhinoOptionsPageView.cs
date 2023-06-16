using Eto.Forms;

namespace Rhino.VisualStudio
{
    public abstract class BaseCppRhinoOptionsPageView : BaseRhinoPageView
    {
        protected virtual void AddCPPSdkDownloadInfo(DynamicLayout information)
        {
            // Link to download the Rhino SDK
            var rhinoSdkDownloadLabel = new Label { TextAlignment = TextAlignment.Center };
            rhinoSdkDownloadLabel.BindDataContext(c => c.Text,
                Binding.Property((CppRhinoSkinOptionsViewModel m) => m.RhinoVersion)
                .Convert(v => v > 0 ? $"This project requires the\nC++ SDK for Rhino {v} to be installed." : "This project requires the Rhino C++ SDK to be installed."));
            var rhinoSdkDownload = new LinkButton { Text = "Click here to download the C++ SDK" };
            rhinoSdkDownload.Click += (sender, e) =>
            {
                var version = ((CppRhinoSkinOptionsViewModel)DataContext).RhinoVersion;
                if (version == 0)
                    version = Global.LatestSdkRelease;
                Application.Instance.Open($"https://www.rhino3d.com/download/Rhino-SDK/{version}.0/latest/");
            };

            var rhinoSdkInfo = new TableLayout(
                rhinoSdkDownloadLabel,
                TableLayout.AutoSized(rhinoSdkDownload, centered: true)
            );

            rhinoSdkInfo.BindDataContext(c => c.Visible, Binding.Property((CppRhinoSkinOptionsViewModel m) => m.IsSdkPathValid).Convert(v => !v));
            information.Add(rhinoSdkInfo);
        }
        
    }
}