using System;
using System.IO;
using System.Linq;
using MonoDevelop.Projects;

namespace MonoDevelop.Debugger.Soft.Rhino
{
  class RhinoDebuggerStartInfo : Mono.Debugging.Soft.SoftDebuggerStartInfo
  {
    string m_start_args;

    public RhinoDebuggerStartInfo(string appName, string startArgs, string binDir, string pluginPath, string rhinocommonPath, bool isGrasshopper)
      : base(new Mono.Debugging.Soft.SoftDebuggerListenArgs(appName, System.Net.IPAddress.Loopback, 0))
    {
      m_start_args = startArgs;
      PluginPath = pluginPath;
      TargetDirectory = binDir;
      RhinoCommonPath = rhinocommonPath;
      IsGrasshopper = isGrasshopper;
    }

    public bool ContainsCustomStartArgs
    {
      get
      {
        return !string.IsNullOrWhiteSpace(m_start_args);
      }
    }

    /// <summary>
    /// Full path to the Rhinoceros executable to start
    /// </summary>
    public string GetApplicationPath()
    {
      // always attempt to run the Rhino that contains the RhinoCommon we are referencing first
      // only command line args can override this behavior

      if (string.IsNullOrEmpty(m_start_args) && !string.IsNullOrEmpty(RhinoCommonPath))
      {
        var fileinfo = new System.IO.FileInfo(RhinoCommonPath);
        if (fileinfo.Exists)
        {
          var contents_dir = fileinfo.Directory.Parent;
          string path = System.IO.Path.Combine(contents_dir.FullName, "MacOS", "Rhinoceros");
          if (System.IO.File.Exists(path))
            return path;
        }
      }


      // The actual executable name changed in the past from Rhino to Rhinoceros.
      // Just check for both
      return Helpers.GetAppPath(m_start_args, "Contents/MacOS/Rhinoceros")
        ?? Helpers.GetAppPath(m_start_args, "Contents/MacOS/Rhino");
    }

    public string TargetDirectory { get; private set; }
    public string PluginPath { get; private set; }
    public string RhinoCommonPath { get; private set; }
    public bool IsGrasshopper { get; private set; }
  }
}

