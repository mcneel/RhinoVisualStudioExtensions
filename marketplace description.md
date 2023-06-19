RhinoCommon and Grasshopper template wizards for Rhinoceros 3D


## RhinoCommon templates:

*   **RhinoCommon Plugin** - Provides a project with a PlugIn-derived class and a Command-derived class.
*   **RhinoCommon Command** - Adds a single command class item.

*   **Zoo Plug-ins** - Adds a single project with a Zoo plug-in (requires Zoo to be installed).

To load the resulting .rhp file, open Rhino. Explore to the _bin/_ output folder in the solution. Then, drag-and-drop that file onto Rhino.

## Grasshopper templates:

*   **Grasshopper Assembly** - Provides a project with a GH_Component-derived class and a GH_AssemblyInfo-derived class.
*   **Grasshopper Component** - Adds a single component class item.

Download the Grasshopper SDK with the Rhino `_GrasshopperGetSDKDocumentation` command, or obtain the .chm file from the Related links section below here.

In order to load the result .gha file, you can use the `_GrasshopperDeveloperSettings` command. Add the bin/ folder as a looked-up folder.

## C++ SDK templates:

*  **Rhino Plug-In** - Provides a PlugIn Project

## Features

This extension provides add-on and component wizards for RhinoCommon and Grasshopper projects in C# and VB.NET, and wizards for the Rhino C++ SDK. For RhinoCommon and C++ project templates, this includes utility, digitizier, import and export plug-ins. For Grasshopper project templates, this includes a spiral for an Archimedean spiral component. 

It makes setting up debugging easier and automatically references the RhinoCommon and/or Grasshopper NuGet packages. Rhino and Grasshopper are requirements for this wizard to operate correctly.

## Related Links

- [Guides for our SDKs](https://developer.rhino3d.com/guides/)
- [RhinoCommon API documentation](https://developer.rhino3d.com/api/rhinocommon/)
- [Grasshopper API documentation](https://developer.rhino3d.com/api/grasshopper/)

---

(c) 2012-2023 Robert McNeel & Associates
For questions, email curtis@mcneel.com