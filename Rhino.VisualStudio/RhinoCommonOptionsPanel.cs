using Eto.Forms;
using Eto.Drawing;
using System;
using Rhino.VisualStudio.Controls;

namespace Rhino.VisualStudio
{
    public class RhinoCommonOptionsPanel : BaseRhinoPageView
    {
        public RhinoCommonOptionsPanel(bool showProjectName)
        {
            var padding = new Padding(8);

            var pluginClassNameTextBox = new TextBox();
            pluginClassNameTextBox.TextBinding.BindDataContext((RhinoCommonOptionsViewModel m) => m.PluginClassName);

            var commandClassNameTextBox = new TextBox();
            commandClassNameTextBox.TextBinding.BindDataContext((RhinoCommonOptionsViewModel m) => m.CommandClassName);

            var provideCommandSampleCheckBox = new CheckBox { Text = "Provide command sample", ToolTip = "Check to provide a sample implementation for the command" };
            provideCommandSampleCheckBox.CheckedBinding.BindDataContext((RhinoCommonOptionsViewModel m) => m.IncludeSample);
            provideCommandSampleCheckBox.BindDataContext(c => c.Enabled, (RhinoCommonOptionsViewModel m) => m.CanProvideSample);


            Styles.Add<GroupBox>(null, g => g.Padding = padding);

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
            layout.Add(provideCommandSampleCheckBox);

            AddFileOptions(layout);
            layout.EndVertical();


            AddWindowsUI(layout);

            // AddRhinoLocation(layout);

            Content = layout;

            var information = new DynamicLayout();
            information.AddSpace();
            AddRhinoDownloadInfo(information);
            information.AddSpace();

            Information = information;
        }
    }
}