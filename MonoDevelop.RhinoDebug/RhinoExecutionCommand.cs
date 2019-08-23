using MonoDevelop.Projects;
using MonoDevelop.Core.Execution;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace MonoDevelop.RhinoDebug
{
  class RhinoExecutionCommand : ProcessExecutionCommand
  {
    string _applicationPath;
    string _executablePath;

    public bool ExternalConsole { get; set; }

    public bool PauseConsoleOutput { get; set; }

    public DotNetProject Project { get; set; }

    public string RhinoCommonPath { get; set; }

    public string PluginPath { get; set; }

    public string BinDir { get; set; }

    public int RhinoVersion { get; set; }

    public McNeelProjectType RhinoPluginType { get; set; }

    public string ApplicationPath => _applicationPath ?? (_applicationPath = GetApplicationPath());

    public string ExecutablePath => _executablePath ?? (_executablePath = GetExecutablePath(ApplicationPath));

    public RhinoExecutionCommand(DotNetProject project, McNeelProjectType pluginType, string workingDirectory, string outputname, string startArguments, IDictionary<string, string> environmentVariables)
    {
      Project = project;
      Arguments = startArguments;
      WorkingDirectory = workingDirectory;
      Command = outputname;
      RhinoVersion = project.GetRhinoVersion() ?? Helpers.DefaultRhinoVersion;
      RhinoPluginType = pluginType;

      for (int i = 0; i < Project.References.Count; i++)
      {
        // old way of finding app by using the path to RhinoCommon.dll
        var reference = Project.References[i];
        if (reference.HintPath != null && reference.HintPath.FileNameWithoutExtension == Helpers.RhinoCommonReferenceName)
        {
          RhinoCommonPath = reference.HintPath;
        }
      }

      string target = Command;
      if (!string.IsNullOrWhiteSpace(target) && File.Exists(target))
      {
        PluginPath = target;
        BinDir = Path.GetDirectoryName(target);
      }

      EnvironmentVariables = environmentVariables != null ? new Dictionary<string, string>(environmentVariables) : new Dictionary<string, string>();

      if (!string.IsNullOrEmpty(BinDir))
      {
        EnvironmentVariables["RHINO_BIN_DIRECTORY"] = BinDir;
      }
      if (!string.IsNullOrEmpty(PluginPath))
      {
        if (RhinoPluginType == McNeelProjectType.Grasshopper)
          EnvironmentVariables["GRASSHOPPER_PLUGINS"] = PluginPath;
        else if (RhinoPluginType == McNeelProjectType.RhinoCommon)
          EnvironmentVariables["RHINO_PLUGIN_PATH"] = PluginPath;
      }
    }

    string GetExecutablePath(string applicationPath)
    {
      if (string.IsNullOrEmpty(applicationPath))
        return null;

      var executablePath = Path.Combine(applicationPath, "Contents", "MacOS", "Rhinoceros");
      if (File.Exists(executablePath))
        return executablePath;

      // old versions of v5 use this executable
      executablePath = Path.Combine(applicationPath, "Contents", "MacOS", "Rhino");
      if (File.Exists(executablePath))
        return executablePath;

      return null;
    }

    /// <summary>
    /// Full path to the Rhinoceros executable to start
    /// </summary>
    string GetApplicationPath()
    {
      var launcher = Project.ProjectProperties.GetValue(Helpers.RhinoLauncherProperty);
      if (launcher != null)
      {
        switch (launcher.ToLowerInvariant())
        {
          case "xcode":
            return Helpers.GetXcodeDerivedDataPath(BinDir);
          case "app":
            return Helpers.StandardInstallPath;
          case "wip":
            return Helpers.StandardInstallWipPath;
        }
        if (!string.IsNullOrEmpty(launcher))
          return launcher;
      }

      // always attempt to run the Rhino that contains the RhinoCommon we are referencing first
      // only command line args can override this behavior

      if (string.IsNullOrEmpty(Arguments) && !string.IsNullOrEmpty(RhinoCommonPath))
      {
        var fileinfo = new FileInfo(RhinoCommonPath);
        if (fileinfo.Exists)
        {
          var dir = fileinfo.Directory;
          // old v5 way of referencing assemblies directly in the .app folder, new way is to use nuget packages instead.

          // scan up directory tree for an .app, then test if it's Rhino by finding the executable
          while (dir != null)
          {
            if (dir.Name.EndsWith(".app", StringComparison.Ordinal))
            {
              string path = Path.Combine(dir.FullName, "Contents", "MacOS", "Rhinoceros");
              if (File.Exists(path))
                return dir.FullName;
            }
            dir = dir.Parent;
          }
          var contents_dir = fileinfo.Directory.Parent;

        }
      }

      string appPath;
      if (Arguments != null && Arguments.StartsWith("-xcode", StringComparison.Ordinal))
      {
        // get output path
        appPath = Helpers.GetXcodeDerivedDataPath(BinDir);
      }
      else if (Arguments != null && Arguments.StartsWith("-app_path=", StringComparison.Ordinal))
      {
        string path = Arguments.Substring("-app_path=".Length);
        path = path.Trim(new char[] { '\"', ' ' });
        appPath = path;
      }
      else if (Arguments != null && Arguments.StartsWith("-wip", StringComparison.Ordinal))
      {
        appPath = Helpers.StandardInstallWipPath;
      }
      else if (Arguments != null && Arguments.StartsWith("-app", StringComparison.Ordinal))
      {
        appPath = Helpers.StandardInstallPath;
      }
      else
      {
        appPath = Project.DetectApplicationPath(BinDir, RhinoVersion);
      }
      if (appPath == null)
        return null;

      if (!Directory.Exists(appPath))
        return null;

      // check for the executable that should exist
      var executablePath = GetExecutablePath(appPath);
      if (executablePath == null || !File.Exists(executablePath))
        return null;

      return appPath;
    }
  }
}

