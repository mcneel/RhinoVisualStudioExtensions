RhinoCommon and Grasshopper template wizards for Rhino 7

## RhinoCommon templates:

*   **C# Plugin** - Provides a C# project with a PlugIn-derived class and a Command-derived class.
*   **C# Command** - Adds a single command class item.

*   **Vb.Net Plugin** - Provides a C# project with a PlugIn-derived class and a Command-derived class.
*   **Vb.Net Command** - Adds a single command class item.

*   **C# and Vb.Net Zoo Plug-ins** - Adds a single project with a Zoo plug-in (requires Zoo to be installed).

To load the resulting .rhp file, open Rhino. Explore to the _bin/_ output folder in the solution. Then, drag-and-drop that file onto Rhino.

## Grasshopper templates:

*   **C# Add-on** - Provides a C# project with a GH_Component-derived class and a GH_AssemblyInfo-derived class.
*   **C# Component** - Adds a single component class item.

*   **Vb.Net Add-on** - Provides a Vb.Net project with a GH_Component-derived class and a GH_AssemblyInfo-derived class.
*   **Vb.Net Component** - Adds a single component class item.

Download the Grasshopper SDK with the Rhino `_GrasshopperGetSDKDocumentation` command, or obtain the .chm file from the Related links section below here.

In order to load the result .gha file, you can use the `_GrasshopperDeveloperSettings` command. Add the bin/ folder as a looked-up folder.

## Features

This extension provides add-on and component wizards for RhinoCommon and Grasshopper projects. For RhinoCommon project templates, this includes utility, digitizier, import and export plug-ins. For Grasshopper project templates, this includes a spiral for an Archimedean spiral component. 

It makes setting up debugging easier and automatically references the RhinoCommon and/or Grasshopper NuGet packages. Rhino and Grasshopper are requirements for this wizard to operate correctly.

## Related Links

- [RhinoCommon samples](http://www.rhino3d.com/go/rhinocommon_guide)
- [RhinoCommon documentation](http://www.rhino3d.com/5/rhinocommon/)
- [Grasshopper CHM help file](http://download.mcneel.com/s3b/mcneel/grasshopper/1.0/sdk/en/GrasshopperSDK.chm)

---

(c) 2012-2021 Robert McNeel & Associates
For questions, email curtis@mcneel.com