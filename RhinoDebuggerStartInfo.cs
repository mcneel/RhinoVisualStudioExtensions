using System;
using System.IO;
using System.Linq;
using MonoDevelop.Projects;
using System.Collections.Generic;

namespace MonoDevelop.RhinoDebug
{
  class RhinoDebuggerStartInfo : Mono.Debugging.Soft.SoftDebuggerStartInfo
  {
    public RhinoDebuggerStartInfo(RhinoExecutionCommand cmd)
      : base(new Mono.Debugging.Soft.SoftDebuggerListenArgs("Rhino", System.Net.IPAddress.Loopback, 0))
    {
      Arguments = cmd.Arguments;
      foreach (var env in cmd.EnvironmentVariables)
      {
        EnvironmentVariables.Add(env.Key, env.Value);
      }
      ApplicationPath = cmd.ApplicationPath;
      ExecutablePath = cmd.ExecutablePath;
    }

    public string ApplicationPath { get; private set; }
    public string ExecutablePath { get; private set; }
  }
}

