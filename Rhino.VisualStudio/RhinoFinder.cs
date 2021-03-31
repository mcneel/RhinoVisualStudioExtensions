using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;

namespace Rhino.VisualStudio
{
  public static class RhinoFinder
  {
    const string RhinoExe = "Rhino.exe";

    public static string FindRhino(int majorVersion)
    {
      if (Environment.Is64BitOperatingSystem && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        var strings = new List<string>();
        SearchRegistryKey($@"Software\McNeel\Rhinoceros\{majorVersion}.0\Install",
          RegistryHive.LocalMachine, RegistryView.Registry64, strings);

        SearchRegistryKey($@"Software\McNeel\Rhinoceros\{majorVersion}.0\Install",
          RegistryHive.CurrentUser, RegistryView.Registry64, strings);


        foreach (var str in strings)
        {
          var path = Path.Combine(str, "System", RhinoExe);
          if (File.Exists(path))
          {
            return path;
          }
        }
      }

      return string.Empty;
    }

    /// <summary>
    /// Caution: this method swallows any exception.
    /// </summary>
    private static bool SearchRegistryKey(string keyName, RegistryHive hive, RegistryView view, IList<string> installPaths)
    {
      try
      {
        using (var registryKey = RegistryKey.OpenBaseKey(hive, view).OpenSubKey(keyName))
        {
          if (registryKey != null)
          {
            string value = registryKey.GetValue("InstallPath") as string;
            if (!string.IsNullOrEmpty(value))
            {
              if (!installPaths.Contains(value))
                installPaths.Add(value);
              return true;
            }
          }
        }
      }
      catch { }
      return false;
    }
  }
}
