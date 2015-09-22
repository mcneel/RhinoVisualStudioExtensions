# RhinoCommon Xamarin Studio Addin

Includes RhinoCommon Plugin template wizards and the Rhino debugger.

Install Instructions
--------------------

1. As of this writing, update to **Xamarin Studio 5.9**.
1. Download [the latest release](https://github.com/mcneel/RhinoCommonXamarinStudioAddin/releases).
1. Launch **Xamarin Studio**.
1. Navigate to **Xamarin Studio** > **Add-in Manager...**
1. Expand **Debugging**
1. **Uninstall** all previous versions of the **Mono Soft Debugger for Rhinoceros** (BUT NOT the Mono Soft Debugger).  If none are installed, you can ignore the next two steps (skip to step 9).
1. **Quit** and **Restart** Xamarin Studio.
1. Navigate to **Xamarin Studio** > **Add-in Manager...** again.
1. Click **Install from file...** button in the lower-left corner.
1. Navigate to the mpack file you downloaded in step 1, then click **Open**.
1. Click **Install**.  The plugin should install.
1. *WARNING*: There is [a known bug](https://forums.xamarin.com/discussion/39098/packaging-add-ins) in the Xamarin AddIn installer engine that might display "The installation failed!  Could not read add-in description."  **Ignore this warning**.  Click **Close**.  Despite the bug, the AddIn *should* work.  **However, you must**...
1. **Quit** and **Restart** Xamarin Studio.
1. Navigate to **Xamarin Studio** > **Add-in Manager..** > **Install** tab.  Verify that **RhinoCommon Plugin Support** exists under the **Debugging** category.  If it's there, you have successfully installed the AddIn and you are **DONE**.

Debugging Rhino
---------------

1. Click the **Run** button in the upper left-hand corner of Xamarin Studio.
1. **NOTE: For developers working with RhinoWIP builds**: please see the [Debug in RhinoWIP (Mac)](http://developer.rhino3d.com/guides/rhinocommon/debug_rhinowip_mac/) guide.

For Developers (of this AddIn)
--------------
Building this project requires the **Addin Maker Add-in**.  You can install this from within Xamarin Studio by navigating to **Xamarin Studio** > **Add-in Manager...** > **Gallery** > **Addin Development** > **Addin Maker**.  Click the **Install** button.  **Quit** and **relaunch** Xamarin Studio.

To build the add-in for Xamarin Studio 5.7+, go to the build (/bin/Debug/) directory with **Terminal** and run the following:

`/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool setup pack MonoDevelop.RhinoDebug.dll`

This will generate a **.mpack** file that you can distribute to users. 
