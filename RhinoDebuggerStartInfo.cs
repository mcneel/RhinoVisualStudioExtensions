using System;

namespace MonoDevelop.Debugger.Soft.Rhino
{
  class RhinoDebuggerStartInfo : Mono.Debugging.Soft.SoftDebuggerStartInfo
  {
    string m_start_args;
    string m_bin_dir;

    public RhinoDebuggerStartInfo(string appName, string start_args, string binDir)
      : base(new Mono.Debugging.Soft.SoftDebuggerListenArgs(appName, System.Net.IPAddress.Loopback, 0))
    {
      m_start_args = start_args;
      m_bin_dir = binDir;
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
      // The actual executable name changed in the past from Rhino to Rhinoceros.
      // Just check for both
      return RhinoProjectServiceExtension.GetAppPath(m_start_args, "Contents/MacOS/Rhinoceros")
        ?? RhinoProjectServiceExtension.GetAppPath(m_start_args, "Contents/MacOS/Rhino");
    }

    public string TargetDirectory
    {
      get { return m_bin_dir; }
    }
  }
}

