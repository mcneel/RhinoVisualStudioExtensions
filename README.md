# RhinoCommon Visual Studio Addin

Includes RhinoCommon Plugin template wizards and the Rhino debugger.

Install Instructions
--------------------

1. As of this writing, update to **Visual Studio 6.9**.
1. Download [the latest release](https://github.com/mcneel/RhinoCommonVisualStudioAddin/releases).
1. Launch **Visual Studio for Mac**.
1. Navigate to **Visual Studio** > **Add-in Manager...**
1. Expand **Debugging**
1. **Uninstall** all previous versions of the **Mono Soft Debugger for Rhinoceros** (BUT NOT the Mono Soft Debugger).  If none are installed, you can ignore the next two steps (skip to step 9).
1. **Quit** and **Restart** Visual Studio for Mac.
1. Navigate to **Visual Studio** > **Add-in Manager...** again.
1. Click **Install from file...** button in the lower-left corner.
1. Navigate to the mpack file you downloaded in step 1, then click **Open**.
1. Click **Install**.  The plugin should install.
1. **Quit** and **Restart** Visual Studio for Mac.
1. Navigate to **Visual Studio** > **Add-in Manager..** > **Install** tab.  Verify that **RhinoCommon Plugin Support** exists under the **Debugging** category.  If it's there, you have successfully installed the AddIn and you are **DONE**.

Debugging Rhino
---------------

1. Click the **Run** button in the upper left-hand corner of Xamarin Studio.
1. **NOTE: For developers working with RhinoWIP builds**: please see the [Debug in RhinoWIP (Mac)](http://developer.rhino3d.com/wip/guides/rhinocommon/debug-rhinowip-mac/) guide.

For Developers (of this AddIn)
--------------
Building this project requires the **Addin Maker Add-in**.  You can install this from within Visual Studio by navigating to **Visual Studio** > **Add-in Manager...** > **Gallery** > **Addin Development** > **Addin Maker**.  Click the **Install** button.  **Quit** and **relaunch** Visual Studio.
