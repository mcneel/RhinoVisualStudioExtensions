using Eto.Forms;
using Eto.Drawing;
using System;

namespace Rhino.VisualStudio
{
  public class RhinoCommonOptionsPanel : BasePageView
  {
    public RhinoCommonOptionsPanel(bool showProjectName)
    {
      var spacing = new Size(6, 6);
      var padding = new Padding(8);

      var projectNameTextBox = new TextBox();
      projectNameTextBox.TextBinding.BindDataContext((RhinoCommonOptionsViewModel m) => m.ProjectName);

      var nameValid = new Label { TextColor = Global.Theme.ErrorForeground };
      nameValid.BindDataContext(c => c.Visible, (RhinoCommonOptionsViewModel m) => m.IsProjectNameInvalid);
      nameValid.BindDataContext(c => c.Text, (RhinoCommonOptionsViewModel m) => m.ProjectNameValidationText);

      var pluginClassNameTextBox = new TextBox();
      pluginClassNameTextBox.TextBinding.BindDataContext((RhinoCommonOptionsViewModel m) => m.PluginClassName);

      var commandClassNameTextBox = new TextBox();
      commandClassNameTextBox.TextBinding.BindDataContext((RhinoCommonOptionsViewModel m) => m.CommandClassName);

      var extensionTextBox = new TextBox { Width = 100 };
      extensionTextBox.TextBinding.BindDataContext((RhinoCommonOptionsViewModel m) => m.FileExtension);

      var fileDescriptionTextBox = new TextBox();
      fileDescriptionTextBox.TextBinding.BindDataContext((RhinoCommonOptionsViewModel m) => m.FileDescription);

      var provideCommandSampleCheckBox = new CheckBox { Text = "Provide command sample", ToolTip = "Check to provide a sample implementation for the command" };
      provideCommandSampleCheckBox.CheckedBinding.BindDataContext((RhinoCommonOptionsViewModel m) => m.IncludeSample);
      provideCommandSampleCheckBox.BindDataContext(c => c.Enabled, (RhinoCommonOptionsViewModel m) => m.CanProvideSample);

      var typeRadioButtons = new RadioButtonList { Orientation = Orientation.Vertical, Spacing = spacing };
      typeRadioButtons.BindDataContext(c => c.DataStore, (RhinoCommonOptionsViewModel m) => m.Types);
      typeRadioButtons.SelectedKeyBinding.BindDataContext((RhinoCommonOptionsViewModel m) => m.PluginType);


      // rhino location
      var rhinoLocation = new FilePicker();
      rhinoLocation.Filters.Add(new FileFilter("Rhino.exe", "Rhino.exe"));
      rhinoLocation.BindDataContext(c => c.FilePath, (BaseLocationWizardViewModel m) => m.ExecutableLocation);

      var rhinoLocationInvalid = new Label { Text = "Could not find Rhino.exe at the specified location.", TextColor = Global.Theme.ErrorForeground };
      rhinoLocationInvalid.BindDataContext(c => c.Visible, (BaseLocationWizardViewModel m) => m.IsLocationInvalid);

      Styles.Add<GroupBox>(null, g => g.Padding = padding);

      // layout
      var layout = new DynamicLayout { DefaultSpacing = spacing, Padding = padding };

      // top
      layout.BeginVertical();
      if (showProjectName)
      {
        layout.AddRow("Project name", new TableLayout(projectNameTextBox, nameValid));
      }
      layout.AddRow("Plug-in class name", pluginClassNameTextBox);
      layout.AddRow("Command class name", commandClassNameTextBox);
      layout.EndVertical();

      // type group
      layout.BeginGroup("Plug-in type");
      layout.Add(typeRadioButtons);
      layout.EndGroup();

      layout.BeginGroup("Options");

      layout.Add(provideCommandSampleCheckBox);

      // file type options
      var fileOptionsGroup = layout.BeginVertical();
      layout.AddRow("Extension", TableLayout.AutoSized(extensionTextBox));
      layout.AddRow("File description", fileDescriptionTextBox);
      layout.EndVertical();
      layout.EndGroup();

      var executableLocationGroup = layout.BeginGroup("Rhino.exe location");
      layout.AddColumn(rhinoLocation, rhinoLocationInvalid);
      layout.EndGroup();


      Content = layout;

      Load += (sender, e) =>
      {
        fileOptionsGroup.Table.BindDataContext(c => c.Enabled, (RhinoCommonOptionsViewModel m) => m.ShowFileOptions);
        executableLocationGroup.GroupBox.BindDataContext(c => c.Visible, (BaseLocationWizardViewModel m) => m.ShowExecutableLocation);
      };
    }
  }
}