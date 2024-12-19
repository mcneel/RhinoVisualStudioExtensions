using Eto.Forms;
using Eto.Drawing;
using System;
using Rhino.VisualStudio.Controls;

namespace Rhino.VisualStudio
{
    public class Grasshopper2OptionsPanel : BaseRhinoPageView
    {
        public Grasshopper2OptionsPanel(bool showProjectName)
        {
            var spacing = new Size(6, 6);
            var padding = new Padding(8);

            var projectNameTextBox = new TextBox();
            projectNameTextBox.TextBinding.BindDataContext((Grasshopper2OptionsViewModel m) => m.ProjectName);

            var nameValid = new Label { TextColor = Global.Theme.ErrorForeground };
            nameValid.BindDataContext(c => c.Visible, (Grasshopper2OptionsViewModel m) => m.IsProjectNameInvalid);
            nameValid.BindDataContext(c => c.Text, (Grasshopper2OptionsViewModel m) => m.ProjectNameValidationText);

            var pluginDisplayNameTextBox = new TextBox();
            pluginDisplayNameTextBox.TextBinding.BindDataContext((Grasshopper2OptionsViewModel m) => m.PlugInDisplayName);

            var componentClassNameTextBox = new TextBox();
            componentClassNameTextBox.TextBinding.BindDataContext((Grasshopper2OptionsViewModel m) => m.ComponentClassName);

            var provideCommandSampleCheckBox = new CheckBox { Text = "Provide sample code", ToolTip = "Check to provide a sample implementation for the component" };
            provideCommandSampleCheckBox.CheckedBinding.BindDataContext((Grasshopper2OptionsViewModel m) => m.IncludeSample);


            var componentNameTextBox = new TextBox();
            componentNameTextBox.TextBinding.BindDataContext((Grasshopper2OptionsViewModel m) => m.ComponentName);

            var componentChapterTextBox = new TextBox();
            componentChapterTextBox.TextBinding.BindDataContext((Grasshopper2OptionsViewModel m) => m.ComponentChapter);

            var componentSectionTextBox = new TextBox();
            componentSectionTextBox.TextBinding.BindDataContext((Grasshopper2OptionsViewModel m) => m.ComponentSection);

            var componentInfoTextBox = new TextBox();
            componentInfoTextBox.TextBinding.BindDataContext((Grasshopper2OptionsViewModel m) => m.ComponentInfo);

            // rhino location
            var rhinoLocation = new FilePicker();
            rhinoLocation.Filters.Add(new FileFilter("Rhino.exe", "Rhino.exe"));
            rhinoLocation.BindDataContext(c => c.FilePath, (BaseLocationWizardViewModel m) => m.ExecutableLocation);

            var rhinoLocationInvalid = new Label { Text = "Could not find Rhino.exe at the specified location.", TextColor = Global.Theme.ErrorForeground };
            rhinoLocationInvalid.BindDataContext(c => c.Visible, (BaseLocationWizardViewModel m) => m.IsLocationInvalid);


            // styles
            Styles.Add<GroupBox>(null, g => g.Padding = padding);

            // layout
            var layout = new DynamicLayout { DefaultSpacing = spacing, Padding = padding };

            layout.AddCentered("Adds the first component that has an icon to the toolbar.");
            // top
            layout.BeginVertical();
            if (showProjectName)
            {
                layout.AddRow("Project name", new TableLayout(projectNameTextBox, nameValid));
            }
            layout.AddRow("Plug-In display name", pluginDisplayNameTextBox);
            layout.AddRow("Component class name", componentClassNameTextBox);
            layout.EndVertical();

            // type group
            layout.BeginVertical();
            layout.Add(new PanelSeparator("First component"));;
            layout.AddRow("Name", componentNameTextBox);
            layout.BeginHorizontal();
            layout.Add("Chapter");
            layout.BeginVertical();
            layout.BeginHorizontal();
            layout.Add(componentChapterTextBox, xscale: true);
            layout.Add("Section");
            layout.Add(componentSectionTextBox, xscale: true);
            layout.EndHorizontal();
            layout.EndVertical();
            layout.AddRow("Description", componentInfoTextBox);

            layout.EndVertical();

            layout.BeginVertical();
            layout.Add(new PanelSeparator("Options"));
            AddRhinoVersion(layout);
            AddBuildYakPackage(layout);
            AddIncludeVSCode(layout);
            layout.Add(provideCommandSampleCheckBox);
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