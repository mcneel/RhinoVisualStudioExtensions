# RhinoCommon Visual Studio Extensions

Includes RhinoCommon, Grasshopper, Zoo, and Rhino C++ templates and wizards for Visual Studio on Windows and Mac.

## Manual Install Instructions (Visual Studio)

1. Download [the latest .vsix release](https://github.com/mcneel/RhinoVisualStudioExtensions/releases/latest).
2. Close any instances of Visual Studio.
3. Double click the .vsix in File Explorer.
4. Run through the wizard to install the extension.
5. Start Visual Studio and create a new RhinoCommon or Grasshopper project.

## Using dotnet new

1. Install the templates from nuget. This will show the list of all templates after a successful install.

    `dotnet new install Rhino.Templates`

2. Create a new folder for your project:

    ```bash
    mkdir MyNewRhinoPlugin
    cd MyNewRhinoPlugin
    ```

3. Show options for the templates:

    ```bash
    dotnet new rhino --help
    dotnet new grasshopper --help
    ```

4. Create the project

    `dotnet new rhino`

5. Build your project

    `dotnet build`

## Additional Resources

See <https://developer.rhino3d.com/guides/rhinocommon/> for guides on how to start with Rhino Plug-In development.
