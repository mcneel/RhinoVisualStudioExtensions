using System;
using System.IO;
using System.Linq;
using MonoDevelop.Projects;
using System.Collections.Generic;

namespace MonoDevelop.Debugger.Soft.Rhino
{
  class RhinoDebuggerStartInfo : Mono.Debugging.Soft.SoftDebuggerStartInfo
  {
    public RhinoDebuggerStartInfo(RhinoExecutionCommand cmd)
      : base(new Mono.Debugging.Soft.SoftDebuggerListenArgs("Rhino", System.Net.IPAddress.Loopback, 0))
    {
      Arguments = cmd.Arguments;
      PluginPath = cmd.PluginPath;
      TargetDirectory = cmd.BinDir;
      RhinoCommonPath = cmd.RhinoCommonPath;
      IsGrasshopper = cmd.IsGrasshopper;
      RhinoVersion = cmd.RhinoVersion;
      foreach (var env in cmd.EnvironmentVariables)
      {
        EnvironmentVariables.Add(env.Key, env.Value);
      }
      ApplicationPath = cmd.ApplicationPath;
      ExecutablePath = cmd.ExecutablePath;
    }

    public bool ContainsCustomStartArgs => !string.IsNullOrWhiteSpace(Arguments);


    public string ApplicationPath { get; private set; }
    public string ExecutablePath { get; private set; }
    public string TargetDirectory { get; private set; }
    public string PluginPath { get; private set; }
    public string RhinoCommonPath { get; private set; }
    public bool IsGrasshopper { get; private set; }
    public int RhinoVersion { get; private set; }
  }
}

