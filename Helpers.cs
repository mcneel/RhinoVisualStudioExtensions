using System;
using System.IO;
using MonoDevelop.Projects;
using System.Linq;
using MonoDevelop.Core;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Xml;
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
    public const string StandardInstallPath = "/Applications/Rhinoceros.app";
    public const string StandardInstallWipPath = "/Applications/RhinoWIP.app";
    public const string GrasshopperReferenceName = "Grasshopper";
    public const string RhinoCommonReferenceName = "RhinoCommon";
    public const int DefaultRhinoVersion = 5;

    public static string GetExtension(this McNeelProjectType type)
    {
      switch (type)
      {
        case McNeelProjectType.DebugStarter:
          return ".dll";
        case McNeelProjectType.Grasshopper:
          return ".gha";
        case McNeelProjectType.RhinoCommon:
          return ".rhp";
        default:
          throw new NotSupportedException();
      }
    }

    static object s_RhinoVersionKey = new object();
    public static int? GetRhinoVersion(this IBuildTarget item)
    {
      var project = item as DotNetProject;
      if (project == null)
        return null;

      var isv6 = project.ExtendedProperties[s_RhinoVersionKey] as int?;
      if (isv6 != null)
        return isv6.Value;

      if (int.TryParse(project.ProjectProperties.GetValue<string>("RhinoVersion"), out var ver))
      {
        project.ExtendedProperties[s_RhinoVersionKey] = ver;
        return ver;
      }

      // check grasshopper first as those projects reference both RhinoCommon and Grasshopper
      var reference = project.References.FirstOrDefault(r => r.Reference == GrasshopperReferenceName);
      if (reference == null)
        reference = project.References.FirstOrDefault(r => r.Reference == RhinoCommonReferenceName);

      int? result = null;

      if (reference?.ReferenceType == ReferenceType.Project)
      {
        // the reference is a project type, assume v6
        // and in v5 we include our own props file
        result = 6;
      }
      else if (reference != null)
      {
        try
        {
          var absPath = project.GetAbsoluteChildPath(FilePath.Build(reference.HintPath));
          if (File.Exists(absPath.FullPath))
          {
            var raw = File.ReadAllBytes(absPath.FullPath);
            var asm = Assembly.ReflectionOnlyLoad(raw);
            Console.WriteLine($"Found Rhino assembly version {asm.GetName().Version}");
            result = asm.GetName().Version.Major;
          }
        }
        catch (Exception ex)
        {
          // ignore errors here!
          Console.WriteLine($"Exception was thrown trying to read Rhino assembly version {ex}");
        }
      }
      project.ExtendedProperties[s_RhinoVersionKey] = result;
      return result;
    }

    public static McNeelProjectType? GetMcNeelProjectType(this IBuildTarget item)
    {
      var project = item as DotNetProject;
      if (project == null)
        return null;

      var rhinoProperty = project.ProjectProperties.GetProperty("RhinoPlugin");
      if (rhinoProperty != null)
      {
        if (rhinoProperty.GetValue<bool>())
          return McNeelProjectType.RhinoCommon;
      }
      var ghProperty = project.ProjectProperties.GetProperty("GrasshopperComponent");
      if (ghProperty != null)
      {
        if (ghProperty.GetValue<bool>())
          return McNeelProjectType.Grasshopper;
      }

      // only auto-detect if the project has not specified something explicitly
      if (rhinoProperty != null || ghProperty != null)
        return null;

      // check grasshopper first as those projects reference both RhinoCommon and Grasshopper
      var reference = project.References.FirstOrDefault(r => r.Reference == GrasshopperReferenceName);
      if (reference == null)
        reference = project.References.FirstOrDefault(r => r.Reference == RhinoCommonReferenceName);

      if (reference == null)
        return null;

      // if it's a project reference (vs assembly), treat it as a debug starter so we don't rename the output
      if (reference.ReferenceType == ReferenceType.Project)
        return McNeelProjectType.DebugStarter;

      switch (reference.Reference)
      {
        case GrasshopperReferenceName:
          return McNeelProjectType.Grasshopper;
        case RhinoCommonReferenceName:
          return McNeelProjectType.RhinoCommon;
        default:
          return McNeelProjectType.DebugStarter;
      }
    }
  }
}

