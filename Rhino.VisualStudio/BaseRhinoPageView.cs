using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using Rhino.VisualStudio.Controls;

namespace Rhino.VisualStudio
{
    public class BaseRhinoPageView : BasePageView
    {
        public Size DefaultSpacing { get; set; } = new Size(6, 6);
        public Padding DefaultPadding { get; set; } = new Padding(8);
        
        protected virtual void AddProjectName(DynamicLayout layout)
        {
            var projectNameTextBox = new TextBox();
            projectNameTextBox.TextBinding.BindDataContext((BaseWizardViewModel m) => m.ProjectName);

            var nameValid = new Label { TextColor = Global.Theme.ErrorForeground };
            nameValid.BindDataContext(c => c.Visible, (BaseWizardViewModel m) => m.IsProjectNameInvalid);
            nameValid.BindDataContext(c => c.Text, (BaseWizardViewModel m) => m.ProjectNameValidationText);
            layout.AddRow("Project name", new TableLayout(projectNameTextBox, nameValid));
        }
        
        protected virtual void AddPluginType(DynamicLayout layout)
        {
            // type group
            var typeRadioButtons = new RadioButtonList { Orientation = Orientation.Vertical, Spacing = DefaultSpacing };
            typeRadioButtons.BindDataContext(c => c.DataStore, (BaseRhinoOptionsViewModel m) => m.Types);
            typeRadioButtons.SelectedKeyBinding.BindDataContext((BaseRhinoOptionsViewModel m) => m.PluginType);

            layout.BeginVertical();
            layout.Add(new PanelSeparator("Plug-in type"));
            layout.Add(typeRadioButtons);
            layout.EndVertical();
        }
        
        protected virtual void AddRhinoVersion(DynamicLayout layout)
        {
            var rhinoVersionDropDown = new DropDown();
            rhinoVersionDropDown.BindDataContext(
                c => c.DataStore,
                Binding.Property((BaseRhinoOptionsViewModel m) => m.RhinoVersionsAvailable)
                .Convert(r => r.Select(v => new ListItem { Key = v.ToString(), Text = $"Rhino {v}" })).OfType<object>());
            rhinoVersionDropDown.BindDataContext(c => c.SelectedKey,
                Binding.Property((BaseRhinoOptionsViewModel m) => m.RhinoVersion)
                .Convert(v => v.ToString(), v => int.Parse(v)));

            var rhinoVersionInvalid = new Label { TextColor = Global.Theme.ErrorForeground };
            rhinoVersionInvalid.BindDataContext(c => c.Visible, Binding.Property((BaseRhinoOptionsViewModel m) => m.IsRhinoVersionValid).Convert(v => !v, v => !v));
            rhinoVersionInvalid.BindDataContext(c => c.Text, (BaseRhinoOptionsViewModel m) => m.RhinoVersionValidationText);
            layout.AddSeparateRow("Target Version", TableLayout.AutoSized(rhinoVersionDropDown));
            layout.Add(rhinoVersionInvalid);
        }
         
        protected virtual DynamicTable AddRhinoLocation(DynamicLayout layout)
        {
            // rhino location
            var rhinoLocation = new FilePicker();
            rhinoLocation.Filters.Add(new FileFilter("Rhino.exe", "Rhino.exe"));
            rhinoLocation.BindDataContext(c => c.FilePath, (BaseLocationWizardViewModel m) => m.ExecutableLocation);

            var rhinoLocationInvalid = new Label { Text = "Could not find Rhino.exe at the specified location.", TextColor = Global.Theme.ErrorForeground };
            rhinoLocationInvalid.BindDataContext(c => c.Visible, (BaseLocationWizardViewModel m) => m.IsLocationInvalid);

            var executableLocationGroup = layout.BeginVertical();
            layout.Add(new PanelSeparator("Rhino location"));
            layout.AddColumn(rhinoLocation, rhinoLocationInvalid);
            layout.EndVertical();

            layout.Load += (sender, e) =>
            {
                executableLocationGroup.Table.BindDataContext(c => c.Visible, (BaseLocationWizardViewModel m) => m.ShowExecutableLocation);
            };
            return executableLocationGroup;
        }
         
        protected virtual DynamicTable AddWindowsUI(DynamicLayout layout)
        {
            // wpf/winforms desktop
            var useWpf = new CheckBox { Text = "Use WPF" };
            useWpf.BindDataContext(c => c.Checked, (BaseDesktopWizardViewModel m) => m.UseWpf);

            var useWinForms = new CheckBox { Text = "Use Windows Forms" };
            useWinForms.BindDataContext(c => c.Checked, (BaseDesktopWizardViewModel m) => m.UseWinForms);

            var windowsDesktopUIGroup = layout.BeginVertical();
            layout.Add(new PanelSeparator("Windows UI"));
            layout.BeginVertical();
            layout.Add(useWinForms);
            layout.Add(useWpf);
            layout.EndVertical();
            layout.Add("Note: These options will limit the project to only run/compile on Windows.\nEto.Forms is included by default for cross-platform UI.");
            layout.EndVertical();

            layout.Load += (sender, e) =>
            {
                windowsDesktopUIGroup.Table.BindDataContext(c => c.Visible, (BaseDesktopWizardViewModel m) => m.ShowWindowsDesktop);
            };

            return windowsDesktopUIGroup;
        }
         
        protected virtual DynamicTable AddFileOptions(DynamicLayout layout)
        {
            // file type options
            var extensionTextBox = new TextBox { Width = 100 };
            extensionTextBox.TextBinding.BindDataContext((BaseRhinoOptionsViewModel m) => m.FileExtension);

            var fileDescriptionTextBox = new TextBox();
            fileDescriptionTextBox.TextBinding.BindDataContext((BaseRhinoOptionsViewModel m) => m.FileDescription);

            var fileOptionsGroup = layout.BeginVertical();
            layout.AddRow("Extension", TableLayout.AutoSized(extensionTextBox));
            layout.AddRow("File description", fileDescriptionTextBox);
            layout.EndVertical();
            
            layout.Load += (sender, e) =>
            {
                fileOptionsGroup.Table.BindDataContext(c => c.Enabled, (BaseRhinoOptionsViewModel m) => m.ShowFileOptions);
            };
            return fileOptionsGroup;
        }
         
        protected virtual void AddRhinoDownloadInfo(DynamicLayout information)
        {
            var rhinoDownloadLabel = new Label { TextAlignment = TextAlignment.Center };
            rhinoDownloadLabel.BindDataContext(c => c.Text,
                Binding.Property((BaseLocationWizardViewModel m) => m.RhinoVersion)
                .Convert(v => $"This project requires Rhino 3D to be installed."));
            var rhinoDownload = new LinkButton { Text = "Click here to download Rhino" };
            rhinoDownload.Click += (sender, e) =>
            {
                var version = ((BaseLocationWizardViewModel)DataContext).RhinoVersion;
                Application.Instance.Open($"https://www.rhino3d.com/download/");
            };

            var rhinoDownloadInfo = new TableLayout(
                rhinoDownloadLabel,
                TableLayout.AutoSized(rhinoDownload, centered: true)
            );

            rhinoDownloadInfo.BindDataContext(c => c.Visible, Binding.Property((BaseLocationWizardViewModel m) => m.IsRhinoAvailable).Convert(v => !v));

            information.Add(rhinoDownloadInfo);
        }
         
    }
}