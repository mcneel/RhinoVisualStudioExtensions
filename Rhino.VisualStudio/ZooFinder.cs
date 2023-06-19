using System;
using System.IO;

namespace Rhino.VisualStudio
{
    public static class ZooFinder
    {
        const string ZOO_DIR = "Zoo ";
        const string ZOO_DLL = "ZooPlugin.dll";

        public static string FindZooDll(int version)
        {
            string prog_folder = Environment.GetFolderPath(Environment.Is64BitOperatingSystem ?
              Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles);

            if (!Directory.Exists(prog_folder))
                throw new InvalidOperationException("The program files folder could not be found.");

            var path = Path.Combine(prog_folder, ZOO_DIR + version);

            if (!Directory.Exists(path))
                return null;
            //     throw new InvalidOperationException(
            //   string.Format("No zoo directory:\n{0}", path));

            var final_location = Path.Combine(path, ZOO_DLL);

            //if (!File.Exists(final_location))
            //  throw new InvalidOperationException(
            //    $"The Zoo {version}.0 folder was found in {path}\nbut the file \"{ZOO_DLL}\" was not present.");

            return final_location;
        }
    }
}
