using Eto.Forms;
using Eto.Drawing;
using System;
using Rhino.VisualStudio.Controls;

namespace Rhino.VisualStudio
{
    public class GrasshopperOptionsPanel : BaseRhinoPageView
    {
        public GrasshopperOptionsPanel(bool showProjectName)
        {
            var spacing = new Size(6, 6);
            var padding = new Padding(8);

            var projectNameTextBox = new TextBox();
            projectNameTextBox.TextBinding.BindDataContext((GrasshopperOptionsViewModel m) => m.ProjectName);

            var nameValid = new Label { TextColor = Global.Theme.ErrorForeground };
            nameValid.BindDataContext(c => c.Visible, (GrasshopperOptionsViewModel m) => m.IsProjectNameInvalid);
            nameValid.BindDataContext(c => c.Text, (GrasshopperOptionsViewModel m) => m.ProjectNameValidationText);

            var addonDisplayNameTextBox = new TextBox();
            addonDisplayNameTextBox.TextBinding.BindDataContext((GrasshopperOptionsViewModel m) => m.AddonDisplayName);

            var componentClassNameTextBox = new TextBox();
            componentClassNameTextBox.TextBinding.BindDataContext((GrasshopperOptionsViewModel m) => m.ComponentClassName);

            var provideCommandSampleCheckBox = new CheckBox { Text = "Provide sample code", ToolTip = "Check to provide a sample implementation for the component" };
            provideCommandSampleCheckBox.CheckedBinding.BindDataContext((GrasshopperOptionsViewModel m) => m.IncludeSample);


            var componentNameTextBox = new TextBox();
            componentNameTextBox.TextBinding.BindDataContext((GrasshopperOptionsViewModel m) => m.ComponentName);

            var componentNicknameTextBox = new TextBox();
            componentNicknameTextBox.TextBinding.BindDataContext((GrasshopperOptionsViewModel m) => m.ComponentNickname);

            var componentCategoryTextBox = new TextBox();
            componentCategoryTextBox.TextBinding.BindDataContext((GrasshopperOptionsViewModel m) => m.ComponentCategory);

            var componentSubcategoryTextBox = new TextBox();
            componentSubcategoryTextBox.TextBinding.BindDataContext((GrasshopperOptionsViewModel m) => m.ComponentSubcategory);

            var componentDescriptionTextBox = new TextBox();
            componentDescriptionTextBox.TextBinding.BindDataContext((GrasshopperOptionsViewModel m) => m.ComponentDescription);

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
            layout.AddRow("Add-on display name", addonDisplayNameTextBox);
            layout.AddRow("Component class name", componentClassNameTextBox);
            layout.EndVertical();

            // type group
            layout.BeginVertical();
            layout.Add(new PanelSeparator("First component"));;
            layout.AddRow("Name", componentNameTextBox);
            layout.AddRow("Nickname", componentNicknameTextBox);
            layout.BeginHorizontal();
            layout.Add("Category");
            layout.BeginVertical();
            layout.BeginHorizontal();
            layout.Add(componentCategoryTextBox, xscale: true);
            layout.Add("Subcategory");
            layout.Add(componentSubcategoryTextBox, xscale: true);
            layout.EndHorizontal();
            layout.EndVertical();
            layout.AddRow("Description", componentDescriptionTextBox);

            layout.EndVertical();

            layout.BeginVertical();
            layout.Add(new PanelSeparator("Options"));
            AddRhinoVersion(layout);
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