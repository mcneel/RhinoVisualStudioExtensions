using System;
using System.Linq;
using MonoDevelop.Projects;
using MonoDevelop.Core;
using MonoDevelop.Debugger;
using MonoDevelop.Ide;
using MonoDevelop.Core.Execution;
using Mono.Debugging.Client;
using MonoDevelop.RhinoDebug;
using System.Reflection;
using System.IO;
using MonoDevelop.Projects.MSBuild;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoDevelop.RhinoDebug
{

  public class RhinoProjectServiceExtension : DotNetProjectExtension
  {
    Lazy<McNeelProjectType?> detectedType;
    Lazy<McNeelProjectType?> projectType;

    public RhinoProjectServiceExtension()
    {
      RefreshRhinoProjectType();
    }

    McNeelProjectType? LookupDetectedProjectType() => Project.DetectMcNeelProjectType();
    McNeelProjectType? LookupSpecifiedProjectType() => Project.GetPluginProjectType();

    internal McNeelProjectType? RhinoPluginType => projectType.Value ?? detectedType.Value;

    void RefreshRhinoProjectType()
    {
      detectedType = new Lazy<McNeelProjectType?>(LookupDetectedProjectType);
      projectType = new Lazy<McNeelProjectType?>(LookupSpecifiedProjectType);
    }

    protected override void OnModified(SolutionItemModifiedEventArgs args)
    {
      base.OnModified(args);
      if (args.Any(r => r.Hint == Helpers.RhinoPluginTypeProperty))
      {
        // refresh the rhino project type
        RefreshRhinoProjectType();
      }
    }

    protected override void OnBeginLoad()
    {
      base.OnBeginLoad();

      RefreshRhinoProjectType();
      RhinoGlobalProperties.RequiresMdb = false;
    }

    protected override void OnEndLoad()
    {
      base.OnEndLoad();

      SetRequiresMdb();
    }

    protected override void OnReferencedAssembliesChanged()
    {
      base.OnReferencedAssembliesChanged();
      var wasSupported = IsSupportedProject;
      RefreshRhinoProjectType();
      if (wasSupported != IsSupportedProject)
      {
        //Project.ClearCachedData();
        Project.NeedsReload = true;

        Project.NotifyExecutionTargetsChanged();
        Project.NotifyRunConfigurationsChanged();
        Project.RefreshExtensions();

        Trace.WriteLine($"{Project.Name}: RhinoPluginType: {RhinoPluginType}");
      }
    }

    protected override DotNetProjectFlags OnGetDotNetProjectFlags()
    {
      if (!IsSupportedProject)
        return base.OnGetDotNetProjectFlags();

      return base.OnGetDotNetProjectFlags() | DotNetProjectFlags.IsLibrary;
    }

    protected override Task OnExecuteCommand(ProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration, ExecutionCommand executionCommand)
    {
      if (!IsSupportedProject)
        return base.OnExecuteCommand(monitor, context, configuration, executionCommand);

      return OnExecuteRhinoCommand(monitor, context, configuration, executionCommand);
    }

    async Task OnExecuteRhinoCommand(ProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration, ExecutionCommand executionCommand)
    {
      bool externalConsole = false;
      bool pauseConsole = false;

      if (executionCommand is RhinoExecutionCommand rhinoExecutionCommand)
      {
        externalConsole = rhinoExecutionCommand.ExternalConsole;
        pauseConsole = rhinoExecutionCommand.PauseConsoleOutput;
      }

      OperationConsole console = externalConsole ? context.ExternalConsoleFactory.CreateConsole(!pauseConsole, monitor.CancellationToken)
        : context.ConsoleFactory.CreateConsole(OperationConsoleFactory.CreateConsoleOptions.Default.WithTitle(Project.Name), monitor.CancellationToken);

      using (console)
      {
        ProcessAsyncOperation asyncOp = context.ExecutionHandler.Execute(executionCommand, console);

        try
        {
          using (var stopper = monitor.CancellationToken.Register(asyncOp.Cancel))
            await asyncOp.Task;

          monitor.Log.WriteLine(GettextCatalog.GetString("The application exited with code: {0}", asyncOp.ExitCode));
        }
        catch (OperationCanceledException)
        {
        }
      }
    }

    protected override FilePath OnGetOutputFileName(ConfigurationSelector configuration)
    {
      if (!IsSupportedProject)
        return base.OnGetOutputFileName(configuration);

      var output = base.OnGetOutputFileName(configuration);
      if (!disableOutputNameChange)
        return output.ChangeExtension(PluginExtension);
      return output;
    }

		bool IsSupportedProject
    {
      get
      {
        var type = RhinoPluginType;
        return type != null && type != McNeelProjectType.None;
      }
    }

    int? RhinoVersion => Project.GetRhinoVersion();

    bool RequiresMdb => RhinoPluginType != McNeelProjectType.None && RhinoVersion < 6;

		string PluginExtension => RhinoPluginType?.GetExtension() ?? ".dll";

    bool disableOutputNameChange;

    void SetRequiresMdb()
    {
			if (IsSupportedProject)
				RhinoGlobalProperties.RequiresMdb |= RequiresMdb;
		}

    protected override async Task<BuildResult> OnBuild(ProgressMonitor monitor, ConfigurationSelector configuration, OperationContext operationContext)
    {
      if (!IsSupportedProject)
        return await base.OnBuild(monitor, configuration, operationContext);

      SetRequiresMdb();
			var result = await base.OnBuild(monitor, configuration, operationContext);

			try
      {
        if (!result.Failed)
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
            RenameOutputFile(file.ChangeExtension(".pdb"), file.ChangeExtension(ext + ".pdb"));
          }
        }
      }
      catch (Exception ex)
      {
        monitor.ReportError($"An error occurred renaming output files to .rhi/.ghp.\n{ex}", ex);
      }
      return result;
    }

    protected override ProjectRunConfiguration OnCreateRunConfiguration(string name)
    {
      if (!IsSupportedProject)
        return base.OnCreateRunConfiguration(name);

      return new RhinoRunConfiguration(name);
    }

    protected override ExecutionCommand OnCreateExecutionCommand(ConfigurationSelector configSel, DotNetProjectConfiguration configuration, ProjectRunConfiguration runConfiguration)
    {
      if (!IsSupportedProject)
        return base.OnCreateExecutionCommand(configSel, configuration, runConfiguration);

      return CreateRhinoExecutionCommand(configSel, configuration, runConfiguration);
    }


    RhinoExecutionCommand CreateRhinoExecutionCommand(ConfigurationSelector configSel, DotNetProjectConfiguration configuration, ProjectRunConfiguration runConfiguration)
    {
      FilePath outputFileName;
      var rhinoRunConfiguration = runConfiguration as RhinoRunConfiguration;
      if (rhinoRunConfiguration?.StartAction == AssemblyRunConfiguration.StartActions.Program)
        outputFileName = rhinoRunConfiguration.StartProgram;
      else
        outputFileName = Project.GetOutputFileName(configuration.Selector);

      // find the rhino to use!
      var cmd = new RhinoExecutionCommand(
        Project,
        RhinoPluginType.Value,
        string.IsNullOrEmpty(rhinoRunConfiguration?.StartWorkingDirectory) ? Project.BaseDirectory : rhinoRunConfiguration.StartWorkingDirectory,
        outputFileName,
        rhinoRunConfiguration?.StartArguments,
        rhinoRunConfiguration?.EnvironmentVariables
      );
      cmd.ExternalConsole = rhinoRunConfiguration?.ExternalConsole ?? false;
      cmd.PauseConsoleOutput = rhinoRunConfiguration?.PauseConsoleOutput ?? false;
      return cmd;
    }

    protected override async Task<BuildResult> OnClean(ProgressMonitor monitor, ConfigurationSelector configuration, OperationContext operationContext)
    {
      if (!IsSupportedProject)
        return await base.OnClean(monitor, configuration, operationContext);

      try
      {
				SetRequiresMdb();
				
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

          debugFile = file.ChangeExtension(ext + ".pdb");
          if (File.Exists(debugFile))
            File.Delete(debugFile);
        }
      }
      catch (Exception ex)
      {
        monitor.ReportError($"An error occurred cleaning output files.\n{ex}", ex);
      }
      return await base.OnClean(monitor, configuration, operationContext);
    }

    static void RenameOutputFile(FilePath file, FilePath output)
    {
      if (File.Exists(file.FullPath))
      {
        if (File.Exists(output.FullPath))
          File.Delete(output.FullPath);

        File.Move(file.FullPath, output.FullPath);
      }
    }

    protected override bool OnGetCanExecute(ExecutionContext context, ConfigurationSelector configuration, SolutionItemRunConfiguration runConfiguration)
    {
      if (!IsSupportedProject)
        return base.OnGetCanExecute(context, configuration, runConfiguration);

      // need to call base regardless to set context.RunConfiguration properly.
      // this is probably a flaw in the API design, we really shouldn't need this (it wasn't required with the old API).
      // without this, the breakpoint options dialog does not allow you to set conditions, etc.
      base.OnGetCanExecute(context, configuration, runConfiguration);

      return true;
    }

    protected override ProjectFeatures OnGetSupportedFeatures()
    {
      if (!IsSupportedProject)
        return base.OnGetSupportedFeatures();

      return base.OnGetSupportedFeatures() | ProjectFeatures.RunConfigurations | ProjectFeatures.Execute;
    }
  }
}

