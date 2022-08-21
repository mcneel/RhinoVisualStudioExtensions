using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MonoDevelop.Components;
using MonoDevelop.Ide.Gui.Dialogs;
using MonoDevelop.Projects;
using Eto.Forms;
using Eto.Drawing;
using Eto.Mac;
using CoreGraphics;

namespace Rhino.VisualStudio.Mac.OptionPanels
{
    internal class RhinoOptionsPanel : ItemOptionsPanel
    {

        static RhinoOptionsPanel()
        {
            EtoInitializer.Initialize();
        }
        
        public RhinoOptionsPanel()
        {
#if VS2022
            IsView = true;
#endif
        }

        RhinoOptionsPanelWidget widget;
        MonoDevelop.Components.Control control;

        public override bool IsVisible()
        {
            return ConfiguredProject.DetectMcNeelProjectType() != null || ConfiguredProject.GetPluginProjectType() != null;
        }
        
        public class EtoContainerView : AppKit.NSView
        {
            public override CGSize IntrinsicContentSize => PreferredSize;

            public CGSize PreferredSize { get; set; }
            
            Eto.Forms.Control _control;
            
            public EtoContainerView(Eto.Forms.Control control)
            {
                _control = control;
                
                var native = control.ToNative(true);
                native.AutoresizingMask = AppKit.NSViewResizingMask.WidthSizable | AppKit.NSViewResizingMask.HeightSizable;
                native.TranslatesAutoresizingMaskIntoConstraints = true;
                Application.Instance.AsyncInvoke(() => native.Window?.RecalculateKeyViewLoop());
                
                var size = control.GetPreferredSize().ToNS();
                native.SetFrameSize(size);
                SetFrameSize(size);
                AddSubview(native);
                PreferredSize = size;
            }
        }


        public override MonoDevelop.Components.Control CreatePanelWidget()
        {
            if (widget == null)
            {
                widget = new RhinoOptionsPanelWidget((DotNetProject)ConfiguredProject, ItemConfigurations);
            }

            var containerView = new EtoContainerView(widget);

            control = containerView;

            return control;
        }

        public override void ApplyChanges()
        {
            widget.Store();
        }
    }

    class RhinoOptionsPanelWidget : StackLayout
    {
        DropDown pluginTypeCombo;
        DropDown launcherCombo;
        TextBox customLauncherEntry;
        Button browseButton;
        Panel autodetectedTypeLabel;
        Panel bundleInformationLabel;
        bool pluginTypeChanged;
        bool launcherChanged;
        DotNetProject project;
        bool supportsDevelopment;
        string[] currentLauncherEntries = defaultLauncherEntries;
        static string[] typeEntries = { "Autodetect", "Rhino Plugin", "Grasshopper Component", "Library" };
        static string[] defaultLauncherEntries = { "Autodetect", /*"Rhinoceros", "RhinoWIP",*/ "Custom" };
        static string[] debugLauncherEntries = { "XCode" };

        string GetSelectedPluginType()
        {
            switch (pluginTypeCombo.SelectedIndex)
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
                    // case "app":
                    //   return 1;
                    // case "wip":
                    //   return 2;
                    case "xcode":
                        return supportsDevelopment ? 2 : 0;
                    default:
                        // custom
                        return 1;
                }
            }
            return 0; // auto
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
            switch (launcherCombo.SelectedIndex)
            {
                default:
                case 0:
                    return null;
                // case 1:
                //   return "app";
                // case 2:
                //   return "wip";
                case 1:
                    return customLauncherEntry.Text;
                case 2:
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
                project.NotifyModified(Helpers.RhinoPluginTypeProperty);
            }

