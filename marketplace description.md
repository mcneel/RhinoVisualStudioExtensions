# Rhino and Grasshopper templates for Visual Studio

Start a new [Rhino](https://www.rhino3d.com) or Grasshopper plug-in in seconds. Choose File › New › Project, search for **Rhino** or **Grasshopper**, and a wizard sets up the project, the SDK references and debugging for you.

Press **F5** and Rhino starts with your plug-in already loaded. There's no need to drag files into Rhino or add folders to Grasshopper's settings.

Templates target Rhino 9 by default, and Rhino 6, 7 and 8 are also supported.

## Project templates

| Template | What you get | Languages |
| --- | --- | --- |
| **Rhino Plug-In** | A plug-in with one command. Choose a utility, digitizer, import, export or render plug-in, with optional sample code. | C#, VB.NET, C++ |
| **Grasshopper Assembly** | A Grasshopper plug-in with one component, with an optional Archimedean spiral sample. | C#, VB.NET |
| **Grasshopper 2 Plug-In** | A Grasshopper 2 plug-in with one component. | C# |
| **Zoo Plug-In** | A plug-in for licensing your product with Zoo. Requires Zoo to be installed. | C#, VB.NET |
| **Rhino Skin** | A custom-branded Rhino. | C++ |
| **Rhino Test** | An NUnit test project that runs your tests inside Rhino, using Rhino.Testing. | C# |

The C++ plug-in can also include an Xcode project or a CMake build, so it builds on macOS too.

## Item templates

Add these to an existing project:

* **Rhino Command** (C#, VB.NET, C++)
* **Grasshopper Component** (C#, VB.NET)
* **Grasshopper 2 Component** (C#)

## What's new in 9.0

* Rhino 9 is now the default.
* New Grasshopper 2 plug-in and component templates.
* C++ plug-ins can include an Xcode project or CMake build, and load into Rhino automatically when debugging.
* Supports Visual Studio 2026.
* Refreshed wizards and icons, with dark mode support.

## Requirements

* Visual Studio 2022 or 2026 on Windows.
* Rhino installed, to run and debug your plug-in.
* For C++ plug-ins, the [Rhino C++ SDK](https://developer.rhino3d.com/guides/cpp/).

On macOS or another editor, use the same templates from the command line with `dotnet new install Rhino.Templates`.

## Related links

* [Getting started guides](https://developer.rhino3d.com/guides/)
* [RhinoCommon API](https://developer.rhino3d.com/api/rhinocommon/)
* [Grasshopper API](https://developer.rhino3d.com/api/grasshopper/)
* [Source code and issues](https://github.com/mcneel/RhinoVisualStudioExtensions)

---

© 2012–2026 Robert McNeel & Associates. Questions? Email [curtis@mcneel.com](mailto:curtis@mcneel.com).
