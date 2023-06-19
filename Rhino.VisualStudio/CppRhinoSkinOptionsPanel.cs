using Eto.Forms;
using Eto.Drawing;
using System;
using System.Linq;
using Rhino.VisualStudio.Controls;

namespace Rhino.VisualStudio
{
    public class CppRhinoSkinOptionsPanel : BaseCppRhinoOptionsPageView
    {
        public CppRhinoSkinOptionsPanel(bool showProjectName)
        {
            var useCustomMenusCheckBox = new CheckBox { Text = "Custom Menus", ToolTip = "Enables support for custom menus." };
            useCustomMenusCheckBox.CheckedBinding.BindDataContext((CppRhinoSkinOptionsViewModel m) => m.UseCustomMenus);

            var useSDLCheckBox = new CheckBox { Text = "Security Development Lifecycle (SDL) checks", ToolTip = "Enable additional Security Development Lifecycle (SDL) checks." };
            useSDLCheckBox.CheckedBinding.BindDataContext((CppRhinoSkinOptionsViewModel m) => m.UseSDL);


            // layout
            var layout = new DynamicLayout { DefaultSpacing = DefaultSpacing, Padding = DefaultPadding };

            // top
            layout.BeginVertical();
            if (showProjectName)
            {
                AddProjectName(layout);
            }
            layout.EndVertical();

            layout.BeginVertical();
            layout.Add(new PanelSeparator("Options"));

            AddRhinoVersion(layout);

            layout.Add(useCustomMenusCheckBox);
            layout.Add(useSDLCheckBox);

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