using System;


namespace MonoDevelop.Debugger.Soft.Rhino
{
  public class RhinoSoftDebuggerEngine : SoftDebuggerEngine
  {
    public string Id { get { return "Mono.Debugger.Soft.Rhino"; } }

    public const string DebuggerName = "RhinoCommon Plug-In Debugger";
    public string Name { get { return DebuggerName; } }

    public override bool CanDebugCommand(MonoDevelop.Core.Execution.ExecutionCommand cmd)
    {
      return true;
    }

    public override Mono.Debugging.Client.DebuggerStartInfo CreateDebuggerStartInfo(MonoDevelop.Core.Execution.ExecutionCommand cmd)
    {
      string start_args = String.Empty;
      string bin_dir = String.Empty;
      string plugin_path = String.Empty;
      string rhinocommon_path = String.Empty;
      bool isGrasshopper = false;
      var execution_cmd = cmd as RhinoCommonExecutionCommand;
      if (execution_cmd != null)
      {
        start_args = execution_cmd.Arguments;
        string target = execution_cmd.Command;
        if (!string.IsNullOrWhiteSpace(target) && System.IO.File.Exists(target))
        {
          plugin_path = target;
          bin_dir = System.IO.Path.GetDirectoryName(target);
        }

        if (execution_cmd.Project != null)
        {
          for (int i = 0; i < execution_cmd.Project.References.Count; i++)
          {
            var reference = execution_cmd.Project.References[i];
            if (reference.HintPath != null && reference.HintPath.FileNameWithoutExtension == "RhinoCommon")
            {
              rhinocommon_path = reference.HintPath;
            }
            if (reference.HintPath != null && reference.HintPath.FileNameWithoutExtension  == "Grasshopper")
            {
              isGrasshopper = true;
            }
          }
        }
      }

      return new RhinoDebuggerStartInfo("Rhino", start_args, bin_dir, plugin_path, rhinocommon_path, isGrasshopper);
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