            if (launcherChanged)
            {
                var launcherType = GetSelectedLauncher();
                if (launcherType != null)
                    project.ProjectProperties.SetValue(Helpers.RhinoLauncherProperty, launcherType);
                else
                    project.ProjectProperties.RemoveProperty(Helpers.RhinoLauncherProperty);
                project.NotifyModified(Helpers.RhinoLauncherProperty);
            }
        }

        public RhinoOptionsPanelWidget(DotNetProject project, IEnumerable<ItemConfiguration> configurations)
        {
            supportsDevelopment = project.AsFlavor<RhinoProjectServiceExtension>()?.RhinoPluginType == McNeelProjectType.DebugStarter;

            if (supportsDevelopment)
                currentLauncherEntries = currentLauncherEntries.Concat(debugLauncherEntries).ToArray();

            this.project = project;
            Build();

            pluginTypeCombo.SelectedIndex = GetPluginComboIndex();
            pluginTypeCombo.SelectedIndexChanged += (sender, e) =>
            {
                pluginTypeChanged = true;
                SetAutodetectLabel();
            };
            SetAutodetectLabel();


            launcherCombo.SelectedIndexChanged += (sender, e) =>
            {
                launcherChanged = true;
                SetCustomLauncherText();
            };
            launcherCombo.SelectedIndex = GetLauncherComboIndex();
            SetCustomLauncherText();
        }

        void SetCustomLauncherText()
        {
            var enableCustom = launcherCombo.SelectedIndex == 1;
            customLauncherEntry.ReadOnly = !enableCustom;
            browseButton.Enabled = enableCustom;
            var outputFileName = project.GetOutputFileName(project.DefaultConfiguration.Selector);
            switch (launcherCombo.SelectedIndex)
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

        Label Bold(string text) => new Label { Text = text, Font = SystemFonts.Bold() };

        void SetBundleVersionLabel()
        {
            if (!string.IsNullOrEmpty(customLauncherEntry.Text))
            {
                var version = Helpers.GetVersionOfAppBundle(customLauncherEntry.Text);
                var versionString = version == null ? "unknown" : version.ToString();
                bundleInformationLabel.Content = new TableLayout(new TableRow("Application Version: ", Bold(versionString)));
            }
            else
                bundleInformationLabel.Content = null;
        }

        void SetAutodetectLabel()
        {
            var sbInfo = new List<Eto.Forms.Control>();
            if (pluginTypeCombo.SelectedIndex == 0) // autodetect
            {
                var detectedType = project.DetectMcNeelProjectType();
                if (detectedType != null)
                {
                    sbInfo.Add("Type: ");
                    sbInfo.Add(Bold(GetTypeLabel(detectedType.Value)));
                }
            }

            var detectedVersion = project.GetRhinoVersion();
            if (detectedVersion != null)
            {
                if (sbInfo.Count > 0)
                    sbInfo.Add(", ");
                sbInfo.Add("Version: ");
                sbInfo.Add(Bold(detectedVersion.Value.ToString()));
            }

            if (sbInfo.Count > 0)
                sbInfo.Insert(0, "Detected ");

            autodetectedTypeLabel.Content = new TableRow(sbInfo.Select(r => new TableCell(r)));
        }
        
        void SetContent(Panel panel, string value)
        {
            
        }

        void Build()
        {
            Spacing = 6;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;

            //var heading = new Gtk.Label("<b>Rhino Options</b>");
            //heading.UseMarkup = true;
            //heading.Xalign = 0;

            pluginTypeCombo = new DropDown { DataStore = typeEntries };

            launcherCombo = new DropDown { DataStore = currentLauncherEntries };

            customLauncherEntry = new TextBox();
            customLauncherEntry.TextChanged += (sender, e) => SetBundleVersionLabel();

            autodetectedTypeLabel = new Panel();

            bundleInformationLabel = new Panel();

            // layout

            //PackStart(heading, false, false, 0);


            var table = new TableLayout { Spacing = new Size(6, 6) };

            browseButton = new Button();
            browseButton.Text = "Browse...";
            browseButton.Click += (sender, e) =>
            {
                var fd = new Eto.Forms.OpenFileDialog();
                fd.Directory = new Uri("file:///Applications");
                fd.Filters.Add(new Eto.Forms.FileFilter("Application bundles", ".app"));
                if (fd.ShowDialog(null) == Eto.Forms.DialogResult.Ok)
                {
                    customLauncherEntry.Text = fd.FileName;
                }
            };

            AddRow(table, 0, "Plugin Type:", AutoSized(6, pluginTypeCombo, autodetectedTypeLabel));
            AddRow(table, 1, "Launcher:", AutoSized(6, launcherCombo));
            AddRow(table, 2, "", AutoExpanded(6, customLauncherEntry, browseButton)); // add browse button?
            AddRow(table, 3, "", AutoSized(0, bundleInformationLabel));

            Items.Add(new StackLayoutItem(table, true));
        }

        void AddRow(TableLayout table, uint rowi, string label, Eto.Forms.Control widget)
        {
            var row = new TableRow();
            row.Cells.Add(new Label { Text = label, TextAlignment = TextAlignment.Right });
            row.Cells.Add(new TableCell(widget, true));
            table.Rows.Add(row);
        }

        StackLayout AutoSized(int spacing, params Eto.Forms.Control[] widgets)
        {
            var box = new StackLayout { Orientation = Orientation.Horizontal };
            box.Spacing = spacing;
            foreach (var widget in widgets)
            {
                box.Items.Add(widget);
            }
            return box;
        }

        StackLayout AutoExpanded(int spacing, Eto.Forms.Control expandedWidget, params Eto.Forms.Control[] widgets)
        {
            var box = new StackLayout { Orientation = Orientation.Horizontal };
            box.Spacing = spacing;
            box.Items.Add(new StackLayoutItem(expandedWidget, true));
            foreach (var widget in widgets)
            {
                box.Items.Add(widget);
            }
            return box;
        }
    }
}
