using System;

namespace MonoDevelop.Debugger.Soft.Rhino
{
  class RhinoDebuggerStartInfo : Mono.Debugging.Soft.SoftDebuggerStartInfo
  {
    string m_start_args;

    public RhinoDebuggerStartInfo(string appName, string startArgs, string binDir, string pluginPath)
      : base(new Mono.Debugging.Soft.SoftDebuggerListenArgs(appName, System.Net.IPAddress.Loopback, 0))
    {
      m_start_args = startArgs;
      PluginPath = pluginPath;
      TargetDirectory = binDir;
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

    public string TargetDirectory { get; private set; }
    public string PluginPath { get; private set; }
  }
}

