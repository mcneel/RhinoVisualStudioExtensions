using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;
using System.Reflection;
using System.IO;
using Eto.Forms;
using Microsoft.VisualStudio.Shell;

namespace Rhino.VisualStudio.Windows.Wizard
{
    /// <summary>
    /// Base wizard for eto-based UI
    /// </summary>
    /// <remarks>
    /// See: http://msdn.microsoft.com/en-us/library/ms185301.aspx
    /// </remarks>
    public abstract class EtoWizard : IWizard
    {
        static EtoWizard()
        {
            EtoInitializer.Initialize();
        }

        protected abstract BaseWizardViewModel CreateViewModel();

        protected abstract Control CreatePanel();

        // This method is called before opening any item that 
        // has the OpenInEditor attribute.
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        // This method is only called for item templates,
        // not for project templates.
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        // This method is called after the project is created.
        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                var model = CreateViewModel();
                model.Host = new WizardHost(replacementsDictionary);
                model.ProjectName = replacementsDictionary["$projectname$"];

                var panel = CreatePanel();

                var dialog = new BaseDialog();
                dialog.Content = panel;
                dialog.Title = model.ProjectTitle;
                dialog.DataContext = model;

                if (!dialog.ShowModal(Helpers.MainWindow))
                {
                    throw new WizardBackoutException("User cancelled the wizard.");
                }

                model.Finish();
            }
            catch (Exception ex)
            {
                if (ex is WizardCancelledException || ex is WizardBackoutException)
                    throw;
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                throw new WizardCancelledException("An error occurred.", ex);
            }

        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        // This method is only called for item templates,
        // not for project templates.
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

    }
}