using System;
using System.IO;
using MonoDevelop.Projects;
using System.Linq;
using MonoDevelop.Core;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Xml;
using System.Threading.Tasks;
using System.Threading;
using MonoDevelop.Ide.Templates;
using Foundation;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Rhino.VisualStudio.Mac
{
    enum McNeelProjectType
    {
        None,
        DebugStarter,
        RhinoCommon,
        Grasshopper
    }

    static class Helpers
    {
        public const string StandardInstallPath = "/Applications/Rhino 7.app";
        public const string StandardInstallWipPath = "/Applications/RhinoWIP.app";
        public const string StandardInstallBetaPath = "/Applications/RhinoBETA.app";
        public const string GrasshopperReferenceName = "Grasshopper";
        public const string RhinoCommonReferenceName = "RhinoCommon";
        public const int DefaultRhinoVersion = 7;

        public const string RhinoPluginTypeProperty = "RhinoPluginType";
        public const string RhinoLauncherProperty = "RhinoMacLauncher";
        public const string RhinoVersionProperty = "RhinoVersion";

        public static string GetExtension(this McNeelProjectType type)
        {
            switch (type)
            {
                case McNeelProjectType.None:
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

        public static readonly IEnumerable<int> RhinoVersions = new[] { 6, 7, 8 };

        public static void RefreshRhinoVersion(this IBuildTarget item)
        {
            var project = item as DotNetProject;
            if (project == null)
                return;

            project.ExtendedProperties[s_RhinoVersionKey] = null;
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

            if (int.TryParse(project.ProjectProperties.GetValue<string>(RhinoVersionProperty), out var ver))
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
                // compiling rhino itself, find version from our version.txt if it exists
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
                        using (var stream = File.OpenRead(absPath.FullPath))
                        {
                            var reader = new PEReader(stream);
                            var metadataReader = reader.GetMetadataReader();
                            var version = metadataReader.GetAssemblyDefinition().Version;
                            Console.WriteLine($"Found Rhino assembly version {version}");
                            result = version.Major;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ignore errors here!
                    Console.WriteLine($"Exception was thrown trying to read Rhino assembly version {ex}");
                }
            }

            if (result == null)
            {
                foreach (var projectItem in project.Items.OfType<ProjectItem>().Where(r => r.ItemName == "PackageReference"))
                {
                    if (projectItem.Include == GrasshopperReferenceName
                      || projectItem.Include == RhinoCommonReferenceName)
                    {
                        var version = projectItem.Metadata.GetValue("Version");
                        var idx = version?.IndexOf('.') ?? -1;
                        if (idx > 0)
                        {
                            if (int.TryParse(version.Substring(0, idx), out var r))
                                result = r;
                        }
                        break;
                    }
                }
            }
            project.ExtendedProperties[s_RhinoVersionKey] = result;
            return result;
        }

        public static McNeelProjectType? GetPluginProjectType(this IBuildTarget item)
        {
            var project = item as DotNetProject;
            if (project == null)
                return null;

            var pluginType = project.ProjectProperties.GetValue(RhinoPluginTypeProperty);
            if (pluginType != null)
            {
                // project has explicitly set the plugin type
                switch (pluginType.ToLowerInvariant())
                {
                    case "gha":
                    case "grasshopper":
                        return McNeelProjectType.Grasshopper;
                    case "rhp":
                    case "rhino":
                        return McNeelProjectType.RhinoCommon;
                    case "dev":
                        return McNeelProjectType.DebugStarter;
                    case "none":
                        return McNeelProjectType.None;
                }
            }

            return null;
        }

        public static string DetectApplicationPath(this IBuildTarget item, string binDir, int rhinoVersion)
        {
            // check the derived data folders if they were built from the same code tree
            var appPath = GetXcodeDerivedDataPath(binDir);

            if (appPath == null)
            {
                // find all rhinos on the system

                appPath = FindRhinoWithVersion(rhinoVersion);
            }
            //if (appPath == null && rhinoVersion > DefaultRhinoVersion && Directory.Exists(StandardInstallWipPath))
            //appPath = StandardInstallWipPath;
            //if (appPath == null)
            //  appPath = StandardInstallPath;

            return appPath;
        }

        class RhinoInfo
        {
            public string Path { get; set; }
            public Version Version { get; set; }
        }

        public static string FindRhinoWithVersion(int rhinoVersion)
        {
            var identifier = "com.mcneel.rhinoceros";
            if (rhinoVersion >= 7)
                identifier += "." + rhinoVersion;
            var foundRhino = FindRhinosWithIdentifier(identifier)
              .Where(r => r.Version?.Major == rhinoVersion) // version can be null for development builds
              .OrderByDescending(r => r.Version) // find the latest/greatest
              .FirstOrDefault();
            return foundRhino?.Path;
        }

        static IEnumerable<RhinoInfo> FindRhinosWithIdentifier(string identifier)
        {
            return FindApplicationsWithIdentifier(identifier)
              .Select(r => new RhinoInfo { Path = r, Version = GetVersionOfAppBundle(r) });
        }

        static IEnumerable<string> FindApplicationsWithIdentifier(string identifier)
        {
            var urls = CoreServices.LaunchServices.GetApplicationUrlsForBundleIdentifier(identifier);
            if (urls != null)
            {
                var paths = urls.Where(r => r.IsFileUrl).Select(r => r.FilePathUrl.Path).ToList();

                // find ones in /Applications folder first
                var applicationPaths = paths.Where(r => r.StartsWith("/Applications/", StringComparison.OrdinalIgnoreCase)).ToList();
                foreach (var path in applicationPaths)
                    yield return path;

                // then use the others
                var otherPaths = paths.Where(r => !applicationPaths.Contains(r));
                foreach (var path in otherPaths)
                    yield return path;

            }
        }

        static NSString s_VersionKey = new NSString("CFBundleVersion");
        static NSString s_ShortVersionKey = new NSString("CFBundleShortVersionString");

        public static Version GetVersionOfAppBundle(string appBundlePath)
        {
            if (!Directory.Exists(appBundlePath))
                return null;

            using (var bundle = NSBundle.FromPath(appBundlePath))
            {
                var info = bundle?.InfoDictionary;
                if (info == null)
                    return null;
                var versionString = info.ObjectForKey(s_VersionKey) as NSString;
                if (versionString != null && Version.TryParse(versionString, out var version))
                    return version;
                var shortVersionString = info.ObjectForKey(s_ShortVersionKey) as NSString;
                if (shortVersionString != null && Version.TryParse(shortVersionString, out version))
                    return version;
                return null;
            }
        }

        public static McNeelProjectType? DetectMcNeelProjectType(this IBuildTarget item, bool onlyWithTargetExt)
        {
            var project = item as DotNetProject;
            if (project == null)
                return null;
                
            // only library types
            if (project.CompileTarget != CompileTarget.Library)
                return null;

            // auto-detect the type of project based on the references/packages
            McNeelProjectType? type = null;

            // check grasshopper first as those projects reference both RhinoCommon and Grasshopper
            foreach (var reference in project.References)
            {
                if (reference.ReferenceType == ReferenceType.Project)
                {
                    if (reference.Reference == RhinoCommonReferenceName)
                        return McNeelProjectType.DebugStarter;
                }
                else
                {
                    if (reference.Reference == GrasshopperReferenceName)
                    {
                        type = McNeelProjectType.Grasshopper;
                        // if we find grasshopper reference, we detect it as a grasshopper plugin
                        break;
                    }
                    if (reference.Reference == RhinoCommonReferenceName)
                    {
                        type = McNeelProjectType.RhinoCommon;
                        // found rhinocommon, but keep searching for grasshopper if it exists
                    }
                }
            }
            if (type == null)
            {

                // check for  packages (e.g. if you are using a sdk-style library)
                // only check one level deep, so you need to reference the package directly
                foreach (var reference in project.Items.OfType<ProjectItem>().Where(r => r.ItemName == "PackageReference"))
                {
                    if (reference.Include == GrasshopperReferenceName)
                    {
                        type = McNeelProjectType.Grasshopper;
                        break;
                    }
                    if (reference.Include == RhinoCommonReferenceName)
                    {
                        type = McNeelProjectType.RhinoCommon;
                        // found rhinocommon, but keep searching for grasshopper if it exists
                    }
                }
            }


            if (type != null)
            {
                // found a reference, but the project might have a target extension that tells a different story
                var targetExt = project.ProjectProperties.GetValue("TargetExt");
                if (targetExt != null)
                {
                    switch (targetExt.ToLowerInvariant())
                    {
                        case ".gha":
                            type = McNeelProjectType.Grasshopper;
                            break;
                        case ".rhp":
                            type = McNeelProjectType.RhinoCommon;
                            break;
                        default:
                            // TargetExt is our source of truth.
                            if (onlyWithTargetExt)
                                type = null;
                            break;
                    }
                }
                else if (onlyWithTargetExt)
                    type = null;
            }

            return type;
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

