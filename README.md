MonoDevelop Add-In for debugging RhinoCommon plug-ins on Rhino for Mac

Install Instructions
--------------------

1. As of this writing, update to **Xamarin Studio 5.8.3**.
1. Download [the latest release](https://github.com/mcneel/RhinoMonodevelopAddin/releases).
1. Extract the downloaded zip file to get an .mpack
1. Launch Xamarin Studio
1. Navigate to **Xamarin Studio** > **Add-in manager...**
2. Expand **Debugging**
3. Uninstall all previous versions of the Mono Soft Debugger for Rhinoceros (BUT NOT the Mono Soft Debugger)
1. Click **Install from file...** button
1. Navigate to the mack file you extracted, then click **Open**
1. Click **Install**
1. Click **Close**
1. Verify that "Mono Soft Debugger Support for Rhinoceros" exists under the Debugging section of the Installed tab of the Add-in Manager.

Debugging Rhino
---------------

1. Right-click a project in Xamarin Studio, then click **Run With...** -> **Mono Soft Debugger for Rhinoceros** (maybe someday this can be renamed *Rhino Debugger* so it's easier to find)

