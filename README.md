# Rhino Templates

Project and item templates for Rhino plug-ins: RhinoCommon, Grasshopper, Grasshopper 2, Zoo and Rhino C++. They come in three forms:

- **Visual Studio extension** (Windows): wizards under File › New › Project.
- **`dotnet new` templates** (Windows and macOS): for the command line and any editor, including the Xcode project for C++ plug-ins.
- **Xcode templates** (macOS): C++ plug-in projects and files from within Xcode. See [Xcode templates](#xcode-templates).

## Visual Studio

1. Download the `.vsix` from [the latest release](https://github.com/mcneel/RhinoVisualStudioExtensions/releases/latest).
2. Close Visual Studio, then double-click the `.vsix` and follow the installer.
3. In Visual Studio, choose File › New › Project and search for Rhino or Grasshopper.

## dotnet new

You'll need the [.NET SDK](https://dotnet.microsoft.com/download).

1. Install the templates:

    ```bash
    dotnet new install Rhino.Templates
    ```

2. Create a project in a new folder, then build it:

    ```bash
    dotnet new rhino -o MyRhinoPlugIn
    cd MyRhinoPlugIn
    dotnet build
    ```

To see a template's options, add `--help`, for example `dotnet new rhino --help`, or `dotnet new rhino --help -lang cpp` for C++.

### Templates

| Template | Short name | Languages |
| --- | --- | --- |
| Rhino plug-in | `rhino` | C#, VB, C++ |
| Rhino command | `rhinocommand` | C#, VB, C++ |
| Rhino skin | `rhinoskin` | C++ |
| Rhino tests (NUnit, using Rhino.Testing) | `rhinotest` | C# |
| Grasshopper plug-in | `grasshopper` | C#, VB |
| Grasshopper component | `ghcomponent` | C#, VB |
| Grasshopper 2 plug-in | `gh2` | C# |
| Grasshopper 2 component | `gh2component` | C# |
| Zoo plug-in | `zooplugin` | C#, VB |

C# is the default; pick another language with `-lang VB` or `-lang cpp`.

Common options for the .NET templates:

- `--version`: the Rhino version to target. Defaults to 9.
- `-sample`: include sample code.
- `-yak`: build a [yak package](https://developer.rhino3d.com/guides/yak/) with your plug-in.
- `-vscode`: add `tasks.json` and `launch.json` to build and debug in VS Code.
- `-wpf` / `-wf`: use WPF or Windows Forms (Windows only).

## C++ plug-ins

`dotnet new rhino -lang cpp` creates a C++ plug-in that builds on Windows with Visual Studio, and on macOS with Xcode for Rhino 9.

Options:

- `-pt`: the plug-in type: `utility` (default), `digitize`, `import`, `export` or `render`.
- `--mac`: add an Xcode project. Defaults to `auto`, which adds it only when you create the project on a Mac.
- `--cmake`: add a `CMakeLists.txt` that generates both the Visual Studio and Xcode projects from one set of sources.
- `--debuggable`: when running from Xcode, re-sign Rhino so the debugger can attach. Defaults to `true`.
- `--automation`, `--sockets`, `--sdl`: Windows MFC options.

The Xcode project and CMake are Rhino 9 only, and not available for render plug-ins.

### Create, open and run on macOS

You'll need Xcode, Rhino 9 for Mac, git, and the [.NET SDK](https://dotnet.microsoft.com/download) to create the project. On a Mac:

```bash
dotnet new install Rhino.Templates
dotnet new rhino -lang cpp -o MyPlugIn
cd MyPlugIn
git init
git submodule add https://github.com/mcneel/rhino_sdk_cpp.git SDK
open MyPlugIn.xcodeproj
```

The Xcode and CMake builds use the Rhino C++ SDK from that `SDK` submodule. Press Run in Xcode. It builds the plug-in, then starts Rhino 9 with the plug-in loaded from the build folder. Type `MyPlugIn` in Rhino to try the sample command.

The first run re-signs Rhino so Xcode's debugger can attach, so breakpoints work straight away. Updating Rhino undoes this; the next run re-signs it again.

To use a different Rhino, such as a BETA, choose Product › Scheme › Edit Scheme › Run › Info › Executable.

The `README.md` in the new project covers building on Windows and with CMake.

### Xcode templates

If you'd rather work entirely in Xcode, install `RhinoXcodeTemplates-<version>.pkg` from [the latest release](https://github.com/mcneel/RhinoVisualStudioExtensions/releases/latest), then restart Xcode. This adds:

- A **Rhino Plug-In** project, under File › New › Project… › macOS.
- **Rhino Command**, **Rhino Event Watcher** and **Rhino User Data** files, under File › New › File from Template… › macOS. These work in projects made with `dotnet new` too.

How they differ from `dotnet new`:

- **Run needs setting up by hand.** Xcode project templates can't include a scheme, so the first time, choose Product › Scheme › Edit Scheme › Run and:
  - Set Info › Executable to `Rhino 9.app`.
  - Under Arguments › Environment Variables, add `RHINO_PACKAGE_DIRS` with the value `$(BUILT_PRODUCTS_DIR)`, so Rhino loads the plug-in from the build folder.

  The project from `dotnet new` comes with this already set up.
- **Debugging follows a build setting instead of the scheme.** The project re-signs the Rhino in the `RHINO_APP` build setting (default `/Applications/Rhino 9.app`). If you run a different Rhino, change `RHINO_APP` to match, or the debugger won't be able to attach.
- **macOS only.** There's no Visual Studio project or CMake, so use `dotnet new` if the plug-in also needs to build on Windows.

The SDK submodule is needed here too; the new project's `README.md` explains how to add it.

## Additional Resources

See <https://developer.rhino3d.com/guides/rhinocommon/> for guides on how to start with Rhino Plug-In development, and <https://developer.rhino3d.com/guides/cpp/> for C++.
