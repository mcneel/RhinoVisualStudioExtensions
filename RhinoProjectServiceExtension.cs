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
using MonoDevelop.Projects.MSBuild;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace MonoDevelop.Debugger.Soft.Rhino
{
  class RhinoCommonExecutionCommand : DotNetExecutionCommand
  {
    public DotNetProject Project { get; set; }
    public RhinoCommonExecutionCommand(string outputname, DotNetProject project): base(outputname)
    {
      Project = project;
    }
  }

  public class RhinoProjectServiceExtension : ProjectExtension
  {
    protected override Task OnExecute(ProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
    {
      if (base.OnGetCanExecute(context, configuration))
      {
        return base.OnExecute(monitor, context, configuration);
      }

      try
      {
        var project = Project as DotNetProject;
        if (project != null && IsSupportedProject)
        {
          const string SoftDebuggerName = RhinoSoftDebuggerEngine.DebuggerName;
          var config = project.GetConfiguration(configuration) as DotNetProjectConfiguration;
          var cmd = new RhinoCommonExecutionCommand(project.GetOutputFileName(configuration), project);
          cmd.Arguments = config.CommandLineParameters;
          cmd.WorkingDirectory = Path.GetDirectoryName(config.CompiledOutputName);
          cmd.EnvironmentVariables = config.GetParsedEnvironmentVariables();
          cmd.TargetRuntime = project.TargetRuntime;
          cmd.UserAssemblyPaths = project.GetUserAssemblyPaths(configuration);

          var executionModes = Runtime.ProcessService.GetExecutionModes();
          var executionMode = executionModes.SelectMany(r => r.ExecutionModes).FirstOrDefault(r => r.Id == SoftDebuggerName);
          var console = context.ConsoleFactory.CreateConsole(new OperationConsoleFactory.CreateConsoleOptions(true));
          var operation = executionMode.ExecutionHandler.Execute(cmd, console);
          monitor.CancellationToken.Register(() => operation.Cancel());
          return operation.Task;
        }
      }
      catch (Exception ex)
      {
        monitor.ReportError($"An error occurred starting Rhino.\n{ex}", ex);
      }
      return null;
    }

    protected override FilePath OnGetOutputFileName (ConfigurationSelector configuration)
    {
      var output = base.OnGetOutputFileName (configuration);
      if (!disableOutputNameChange && IsSupportedProject)
        return output.ChangeExtension (PluginExtension);
      return output;
    }

    bool IsSupportedProject {
      get {
        return Project.GetMcNeelProjectType() != null;
      }
    }

    string PluginExtension {
      get {
        return Project.GetMcNeelProjectType()?.GetExtension() ?? ".dll";
      }
    }

    bool disableOutputNameChange;

    protected override async Task<BuildResult> OnBuild(ProgressMonitor monitor, ConfigurationSelector configuration, OperationContext operationContext)
    {
      var result = await base.OnBuild(monitor, configuration, operationContext);

      try
      {
        if (!result.Failed && IsSupportedProject)
        {
          // rename plugin output to .rhi or .ghp
          disableOutputNameChange = true;
          var file = Project.GetOutputFileName(configuration);
          disableOutputNameChange = false;
          var ext = PluginExtension;

          if (file.Extension != ext)
          {
            RenameOutputFile(file, file.ChangeExtension(ext));
            RenameOutputFile(file.ChangeExtension(file.Extension + ".mdb"), file.ChangeExtension(ext + ".mdb"));
          }
        }
      }
      catch (Exception ex)
      {
        monitor.ReportError($"An error occurred renaming output files to .rhi/.ghp.\n{ex}", ex);
      }
      return result;
    }

    protected override async Task<BuildResult> OnClean(ProgressMonitor monitor, ConfigurationSelector configuration, OperationContext operationContext)
    {
      try
      {
        if (IsSupportedProject)
        {
          // clean up the renamed output files
          disableOutputNameChange = true;
          var file = Project.GetOutputFileName(configuration);
          disableOutputNameChange = false;
          var ext = PluginExtension;

          if (file.Extension != ext)
          {
            var assemblyFile = file.ChangeExtension(ext);
            if (File.Exists(assemblyFile))
              File.Delete(assemblyFile);
            var debugFile = file.ChangeExtension(ext + ".mdb");
            if (File.Exists(debugFile))
              File.Delete(debugFile);
          }
        }
      }
      catch (Exception ex)
      {
        monitor.ReportError($"An error occurred cleaning output files.\n{ex}", ex);
      }
      return await base.OnClean(monitor, configuration, operationContext);
    }

    static void RenameOutputFile (FilePath file, FilePath output)
    {
      if (File.Exists (output.FullPath))
        File.Delete (output.FullPath);

      if (File.Exists(file.FullPath))
        File.Move (file.FullPath, output.FullPath);
    }

    protected override ProjectFeatures OnGetSupportedFeatures ()
    {
      var features = base.OnGetSupportedFeatures ();
      if (IsSupportedProject)
        features |= ProjectFeatures.Execute;
      return features;
    }

    protected override bool OnGetCanExecute (ExecutionContext context, ConfigurationSelector configuration)
    {
      bool res = base.OnGetCanExecute (context, configuration);
      if (res)
        return true;

      var dotNetProject = Project as DotNetProject;
      if (dotNetProject == null || !IsSupportedProject)
        return false;

      var config = dotNetProject.GetConfiguration (configuration) as DotNetProjectConfiguration;
      if (config == null)
        return false;

      var cmd = dotNetProject.CreateExecutionCommand (configuration, config);
      if (context.ExecutionTarget != null)
        cmd.Target = context.ExecutionTarget;
      if (context.ExecutionHandler == null)
        return false;
      
      var result = context.ExecutionHandler.CanExecute (cmd);

      return result;
    }
  }
}

