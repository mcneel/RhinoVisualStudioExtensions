//___FILEHEADER___

#include "___PACKAGENAMEASIDENTIFIER___PlugIn.h"

// Do NOT put the definition of class CCommand___PACKAGENAMEASIDENTIFIER___ in a header
// file. There is only ONE instance of a CCommand___PACKAGENAMEASIDENTIFIER___ class
// and that instance is the static the___PACKAGENAMEASIDENTIFIER___Command that appears
// immediately below the class definition.

class CCommand___PACKAGENAMEASIDENTIFIER___ : public CRhinoCommand
{
public:
  // The one and only instance of CCommand___PACKAGENAMEASIDENTIFIER___ is created below.
  // Values of member variables persist for the duration of the application.
  CCommand___PACKAGENAMEASIDENTIFIER___() = default;

  // The destructor should not make any calls to the Rhino SDK.
  // If your command has persistent settings, then override
  // CRhinoCommand::SaveProfile and CRhinoCommand::LoadProfile.
  ~CCommand___PACKAGENAMEASIDENTIFIER___() = default;

  // Returns a unique UUID for this command.
  // If you try to use an id that is already being used, then
  // your command will not work.
  UUID CommandUUID() override
  {
    static const UUID id = ON_UuidFromString("___UUID___");
    return id;
  }

  // Returns the English command name.
  // If you want to provide a localized command name, then override
  // CRhinoCommand::LocalCommandName.
  const wchar_t* EnglishCommandName() override { return L"___PACKAGENAMEASIDENTIFIER___"; }

  // Rhino calls RunCommand to run the command.
  CRhinoCommand::result RunCommand(const CRhinoCommandContext& context) override;
};

// The one and only CCommand___PACKAGENAMEASIDENTIFIER___ object
// Do NOT create any other instance of a CCommand___PACKAGENAMEASIDENTIFIER___ class.
static class CCommand___PACKAGENAMEASIDENTIFIER___ the___PACKAGENAMEASIDENTIFIER___Command;

CRhinoCommand::result CCommand___PACKAGENAMEASIDENTIFIER___::RunCommand(const CRhinoCommandContext& context)
{
  // TODO: Add command code here.

  // Rhino commands that display a dialog box interface should also support
  // a command-line, or scriptable interface.

  ON_wString str;
  str.Format(L"The \"%ls\" command is under construction.\n", EnglishCommandName());
  if (context.IsInteractive())
    RhinoMessageBox(str, ___PACKAGENAMEASIDENTIFIER___PlugIn().PlugInName(), MB_OK);
  else
    RhinoApp().Print(str);

  // TODO: Return one of the following values:
  //   CRhinoCommand::success:  The command worked.
  //   CRhinoCommand::failure:  The command failed because of invalid input, inability
  //                            to compute the desired result, or some other reason.
  //   CRhinoCommand::cancel:   The user interactively canceled the command
  //                            (by pressing ESCAPE, clicking a CANCEL button, etc.)
  return CRhinoCommand::success;
}
