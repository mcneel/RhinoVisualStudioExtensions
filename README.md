# RhinoCommon Xamarin Studio Addin

Includes RhinoCommon Plugin template wizards and the Rhino debugger.

Install Instructions
--------------------

1. As of this writing, update to **Xamarin Studio 5.9**.
1. Download [the latest release](https://github.com/mcneel/RhinoMonodevelopAddin/releases).
1. Extract the downloaded zip file to get an .mpack
1. Launch Xamarin Studio
1. Navigate to **Xamarin Studio** > **Add-in manager...**
2. Expand **Debugging**
3. Uninstall all previous versions of the Mono Soft Debugger for Rhinoceros (BUT NOT the Mono Soft Debugger)
1. Click **Install from file...** button
1. Navigate to the mpack file you extracted, then click **Open**
1. Click **Install**
1. Click **Close**
1. Verify that "Mono Soft Debugger Support for Rhinoceros" exists under the Debugging section of the Installed tab of the Add-in Manager.

Debugging Rhino
---------------

1. Click the **Run** button in the upper left-hand corner of Xamarin Studio
1. **NOTE: For McNeel developers working with internal builds** Verify that **DebugStarterExe** is set as your Startup project (right-click the project in Xamarin Studio, then click **Set as Startup project**).  Click the **Run** button in the upper left-hand corner.

For Developers
--------------
Building this project requires the **Addin Maker Add-in**.  You can install this from within Xamarin Studio by navigating to **Xamarin Studio** > **Add-in Manager...** > **Gallery** > **Addin Development** > **Addin Maker**.  Click the **Install** button.  **Quit** and **relaunch** Xamarin Studio.

To build the add-in for Xamarin Studio 5.7+, go to the build (/bin/Debug/) directory with **Terminal** and run the following:

`/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool setup pack MonoDevelop.RhinoDebug.dll`

This will generate a **.mpack** file that you can distribute to users.
