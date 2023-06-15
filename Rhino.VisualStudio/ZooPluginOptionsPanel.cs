using Eto.Forms;
using Eto.Drawing;
using System;
using Rhino.VisualStudio.Controls;

namespace Rhino.VisualStudio
{
  public class ZooPluginOptionsPanel : BasePageView
  {
    public ZooPluginOptionsPanel(bool showProjectName)
    {
      var spacing = new Size(6, 6);
      var padding = new Padding(8);

      var projectNameTextBox = new TextBox();
      projectNameTextBox.TextBinding.BindDataContext((ZooPluginOptionsViewModel m) => m.ProjectName);

      var nameValid = new Label { TextColor = Global.Theme.ErrorForeground };
      nameValid.BindDataContext(c => c.Visible, (ZooPluginOptionsViewModel m) => m.IsProjectNameInvalid);
      nameValid.BindDataContext(c => c.Text, (ZooPluginOptionsViewModel m) => m.ProjectNameValidationText);

      var zooPluginClassName = new TextBox();
      zooPluginClassName.TextBinding.BindDataContext((ZooPluginOptionsViewModel m) => m.PluginClassName);

      var rhinoPluginId = new TextBox();
      rhinoPluginId.TextBinding.BindDataContext((ZooPluginOptionsViewModel m) => m.RhinoPluginId);

      var rhinoPluginIdValid = new Label { Text = "Enter a valid GUID", TextColor = Global.Theme.ErrorForeground };
      rhinoPluginIdValid.BindDataContext(c => c.Visible, (ZooPluginOptionsViewModel m) => m.IsRhinoPluginIdInvalid);


      // zoo plugin location
      var zooLocation = new FilePicker();
      zooLocation.Filters.Add(new FileFilter("ZooPlugin.dll", "ZooPlugin.dll"));
      zooLocation.BindDataContext(c => c.FilePath, (BaseLocationWizardViewModel m) => m.ExecutableLocation);

      var zooLocationInvalid = new Label { Text = "Could not find ZooPlugin.dll at the specified location.", TextColor = Global.Theme.ErrorForeground };
      zooLocationInvalid.BindDataContext(c => c.Visible, (BaseLocationWizardViewModel m) => m.IsLocationInvalid);

      Styles.Add<GroupBox>(null, g => g.Padding = padding);

      // layout
      var layout = new DynamicLayout { DefaultSpacing = spacing, Padding = padding };

      // top
      layout.BeginVertical();
      if (showProjectName)
      {
        layout.AddRow("Project name", new TableLayout(projectNameTextBox, nameValid));
      }
      layout.AddRow("Zoo Plug-in class name", zooPluginClassName);
      layout.EndVertical();

      // type group
      layout.BeginVertical();
      layout.Add(new PanelSeparator("Linked ID"));
      layout.AddCentered(@"The unique identifier, or PlugInId, of your existing Rhino plug-in.
- C++ SDK: returned by the CRhinoPlugIn::PlugInID() override.
- old.NET SDK: the value returned by MRhinoPlugIn.PlugInID().
- RhinoCommon SDK: stored in the ""Guid"" assembly attribute.");
      layout.BeginVertical();
      layout.BeginHorizontal();
      layout.Add("Linked Rhino Plug-in ID");
      layout.AddColumn(rhinoPluginId, rhinoPluginIdValid);
      layout.EndHorizontal();
      layout.EndVertical();
      layout.EndVertical();

      var executableLocationGroup = layout.BeginVertical();
      layout.Add(new PanelSeparator("ZooPlugin.dll location"));
      layout.AddColumn(zooLocation, zooLocationInvalid);
      layout.EndVertical();

      Content = layout;

      Load += (sender, e) =>
      {
        executableLocationGroup.Table.BindDataContext(c => c.Visible, (BaseLocationWizardViewModel m) => m.ShowExecutableLocation);
      };
    }
  }
}