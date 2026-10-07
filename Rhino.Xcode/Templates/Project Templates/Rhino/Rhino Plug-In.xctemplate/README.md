# ___PACKAGENAME___

A Rhino C++ plug-in for macOS.

## Rhino C++ SDK

The project expects the SDK in an `SDK` folder next to `___PACKAGENAME___.xcodeproj`, added as a git submodule. In Terminal, from that folder:

```sh
git init    # if Xcode didn't create a git repository
git submodule add https://github.com/mcneel/rhino_sdk_cpp.git SDK
```

After cloning this project somewhere else, fetch the SDK with:

```sh
git submodule update --init
```

## Build and run

The first time, set up Run to start Rhino:

1. Choose Product › Scheme › Edit Scheme… › Run › Info, and set Executable to `Rhino 9.app`.
2. Under Arguments › Environment Variables, add `RHINO_PACKAGE_DIRS` with the value `$(BUILT_PRODUCTS_DIR)`, so Rhino loads the plug-in from the build folder.
3. Tick Shared at the bottom of the scheme editor if others will use this project.

Then press Run. Type `___PACKAGENAMEASIDENTIFIER___` in Rhino to try the sample command.

## Debugging

Rhino doesn't allow a debugger to attach by default. The first build re-signs the Rhino app in `RHINO_APP` (default `/Applications/Rhino 9.app`) so Xcode can debug it, and again after Rhino is updated.

To turn this off, set `RHINO_MAKE_DEBUGGABLE` to `NO` under the target's Build Settings › User-Defined.

## Adding files

File › New › File from Template… has Rhino templates for commands, event watchers and user data under macOS.

See [Creating your first C/C++ plug-in (Cross-Platform)](https://developer.rhino3d.com/guides/cpp/your-first-plugin-crossplatform/) for more.
