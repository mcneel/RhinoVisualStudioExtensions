using System;
using System.IO;
using MonoDevelop.Projects;
using System.Linq;
using MonoDevelop.Core;
using System.Collections.Generic;
namespace MonoDevelop.Debugger.Soft.Rhino
{
  enum McNeelProjectType
  {
    DebugStarter,
    RhinoCommon,
    Grasshopper
  }

  static class Helpers
  {
    const string StandardInstallPath = "/Applications/Rhinoceros.app";
    const string GrasshopperReferenceName = "Grasshopper";
    const string RhinoCommonReferenceName = "RhinoCommon";

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

    public static string GetExtension (this McNeelProjectType type)
    {
      switch (type) {
      case McNeelProjectType.DebugStarter:
        return ".dll";
      case McNeelProjectType.Grasshopper:
        return ".gha";
      case McNeelProjectType.RhinoCommon:
        return ".rhp";
      default:
        throw new NotSupportedException ();
      }
    }

    public static McNeelProjectType? GetMcNeelProjectType (this IBuildTarget item)
    {
      var project = item as DotNetProject;
      if (project == null)
        return null;

      // check grasshopper first as those projects reference both RhinoCommon and Grasshopper
      var reference = project.References.FirstOrDefault (r => r.Reference == GrasshopperReferenceName);
      if (reference == null)
        reference = project.References.FirstOrDefault (r => r.Reference == RhinoCommonReferenceName);

      if (reference == null)
        return null;

      // if it's a project reference (vs assembly), treat it as a debug starter so we don't rename the output
      if (reference.ReferenceType == ReferenceType.Project)
        return McNeelProjectType.DebugStarter;

      switch (reference.Reference) {
      case GrasshopperReferenceName:
        return McNeelProjectType.Grasshopper;
      case RhinoCommonReferenceName:
        return McNeelProjectType.RhinoCommon;
      default:
        return McNeelProjectType.DebugStarter;
      }
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

