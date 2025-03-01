using Eto.Forms;
using Eto.Drawing;
using System;
using Rhino.VisualStudio.Controls;

namespace Rhino.VisualStudio
{
    public class RhinoTestingOptionsPanel : BaseRhinoPageView
    {
        public RhinoTestingOptionsPanel(bool showProjectName)
        {
            var padding = new Padding(8);

            InitLayout(showProjectName);

            var information = new DynamicLayout();
            information.AddSpace();
            AddRhinoDownloadInfo(information);
            information.AddSpace();

            Information = information;
        }

        private void InitLayout(bool showProjectName)
        {
            var testClassNameTextBox = new TextBox();
            testClassNameTextBox.TextBinding.BindDataContext((RhinoTestingOptionsViewModel m) => m.TestClassName);

            var provideCommandSampleCheckBox = new CheckBox { Text = "Provide test sample", ToolTip = "Check to provide a sample test implementation" };
            provideCommandSampleCheckBox.CheckedBinding.BindDataContext((RhinoTestingOptionsViewModel m) => m.IncludeSample);
            provideCommandSampleCheckBox.BindDataContext(c => c.Enabled, (RhinoTestingOptionsViewModel m) => m.CanProvideSample);

            Styles.Add<GroupBox>(null, g => g.Padding = this.Padding);

            // layout
            var layout = new DynamicLayout { DefaultSpacing = DefaultSpacing, Padding = this.Padding };

            // top
            layout.BeginVertical();
            
            if (showProjectName)
                AddProjectName(layout);

            layout.AddRow("Test class name", testClassNameTextBox);
            layout.EndVertical();

            layout.BeginVertical();
            layout.Add(new PanelSeparator("Options"));

            AddRhinoVersion(layout);

            AddIncludeVSCode(layout);

            layout.Add(provideCommandSampleCheckBox);
            layout.EndVertical();

            Content = layout;
        }

    }
}