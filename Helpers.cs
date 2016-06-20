using System;
using System.IO;
using MonoDevelop.Projects;
using System.Linq;
using MonoDevelop.Core;
namespace MonoDevelop.Debugger.Soft.Rhino
{
  enum McNeelProjectType
  {
    Grasshopper,
    Rhino
  }

  static class Helpers
  {
    const string StandardInstallPath = "/Applications/Rhinoceros.app";

    public static string GetXcodePath ()
    {
      var homePath = Environment.GetEnvironmentVariable ("HOME");
      var derivedDataPath = Path.Combine (homePath, "Library/Developer/Xcode/DerivedData");
      if (!Directory.Exists (derivedDataPath))
        return null;
      var appPath = Directory.GetDirectories (derivedDataPath).FirstOrDefault (r => Path.GetFileName (r).StartsWith ("MacRhino-", StringComparison.Ordinal));
      if (appPath == null)
        return null;
      appPath = Path.Combine (appPath, "Build/Products/Debug/Rhinoceros.app");
      if (!Directory.Exists (appPath))
        return null;
      return appPath;
    }

    static bool IsDevProjectReference (DotNetProject project, ProjectReference r)
    {
      return project.Name != "DebugStarter" && r.ReferenceType == ReferenceType.Project;
      /*var devPath = FilePath.Build ("~", "Library", "Developer");
      return r.HintPath.IsChildPathOf(devPath)*/;
    }

    public static string GetExtension (this McNeelProjectType type)
    {
      switch (type) {
      case McNeelProjectType.Grasshopper:
        return "gha";
      case McNeelProjectType.Rhino:
        return "rhp";
      default:
        throw new NotSupportedException ();
      }
    }

    public static McNeelProjectType? GetMcNeelProjectType (this IBuildTarget item)
    {
      var project = item as DotNetProject;
      if (project == null)
        return null;
      if (project.References.Any (r => r.Reference == "Grasshopper" && !IsDevProjectReference (project, r)))
        return McNeelProjectType.Grasshopper;
      if (project.References.Any (r => r.Reference == "RhinoCommon" && !IsDevProjectReference (project, r)))
        return McNeelProjectType.Rhino;
      return null;
    }

    public static string GetAppPath (string parameters, string childPath)
    {
      string appPath;
      if (parameters != null && parameters.StartsWith ("-xcode", StringComparison.Ordinal)) {
        // get output path
        appPath = GetXcodePath ();
      } else if (parameters != null && parameters.StartsWith ("-app_path=", StringComparison.Ordinal)) {
        string path = parameters.Substring ("-app_path=".Length);
        path = path.Trim (new char [] { '\"', ' ' });
        appPath = path;
      } else if (parameters != null && parameters.StartsWith ("-app", StringComparison.Ordinal)) {
        appPath = StandardInstallPath;
      } else {
        appPath = GetXcodePath () ?? StandardInstallPath;
      }
      if (!string.IsNullOrEmpty (childPath))
        appPath = Path.Combine (appPath, childPath);
      return Directory.Exists (appPath) || File.Exists (appPath) ? appPath : null;
    }
  }
}

