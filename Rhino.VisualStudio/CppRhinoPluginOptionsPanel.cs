using Eto.Forms;
using Eto.Drawing;
using System;
using System.Linq;
using Rhino.VisualStudio.Controls;

namespace Rhino.VisualStudio
{
    public class CppRhinoPluginOptionsPanel : BaseCppRhinoOptionsPageView
    {
        public CppRhinoPluginOptionsPanel(bool showProjectName)
        {
            var commandClassNameTextBox = new TextBox();
            commandClassNameTextBox.TextBinding.BindDataContext((CppRhinoPluginOptionsViewModel m) => m.CommandName);

            // var provideCommandSampleCheckBox = new CheckBox { Text = "Provide command sample", ToolTip = "Check to provide a sample implementation for the command" };
            // provideCommandSampleCheckBox.CheckedBinding.BindDataContext((CppRhinoPluginOptionsViewModel m) => m.IncludeSample);
            // provideCommandSampleCheckBox.BindDataContext(c => c.Enabled, (CppRhinoPluginOptionsViewModel m) => m.CanProvideSample);

            var useAutomationCheckBox = new CheckBox { Text = "Automation", ToolTip = "Exposes DLL objects to scripting tools and other applications." };
            useAutomationCheckBox.CheckedBinding.BindDataContext((CppRhinoPluginOptionsViewModel m) => m.UseAutomation);

            var useSocketsCheckBox = new CheckBox { Text = "Windows Sockets", ToolTip = "Includes support for MFC Windows Sockets in the DLL." };
            useSocketsCheckBox.CheckedBinding.BindDataContext((CppRhinoPluginOptionsViewModel m) => m.UseSockets);

            var useSDLCheckBox = new CheckBox { Text = "Security Development Lifecycle (SDL) checks", ToolTip = "Enable additional Security Development Lifecycle (SDL) checks." };
            useSDLCheckBox.CheckedBinding.BindDataContext((CppRhinoPluginOptionsViewModel m) => m.UseSDL);

            // layout
            var layout = new DynamicLayout { DefaultSpacing = DefaultSpacing, Padding = DefaultPadding };

            // top
            layout.BeginVertical();
            if (showProjectName)
            {
                AddProjectName(layout);
            }
            layout.AddRow("Command name", commandClassNameTextBox);
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
            AddCPPSdkDownloadInfo(information);

            information.AddSpace();

            Information = information;
        }

    }
}