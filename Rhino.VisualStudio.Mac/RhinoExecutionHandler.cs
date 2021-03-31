using System;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;

namespace Rhino.VisualStudio.Mac
{
  public class RhinoExecutionHandler : NativePlatformExecutionHandler
  {
    public override bool CanExecute(ExecutionCommand command)
    {
      return command is RhinoExecutionCommand;
    }

    public override ProcessAsyncOperation Execute(ExecutionCommand command, OperationConsole console)
    {
      // run without debugger
      var cmd = (RhinoExecutionCommand)command;

      var nativeCmd = new NativeExecutionCommand(cmd.ExecutablePath, cmd.Arguments, cmd.WorkingDirectory, cmd.EnvironmentVariables);
      return base.Execute(nativeCmd, console);
    }
  }
}
