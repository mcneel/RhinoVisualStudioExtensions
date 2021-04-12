using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TemplateWizard;

namespace Rhino.VisualStudio.Windows.Wizard
{
  class WizardHost : IWizardHost
  {
    Dictionary<string, string> _replacementsDictionary;

    const string ParametersPrefix = "$passthrough:";

    static WizardHost()
    {
      EtoInitializer.Initialize();
    }

    public WizardHost(Dictionary<string, string> replacementsDictionary)
    {
      _replacementsDictionary = replacementsDictionary;
    }

    public void Abort()
    {
      throw new WizardBackoutException("User cancelled the wizard");
    }

    public void Continue()
    {
      
    }

    public string GetParameter(string name)
    {
      string value;
      if (
        _replacementsDictionary.TryGetValue("$" + name + "$", out value)
        || _replacementsDictionary.TryGetValue("$root." + name + "$", out value)
        || _replacementsDictionary.TryGetValue(ParametersPrefix + name + "$", out value)
        || _replacementsDictionary.TryGetValue("$root." + (ParametersPrefix + name).TrimStart('$') + "$", out value)
        )
        return value;
      return null;
    }

    public bool IsSupportedParameter(string name)
    {
      if (!_replacementsDictionary.TryGetValue("SupportedParameters", out var supportedParameterString))
        return false;
      var parameters = supportedParameterString.Split(';');

      return parameters.Any(r => string.Equals(r, name, StringComparison.OrdinalIgnoreCase));
    }

    public void SetParameter(string name, string value)
    {
      _replacementsDictionary[ParametersPrefix + name.Trim('$') + "$"] = value;
    }

  }
}