using System;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Components;
using MonoDevelop.Ide.Gui.Dialogs;
using MonoDevelop.Projects;

namespace MonoDevelop.Debugger.Soft.Rhino.OptionPanels
{
  internal class RhinoOptionsPanel : ItemOptionsPanel
  {
    RhinoOptionsPanelWidget widget;

    public override bool IsVisible()
    {
      return ConfiguredProject.GetMcNeelProjectType(false) != null
      || ConfiguredProject.ProjectProperties.GetProperty(Helpers.RhinoPluginTypeProperty) != null;
    }

    public override Control CreatePanelWidget()
    {
      widget = new RhinoOptionsPanelWidget((DotNetProject)ConfiguredProject, ItemConfigurations);

      return widget;
    }

    public override void ApplyChanges()
    {
      widget.Store();
    }
  }

  class RhinoOptionsPanelWidget : Gtk.VBox
  {
    Gtk.ComboBox pluginTypeCombo;
    Gtk.ComboBox launcherCombo;
    Gtk.Entry customLauncherEntry;
    Gtk.Label autodetectedTypeLabel;
    bool pluginTypeChanged;
    bool launcherChanged;
    DotNetProject project;
    bool supportsDevelopment;
    string[] currentLauncherEntries = defaultLauncherEntries;
    static string[] typeEntries = { "Autodetect", "Rhino Plugin", "Grasshopper Component", "Library" };
    static string[] defaultLauncherEntries = { "Autodetect", "Rhinoceros", "RhinoWIP", "Custom" };
    static string[] debugLauncherEntries = { "XCode" };

    string GetSelectedPluginType()
    {
      switch (pluginTypeCombo.Active)
      {
        case 0:
          return null;
        case 1:
          return "rhp";
        case 2:
          return "gha";
        case 3:
        default:
          return "none";
      }
    }

    int GetPluginComboIndex()
    {
      var type = project.GetPluginProjectType();
      if (type != null)
      {
        switch (type.Value)
        {
          case McNeelProjectType.None:
            return 3;
          case McNeelProjectType.RhinoCommon:
            return 1;
          case McNeelProjectType.Grasshopper:
            return 2;
        }
      }
      return 0;
    }

    int GetLauncherComboIndex()
    {
      var type = project.ProjectProperties.GetValue(Helpers.RhinoLauncherProperty);
      if (!string.IsNullOrEmpty(type))
      {
        switch (type.ToLowerInvariant())
        {
          case "app":
            return 1;
          case "wip":
            return 2;
          case "xcode":
            return supportsDevelopment ? 4 : 0;
          default:
            // custom
            return 3;
        }
      }
      return 0;
    }

    string GetTypeLabel(McNeelProjectType type)
    {
      switch (type)
      {
        case McNeelProjectType.None:
          return "Library";
        case McNeelProjectType.DebugStarter:
          return "Rhino Development";
        case McNeelProjectType.RhinoCommon:
          return "Rhino Plugin";
        case McNeelProjectType.Grasshopper:
          return "Grasshopper Component";
        default:
          return null;
      }
    }

    string GetSelectedLauncher()
    {
      switch (launcherCombo.Active)
      {
        default:
        case 0:
          return null;
        case 1:
          return "app";
        case 2:
          return "wip";
        case 3:
          return customLauncherEntry.Text;
        case 4:
          return "xcode";
      }
    }

    public void Store()
    {
      if (pluginTypeChanged)
      {
        var pluginType = GetSelectedPluginType();
        if (pluginType != null)
          project.ProjectProperties.SetValue(Helpers.RhinoPluginTypeProperty, pluginType);
        else
          project.ProjectProperties.RemoveProperty(Helpers.RhinoPluginTypeProperty);

        project.NeedsReload = true;
      }

      if (launcherChanged)
      {
        var launcherType = GetSelectedLauncher();
        if (launcherType != null)
          project.ProjectProperties.SetValue(Helpers.RhinoLauncherProperty, launcherType);
        else
          project.ProjectProperties.RemoveProperty(Helpers.RhinoLauncherProperty);
      }
    }

