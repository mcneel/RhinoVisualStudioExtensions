using Eto.Forms;
using Eto.Drawing;
using System;
using System.Linq;
using Rhino.VisualStudio.Controls;

namespace Rhino.VisualStudio
{
    public class CppRhinoPluginOptionsPanel : BaseRhinoPageView
    {
        public CppRhinoPluginOptionsPanel(bool showProjectName)
        {
            var padding = new Padding(8);


            var pluginClassNameTextBox = new TextBox();
            pluginClassNameTextBox.TextBinding.BindDataContext((CppRhinoPluginOptionsViewModel m) => m.PluginClassName);

            var commandClassNameTextBox = new TextBox();
            commandClassNameTextBox.TextBinding.BindDataContext((CppRhinoPluginOptionsViewModel m) => m.CommandClassName);

            // var provideCommandSampleCheckBox = new CheckBox { Text = "Provide command sample", ToolTip = "Check to provide a sample implementation for the command" };
            // provideCommandSampleCheckBox.CheckedBinding.BindDataContext((CppRhinoPluginOptionsViewModel m) => m.IncludeSample);
            // provideCommandSampleCheckBox.BindDataContext(c => c.Enabled, (CppRhinoPluginOptionsViewModel m) => m.CanProvideSample);

            var useAutomationCheckBox = new CheckBox { Text = "Automation", ToolTip = "Exposes DLL objects to scripting tools and other applications." };
            useAutomationCheckBox.CheckedBinding.BindDataContext((CppRhinoPluginOptionsViewModel m) => m.UseAutomation);

            var useSocketsCheckBox = new CheckBox { Text = "Windows Sockets", ToolTip = "Includes support for MFC Windows Sockets in the DLL." };
            useSocketsCheckBox.CheckedBinding.BindDataContext((CppRhinoPluginOptionsViewModel m) => m.UseSockets);

            var useSDLCheckBox = new CheckBox { Text = "Security Development Lifecycle (SDL) checks", ToolTip = "Enable additional Security Development Lifecycle (SDL) checks." };
            useSDLCheckBox.CheckedBinding.BindDataContext((CppRhinoPluginOptionsViewModel m) => m.UseSDL);

            Styles.Add<GroupBox>(null, g => g.Padding = padding);


            // Link to download the Rhino SDK
            var rhinoSdkDownloadLabel = new Label { TextAlignment = TextAlignment.Center };
            rhinoSdkDownloadLabel.BindDataContext(c => c.Text,
                Binding.Property((CppRhinoPluginOptionsViewModel m) => m.RhinoVersion)
                .Convert(v => v > 0 ? $"This project requires the\nC++ SDK for Rhino {v} to be installed." : "This project requires the Rhino C++ SDK to be installed."));
            var rhinoSdkDownload = new LinkButton { Text = "Click here to download the C++ SDK" };
            rhinoSdkDownload.Click += (sender, e) =>
            {
                var version = ((CppRhinoPluginOptionsViewModel)DataContext).RhinoVersion;
                if (version == 0)
                    version = Global.LatestSdkRelease;
                Application.Instance.Open($"https://www.rhino3d.com/download/Rhino-SDK/{version}.0/latest/");
            };

            var rhinoSdkInfo = new TableLayout(
                rhinoSdkDownloadLabel,
                TableLayout.AutoSized(rhinoSdkDownload, centered: true)
            );

            rhinoSdkInfo.BindDataContext(c => c.Visible, Binding.Property((CppRhinoPluginOptionsViewModel m) => m.IsSdkPathValid).Convert(v => !v));

            // layout
            var layout = new DynamicLayout { DefaultSpacing = DefaultSpacing, Padding = padding };

            // top
            layout.BeginVertical();
            if (showProjectName)
            {
                AddProjectName(layout);
            }
            layout.AddRow("Plug-in class name", pluginClassNameTextBox);
            layout.AddRow("Command class name", commandClassNameTextBox);
            layout.EndVertical();

            AddPluginType(layout);

            layout.BeginVertical();
            layout.Add(new PanelSeparator("Options"));

            AddRhinoVersion(layout);

            // layout.Add(provideCommandSampleCheckBox);
            layout.Add(useAutomationCheckBox);
            layout.Add(useSocketsCheckBox);
            layout.Add(useSDLCheckBox);

            AddFileOptions(layout);

            Content = layout;

            var information = new DynamicLayout();
            information.AddSpace();
            AddRhinoDownloadInfo(information);
            information.Add(rhinoSdkInfo);

            information.AddSpace();

            Information = information;
        }

    }
}