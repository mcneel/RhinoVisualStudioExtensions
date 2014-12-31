using System;
using System.Linq;
using MonoDevelop.Projects;
using MonoDevelop.Core;
using MonoDevelop.Debugger;
using MonoDevelop.Ide;
using MonoDevelop.Core.Execution;
using Mono.Debugging.Client;
using MonoDevelop.Debugger.Soft.Rhino;
using System.Reflection;
using System.IO;


namespace MonoDevelop.RhinoDebug
{
  public class RhinoProjectServiceExtension: ProjectServiceExtension
  {
    const string SoftDebuggerName = "Mono Soft Debugger for Rhinoceros";

    public override void Execute(MonoDevelop.Core.IProgressMonitor monitor, IBuildTarget item, ExecutionContext context, ConfigurationSelector configuration)
    {
      if (base.CanExecute(item, context, configuration))
      {
        // It is executable by default
        base.Execute(monitor, item, context, configuration);
        return;
      }

      var project = item as DotNetProject;
      if (project != null)
      {
        var config = project.GetConfiguration(configuration) as DotNetProjectConfiguration;
        var cmd = new DotNetExecutionCommand (config.CompiledOutputName);
        cmd.Arguments = config.CommandLineParameters;
        cmd.WorkingDirectory = Path.GetDirectoryName (config.CompiledOutputName);
        cmd.EnvironmentVariables = config.GetParsedEnvironmentVariables ();
        cmd.TargetRuntime = project.TargetRuntime;
        cmd.UserAssemblyPaths = project.GetUserAssemblyPaths(configuration);

        var executionModes = Runtime.ProcessService.GetExecutionModes();
        var executionMode = executionModes.SelectMany(r => r.ExecutionModes).FirstOrDefault(r => r.Id == SoftDebuggerName);
        var console = context.ConsoleFactory.CreateConsole(false);
        var operation = executionMode.ExecutionHandler.Execute(cmd, console);
        monitor.CancelRequested += monitor2 => operation.Cancel();
        operation.WaitForCompleted();
      }
    }

    public override bool GetNeedsBuilding(IBuildTarget item, ConfigurationSelector configuration)
    {
      return base.GetNeedsBuilding(item, configuration) && item == IdeApp.ProjectOperations.CurrentSelectedBuildTarget;
    }

    protected override BuildResult Build(IProgressMonitor monitor, IBuildTarget item, ConfigurationSelector configuration)
    {
      var result = base.Build(monitor, item, configuration);
      if (!result.Failed && item == IdeApp.ProjectOperations.CurrentSelectedBuildTarget)
      {
        // copy files over
        //CopyFiles(item, configuration);
      }
      return result;
    }

    public static string GetAppPath(string parameters, string childPath)
    {
      string appPath;
      if (parameters != null && parameters.StartsWith("-xcode", StringComparison.Ordinal))
      {
        // get output path
        var homePath = Environment.GetEnvironmentVariable("HOME");
        var derivedDataPath = Path.Combine(homePath, "Library/Developer/Xcode/DerivedData");
        if (!Directory.Exists(derivedDataPath))
          return null;
        appPath = Directory.GetDirectories(derivedDataPath).FirstOrDefault(r => Path.GetFileName(r).StartsWith("MacRhino-", StringComparison.Ordinal));
        if (appPath == null)
          return null;
        appPath = Path.Combine(appPath, "Build/Products/Debug/Rhinoceros.app");
      }
      else if (parameters != null && parameters.StartsWith("-app_path=", StringComparison.Ordinal))
      {
        string path = parameters.Substring("-app_path=".Length);
        path = path.Trim(new char[]{ '\"', ' ' });
        appPath = path;
      }
      else
      {
        appPath = "/Applications/Rhinoceros.app";
      }
      if (!string.IsNullOrEmpty(childPath))
        appPath = Path.Combine(appPath, childPath);
      return Directory.Exists(appPath) || File.Exists(appPath) ? appPath : null;
    }

    static void CopyFiles(IBuildTarget item, ConfigurationSelector configuration)
    {
      if (!IsRhinoProject(item))
        return;
      var project = item as DotNetProject;
      if (project == null)
        return;
      var config = project.GetConfiguration(configuration) as DotNetProjectConfiguration;
      if (config == null)
        return;

      var destPath = GetAppPath(config.CommandLineParameters, "Contents/Resources");
      if (string.IsNullOrEmpty(destPath))
        return;

      var path = config.OutputDirectory;
      var files = Directory.GetFiles(path);
      foreach (var file in files)
      {
        var destFile = Path.Combine(destPath, Path.GetFileName(file));
        File.Copy(file, destFile, true);
      }
    }

    static bool IsRhinoProject(IBuildTarget item)
    {
      var project = item as DotNetProject;
      return project != null && project.References.Any(r => r.Reference == "RhinoCommon" || r.Reference == "RhinoMac");
    }

    public override bool CanExecute(IBuildTarget item, ExecutionContext context, ConfigurationSelector configuration)
    {
      bool res = base.CanExecute(item, context, configuration);
      return res || IsRhinoProject(item);
    }
  }
}

