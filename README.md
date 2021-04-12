# RhinoCommon Visual Studio Extensions

Includes RhinoCommon and Grasshopper template wizards for Visual Studio on Windows and Mac.

Manual Install Instructions (Mac)
---------------------------------

1. As of this writing, update to **Visual Studio for Mac 8.9.5**.
1. Download [the latest .mpack release](https://github.com/mcneel/RhinoVisualStudioExtension/releases).
1. Launch **Visual Studio for Mac**.
1. Navigate to **Visual Studio** > **Extensions...**
1. Expand **Debugging**
1. **Uninstall** all previous versions of the **RhinoCommon Plugin Support**.  If none are installed, you can ignore the next two steps (skip to step 9).
1. **Quit** and **Restart** Visual Studio for Mac.
1. Navigate to **Visual Studio** > **Extensions...** again.
1. Click **Install from file...** button in the lower-left corner.
1. Navigate to the mpack file you downloaded in step 1, then click **Open**.
1. Click **Install**.  The plugin should install.
1. **Quit** and **Restart** Visual Studio for Mac.
1. Navigate to **Visual Studio** > **Extensions..** > **Install** tab.  Verify that **RhinoCommon Plugin Support** exists under the **Debugging** category.  If it's there, you have successfully installed the extension and you are **DONE**.

Manual Install Instructions (Windows)
-------------------------------------

1. Download [the latest .vsix release](https://github.com/mcneel/RhinoVisualStudioExtension/releases).
2. Close any instances of Visual Studio.
3. Double click the .vsix in File Explorer.
4. Run through the wizard to install the extension.
5. Start Visual Studio and create a new RhinoCommon or Grasshopper project.

Debugging Rhino
---------------

1. Click the **Run** button in the upper left-hand corner of Visual Studio

For Developers (of this Extension)
--------------

## Mac
Building this project requires the **Addin Maker Extension**.  You can install this from within Visual Studio by navigating to **Visual Studio** > **Extensions...** > **Gallery** > **Extension Development** > **AddinMaker**.  Click the **Install** button.  **Quit** and **relaunch** Visual Studio for Mac.

