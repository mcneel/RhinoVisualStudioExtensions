# Rhino Xcode Templates

Xcode project and file templates for Rhino 9 C++ plug-ins on macOS:

- **Rhino Plug-In** project (utility, digitizer, import or export), under File › New › Project… › macOS.
- **Rhino Command**, **Rhino Event Watcher** and **Rhino User Data** files, under File › New › File from Template… › macOS.

## Building the installer

```sh
./build-pkg.sh
```

This writes `artifacts/xcode/RhinoXcodeTemplates-<version>.pkg`, using the version in `Directory.Build.props`. To sign it, pass a Developer ID Installer identity:

```sh
./build-pkg.sh "Developer ID Installer: Robert McNeel & Associates (TEAMID)"
```

The pkg installs the templates to `/Library/Application Support/McNeel/Rhino Xcode Templates`, then copies them into the logged-in user's `~/Library/Developer/Xcode/Templates`, the only place Xcode looks. Restart Xcode to see them.

## Testing changes

Copy the templates straight into place instead of building the pkg:

```sh
ditto Templates ~/Library/Developer/Xcode/Templates
```

Xcode reads templates when it starts, so quit and reopen it after each change.

## Uninstalling

```sh
rm -rf ~/Library/Developer/Xcode/Templates/*/Rhino
```
