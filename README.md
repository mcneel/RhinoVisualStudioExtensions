MonoDevelop Add-In for debugging RhinoCommon plug-ins on OSX Rhino

Notes
-------
In order to debug, add a 'dummy' console application to your solution and use that to start the debugger.  I don't know how to tell MonoDevelop to run the debugger for a DLL project yet.

I don't know exactly where to place the compiled DLL, but building to an mpack and then installing in MonoDevelop adding manager "from file" appears to work

For internal debug build of Rhino from Xcode

In MonoDevelop, right click on 'dummy' project and select Run With

Create a custom mode for the Rhino Soft Debugger

In the 'arguments' box enter
-app_path="/Users/steve/Library/Developer/Xcode/DerivedData/MacRhino-bqyvykanvglloidkhdqerybggumx/Build/Products/Debug/Rhinoceros.app/Contents/MacOS/Rhinoceros"

Useful Links
-------
http://monodevelop.com/Developers/Articles/Creating_a_Simple_Add-in

MoonLight SoftDebugger is very similar add-in
https://github.com/mono/monodevelop/tree/master/main/src/addins/MonoDevelop.Debugger.Soft/MonoDevelop.Debugger.Soft.Moonlight
