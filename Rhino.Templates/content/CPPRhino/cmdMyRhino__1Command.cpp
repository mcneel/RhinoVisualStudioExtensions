// cmdMyRhino__1Command.cpp : command file
//

#include "stdafx.h"
#include "MyRhino.1PlugIn.h"

////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////
//
// BEGIN MyRhino__1Command command
//

#pragma region MyRhino__1Command command

// Do NOT put the definition of class CCommandMyRhino__1Command in a header
// file. There is only ONE instance of a CCommandMyRhino__1Command class
// and that instance is the static theMyRhino__1CommandCommand that appears
// immediately below the class definition.

class CCommandMyRhino__1Command : public CRhinoCommand
{
public:
  // The one and only instance of CCommandMyRhino__1Command is created below.
  // No copy constructor or operator= is required.
  // Values of member variables persist for the duration of the application.

  // CCommandMyRhino__1Command::CCommandMyRhino__1Command()
  // is called exactly once when static theMyRhino__1CommandCommand is created.
  CCommandMyRhino__1Command() = default;

  // CCommandMyRhino__1Command::~CCommandMyRhino__1Command()
  // is called exactly once when static theMyRhino__1CommandCommand is destroyed.
  // The destructor should not make any calls to the Rhino SDK. 
  // If your command has persistent settings, then override 
  // CRhinoCommand::SaveProfile and CRhinoCommand::LoadProfile.
  ~CCommandMyRhino__1Command() = default;

  // Returns a unique UUID for this command.
  // If you try to use an id that is already being used, then
  // your command will not work. Use GUIDGEN.EXE to make unique UUID.
  UUID CommandUUID() override
  {
    // {80A47E60-E77D-4299-BA1F-F84B53304756}
    static const GUID MyRhino__1CommandCommand_UUID = 
    { 0x80a47e60, 0xe77d, 0x4299, { 0xba, 0x1f, 0xf8, 0x4b, 0x53, 0x30, 0x47, 0x56 } };
    return MyRhino__1CommandCommand_UUID;
  }

  // Returns the English command name.
  // If you want to provide a localized command name, then override 
  // CRhinoCommand::LocalCommandName.
  const wchar_t* EnglishCommandName() override { return L"MyRhino__1Command"; }

  // Rhino calls RunCommand to run the command.
  CRhinoCommand::result RunCommand(const CRhinoCommandContext& context) override;
};

// The one and only CCommandMyRhino__1Command object
// Do NOT create any other instance of a CCommandMyRhino__1Command class.
static class CCommandMyRhino__1Command theMyRhino__1CommandCommand;

CRhinoCommand::result CCommandMyRhino__1Command::RunCommand(const CRhinoCommandContext& context)
{
  // CCommandMyRhino__1Command::RunCommand() is called when the user
  // runs the "MyRhino__1Command".

  // TODO: Add command code here.

  // Rhino command that display a dialog box interface should also support
  // a command-line, or script-able interface.

  ON_wString str;
  str.Format(L"The \"%s\" command is under construction.\n", EnglishCommandName());
  const wchar_t* pszStr = static_cast<const wchar_t*>(str);
  if (context.IsInteractive())
    RhinoMessageBox(pszStr, MyRhino__1PlugIn().PlugInName(), MB_OK);
  else
    RhinoApp().Print(pszStr);

  // TODO: Return one of the following values:
  //   CRhinoCommand::success:  The command worked.
  //   CRhinoCommand::failure:  The command failed because of invalid input, inability
  //                            to compute the desired result, or some other reason
  //                            computation reason.
  //   CRhinoCommand::cancel:   The user interactively canceled the command 
  //                            (by pressing ESCAPE, clicking a CANCEL button, etc.)
  //                            in a Get operation, dialog, time consuming computation, etc.

  return CRhinoCommand::success;
}

#pragma endregion

//
// END MyRhino__1Command command
//
////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////
