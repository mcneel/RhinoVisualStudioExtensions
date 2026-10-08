# MyRhino.1

A Rhino C++ plug-in.
<!--#if (MacProject || CMakeProject)-->

## Rhino C++ SDK

The Xcode and CMake builds expect the SDK in an `SDK` folder, added as a git submodule:

```sh
git init    # if this folder isn't in a git repository yet
git submodule add https://github.com/mcneel/rhino_sdk_cpp.git SDK
```

After cloning this project somewhere else, fetch the SDK with:

```sh
git submodule update --init
```
<!--#endif-->

## Building on Windows

### 1. Install the tools

Install Visual Studio and the Rhino C++ SDK for your version of Rhino. See [Installing Tools (Windows)](https://developer.rhino3d.com/guides/cpp/installing-tools-windows/).

### 2. Build and run

Open `MyRhino.1.vcxproj` in Visual Studio, choose the `x64` platform, then Debug › Start Debugging. Visual Studio builds the plug-in and starts Rhino.

The first time, install the plug-in in Rhino from Tools › Options › Plug-ins, choosing `MyRhino.1.rhp` from the `x64\Debug` folder. Rhino remembers it after that. Type `MyRhino__1Command` in Rhino to try the sample command.

See [Creating your first C/C++ plug-in (Windows)](https://developer.rhino3d.com/guides/cpp/your-first-plugin-windows/) for more.

## Building on macOS
<!--#if (MacProject)-->

See [Installing Tools (Mac)](https://developer.rhino3d.com/guides/cpp/installing-tools-mac/) for the tools you need, and add the [Rhino C++ SDK](#rhino-c-sdk) first.

### 1. Build and run

Open `MyRhino.1.xcodeproj` and press Run. Xcode starts Rhino with the plug-in loaded from the build folder. Type `MyRhino__1Command` in Rhino to try the sample command.

To use a different Rhino, choose Product › Scheme › Edit Scheme › Run › Info › Executable.

### 2. Debugging

Rhino doesn't allow a debugger to attach by default. Before each run, the scheme re-signs the Rhino app so Xcode can debug it. This only happens once per Rhino install; updating Rhino undoes it.

To turn this off, set `RHINO_MAKE_DEBUGGABLE` to `NO` under the target's Build Settings › User-Defined.
<!--#elseif (CMakeProject)-->

This project was created without a ready-made Xcode project. Generate one with [CMake](#building-with-cmake).
<!--#else-->

This project was created without an Xcode project. Building on macOS needs Rhino 9, and render plug-ins are Windows only.

To set up a Mac build, see [Installing Tools (Mac)](https://developer.rhino3d.com/guides/cpp/installing-tools-mac/) and [Creating your first C/C++ plug-in (Cross-Platform)](https://developer.rhino3d.com/guides/cpp/your-first-plugin-crossplatform/).
<!--#endif-->
<!--#if (CMakeProject)-->

## Building with CMake

`CMakeLists.txt` generates a Visual Studio or Xcode project from the same sources, using the [Rhino C++ SDK](#rhino-c-sdk) submodule on both platforms. You need CMake 3.21 or later.

Generate the project into the `build` folder:

```sh
# macOS
cmake -G Xcode -S . -B build
open "build/MyRhino.1.xcodeproj"

# Windows
cmake -G "Visual Studio 18 2026" -A x64 -S . -B build
```

On Windows, open the solution in the `build` folder. Run (Xcode) or Debug › Start Debugging (Visual Studio) builds the plug-in and starts Rhino.

- On macOS, Rhino loads the plug-in from the build folder.
- On Windows, install the plug-in the first time from Tools › Options › Plug-ins, choosing `MyRhino.1.rhp` from `build\Debug`.

To build from the terminal instead, run `cmake --build build --config Debug`.

Add new source files to `SOURCES` in `CMakeLists.txt`; the generated project picks up the change on the next build.

### Settings

Pass these when generating, e.g. `cmake -G Xcode -S . -B build -DRHINO_APP="/Applications/Rhino 9 BETA.app"`:

- `RHINO_APP`: the Rhino app Xcode starts. Defaults to `/Applications/Rhino 9.app`.
- `RHINO_EXE`: the Rhino.exe Visual Studio starts. Defaults to `C:/Program Files/Rhino 9/System/Rhino.exe`.
- `RHINO_MAKE_DEBUGGABLE`: on macOS, re-signs `RHINO_APP` after each build so the debugger can attach. Set to `OFF` to turn it off.

See [Creating your first C/C++ plug-in (Cross-Platform)](https://developer.rhino3d.com/guides/cpp/your-first-plugin-crossplatform/) for more.
<!--#endif-->
