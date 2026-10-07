# MyRhino.1

A Rhino C++ plug-in.

## Building on Windows

### 1. Install the tools

Install Visual Studio and the Rhino C++ SDK for your version of Rhino. See [Installing Tools (Windows)](https://developer.rhino3d.com/guides/cpp/installing-tools-windows/).

### 2. Build and run

Open `MyRhino.1.vcxproj` in Visual Studio, choose the `x64` platform, then Debug › Start Debugging. Visual Studio builds the plug-in and starts Rhino.

The first time, install the plug-in in Rhino from Tools › Options › Plug-ins, choosing `MyRhino.1.rhp` from the `x64\Debug` folder. Rhino remembers it after that. Type `MyRhino__1Command` in Rhino to try the sample command.

See [Creating your first C/C++ plug-in (Windows)](https://developer.rhino3d.com/guides/cpp/your-first-plugin-windows/) for more.

## Building on macOS
<!--#if (MacProject)-->

See [Installing Tools (Mac)](https://developer.rhino3d.com/guides/cpp/installing-tools-mac/) for the tools you need.

### 1. Add the Rhino C++ SDK

The Xcode project expects the SDK in an `SDK` folder, added as a git submodule:

```sh
git init    # if this folder isn't in a git repository yet
git submodule add https://github.com/mcneel/rhino_sdk_cpp.git SDK
```

After cloning this project somewhere else, fetch the SDK with:

```sh
git submodule update --init
```

### 2. Build and run

Open `MyRhino.1.xcodeproj` and press Run. Xcode starts Rhino with the plug-in loaded from the build folder. Type `MyRhino__1Command` in Rhino to try the sample command.

To use a different Rhino, choose Product › Scheme › Edit Scheme › Run › Info › Executable.

### 3. Debugging

Rhino doesn't allow a debugger to attach by default. Before each run, the scheme re-signs the Rhino app so Xcode can debug it. This only happens once per Rhino install; updating Rhino undoes it.

To turn this off, set `RHINO_MAKE_DEBUGGABLE` to `NO` under the target's Build Settings › User-Defined.
<!--#else-->

This project was created without an Xcode project. Building on macOS needs Rhino 9, and render plug-ins are Windows only.

To set up a Mac build, see [Installing Tools (Mac)](https://developer.rhino3d.com/guides/cpp/installing-tools-mac/) and [Creating your first C/C++ plug-in (Cross-Platform)](https://developer.rhino3d.com/guides/cpp/your-first-plugin-crossplatform/).
<!--#endif-->