    public RhinoOptionsPanelWidget(DotNetProject project, IEnumerable<ItemConfiguration> configurations)
    {
      supportsDevelopment = project.GetMcNeelProjectType() == McNeelProjectType.DebugStarter;

      if (supportsDevelopment)
        currentLauncherEntries = currentLauncherEntries.Concat(debugLauncherEntries).ToArray();

      this.project = project;
      Build();

      pluginTypeCombo.Active = GetPluginComboIndex();
      pluginTypeCombo.Changed += (sender, e) =>
      {
        pluginTypeChanged = true;
        SetAutodetectLabel();
      };
      SetAutodetectLabel();


      launcherCombo.Changed += (sender, e) =>
      {
        launcherChanged = true;
        SetCustomLauncherText();
      };
      launcherCombo.Active = GetLauncherComboIndex();
      SetCustomLauncherText();
    }

    void SetCustomLauncherText()
    {
      customLauncherEntry.IsEditable = launcherCombo.Active == 3; // custom
      var outputFileName = project.GetOutputFileName(project.DefaultConfiguration.Selector);
      switch (launcherCombo.Active)
      {
        case 0:
          // auto detect
          var version = project.GetRhinoVersion() ?? Helpers.DefaultRhinoVersion;
          var appPath = project.DetectApplicationPath(outputFileName, version);
          customLauncherEntry.Text = appPath;
          break;
        case 1:
          customLauncherEntry.Text = Helpers.StandardInstallPath;
          break;
        case 2:
          customLauncherEntry.Text = Helpers.StandardInstallWipPath;
          break;
        case 3:
          var currentLauncherIndex = GetLauncherComboIndex();
          if (currentLauncherIndex == 3) // custom
            customLauncherEntry.Text = project.ProjectProperties.GetValue(Helpers.RhinoLauncherProperty);
          else
            customLauncherEntry.Text = string.Empty;
          break;
        case 4:
          customLauncherEntry.Text = Helpers.GetXcodeDerivedDataPath(outputFileName);
          break;
      }
    }

    void SetAutodetectLabel()
    {
      if (pluginTypeCombo.Active == 0) // autodetect
      {
        var detectedType = project.GetMcNeelProjectType(false);
        if (detectedType != null)
        {
          autodetectedTypeLabel.Markup = $"Detected: <b>{GetTypeLabel(detectedType.Value)}</b>";
        }
      }
      else
      {
        autodetectedTypeLabel.Text = string.Empty;
      }
    }

    void Build()
    {
      Spacing = 6;

      //var heading = new Gtk.Label("<b>Rhino Options</b>");
      //heading.UseMarkup = true;
      //heading.Xalign = 0;



      pluginTypeCombo = new Gtk.ComboBox(typeEntries);

      launcherCombo = new Gtk.ComboBox(currentLauncherEntries);

      customLauncherEntry = new Gtk.Entry();

      var optionsBox = new Gtk.HBox();

      autodetectedTypeLabel = new Gtk.Label();

      // layout

      //PackStart(heading, false, false, 0);


      var table = new Gtk.Table(3, 3, false);
      table.RowSpacing = 6;
      table.ColumnSpacing = 6;

      table.Attach(new Gtk.Label("Plugin Type:") { Xalign = 1 }, 0, 1, 0, 1);
      table.Attach(AutoSized(6, pluginTypeCombo, autodetectedTypeLabel), 1, 2, 0, 1);

      table.Attach(new Gtk.Label("Launcher:") { Xalign = 1 }, 0, 1, 1, 2);
      table.Attach(AutoSized(6, launcherCombo), 1, 2, 1, 2);

      table.Attach(new Gtk.Label(), 0, 1, 2, 3);
      table.Attach(customLauncherEntry, 1, 2, 2, 3);

      // indent
      optionsBox.PackStart(new Gtk.Label { WidthRequest = 18 }, false, false, 0);
      optionsBox.PackStart(table, false, true, 0);

      PackStart(optionsBox, true, true, 0);

      ShowAll();
    }

    Gtk.HBox AutoSized(int spacing, params Gtk.Widget[] widgets)
    {
      var box = new Gtk.HBox();
      box.Spacing = spacing;
      foreach (var widget in widgets)
      {
        box.PackStart(widget, false, true, 0);
      }
      return box;
    }
  }
}
