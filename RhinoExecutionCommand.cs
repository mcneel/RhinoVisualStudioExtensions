using MonoDevelop.Projects;
using MonoDevelop.Core.Execution;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace MonoDevelop.Debugger.Soft.Rhino
{
  class RhinoExecutionCommand : ProcessExecutionCommand
  {
    string _applicationPath;

    public DotNetProject Project { get; set; }

    public bool IsGrasshopper { get; set; }

    public string RhinoCommonPath { get; set; }

    public string PluginPath { get; set; }

    public string BinDir { get; set; }

    public int RhinoVersion { get; set; }

    public string ApplicationPath => _applicationPath ?? (_applicationPath = GetApplicationPath());

    public string ExecutablePath => Path.Combine(ApplicationPath, "Contents/MacOS/Rhinoceros");

    public RhinoExecutionCommand(DotNetProject project, string workingDirectory, string outputname, string startArguments, IDictionary<string, string> environmentVariables)
    {
      Project = project;
      Arguments = startArguments;
      WorkingDirectory = workingDirectory;
      Command = outputname;
      RhinoVersion = project.GetRhinoVersion() ?? Helpers.DefaultRhinoVersion;

      for (int i = 0; i < Project.References.Count; i++)
      {
        var reference = Project.References[i];
        if (reference.HintPath != null && reference.HintPath.FileNameWithoutExtension == Helpers.RhinoCommonReferenceName)
        {
          RhinoCommonPath = reference.HintPath;
        }
        if (reference.HintPath != null && reference.HintPath.FileNameWithoutExtension == Helpers.GrasshopperReferenceName)
        {
          IsGrasshopper = true;
        }
      }

      string target = Command;
      if (!string.IsNullOrWhiteSpace(target) && System.IO.File.Exists(target))
      {
        PluginPath = target;
        BinDir = System.IO.Path.GetDirectoryName(target);
      }

      EnvironmentVariables = environmentVariables != null ? new Dictionary<string, string>(environmentVariables) : new Dictionary<string, string>();

      if (!string.IsNullOrEmpty(BinDir))
      {
        EnvironmentVariables["RHINO_BIN_DIRECTORY"] = BinDir;
      }
      if (!string.IsNullOrEmpty(PluginPath))
      {
        if (IsGrasshopper)
          EnvironmentVariables["GRASSHOPPER_PLUGINS"] = PluginPath;
        else
          EnvironmentVariables["RHINO_PLUGIN_PATH"] = PluginPath;
      }
    }

    /// <summary>
    /// Full path to the Rhinoceros executable to start
    /// </summary>
    string GetApplicationPath()
    {
      // always attempt to run the Rhino that contains the RhinoCommon we are referencing first
      // only command line args can override this behavior

      if (string.IsNullOrEmpty(Arguments) && !string.IsNullOrEmpty(RhinoCommonPath))
      {
        var fileinfo = new System.IO.FileInfo(RhinoCommonPath);
        if (fileinfo.Exists)
        {
          var contents_dir = fileinfo.Directory.Parent;
          string path = System.IO.Path.Combine(contents_dir.FullName, "MacOS", "Rhinoceros");
          if (System.IO.File.Exists(path))
            return path;
        }
      }

      string appPath;
      if (Arguments != null && Arguments.StartsWith("-xcode", StringComparison.Ordinal))
      {
        // get output path
        appPath = GetXcodeDerivedDataPath(BinDir);
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
        appPath = GetXcodeDerivedDataPath(BinDir);
        if (appPath == null && RhinoVersion > Helpers.DefaultRhinoVersion)
          appPath = Helpers.StandardInstallWipPath;
        if (appPath == null)
          appPath = Helpers.StandardInstallPath;
      }
      return Directory.Exists(appPath) || File.Exists(appPath) ? appPath : null;
    }


    public static string GetXcodeDerivedDataPath(string targetDirectory)
    {
      var homePath = Environment.GetEnvironmentVariable("HOME");
      var derivedDataPath = Path.Combine(homePath, "Library", "Developer", "Xcode", "DerivedData");
      if (!Directory.Exists(derivedDataPath))
        return null;

      var dataPaths = Directory.GetDirectories(derivedDataPath).Where(r => Path.GetFileName(r).StartsWith("MacRhino-", StringComparison.Ordinal));
      foreach (var dataPath in dataPaths)
      {
        if (dataPath == null)
          continue;

        // load up info.plist to get WorkspacePath and compare with our target directory
        try
        {
          var infoPath = Path.Combine(dataPath, "info.plist");
          var doc = new XmlDocument();
          doc.Load(infoPath);
          var workspacePath = doc.SelectSingleNode("plist/dict/key[.='WorkspacePath']/following-sibling::string[1]")?.InnerText;

          var workspaceDir = new DirectoryInfo(workspacePath);
          if (!workspaceDir.Exists)
            continue;

          var commonPath = FindCommonPath(Path.DirectorySeparatorChar, new[] { workspacePath, targetDirectory });

          // assume the workspacePath points to MacRhino.xcodeproj, so check based on its parent folder, which should be src4.
          if (commonPath == workspaceDir.Parent?.Parent?.FullName)
          {
            var appPath = Path.Combine(dataPath, "Build", "Products", "Debug", "Rhinoceros.app");

            if (Directory.Exists(appPath))
              return appPath;
          }
        }
        catch
        {
          continue;
        }
      }
      return null;
    }

    public static string FindCommonPath(char separator, IEnumerable<string> paths)
    {
      string commonPath = String.Empty;
      var separatedPaths = paths
        .First(str => str.Length == paths.Max(st2 => st2.Length))
        .Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
        .ToList();

      foreach (string segment in separatedPaths)
      {
        if (commonPath.Length == 0 && paths.All(str => str.StartsWith(segment, StringComparison.Ordinal)))
        {
          commonPath = segment;
        }
        else if (paths.All(str => str.StartsWith(commonPath + separator + segment, StringComparison.Ordinal)))
        {
          commonPath += separator + segment;
        }
        else
        {
          break;
        }
      }

      return commonPath;
    }



  }
}

