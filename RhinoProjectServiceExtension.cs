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

  public class RhinoProjectServiceExtension : DotNetProjectExtension
  {
    protected override void OnBeginLoad()
    {
      base.OnBeginLoad();

      RhinoGlobalProperties.RequiresMdb = false;
    }

    protected override void OnEndLoad()
    {
      base.OnEndLoad();

      SetRequiresMdb();
    }

    protected override Task OnExecuteCommand(ProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration, ExecutionCommand executionCommand)
    {
      if (IsSupportedProject)
        return OnExecuteRhinoCommand(monitor, context, configuration, executionCommand);

      return base.OnExecuteCommand(monitor, context, configuration, executionCommand);
    }

    async Task OnExecuteRhinoCommand(ProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration, ExecutionCommand executionCommand)
    {
      bool externalConsole = false;
      bool pauseConsole = false;

      var rhinoExecutionCommand = executionCommand as RhinoExecutionCommand;
      if (rhinoExecutionCommand != null)
      {
        //externalConsole = rhinoExecutionCommand.ExternalConsole;
        //pauseConsole = rhinoExecutionCommand.PauseConsoleOutput;
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
      var output = base.OnGetOutputFileName(configuration);
      if (!disableOutputNameChange && IsSupportedProject)
        return output.ChangeExtension(PluginExtension);
      return output;
    }

		bool IsSupportedProject => Project.GetMcNeelProjectType() != null;

    int? RhinoVersion => Project.GetRhinoVersion();

    bool RequiresMdb => RhinoVersion < 6;

		string PluginExtension => Project.GetMcNeelProjectType()?.GetExtension() ?? ".dll";

    bool disableOutputNameChange;

    void SetRequiresMdb()
    {
			if (IsSupportedProject)
				RhinoGlobalProperties.RequiresMdb |= RequiresMdb;
		}

    protected override async Task<BuildResult> OnBuild(ProgressMonitor monitor, ConfigurationSelector configuration, OperationContext operationContext)
    {
      SetRequiresMdb();
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
      return new RhinoRunConfiguration(name);
    }

    protected override ExecutionCommand OnCreateExecutionCommand(ConfigurationSelector configSel, DotNetProjectConfiguration configuration, ProjectRunConfiguration runConfiguration)
    {
      if (IsSupportedProject)
      {
        return CreateRhinoExecutionCommand(configSel, configuration, runConfiguration);
      }

      return base.OnCreateExecutionCommand(configSel, configuration, runConfiguration);
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
      return new RhinoExecutionCommand(
        Project,
        string.IsNullOrEmpty(rhinoRunConfiguration?.StartWorkingDirectory) ? Project.BaseDirectory : rhinoRunConfiguration.StartWorkingDirectory,
        outputFileName,
        rhinoRunConfiguration?.StartArguments,
        rhinoRunConfiguration?.EnvironmentVariables
      );
    }

    protected override async Task<BuildResult> OnClean(ProgressMonitor monitor, ConfigurationSelector configuration, OperationContext operationContext)
    {
      try
      {
				SetRequiresMdb();
				
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

            debugFile = file.ChangeExtension(".pdb");
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

    static void RenameOutputFile(FilePath file, FilePath output)
    {
      if (File.Exists(file.FullPath))
      {
        if (File.Exists(output.FullPath))
          File.Delete(output.FullPath);

        File.Move(file.FullPath, output.FullPath);
      }
    }

    protected override ProjectFeatures OnGetSupportedFeatures()
    {
      var features = base.OnGetSupportedFeatures();
      if (IsSupportedProject)
        features |= ProjectFeatures.Execute;
      return features;
    }

  }
}

