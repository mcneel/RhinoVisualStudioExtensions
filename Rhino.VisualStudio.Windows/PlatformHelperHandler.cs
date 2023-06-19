
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Rhino.VisualStudio;
using Rhino.VisualStudio.Windows;
using Eto;

[assembly: ExportHandler(typeof(IPlatformHelpers), typeof(PlatformHelperHandler))]

namespace Rhino.VisualStudio.Windows
{
    class PlatformHelperHandler : IPlatformHelpers
    {
        const string RhinoExe = "Rhino.exe";

        public string FindRhino(int version)
        {
            if (Environment.Is64BitOperatingSystem && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var strings = new List<string>();
                SearchRegistryKey($@"Software\McNeel\Rhinoceros\{version}.0\Install",
                  RegistryHive.LocalMachine, RegistryView.Registry64, strings);

                SearchRegistryKey($@"Software\McNeel\Rhinoceros\{version}.0\Install",
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

