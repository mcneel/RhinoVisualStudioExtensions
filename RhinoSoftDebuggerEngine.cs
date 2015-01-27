using System;


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
      string bin_dir = String.Empty;
      var execution_cmd = cmd as MonoDevelop.Core.Execution.DotNetExecutionCommand;
      if (execution_cmd != null)
      {
        start_args = execution_cmd.Arguments;
        string target = execution_cmd.Command;
        if (!string.IsNullOrWhiteSpace(target) && System.IO.File.Exists(target))
        {
          bin_dir = System.IO.Path.GetDirectoryName(target);
        }
      }

      return new RhinoDebuggerStartInfo("Rhino", start_args, bin_dir);
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
}
