using System;
using MonoDevelop.RhinoDebug;

namespace MonoDevelop.Debugger.Soft.Rhino
{
  public class RhinoSoftDebuggerEngine : DebuggerEngineBackend
  {
    public string Id { get { return "Mono.Debugger.Soft.Rhino"; } }

    public string Name { get { return "Mono Soft Debugger for Rhino"; } }

    public override bool CanDebugCommand(MonoDevelop.Core.Execution.ExecutionCommand cmd)
    {
      return true;
    }

    public override Mono.Debugging.Client.DebuggerStartInfo CreateDebuggerStartInfo(MonoDevelop.Core.Execution.ExecutionCommand cmd)
    {
      string start_args = String.Empty;
      var execution_cmd = cmd as MonoDevelop.Core.Execution.DotNetExecutionCommand;
      if (execution_cmd != null)
      {
        start_args = execution_cmd.Arguments;
      }
      return new RhinoDebuggerStartInfo("Rhino", start_args);
    }

    public override Mono.Debugging.Client.DebuggerSession CreateSession()
    {
      return new RhinoSoftDebuggerSession();
    }

    public override Mono.Debugging.Client.ProcessInfo[] GetAttachableProcesses()
    {
      return new Mono.Debugging.Client.ProcessInfo[0];
    }
  }

  class RhinoDebuggerStartInfo : Mono.Debugging.Soft.SoftDebuggerStartInfo
  {
    string m_start_args;

    public RhinoDebuggerStartInfo(string appName, string start_args)
      : base(new Mono.Debugging.Soft.SoftDebuggerListenArgs(appName, System.Net.IPAddress.Loopback, 0))
    {
      m_start_args = start_args;
    }

    public bool ContainsCustomStartArgs
    {
      get
      {
        return !string.IsNullOrWhiteSpace(m_start_args);
      }
    }

    public string GetApplicationPath()
    {
      return RhinoProjectServiceExtension.GetAppPath(m_start_args, "Contents/MacOS/Rhinoceros")
        ?? RhinoProjectServiceExtension.GetAppPath(m_start_args, "Contents/MacOS/Rhino");
    }
  }
}
