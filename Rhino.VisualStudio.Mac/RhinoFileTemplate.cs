using System;
using System.Collections.Generic;
using MonoDevelop.Ide.Templates;
using MonoDevelop.Projects;
using System.Text.RegularExpressions;

namespace Rhino.VisualStudio.Mac
{
  public class RhinoFileTemplate : TextFileDescriptionTemplate
  {
		public override string CreateContent(Project project, Dictionary<string, string> tags, string language)
		{
      var content = base.CreateContent(project, tags, language);
      var keys = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
      return Regex.Replace(content, @"\$\{Guid\d+\}", match =>
      {
        if (!keys.TryGetValue(match.Value, out var key))
        {
          key = Guid.NewGuid();
          keys.Add(match.Value, key);
        }
        return key.ToString();
      });
		}
	}
}
