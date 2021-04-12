using System;
using MonoDevelop.Core.Execution;
using MonoDevelop.Debugger.Soft;

namespace Rhino.VisualStudio.Mac
{
  public class RhinoSoftDebuggerEngine : SoftDebuggerEngine
  {
    public string Id => "Mono.Debugger.Soft.Rhino";

    public const string DebuggerName = "RhinoCommon Plug-In Debugger";

    public string Name => DebuggerName;

    public override bool CanDebugCommand(ExecutionCommand cmd)
    {
      return cmd is RhinoExecutionCommand;
    }

    public override Mono.Debugging.Client.DebuggerStartInfo CreateDebuggerStartInfo(ExecutionCommand c)
    {
      return new RhinoDebuggerStartInfo((RhinoExecutionCommand)c);
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
