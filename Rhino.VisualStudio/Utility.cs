using System;
using System.Text.RegularExpressions;

namespace Rhino.VisualStudio
{
  static class Utility
  {
    public static bool IsValidIdentifier(string value)
    {
      if (string.IsNullOrWhiteSpace(value)) // not invalid, but can't continue
        return false;

      return Regex.IsMatch(value, @"^[a-zA-Z_-][\w\.-_]*$");
    }

    public static string GetSuffixedName(string projectName, string suffix = null, params string[] suffixesToRemove)
    {
      if (projectName == null)
        return string.Empty;

      var comparison = StringComparison.OrdinalIgnoreCase;

      if (suffixesToRemove?.Length > 0)
      {
        foreach (var removeSuffix in suffixesToRemove)
        {
          if (projectName.EndsWith(removeSuffix, comparison))
            projectName = projectName.Substring(0, projectName.Length - removeSuffix.Length);
        }
      }
      var dotIndex = projectName.IndexOf('.');
      if (dotIndex > 0 && dotIndex < projectName.Length - 1)
        projectName = projectName.Substring(dotIndex + 1);

      if (!string.IsNullOrEmpty(suffix) && !projectName.EndsWith(suffix, comparison))
        projectName += suffix;
      return projectName;
    }

    public static string GetSafeName(string projectName, string suffix = null, params string[] suffixesToRemove)
    {
      if (projectName == null)
        return string.Empty;
      projectName = GetSuffixedName(projectName, suffix, suffixesToRemove);

      return Regex.Replace(projectName, @"[ \-\.]", "_");
    }

    public static bool IsValidProjectName(string projectName, bool noDash = false)
    {
      if (string.IsNullOrEmpty(projectName)) // not invalid, but can't continue
        return true;
      if (noDash && projectName.Contains("-"))
        return false;
      if (!Regex.IsMatch(projectName, @"^[a-zA-Z_-][ \w\.-_]*$"))
        return false;
      return true;
    }
  }
}