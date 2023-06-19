using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using stdole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rhino.VisualStudio.Windows.Wizard
{
    public class LegacyTemplateWizard : IWizard
    {
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            if (replacementsDictionary.TryGetValue("$safeitemname$", out var fileinputname))
            {
                replacementsDictionary["$safefileinputname$"] = fileinputname;
            }

            for (int i = 1; i <= 10; i++)
            {
                if (!replacementsDictionary.TryGetValue($"$guid{i}$", out var guidString))
                    continue;

                var guid = Guid.Parse(guidString);
                replacementsDictionary[$"$guid{i}x$"] = guid.ToString("X");
            }
        }

        public bool ShouldAddProjectItem(string filePath) => true;
    }
}